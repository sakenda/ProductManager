using ProductManager.Model.Product.Metadata;
using ProductManager.ViewModel.Controller;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace ProductManager.ViewModel.Product.Metadata
{
    public class ImageVM : ViewModelBase
    {
        private string _originPath;

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
            ChangeImage(_fileName.Value);
        }

        public void UndoChanges()
        {
            _fileName.UndoChanges();
            ChangeImage(_fileName.Value);
        }

        public void AcceptChanges()
        {
            if (File.Exists(_originPath + "/" + _fileName.Value))
            {
                SaveAsCurrentImage(_fileName.Value, _originPath);
            }
            _fileName.AcceptChanges();
            _imageModel.FileName = _fileName.Value;
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
                _originPath = path;

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
                    RemoveCurrentImage();
                    AcceptChanges();
                }
            }
        }

        /// <summary>
        /// Entfernt das Aktuelle Bild.
        /// </summary>
        public void RemoveCurrentImage()
        {
            if (File.Exists(Properties.IMAGE_PATH + _fileName.Value))
            {
                FilesController fc = new FilesController(FilesController.FileType.ImageArchived, _fileName.Value, Properties.IMAGE_PATH);
                FilesController.Move(fc);
            }

            _fileName.Value = null;
            CurrentImage = null;
        }

        /// <summary>
        /// Übersetzt die Bilddatei in eine <see cref="BitmapImage"/>.
        /// </summary>
        private void BuildImage(string path = null)
        {
            if (path == null) path = Properties.IMAGE_PATH;
            else path += "/";

            try
            {
                CurrentImage = new BitmapImage();
                CurrentImage.BeginInit();
                CurrentImage.UriSource = new Uri(path + _fileName.Value);
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
        /// Der Dateiname der Kopie wird im aktuellen <see cref="FileName"/> gespeichert.
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