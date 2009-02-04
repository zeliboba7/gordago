/**
* @version $Id: LURootFolderType.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.LiteUpdate
{

  public enum LURootFolderType:uint {
    Application = 0,
    ApplicationData = 1,
    Cookies = 2,
    Desktop = 3,
    LocalApplicationData = 4,
    MyDocuments = 5,
    MyMusic = 6,
    MyPictures = 7,
    Personal = 8,
    ProgramFiles = 9,
    Programs = 10,
    Recent = 11,
    SendTo = 12,
    StartMenu = 13,
    Startup = 14,
    System = 15,
    Templates = 16
  }
}
