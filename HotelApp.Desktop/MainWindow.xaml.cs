using DataAccessLibrary.Data;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

namespace HotelApp.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDataBaseData _db;

        public MainWindow(IDataBaseData db)
        {
            InitializeComponent();
            LastNameBox.Focus();
            _db = db;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var bookings = _db.SerachBookings(LastNameBox.Text);
            BookingsListBox.ItemsSource = bookings;
            LastNameBox.Focus();
        }

        private void CheckInButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is int bookingId)
            {
                _db.CheckInGuest(bookingId);
                var bookings = _db.SerachBookings(LastNameBox.Text);
                BookingsListBox.ItemsSource = bookings;
            }
            LastNameBox.Focus();
        }
    }
}
