/**
* @version $Id: PropertyChangedEventHandler.cs 3 2009-02-03 12:52:09Z AKuzmin $
* @package Gordago
* @copyright Copyright (C) 2008 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
namespace Gordago.Core
{
  using System;

  public delegate void EasyPropertiesChangedEventHandler (object sender, EasyPropertiesChangedEventArgs e);

  public class EasyPropertiesChangedEventArgs:EventArgs{

    private EasyPropertiesNode _properties;

    private object _oldValue, _newValue;

    public EasyPropertiesChangedEventArgs(EasyPropertiesNode properties, object oldValue, object newValue):base() {
      _oldValue = oldValue;
      _newValue = newValue;
      _properties = properties;
    }

    #region public EasyPropertiesNode EasyProperties
    public EasyPropertiesNode EasyProperties {
      get { return _properties; }
    }
    #endregion

    #region public object OldValue
    public object OldValue {
      get { return _oldValue; }
    }
    #endregion

    #region public object NewValue
    public object NewValue {
      get { return this._newValue; }
    }
    #endregion
  }
}
