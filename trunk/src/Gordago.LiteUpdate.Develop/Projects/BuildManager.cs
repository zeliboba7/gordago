/**
* @version $Id: BuildManager.cs 4 2009-02-03 13:20:59Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate.Develop.Projects
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.Threading;
  using System.IO;
  using ICSharpCode.SharpZipLib.Zip;

  public delegate void BuilderEventHandler(object sender, BuilderEventArgs e);
  public delegate void BuilderProgressEventHandler(object sender, BuilderProgressEventArgs e);
  public delegate void ZipEventHandler(object sender, ZipEventArgs e);


  #region public class BuilderEventArgs
  public class BuilderEventArgs : EventArgs {
    private readonly Project _project;

    public BuilderEventArgs(Project project) {
      _project = project;
    }

    #region public Project Project
    public Project Project {
      get { return _project; }
    }
    #endregion
  }
  #endregion

  #region public class BuilderProgressEventArgs : BuilderEventArgs
  public class BuilderProgressEventArgs : BuilderEventArgs {

    private int _current;
    private int _total;

    public BuilderProgressEventArgs(Project project, int current, int total):base(project){
      _current = current;
      _total = total;
    }
    public int Current {
      get { return _current; }
    }

    public int Total {
      get { return _total; }
    }
  }
  #endregion

  #region public class ZipEventArgs : EventArgs
  public class ZipEventArgs : EventArgs {

    private FileInfo _file;
    private FileInfo _zipFile;
    private long _current;
    private long _total;

    public ZipEventArgs(FileInfo file, FileInfo zipFile, long current, long total):base() {
      _file = file;
      _zipFile = zipFile;
      _current = current;
      _total = total;
    }

    #region public FileInfo File
    public FileInfo File {
      get { return _file; }
    }
    #endregion

    #region public FileInfo ZipFile
    public FileInfo ZipFile {
      get { return _zipFile; }
    }
    #endregion

    #region public long Current
    public long Current {
      get { return _current; }
    }
    #endregion

    #region public long Total
    public long Total {
      get { return _total; }
    }
    #endregion
  }
  #endregion

  public class BuildManager {

    #region class TMZipFile
    class TMZipFile {

      private readonly FileInfo _zipFile;
      private readonly TMFile _file;

      public TMZipFile(FileInfo zipFile, TMFile file){
        _zipFile = zipFile;
        _file = file;
      }

      public FileInfo ZipFile {
        get { return _zipFile; }
      }

      public TMFile TMFile {
        get { return _file; }
      }
    }
    #endregion

    public event BuilderEventHandler StartBuild;
    public event BuilderEventHandler StopBuild;
    public event BuilderProgressEventHandler Progress;

    public event ZipEventHandler CompressFile;

    private int _total = 0;
    private int _current = 0;
    private Project _project;

    public BuildManager() { }

    #region public void Build(Project project)
    public void Build(Project project) {
      Thread th = new Thread(new ParameterizedThreadStart(this.BuildProcess));
      th.Priority = ThreadPriority.Lowest;
      th.Name = "BuildProject_" + project.Name;
      th.IsBackground = true;
      th.Start(project);
    }
    #endregion

    #region protected virtual void OnCompressFile(ZipEventArgs e)
    protected virtual void OnCompressFile(ZipEventArgs e) {
      if (this.CompressFile != null)
        this.CompressFile(this, e);
    }
    #endregion

    #region protected virtual void OnProgress(BuilderProgressEventArgs e)
    protected virtual void OnProgress(BuilderProgressEventArgs e) {
      if (this.Progress != null)
        this.Progress(this, e);
    }
    #endregion

    #region private void BuildProcess(object param)
    private void BuildProcess(object param) {
      Project project = param as Project;

      if (StartBuild != null) 
        StartBuild(this, new BuilderEventArgs(project));

      Console.WriteLine(string.Format("----- Build started: {0} -----", project.Name));
      List<string> errors = new List<string>();

      DirectoryInfo outputDir = new DirectoryInfo(Path.Combine(project.File.Directory.FullName, "Output"));
      
      try {
        if (outputDir.Exists)
          outputDir.Delete(true);
        outputDir.Create();

        _project = project;
        _current = 0;
        _total = project.FileSystem.GetCountFiles();

        List<TMZipFile> zipFiles = new List<TMZipFile>();

        foreach (TMRootFolder rootFolder in project.FileSystem) {
          DirectoryInfo rootDir = new DirectoryInfo(Path.Combine(outputDir.FullName, rootFolder.ToString()));
          rootDir.Create();
          this.Compile(zipFiles, rootFolder, rootDir, errors);
        }

        Console.Write("Create info - ");
        this.CompileInfo(zipFiles, outputDir);
        Console.WriteLine("ok");

        long appLenght = 0;
        long zipLenght = 0;
        foreach (TMZipFile zfile in zipFiles) {
          appLenght += zfile.TMFile.File.Length;
          zipLenght += zfile.ZipFile.Length;
        }

        long percent = 100 - zipLenght / (appLenght / 100);
        Console.WriteLine(string.Format("Report: Files={0}, Compress={1}%", zipFiles.Count, percent));

      } catch (Exception ex) {
        errors.Add("Application Error - " + ex.Message);
        
      } finally {
        if (StopBuild != null) 
          StopBuild(this, new BuilderEventArgs(project));

        foreach (string s in errors) {
          Console.WriteLine(string.Format("Error: {0}", s));
        }
        Console.WriteLine(string.Format("----- Build complete (errors - {1}) -----", project.Name, errors.Count));
        _project = null;
      }
    }
    #endregion

    #region private void Compile(List<TMZipFile> zipFiles, TMFolder rootFolder, DirectoryInfo dir, List<string> errors)
    private void Compile(List<TMZipFile> zipFiles, TMFolder rootFolder, DirectoryInfo dir, List<string> errors){

      foreach (TMFolder folder in rootFolder.Folders) {
        DirectoryInfo rootDir = new DirectoryInfo(Path.Combine(dir.FullName, folder.Name));
        rootDir.Create();
        this.Compile(zipFiles, folder, rootDir, errors);
      }

      foreach (TMFile tmFile in rootFolder.Files) {
        FileInfo file = tmFile.File;

        if (!file.Exists) {
          errors.Add(string.Format("File not found: {0}", file.FullName));
          continue;
        }

        FileInfo zipFile = new FileInfo(Path.Combine(dir.FullName, file.Name + ".zip"));
        zipFiles.Add(new TMZipFile(zipFile, tmFile));

        OnCompressFile(new ZipEventArgs(file, zipFile, 0, file.Length));

        string message = string.Format("Compression: {0}", file.FullName);
        Console.Write(message);
        try {
          using (ZipOutputStream s = new ZipOutputStream(File.Create(zipFile.FullName))) {

            s.SetLevel(9);

            byte[] buffer = new byte[4096];

            ZipEntry entry = new ZipEntry(file.Name);

            entry.DateTime = file.LastWriteTime;
            entry.Size = file.Length;

            s.PutNextEntry(entry);

            DateTime limitTime = DateTime.Now;

            using (FileStream fs = File.OpenRead(file.FullName)) {

              int sourceBytes;
              do {
                sourceBytes = fs.Read(buffer, 0, buffer.Length);
                s.Write(buffer, 0, sourceBytes);

                if (DateTime.Now.Ticks - limitTime.Ticks > 10000000L) {
                  limitTime = DateTime.Now;
                  OnCompressFile(new ZipEventArgs(file, zipFile, fs.Position, file.Length));
                }

              } while (sourceBytes > 0);
            }
            s.Finish();
            s.Close();
          }
          Console.WriteLine(" - ok");
        } catch (Exception ex) {

          string error = string.Format(" - error: {0}", ex.Message);
          errors.Add(message + error);
          Console.WriteLine(error);

        } finally {
          OnCompressFile(new ZipEventArgs(file, zipFile, 0, file.Length));
          OnProgress(new BuilderProgressEventArgs(_project, ++_current, _total));
        }
      }
    }
    #endregion

    #region private void CompileInfo(List<TMZipFile> zipFiles, DirectoryInfo outputDir)
    private void CompileInfo(List<TMZipFile> zipFiles, DirectoryInfo outputDir) {
      FileInfo file = new FileInfo(Path.Combine(outputDir.FullName, Configure.VersionNumberFileName));
      FileInfo fileApp = new FileInfo(Path.Combine(_project.AppUpdateDirectory.FullName, Configure.VersionNumberFileName));

      if (file.Exists)
        file.Delete();

      File.AppendAllText(file.FullName, _project.Version.ToString());
      File.Copy(file.FullName, fileApp.FullName, true);
      
      List<string> removed = new List<string>();

      DirectoryInfo versionsDir = new DirectoryInfo(Path.Combine(outputDir.FullName, Configure.VersionUserInfoDirectoryName));
      DirectoryInfo versionsDirApp = new DirectoryInfo(Path.Combine(_project.AppUpdateDirectory.FullName, Configure.VersionUserInfoDirectoryName));

      versionsDir.Create();
      versionsDirApp.Create();

      for (int i = 1; i <= _project.Version; i++) {
        OnProgress(new BuilderProgressEventArgs(_project, i, _project.Version));

        VersionInfo version = _project.OpenVersion(i);
        removed.AddRange(version.Modify.FilesRemoved.ToArray());

        FileInfo fileUserInfo = new FileInfo(Path.Combine(versionsDir.FullName, string.Format(Configure.VersionUserInfoFileName, i)));
        FileInfo fileUserInfoApp = new FileInfo(Path.Combine(versionsDirApp.FullName, string.Format(Configure.VersionUserInfoFileName, i)));

        if (fileUserInfo.Exists)
          fileUserInfo.Delete();

        File.AppendAllText(fileUserInfo.FullName, version.InfoHtml, Encoding.Unicode);
        File.Copy(fileUserInfo.FullName, fileUserInfoApp.FullName, true);
      }

      string str = "Format=1.0,ProductVersion=" + _project.Version.ToString() + Environment.NewLine;

      List<string> sa = new List<string>();
      foreach (TMZipFile zfile in zipFiles) {
        sa.Clear();
        sa.Add("+");
        sa.Add(zfile.TMFile.FullName);
        sa.Add(zfile.TMFile.Version.ToString());
        sa.Add(zfile.ZipFile.Length.ToString());
        sa.Add(zfile.TMFile.File.Length.ToString());

        str += string.Join(",", sa.ToArray()) + Environment.NewLine;
      }
      foreach (string dfile in removed) {
        sa.Clear();
        sa.Add("-");
        sa.Add(dfile);
        str += string.Join(",", sa.ToArray()) + Environment.NewLine;
      }

      FileInfo fileUpdate = new FileInfo(Path.Combine(outputDir.FullName, Configure.UpdateFileListName));
      FileInfo fileUpdateApp = new FileInfo(Path.Combine(_project.AppUpdateDirectory.FullName, Configure.UpdateFileListName));

      if (fileUpdate.Exists)
        fileUpdate.Delete();
      File.AppendAllText(fileUpdate.FullName, str);
      File.Copy(fileUpdate.FullName, fileUpdateApp.FullName, true);
    }
    #endregion
  }
}
