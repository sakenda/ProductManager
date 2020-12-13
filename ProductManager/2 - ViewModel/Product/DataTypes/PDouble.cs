using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.ViewModel
{
    public class PDouble : ViewModelBase, IComparable
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
            PDouble value = obj as PDouble;
            if (value == null)
                throw new ArgumentException("Double erwartet");

            if (_currentValue > value._currentValue) return 1;
            if (_currentValue == value._currentValue) return 0;

            return -1;
        }
    }
}