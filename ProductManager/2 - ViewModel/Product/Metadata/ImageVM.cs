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
        private List<string> _deletedImages;

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
            LoadImage(_fileName.Value);
        }

        public void UndoChanges()
        {
            _deletedImages.Add(_fileName.Value);
            _fileName.UndoChanges();
            LoadImage(_fileName.Value);
            _deletedImages.Remove(_fileName.Value);
        }

        public void AcceptChanges()
        {
            _fileName.AcceptChanges();

            if (_deletedImages.Count != 0)
            {
                foreach (string item in _deletedImages)
                {
                    FilesController fc = new FilesController(FilesController.FileType.Image, item);
                    FilesController.Delete(fc);
                }

                _deletedImages.Clear();
            }
        }

        /// <summary>
        /// Ladet und übersetzt das angegebene Bild in die <see cref="CurrentImage"/> Eigenschaft.
        /// </summary>
        /// <param name="filename">Dateiname des Ursprungbildes, bzw. aus der Datenbank gespeicherten Wertes</param>
        /// <param name="path">Nur anzugeben wenn die Datei sich auserhalb vom Anwendungsordner befindet</param>
        public void LoadImage(string filename, string path = null)
        {
            // Pfad wird nur in der View definiert
            if (!string.IsNullOrEmpty(path))
            {
                // Wenn zuvor noch kein Bild definiert war
                if (string.IsNullOrEmpty(_fileName.Value))
                {
                    SaveAsCurrentImage(filename, path);
                }
                // Zuvor definiertes Bild wird zum löschen markiertt
                else
                {
                    _deletedImages.Add(_fileName.Value);
                    SaveAsCurrentImage(filename, path);
                }

                BuildImage();
            }
            // Pfad wird NICHT definiert und kommt vom Konstruktor
            else
            {
                // Zur sicherheit alles auf null setzen, damit die View sich aktualisiert.
                if (string.IsNullOrEmpty(_fileName.Value))
                {
                    FileName.Value = null;
                    CurrentImage = null;
                    return;
                }
                else
                {
                    if (File.Exists(Properties.IMAGE_PATH + _fileName.Value))
                    {
                        BuildImage();
                    }
                    else
                    {
                        CurrentImage = null;
                        _fileName.Value = null;
                        AcceptChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Entfernt das Aktuelle Bild, und markiert sie zum Löschen
        /// </summary>
        public void RemoveCurrentImage()
        {
            _deletedImages.Add(_fileName.Value);
            FileName.Value = null;
            CurrentImage = null;
        }

        /// <summary>
        /// Übersetzt die Bilddatei in eine <see cref="BitmapImage"/>.
        /// </summary>
        private void BuildImage()
        {
            try
            {
                CurrentImage = new BitmapImage();
                CurrentImage.BeginInit();
                CurrentImage.UriSource = new Uri(Properties.IMAGE_PATH + _fileName.Value);
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