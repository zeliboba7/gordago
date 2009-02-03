/**
* @version $Id: TicksFileMapData.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 CMSBrick. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Trader.Data
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.IO;

  class TicksFileMapData: FileReadWrite {

    const int INFO_SIZE = 4 + 8 + 64;
    const int FILED_SIZE = 4 + 4;
    const int VERSION = 2;

    private const int TIME_FRAME_SECOND = 86400;

    private string _symbolName;
    private int _version;
    private int _countTicks;

    public TicksFileMapData(FileInfo file)
      : base(file, INFO_SIZE, FILED_SIZE) {
    }

    #region public int CountTicks
    public int CountTicks {
      get { return _countTicks; }
      set { _countTicks = value; }
    }
    #endregion

    #region public string SymbolName
    public string SymbolName {
      get { return this._symbolName; }
      set { this._symbolName = value; }
    }
    #endregion

    #region protected override void OnLoadFileInfo()
    protected override void OnLoadFileInfo() {
      _version = Reader.ReadInt32();
      _symbolName = ReadString(8);
      Reader.ReadBytes(60);
      _countTicks = Reader.ReadInt32();
    }
    #endregion

    #region protected override void OnSaveFileInfo()
    protected override void OnSaveFileInfo() {
      Writer.Write(VERSION);
      Write(_symbolName, 8);
      Write(BarsFileData.COPYRATE, 60);
      Writer.Write(_countTicks);
    }
    #endregion

    #region public TicksMapItem Read(int index)
    public TicksMapItem Read(int index) {

      lock (Locked) {
        this.SeekReader(index);

        long time = this.ReadUnixTime();
        int cnttick = Reader.ReadInt32();

        return new TicksMapItem(time, cnttick);
      }
    }
    #endregion

    #region public void Write(TicksMapItem map, int index)
    public void Write(TicksMapItem map, int index) {
      lock (Locked) {
        this.CheckWriterOffset(index);
        Write(new DateTime(map.Time));
        Writer.Write(map.CountTick);
      }
    }
    #endregion

  }
}
