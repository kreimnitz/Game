using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Utilities.ViewModel;

namespace Utilities.View
{
    /// <summary>
    /// Interaction logic for RadialMeter.xaml
    /// </summary>
    public partial class RadialMeter : UserControl
    {
        public RadialMeter()
        {
            InitializeComponent();
            DataContext = new RadialMeterViewModel();
        }

        public RadialMeterViewModel ViewModel => DataContext as RadialMeterViewModel;

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value), typeof(int), typeof(RadialMeter), new PropertyMetadata(0));

        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            nameof(Maximum), typeof(int), typeof(RadialMeter), new PropertyMetadata(0));

        public int Maximum
        {
            get => (int)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public static readonly DependencyProperty ValueTextProperty = DependencyProperty.Register(
            nameof(ValueText), typeof(string), typeof(RadialMeter), new PropertyMetadata(string.Empty));

        public string ValueText
        {
            get => (string)GetValue(ValueTextProperty);
            set => SetValue(ValueTextProperty, value);
        }

        public static readonly DependencyProperty ValueSubtextTextProperty = DependencyProperty.Register(
            nameof(ValueSubtext), typeof(string), typeof(RadialMeter), new PropertyMetadata(string.Empty));

        public string ValueSubtext
        {
            get => (string)GetValue(ValueSubtextTextProperty);
            set => SetValue(ValueSubtextTextProperty, value);
        }
    }
}
