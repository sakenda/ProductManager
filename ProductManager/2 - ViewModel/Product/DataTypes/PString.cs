using System;

namespace ProductManager.ViewModel
{
    public class PString : ViewModelBase, IComparable
    {
        private string _currentValue;
        private string _originalValue;
        private bool _hasChanged;

        /// <summary>
        /// Öffentlicher Zugriff auf Status des Strings. Gibt true bei veränderung des Wertes zurück.
        /// </summary>
        public bool HasChanged
        {
            get => _hasChanged;
            set => SetProperty(ref _hasChanged, value);
        }

        /// <summary>
        /// Öffentlicher zugriff auf den Inhalt des Strings
        /// </summary>
        public string Value
        {
            get => _currentValue;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = null;
                }

                SetProperty(ref _currentValue, value);
                HasChanged = _currentValue != _originalValue;
            }
        }

        public PString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = null;
            }

            _currentValue = value;
            _originalValue = value;
        }

        /// <summary>
        /// Öffentlicher Zugriff, zum Speichern des bearbeiteten Wertes.
        /// </summary>
        public void AcceptChanges()
        {
            _originalValue = _currentValue;
            HasChanged = false;
        }

        /// <summary>
        /// Öffentlicher Zugriff, zum zurücksetzten der bearbeitung.
        /// </summary>
        public void UndoChanges()
        {
            Value = _originalValue;
            HasChanged = false;
        }

        /// <summary>
        /// ICompareable implementierung.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (ReferenceEquals(this, obj)) return 0;

            PString item = obj as PString;
            return string.Compare(_currentValue, item._currentValue);
        }
    }
}