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

namespace MD4_app.Controls
{
    /// <summary>
    /// Логика взаимодействия для EnhancedTextBox.xaml
    /// </summary>
    public partial class EnhancedTextBox : TextBox
    { 
        public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.Register(
            "HeaderContentProperty",
            typeof(TextBlock),
            typeof(EnhancedTextBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, HeaderContentChanged)
        );

        public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register(
           "HeaderTextProperty",
           typeof(string),
           typeof(EnhancedTextBox),
           new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, HeaderTextChanged)
        );

        public TextBlock HeaderContent
        {
            get => (TextBlock)GetValue(HeaderContentProperty);
            set => SetValue(HeaderContentProperty, value);
        }
        public string HeaderText
        {
            get => ((TextBlock)GetValue(HeaderContentProperty)).Text;
            set => SetValue(HeaderContentProperty, new TextBlock() { Text = value });
        }



        public EnhancedTextBox()
        {
            InitializeComponent();
        }


        private static void HeaderContentChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (source is EnhancedTextBox control)
                control.HeaderContent = (TextBlock)e.NewValue;
        }
        private static void HeaderTextChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (source is EnhancedTextBox control)
                control.HeaderText = (string)e.NewValue;
        }

        private void Control_MouseEnter(object sender, MouseEventArgs e)
        {
            
        }

        private void Control_MouseLeave(object sender, MouseEventArgs e)
        {

        }
    }
}
