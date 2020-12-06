using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.ViewModel
{
    public class PDouble : ViewModelBase
    {
        private double _currentValue;
        private double _originalValue;
        private bool _hasChanged;

        /// <summary>
        /// Öffentlicher Zugriff auf Status des Wertes. Gibt true bei veränderung des Wertes zurück.
        /// </summary>
        public bool HasChanged
        {
            get => _hasChanged;
            set => SetProperty(ref _hasChanged, value);
        }

        /// <summary>
        /// Öffentlicher zugriff auf den Inhalt des Integers
        /// </summary>
        public double Value
        {
            get => _currentValue;
            set
            {
                SetProperty(ref _currentValue, value);
                HasChanged = _currentValue != _originalValue;
            }
        }

        public PDouble(double value)
        {
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

            PDouble item = obj as PDouble;
            return _currentValue.CompareTo(item._currentValue);
        }
    }
}