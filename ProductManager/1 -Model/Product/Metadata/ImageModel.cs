﻿namespace ProductManager.Model.Product.Metadata
{
    public class ImageModel
    {
        private int? _id;
        private string _fileName;

        public int? ID => _id;
        public string FileName => _fileName;

        public ImageModel()
        {
            _id = -1;
        }
        public ImageModel(string name) : this()
        {
            _fileName = name;
        }

        public void SetID(int? id)
        {
            _id = id;
        }
    }
}