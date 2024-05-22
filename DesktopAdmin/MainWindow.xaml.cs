using CSharp2Projekt;
using DataLayer.Models;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Applications = DataLayer.Models.Applications;

namespace DesktopAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private readonly string  = "Data Source=D:\\Projects\\Skola\\2ndYear\\2ndSemester\\C#\\Projekt\\WebServerPrihlasky\\databaze.db";
        public School School { get; set; } = new School();
        public SchoolPrograms Program { get; set; } = new SchoolPrograms();

        public ObservableCollection<School> Schools { get; set; } = new ObservableCollection<School>();
        public ObservableCollection<SchoolPrograms> Programs { get; set; } = new ObservableCollection<SchoolPrograms>();
        public ObservableCollection<Applications> Applications { get; set; } = new ObservableCollection<Applications>();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

        }

        private async void ReloadSchools()
        {
            using QueryBuilder queryBuilder = new QueryBuilder();

            var schools = await queryBuilder.SelectAllAsync<School>();

            Schools.Clear();
            foreach (var school in schools)
            {
                Schools.Add(school);
            }
        }

        private async void ReloadPrograms()
        {
            using QueryBuilder queryBuilder = new QueryBuilder();

            var programs = await queryBuilder.SelectAllAsync<SchoolPrograms>();

            Programs.Clear();
            foreach (var program in programs)
            {
                Programs.Add(program);
            }
        }
        private async void ReloadApplications()
        {
            using QueryBuilder queryBuilder = new QueryBuilder();

            var applications = await queryBuilder.SelectAllAsync<Applications>();

            Applications.Clear();
            foreach (var application in applications)
            {
                Applications.Add(application);
            }
        }

        private async void WindowLoaded(object sender, RoutedEventArgs e)
        {
            using QueryBuilder queryBuilder = new QueryBuilder();

            ReloadSchools();
            ReloadPrograms();
            ReloadApplications();
        }

        private async void SchoolPridatBtn(object sender, RoutedEventArgs e)
        {
            using QueryBuilder queryBuilder = new QueryBuilder();

            if (string.IsNullOrEmpty(School.Name) || string.IsNullOrEmpty(School.Country) || string.IsNullOrEmpty(School.City) || string.IsNullOrEmpty(School.Street))
            {
                MessageBox.Show("Vyplňte všechny údaje");
                return;
            }

            if (await queryBuilder.InsertAsync(School) < 1)
            {
                MessageBox.Show("Chyba při přidávání školy");
            }

            ReloadSchools();
        }

        private async void SchoolSmazatBtn(object sender, RoutedEventArgs e)
        {
            using QueryBuilder queryBuilder = new QueryBuilder();

            Button btn = (Button)sender;
            School s = (School)btn.DataContext;

            if (await queryBuilder.DeleteAsync<School>(s.Id) < 1)
            {
                MessageBox.Show("Chyba při mazání školy");
            }

            ReloadSchools();
        }

        private void SchoolUpravitBtn(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            School s = (School)btn.DataContext;

            EditSchoolWindow ew = new EditSchoolWindow(s);

            if (ew.ShowDialog() ?? false)
            {
                ReloadSchools();
            }
            else
            {
                MessageBox.Show("Chyba při úpravě školy");
            }
        }

        private async void ProgramPridatBtn(object sender, RoutedEventArgs e)
        {
            using QueryBuilder queryBuilder = new QueryBuilder();

            var ProgramId = ProgramSchoolInput.SelectedValue as long?;
            Program.School = ProgramId ?? 0;

            if (string.IsNullOrEmpty(Program.Name) || string.IsNullOrEmpty(Program.Description) || Program.School < 1)
            {
                MessageBox.Show("Vyplňte všechny údaje");
                return;
            }

            var school = await queryBuilder.SelectByIdAsync<School>(Program.School);
            if (school == null)
            {
                MessageBox.Show("Škola neexistuje");
                return;
            }

            if (await queryBuilder.InsertAsync(Program) < 1)
            {
                MessageBox.Show("Chyba při přidávání programu");
            }

            ReloadPrograms();
        }

        private async void ProgramSmazatBtn(object sender, RoutedEventArgs e)
        {
            using QueryBuilder queryBuilder = new QueryBuilder();

            Button btn = (Button)sender;
            SchoolPrograms p = (SchoolPrograms)btn.DataContext;

            if (await queryBuilder.DeleteAsync<SchoolPrograms>(p.Id) < 1)
            {
                MessageBox.Show("Chyba při mazání programu");
            }

            ReloadPrograms();
        }

        private void ProgramUpravitBtn(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            SchoolPrograms p = (SchoolPrograms)btn.DataContext;

            EditProgramWindow ew = new EditProgramWindow(p);

            if (ew.ShowDialog() ?? false)
            {
                ReloadPrograms();
            }
            else
            {
                MessageBox.Show("Chyba při úpravě programu");
            }
        }

        private async void ApplicationSmazatBtn(object sender, RoutedEventArgs e)
        {
            using QueryBuilder queryBuilder = new QueryBuilder();
            Button btn = (Button)sender;
            Applications a = (Applications)btn.DataContext;

            if (await queryBuilder.DeleteAsync<Applications>(a.Id) < 1)
            {
                MessageBox.Show("Chyba při mazání přihlášky");
            }

            ReloadApplications();
        }

        private void ApplicationUpravitBtn(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Applications a = (Applications)btn.DataContext;

            EditApplicationWindow ew = new EditApplicationWindow(a);

            if (ew.ShowDialog() ?? false)
            {
                ReloadApplications();
            }
            else
            {
                MessageBox.Show("Chyba při úpravě přihlášky");
            }
        }
    }
}