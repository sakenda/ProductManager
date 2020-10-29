using System;

namespace ProductManager.Models
{
    public delegate void InvalidMeasureEventHandler(object sender, InvalidMeasureEventArgs e);

    public class InvalidMeasureEventArgs : EventArgs
    {
        public InvalidMeasureEventArgs(object invalidMeasure, string propertyName, Exception error)
        {
            _InvalidMeasure = invalidMeasure;
            _Error = error;
            if (string.IsNullOrWhiteSpace(propertyName) || propertyName == null)
                _PropertyName = "[unknown]";
            else
                _PropertyName = propertyName;
        }

        private object _InvalidMeasure;
        private string _PropertyName;
        private Exception _Error;

        public object InvalidMeasure => _InvalidMeasure;
        public string PropertyName => _PropertyName;
        public Exception Error => _Error;
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