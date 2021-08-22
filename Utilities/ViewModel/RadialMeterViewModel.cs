using System.ComponentModel;
using Utilities.Model;

namespace Utilities.ViewModel
{
    public class RadialMeterViewModel : IAutoNotifyPropertyChanged
    {
        public RadialMeterViewModel()
        {
            Value = 50;
            Maximum = 100;
            UpdateValueText();
            ValueSubtext = "Sub";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _valueText = string.Empty;
        public string ValueText
        {
            get { return _valueText; }
            set { NotifyHelpers.SetProperty(this, ref _valueText, value); }
        }

        private int _value = 0;
        public int Value
        {
            get { return _value; }
            set { NotifyHelpers.SetProperty(this, ref _value, value); }
        }

        private int _maximum = 0;
        public int Maximum
        {
            get { return _maximum; }
            set { NotifyHelpers.SetProperty(this, ref _maximum, value); }
        }

        private string _valueSubtext = string.Empty;
        public string ValueSubtext
        {
            get { return _valueSubtext; }
            set { NotifyHelpers.SetProperty(this, ref _valueSubtext, value); }
        }

        public void RaisePropertyChanged(string propertyName)
        {
            if (propertyName == nameof(Value) || propertyName == nameof(Maximum))
            {
                UpdateValueText();
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateValueText()
        {
            ValueText = $"{Value}/{Maximum}";
        }
    }
}
