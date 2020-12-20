using ProductManager.Model.Product.Metadata;
using ProductManager.ViewModel.Controller;
using System;
using System.ComponentModel;

namespace ProductManager.ViewModel.Product.Metadata
{
    public class ImageVM : ViewModelBase
    {
        private ImageModel _imageModel;
        private StringVM _path;
        private bool _changed;

        public StringVM Path => _path;
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
                _path.HasChanged = true;
            }

            _path.PropertyChanged += Image_PropertyChanged;
        }

        private void Image_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_path.HasChanged)
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
            _path = new StringVM(_imageModel.Path);
        }

        public void UndoChanges()
        {
            _path.UndoChanges();
        }

        public void AcceptChanges()
        {
            _path.AcceptChanges();
        }

        /// <summary>
        /// Nimmt den ursprünglichen Pfad Der Bilddatei und speichert eine Kopie davon
        /// in den Anwendungsordner.
        /// </summary>
        /// <param name="originPath">Der Pfad der Originaldatei</param>
        /// <returns></returns>
        public void SaveImage(string originPath)
        {
            string filename = Guid.NewGuid().ToString();
            var fc = new FilesController(originPath, FilesController.Type.Image, filename);
            _path.Value = FilesController.Save(fc);
        }

        /// <summary>
        /// Löscht die angehängte Datei
        /// </summary>
        public void DeleteImage()
        {
            FilesController.Delete(_path.Value);
        }
    }
}