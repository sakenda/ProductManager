using System;

namespace ProductManager.Models
{
    public delegate void InvalidMeasureEventHandler(object sender, InvalidMeasureEventArgs e);

    public class InvalidMeasureEventArgs : EventArgs
    {
        public InvalidMeasureEventArgs(string invalidStringMeasure, string propertyName, Exception error)
        {
            _InvalidStringMeasure = invalidStringMeasure;
            _Error = error;
            if (propertyName == "" || propertyName == null)
                _PropertyName = "[unknown]";
            else
                _PropertyName = propertyName;
        }
        public InvalidMeasureEventArgs(int invalidIntMeasure, string propertyName, Exception error)
        {
            _InvalidIntMeasure = invalidIntMeasure;
            _Error = error;
            if (propertyName == "" || propertyName == null)
                _PropertyName = "[unknown]";
            else
                _PropertyName = propertyName;
        }
        public InvalidMeasureEventArgs(double invalidDoubleMeasure, string propertyName, Exception error)
        {

            _InvalidDoubleMeasure = invalidDoubleMeasure;
            _Error = error;
            if (propertyName == "" || propertyName == null)
                _PropertyName = "[unknown]";
            else
                _PropertyName = propertyName;
        }

        private int _InvalidIntMeasure;
        private double _InvalidDoubleMeasure;
        private string _InvalidStringMeasure;
        private string _PropertyName;
        private Exception _Error;

        public int InvalidIntMeasure
        {
            get => _InvalidIntMeasure;
        }
        public double InvalidDoubleMeasure
        {
            get => _InvalidDoubleMeasure;
        }
        public string InvalidStringMeasure
        {
            get => _InvalidStringMeasure;
        }
        public string PropertyName
        {
            get => _PropertyName;
        }
        public Exception Error
        {
            get => _Error;
        }
    }

    [Serializable]
    public class InvalidMeasureException : Exception
    {
        public InvalidMeasureException() { }
        public InvalidMeasureException(string message) : base(message) { }
        public InvalidMeasureException(string message, Exception inner) : base(message, inner) { }
        protected InvalidMeasureException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}