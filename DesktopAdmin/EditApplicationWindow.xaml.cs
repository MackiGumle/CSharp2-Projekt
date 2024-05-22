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
    /// Interaction logic for EditApplicationWindow.xaml
    /// </summary>
    public partial class EditApplicationWindow : Window
    {
        public Applications ApplicationsCpy { get; set; } = new Applications();
        public EditApplicationWindow(Applications applications)
        {
            InitializeComponent();
            this.DataContext = this;

            ApplicationsCpy.Id = applications.Id;
            ApplicationsCpy.Student = applications.Student;
            ApplicationsCpy.Date = applications.Date;
            ApplicationsCpy.Program1 = applications.Program1;
            ApplicationsCpy.Program2 = applications.Program2;
            ApplicationsCpy.Program3 = applications.Program3;
        }

        private async void UpravitBtn(object sender, RoutedEventArgs e)
        {
            using QueryBuilder queryBuilder = new QueryBuilder();

            if (ApplicationsCpy.Student < 1 || ApplicationsCpy.Program1 < 1)
            {
                MessageBox.Show("Vyplňte všechny údaje");
                return;
            }

            var student = await queryBuilder.SelectByIdAsync<Student>(ApplicationsCpy.Student);

            if (student == null)
            {
                MessageBox.Show("Student neexistuje");
                return;
            }

            var program1 = await queryBuilder.SelectByIdAsync<SchoolPrograms>(ApplicationsCpy.Program1);
            if (program1 == null)
            {
                MessageBox.Show("Program 1 neexistuje");
                return;
            }

            if (ApplicationsCpy.Program2 != null)
            {
                var program2 = await queryBuilder.SelectByIdAsync<SchoolPrograms>(ApplicationsCpy.Program2.Value);
                if (program2 == null)
                {
                    MessageBox.Show("Program 2 neexistuje");
                    return;
                }
            }

            if (ApplicationsCpy.Program3 != null)
            {
                var program3 = await queryBuilder.SelectByIdAsync<SchoolPrograms>(ApplicationsCpy.Program3.Value);
                if (program3 == null)
                {
                    MessageBox.Show("Program 3 neexistuje");
                    return;
                }
            }

            await queryBuilder.UpdateAsync(ApplicationsCpy);

            DialogResult = true;
            Close();
        }
    }
}
