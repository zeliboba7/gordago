/**
* @version $Id: ITabbedDocument.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.FO
{
  using System;
  using System.IO;

  interface ITabbedDocument {
    string GUID { get;}
    void LoadProperties(FileInfo file);
    void SaveProperties(FileInfo file);
  }

  interface ITabbedFileDocument {
    event EventHandler ContentChanged;
    event EventHandler ContentSaved;
    FileInfo ContentFile { get;}
    bool IsContentSaved { get;}
    void SaveContent();
    void SaveAsContent(FileInfo file);
  }
}
