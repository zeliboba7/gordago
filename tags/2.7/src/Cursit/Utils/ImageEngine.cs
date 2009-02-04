/**
* @package Gordago
* @copyright Copyright (C) 2005-2009 Gordago. All rights reserved.
* @license http://www.gnu.org/copyleft/gpl.html GNU/GPL
*/
using System;
using System.Collections;
using System.Drawing;

namespace Cursit.Utils {
	/// <summary>
	/// Хранитель картинок
	/// </summary>
	public class ImageEngine {
		public static ImageCollection Images = new ImageCollection();
	}
	public class ImageCollection{
		private ImageItem[] _images; 
		internal ImageCollection(){
			_images = new ImageItem[]{};
		}

		public Image CreateImage(string fileName, string name, string group){
			Image image = FindImage(name, group);
			if (image != null) return image;
			try{
				image = Image.FromFile(fileName);
			}catch(Exception){
				image = new Bitmap(1,1);
			}

			ArrayList al = new ArrayList(_images);
			al.Add(new ImageItem(image, name, group));
			this._images = (ImageItem[])al.ToArray(typeof(ImageItem));
			ReindexGroup(group);
			return image;
		}

		#region private Image FindImage(string name, string group)
		private Image FindImage(string name, string group){
			foreach (ImageItem item in this._images){
				if (item.Name == name && item.Group == group)
					return item.Image;
			}
			return null;
		}
		#endregion

		#region public Image GetImage(string name, string group)
		public Image GetImage(string name, string group){
			Image image = this.FindImage(name, group);
			if (image != null) return image;
			return new Bitmap(1,1);
		}
		#endregion

		public Image[] GetImagesGroup(string group){
			ArrayList al = new ArrayList();
			foreach (ImageItem item in this._images){
				if (item.Group == group)
					al.Add(item.Image);
			}
			return (Image[])al.ToArray(typeof(Image));
		}

		public System.Windows.Forms.ImageList CreateImageListFromGroup(string group){
			System.Windows.Forms.ImageList imglst = new System.Windows.Forms.ImageList();
			foreach (ImageItem item in this._images){
				if (item.Group == group)
					imglst.Images.Add(item.Image);
			}
			return imglst;
		}

		/// <summary>
		/// Переиндексация в группе
		/// </summary>
		private void ReindexGroup(string group){
			int index=0;
			foreach (ImageItem item in this._images){
				if (item.Group == group)
					item.IndexGroup = index++;
			}
		}

		public int GetIndexGroup(string name, string group){
			int index = -1;
			foreach (ImageItem item in this._images){
				if (item.Name == name && item.Group == group)
					return item.IndexGroup;
			}
			return index;
		}

		#region private new int GetHashCode()
		private new int GetHashCode() {
			return base.GetHashCode ();
		}
		#endregion
		#region private new bool Equals(object obj)
		private new bool Equals(object obj) {
			return base.Equals (obj);
		}
		#endregion

		#region internal class ImageItem
		internal class ImageItem{
			private Image _image;
			private string _name;
			private string _group;
			private int _indexGroup;

			public ImageItem(Image image, string name, string group){
				_image = image;
				_name = name;
				_group = group;
			}

			#region public Image Image
			public Image Image{
				get{return this._image;}
			}
			#endregion

			#region public string Name
			public string Name{
				get{return this._name;}
			}
			#endregion

			#region public string Group
			public string Group{
				get{return this._group;}
			}
			#endregion

			#region public int IndexGroup
			public int IndexGroup{
				get{return _indexGroup;}
				set{_indexGroup = value;}
			}
			#endregion
		}
		#endregion
	}
}
