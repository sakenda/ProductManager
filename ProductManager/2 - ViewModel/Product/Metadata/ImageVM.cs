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
        private List<string> _archivedImages;

        private ImageModel _imageModel;
        private StringVM _fileName;
        private BitmapImage _currentImage;
        private bool _changed;

        public StringVM FileName => _fileName;
        public BitmapImage CurrentImage
        {
            get => _currentImage;
            set => SetProperty(ref _currentImage, value);
        }
        public bool Changed
        {
            get => _changed;
            set => SetProperty(ref _changed, value);
        }

        public ImageVM(ImageModel imageModel)
        {
            _archivedImages = new List<string>();

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

            ChangeImage(_fileName.Value);
            _fileName.PropertyChanged += Image_PropertyChanged;
        }

        private void Image_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_fileName.HasChanged)
            {
                Changed = true;
            }
            else
            {
                Changed = false;
            }
        }

        private void InitializeFields()
        {
            _fileName = new StringVM(_imageModel.FileName);
            ChangeImage(_fileName.Value);
        }

        public void UndoChanges()
        {
            _archivedImages.Add(_fileName.Value);
            _fileName.UndoChanges();
            ChangeImage(_fileName.Value);
            _archivedImages.Remove(_fileName.Value);
        }

        public void AcceptChanges()
        {
            _fileName.AcceptChanges();

            if (_archivedImages.Count != 0)
            {
                foreach (string item in _archivedImages)
                {
                    FilesController fc = new FilesController(FilesController.FileType.Image, item);
                    FilesController.Delete(fc);
                }

                _archivedImages.Clear();
            }
        }

        /// <summary>
        /// Ladet und übersetzt das angegebene Bild in die <see cref="CurrentImage"/> Eigenschaft.
        /// </summary>
        /// <param name="filename">Dateiname des Ursprungbildes, bzw. aus der Datenbank gespeicherten Wertes</param>
        /// <param name="path">Nur anzugeben wenn die Datei sich auserhalb vom Anwendungsordner befindet</param>
        public void ChangeImage(string filename, string path = null)
        {
            // Daten kommen vom View
            if (!string.IsNullOrEmpty(path))
            {
                RemoveCurrentImage();
                _fileName.Value = filename;
                BuildImage(path);
            }
            // Daten kommen von DB
            else
            {
                // File Check
                if (File.Exists(Properties.IMAGE_PATH + _fileName.Value))
                {
                    BuildImage();
                }
                // Wenn Datei nicht existiert, alles auf NULL setzten
                else
                {
                    CurrentImage = null;
                    _fileName.Value = null;
                    AcceptChanges();
                }
            }
        }

        /// <summary>
        /// Entfernt das Aktuelle Bild, und markiert sie zum Löschen
        /// </summary>
        public void RemoveCurrentImage()
        {
            if (_imageModel.ID > 0 && !string.IsNullOrEmpty(_fileName.Value))
            {
                _archivedImages.Add(_fileName.Value);
            }

            FileName.Value = null;
            CurrentImage = null;
        }

        /// <summary>
        /// Übersetzt die Bilddatei in eine <see cref="BitmapImage"/>.
        /// </summary>
        private void BuildImage(string path = null)
        {
            if (path == null) path = Properties.IMAGE_PATH;

            try
            {
                CurrentImage = new BitmapImage();
                CurrentImage.BeginInit();
                CurrentImage.UriSource = new Uri(path + "/" + _fileName.Value);
                CurrentImage.CacheOption = BitmapCacheOption.OnLoad;
                CurrentImage.EndInit();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error initializing Image. ID: {this._imageModel.ID}, Path: {Properties.IMAGE_PATH + this._fileName.Value}, Message: {ex.Message}");
            }
        }

        /// <summary>
        /// Nimmt das ursprungsbild und speichert eine Kopie davon in den Anwendungsordner.
        /// Der Dateiname der kopie wird im aktuellen <see cref="FileName"/> gespeichert.
        /// </summary>
        /// <param name="fileName">Der Dateiname der Originaldatei</param>
        /// <param name="path">Der Pfad der Originaldatei</param>
        private void SaveAsCurrentImage(string fileName, string path)
        {
            string newFilename = Guid.NewGuid().ToString();

            var fc = new FilesController(FilesController.FileType.Image, newFilename, path + "/" + fileName);

            this._fileName.Value = FilesController.Save(fc);
        }
    }
}