using Microsoft.Win32;
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
    /// Логика взаимодействия для EnhancedFileInput.xaml
    /// </summary>
    public partial class EnhancedFileInput : TextBox
    {
        public static readonly RoutedEvent FileSelectedEvent = EventManager.RegisterRoutedEvent(
            "FileSelected",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(EnhancedFileInput)
        );

        public static readonly DependencyProperty ClearButtonVisibileProperty = DependencyProperty.Register(
           "ClearButtonVisibileProperty",
           typeof(bool),
           typeof(EnhancedFileInput),
           new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender, ClearButtonVisibileChanged)
       );

        public static readonly RoutedEvent TextClearEvent = EventManager.RegisterRoutedEvent(
            "TextClear", 
            RoutingStrategy.Bubble, 
            typeof(RoutedEventHandler), 
            typeof(EnhancedFileInput)
        );

        public static readonly DependencyProperty FilesFilterProperty = DependencyProperty.Register(
            "FilesFilterProperty",
            typeof(string),
            typeof(EnhancedFileInput),
            new FrameworkPropertyMetadata("Любой файл|*.*", FrameworkPropertyMetadataOptions.AffectsRender, FilesFilterChanged)
        );

        public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(
            "PlaceholderTextProperty",
            typeof(string),
            typeof(EnhancedFileInput),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, PlaceholderTextChanged)
        );

        public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.Register(
            "HeaderContentProperty",
            typeof(TextBlock),
            typeof(EnhancedFileInput),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, HeaderContentChanged)
        );

        public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register(
           "HeaderTextProperty",
           typeof(string),
           typeof(EnhancedFileInput),
           new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, HeaderTextChanged)
        );


        public bool ClearButtonVisibile
        {
            get => (bool)GetValue(ClearButtonVisibileProperty);
            set => SetValue(ClearButtonVisibileProperty, value);
        }

        public event RoutedEventHandler FileSelected
        {
            add { AddHandler(FileSelectedEvent, value); }
            remove { RemoveHandler(FileSelectedEvent, value); }
        }
        void RaiseFileSelectedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(FileSelectedEvent);
            RaiseEvent(newEventArgs);
        }

        public event RoutedEventHandler TextClear
        {
            add { AddHandler(TextClearEvent, value); }
            remove { RemoveHandler(TextClearEvent, value); }
        }
        void RaiseTextClearEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(TextClearEvent);
            RaiseEvent(newEventArgs);
        }


        public string FilesFilter
        {
            get => (string)GetValue(FilesFilterProperty);
            set => SetValue(FilesFilterProperty, value);
        }
        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }
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



        public EnhancedFileInput()
        {
            InitializeComponent();
        }

        private static void ClearButtonVisibileChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (source is EnhancedFileInput control)
                control.ClearButtonVisibile = (bool)e.NewValue;
        }
        private static void FilesFilterChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (source is EnhancedFileInput control)
                control.FilesFilter = (string)e.NewValue;
        }
        private static void PlaceholderTextChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (source is EnhancedFileInput control)
                control.PlaceholderText = (string)e.NewValue;
        }
        private static void HeaderContentChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (source is EnhancedFileInput control)
                control.HeaderContent = (TextBlock)e.NewValue;
        }
        private static void HeaderTextChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (source is EnhancedFileInput control)
                control.HeaderText = (string)e.NewValue;
        }


        protected void OnTextClear()
        {
            Text = "";
        }

        protected void OnFileSelected()
        {
            RaiseFileSelectedEvent();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Text = "";
            RaiseTextClearEvent();
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new() 
            { 
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = FilesFilter,
                Multiselect = false
            };
            if (dialog.ShowDialog() == true)
            {
                Text = dialog.FileName;
                RaiseFileSelectedEvent();
            }
        }

    }
}
