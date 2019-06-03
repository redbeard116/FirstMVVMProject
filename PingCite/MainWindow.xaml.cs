using System.Windows;
using System.Threading;
using System;
using System.Net;

namespace PingSite
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel();

        }
    }
}
