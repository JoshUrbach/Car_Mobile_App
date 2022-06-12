using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinalProject.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        //Backing Variable for Item List
        private ObservableCollection<Car> _cars;

        //Member variable for creating Add New Car page
        private AddCarPage _addCarPage = new AddCarPage();

        //Member variable for creating Edit New Car Page
        private EditCarPage _editCarPage;

        //Private helper routine for reading cars and setting search parameters
        private void GetCars()
        {
            if (_cars is null)
            {
                _cars = new ObservableCollection<Car>
                {
                        new Car     {Brand      = "Lamborghini",
                                     Model      = "Huracan",
                                     Price      = "$208,571",
                                     ImageURI   = ImageSource.FromResource("FinalProject.Images.Huracan.JPG"),
                                     Horsepower = "630hp",
                                     Torque     = "442 lb/ft",
                                     Engine     = "5.2L V10"},
                        new Car     {Brand      = "Audi",
                                     Model      = "RS7",
                                     Price      = "$118,500",
                                     ImageURI   = ImageSource.FromResource("FinalProject.Images.RS7.JPG"),
                                     Horsepower = "591hp",
                                     Torque     = "590 lb/ft",
                                     Engine     = "4.0L V8"},
                        new Car     {Brand      = "Nissan",
                                     Model      = "GT-R",
                                     Price      = "$113,540",
                                     ImageURI   = ImageSource.FromResource("FinalProject.Images.GTR.jpg"),
                                     Horsepower = "565hp",
                                     Torque     = "467 lb/ft",
                                     Engine     = "3.8L V6"}
                };
            }
            else if (_cars.Count < 4)
            {
                //Creates a list this time with a new entry, as well as the original 3 cars
                _cars.Add(
                    new Car         {Brand = "BMW",
                                     Model = "M3",
                                     Price = "$69,900",
                                     ImageURI = ImageSource.FromResource("FinalProject.Images.M3.JPG"),
                                     Horsepower = "473hp",
                                     Torque = "406 lb/ft",
                                     Engine = "3.0L I6"
                    });
            }
            //Check for Search Bar Text and set the List's Item Source
            if (String.IsNullOrWhiteSpace(SearchBar.Text))
                myList.ItemsSource = _cars;
            else
                //use the List where Method to filter the values
                myList.ItemsSource = _cars.Where(c => c.Brand.ToLower().StartsWith(SearchBar.Text.ToLower()));
         
        }
        public MainPage()
        {
            InitializeComponent();
            //Initialize Member Variables
            _cars = null;
            _addCarPage = null;
            _editCarPage = null;

            //Tell Listview to use the List of Cars
            GetCars();
        }

        async private void myList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //Display the Action Options
            string response = await DisplayActionSheet("Options", "Cancel", "Delete",
                                                        "New Car", "Edit Car",
                                                        "Show Details");
            //Test return value to decide on action
            if (response != "Cancel") //Cancelled, do nothing
            {
                //Check for responses
                if (response == "Delete")
                {
                    DeleteCar();
                }
                else if (response == "Show Details")
                {
                    ShowDetails();
                }
                else if (response == "New Car")
                {
                    AddCar();
                }
                else if (response == "Edit Car")
                {
                    EditCar();
                }
            }
        }
        async private void AddCar()
        {
            //Create add contacts page and modally navigate to it
            _addCarPage = new AddCarPage();
            await Navigation.PushModalAsync(_addCarPage);
        }
        async private void EditCar()
        {
            //Validate selection
            if (myList.SelectedItem == null)
            {
                await DisplayAlert("Error", "No Item Selected", "OK");
            }
            else
            {
                var car = myList.SelectedItem as Car;
                _editCarPage = new EditCarPage(car);
                await Navigation.PushModalAsync(new EditCarPage(car));
                
            }
        }

        async private void DeleteCar()
        {
            //Validate selection
            if (myList.SelectedItem == null)
            {
                await DisplayAlert("Error", "No Item Selected", "OK");
            }
            else
            {
                //Verify Deletion with User
                if (await DisplayAlert("Confirmation", "Are you sure?", "Yes", "No"))
                {
                    //Convert selected item to a Car and remove from the list
                    var car = myList.SelectedItem as Car;
                    _cars.Remove(car);
                }
            }
        }

        async private void ShowDetails()
        {
            //Validate selection
            if (myList.SelectedItem == null)
            {
                await DisplayAlert("Error", "No Item Selected", "OK");
            }
            else
            {
                //Convert selected item to a Contact and display the Detail Page
                var car = myList.SelectedItem as Car;
                await Navigation.PushAsync(new CarSpecs(car));
            }
        }

        private void myList_Refreshing(object sender, EventArgs e)
        {
            //Update teh ItemsSource by getting the latest Information on cars
            GetCars();
            //Indicates the refresh is done
            myList.EndRefresh();
        }
        //Search bar, changes e to a string so that our refresh can have this persist when executed with no change in its functionality 
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetCars();
        }
        private void NewCar_Activated(object sender, EventArgs e)
        {
            AddCar();
        }
        private void EditCar_Activated(object sender, EventArgs e)
        {
            EditCar();
        }
        private void ShowDetails_Activated(object sender, EventArgs e)
        {
            ShowDetails();
        }
        private void DeleteCar_Activated(object sender, EventArgs e)
        {
            DeleteCar();
        }
        //Override to process returns from Add and Edit Contact Pages
        protected override void OnAppearing()
        {
            //Call the Base Forms OnAppearing Method
            base.OnAppearing();
            if (_addCarPage != null && !_addCarPage.Cancelled)
            {
                //Add the new contact to the Observable Collection and Refresh
                _cars.Add(_addCarPage.car);
                GetCars();
            }
            else if (_editCarPage != null && !_editCarPage.CancelledEdit)
            {
                int editIndex = _cars.IndexOf(myList.SelectedItem as Car);

                _cars.Remove(_editCarPage.carEdit);
                _cars.Insert(editIndex, _editCarPage.carEdit);
                GetCars();
            }
            //Make sure that all Modal pages are removed from memory
            _addCarPage = null;
            _editCarPage = null;
        }
    }
}
