using CSharp2Projekt;
using DataLayer.Models;
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
using System.Windows.Shapes;

namespace DesktopAdmin
{
    /// <summary>
    /// Interaction logic for EditSchoolWindow.xaml
    /// </summary>
    public partial class EditSchoolWindow : Window
    {
        public School SchoolCpy { get; set; } = new School();

        public EditSchoolWindow(School School)
        {
            InitializeComponent();
            this.DataContext = this;


            SchoolCpy.Id = School.Id;
            SchoolCpy.Name = School.Name;
            SchoolCpy.Country = School.Country;
            SchoolCpy.City = School.City;
            SchoolCpy.Street = School.Street;
        }

        private async void UpravitBtn(object sender, RoutedEventArgs e)
        {
            using QueryBuilder queryBuilder = new QueryBuilder();

            if (string.IsNullOrEmpty(SchoolCpy.Name) || string.IsNullOrEmpty(SchoolCpy.Country) || string.IsNullOrEmpty(SchoolCpy.City) || string.IsNullOrEmpty(SchoolCpy.Street))
            {
                MessageBox.Show("Vyplňte všechny údaje");
                return;
            }

            await queryBuilder.UpdateAsync(SchoolCpy);

            DialogResult = true;
            Close();
        }
    }
}
