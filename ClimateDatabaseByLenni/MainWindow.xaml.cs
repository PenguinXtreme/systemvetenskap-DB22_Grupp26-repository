using ClimateDatabaseByLenni.Models;
using ClimateDatabaseByLenni.Repositories;
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

namespace ClimateDatabaseByLenni
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            var db = new dbRepositories();
            

        }

        private void addObseverBtn_Click(object sender, RoutedEventArgs e)
        {
            var db = new dbRepositories();
            var observers = new List<Person>();

            var observer = new Person()
            {
                Firstname = firstNameTextBox.ToString(),
                Lastname = lastNameTextBox.ToString()


            };
            observers.Add(observer);




            db.AddObservers(observers);




        }

        private void showObserversBtn_Click(object sender, RoutedEventArgs e)
        {
            var db = new dbRepositories();

            observersListBox.ItemsSource = db.GetClimateObservers();


        }

        private void deleteChosenObserverBtn_Click(object sender, RoutedEventArgs e)
        {
            var db = new dbRepositories();
            Person observer = observersListBox.SelectedItem as Person;

            string message = db.DeleteChosenObserver(observer);

            MessageBox.Show(message);

        }

        private void btnBringForthObserver_Click(object sender, RoutedEventArgs e)
        {
            Person observer = observersListBox.SelectedItem as Person;


           
           
            labelChosenFirstName.Content = observer.Firstname;
            labelChosenLastName.Content = observer.Lastname;
            labelInvisibleId.Content = observer.Id;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var db = new dbRepositories();

            string firstname = (string)labelChosenFirstName.Content;
            string lastname = (string)labelChosenLastName.Content;
            int id = (int)labelInvisibleId.Content;
            string temperature = textBoxAirTemp.Text;
            temperature += " C";

            db.AddObserversPlaceAndTemp(firstname, lastname, temperature, id);




        }

        private void btnGetMeasurements_Click(object sender, RoutedEventArgs e)
        {
            var db = new dbRepositories();
            Person person = listBoxObservation.SelectedItem as Person;

            int observationId = person.ObservationId;


            listBoxType.ItemsSource = db.GetDifferentType(observationId);
            listBoxAbbreviation.ItemsSource = db.GetAbbreviationMeauserements(observationId);


        }

        private void btnBringObservations_Click(object sender, RoutedEventArgs e)
        {
            var db = new dbRepositories();
            int id = (int)labelInvisibleId.Content;

            listBoxObservation.ItemsSource = db.GetTheObservations(id);
        }

        private void btnChangeValue_Click(object sender, RoutedEventArgs e)
        {
            var db = new dbRepositories();
            Person abbreviation = listBoxAbbreviation.SelectedItem as Person;
            abbreviation.Firstname = textBoxCelsiusAntal.Text;
            int number = (int)abbreviation.Id;
            string measurementAbbreviation = abbreviation.Firstname;
            db.ChangeTheValue(measurementAbbreviation, number);
        }

        private void btnChosenAbbreviationValue_Click(object sender, RoutedEventArgs e)
        {
            Person abbreviation = listBoxAbbreviation.SelectedItem as Person;
            string abbreviationText = abbreviation.Firstname;
            
            
            textBoxCelsiusAntal.Text = abbreviationText;
        }
    }
}
