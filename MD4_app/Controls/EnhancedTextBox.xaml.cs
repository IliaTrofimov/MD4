﻿using System;
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
        public static readonly RoutedEvent TextClearEvent = EventManager.RegisterRoutedEvent(
            "TextClear", 
            RoutingStrategy.Bubble, 
            typeof(RoutedEventHandler), 
            typeof(EnhancedTextBox)
        );

        public static readonly DependencyProperty ClearButtonVisibileProperty = DependencyProperty.Register(
            "ClearButtonVisibileProperty",
            typeof(bool),
            typeof(EnhancedTextBox),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender, ClearButtonVisibileChanged)
        );

        public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(
            "PlaceholderTextProperty",
            typeof(string),
            typeof(EnhancedTextBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, PlaceholderTextChanged)
        );

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

        public bool ClearButtonVisibile
        {
            get => (bool)GetValue(ClearButtonVisibileProperty);
            set => SetValue(ClearButtonVisibileProperty, value);
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



        public EnhancedTextBox()
        {
            InitializeComponent();
        }


        private static void ClearButtonVisibileChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (source is EnhancedTextBox control)
                control.ClearButtonVisibile = (bool)e.NewValue;
        }
        private static void PlaceholderTextChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (source is EnhancedTextBox control)
                control.PlaceholderText = (string)e.NewValue;
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


        protected void OnTextClear()
        {
            RaiseTextClearEvent();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Text = "";
            RaiseTextClearEvent();
        }
    }
}
