using ProductManager.Model.Product.Metadata;
using ProductManager.ViewModel.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace ProductManager.ViewModel.Product.Metadata
{
    public class ImageVM : ViewModelBase
    {
        private ImageModel _imageModel;

        private List<string> _deletedImages;
        private StringVM _path;
        private BitmapImage _image;
        private bool _changed;

        public StringVM Path => _path;
        public BitmapImage Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }
        public bool Changed
        {
            get => _changed;
            set => SetProperty(ref _changed, value);
        }

        public ImageVM(ImageModel imageModel)
        {
            _deletedImages = new List<string>();

            if (imageModel != null)
            {
                _imageModel = imageModel;
                InitializeFields();
            }
            else
            {
                _imageModel = new ImageModel();
                InitializeFields();
            }

            _path.PropertyChanged += Image_PropertyChanged;
        }

        private void Image_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_path.HasChanged)
            {
                Changed = true;
                LoadImage(_path.Value);
            }
            else
            {
                Changed = false;
            }
        }

        private void InitializeFields()
        {
            _path = new StringVM(_imageModel.Path);
            LoadImage(_path.Value);
        }

        public void UndoChanges()
        {
            _path.UndoChanges();
        }

        public void AcceptChanges()
        {
            _path.AcceptChanges();
            SaveCurrentImage();

            if (_deletedImages.Count != 0)
            {
                foreach (string item in _deletedImages)
                {
                    FilesController.Delete(item);
                }

                _deletedImages.Clear();
            }
        }

        /// <summary>
        /// Lädt die Datei in ein <see cref="BitmapImage"/>.
        /// </summary>
        /// <param name="path"></param>
        public void LoadImage(string path)
        {
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                if (_path.Value == null)
                {
                    Path.Value = path;
                }
                else
                {
                    _deletedImages.Add(_path.Value);
                    Path.Value = path;
                }

                try
                {
                    Image = new BitmapImage();
                    Image.BeginInit();
                    Image.UriSource = new Uri(path);
                    Image.CacheOption = BitmapCacheOption.OnLoad;
                    Image.EndInit();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error initializing Image. ID: {this._imageModel.ID}, Path: {this._path.Value}, Message: {ex.Message}");
                }
            }
            else
            {
                Image = null;
                _path.Value = null;
                AcceptChanges();
            }
        }

        /// <summary>
        /// Nimmt den ursprünglichen Pfad Der Bilddatei und speichert eine Kopie davon
        /// in den Anwendungsordner.
        /// </summary>
        /// <param name="path">Der Pfad der Originaldatei</param>
        /// <returns></returns>
        public void SaveCurrentImage()
        {
            string filename = Guid.NewGuid().ToString();

            var fc = new FilesController(_path.Value, FilesController.Type.Image, filename);

            _path.Value = FilesController.Save(fc);
        }

        /// <summary>
        /// Löscht die angehängte Datei
        /// </summary>
        public void RemoveCurrentImage()
        {
            if (_path.Value != null)
            {
                _deletedImages.Add(_path.Value);
            }

            Path.Value = null;
        }
    }
}