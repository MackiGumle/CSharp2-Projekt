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
    /// Interaction logic for EditProgramWindow.xaml
    /// </summary>
    public partial class EditProgramWindow : Window
    {
        public SchoolPrograms ProgramCpy { get; set; } = new SchoolPrograms();

        public EditProgramWindow(SchoolPrograms program)
        {
            InitializeComponent();
            this.DataContext = this;

            ProgramCpy.Id = program.Id;
            ProgramCpy.School = program.School;
            ProgramCpy.Name = program.Name;
            ProgramCpy.Description = program.Description;
            ProgramCpy.Capacity = program.Capacity;
        }

        private async void UpravitBtn(object sender, RoutedEventArgs e)
        {
            using QueryBuilder queryBuilder = new QueryBuilder();

            if (string.IsNullOrEmpty(ProgramCpy.Name) || string.IsNullOrEmpty(ProgramCpy.Description) || ProgramCpy.School < 1)
            {
                MessageBox.Show("Vyplňte všechny údaje");
                return;
            }

            var school = await queryBuilder.SelectByIdAsync<School>(ProgramCpy.School);
            if (school == null)
            {
                MessageBox.Show("Škola neexistuje");
                return;
            }

            if(string.IsNullOrEmpty(ProgramCapacityInput.Text))
                ProgramCpy.Capacity = null;

            await queryBuilder.UpdateAsync(ProgramCpy);

            DialogResult = true;
            Close();
        }
    }
}
