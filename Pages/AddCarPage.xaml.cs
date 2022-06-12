using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinalProject.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCarPage : ContentPage
    {
        //Member variable for returning contact info to main page
        public Car car { get; }
        //Member variable for returning OK/Cancel selection to main page
        public bool Cancelled { get; set; }
        public AddCarPage()
        {
            //Create the contact and set defaults
            car = new Car();
            car.ImageURI = ImageSource.FromResource("FinalProject.Images.GenericCar.jpg");

            //Set the Binding Contact to the new contact
            BindingContext = car;
            InitializeComponent();
            //Set the focus to the name entry control
            enteredBrand.Focus();
        }

        async private void OK_Button_Clicked(object sender, EventArgs e)
        {
            //Validate input, Name is required
            if (car.Brand == null || car.Brand.Length == 0)
            {
                //Signify Error to User and reset the focus to correct control
                await DisplayAlert("Error", "You Must Enter a Brand", "OK");
                enteredBrand.Focus();
            }
            else
            {
                //Signal that user accepted new value and remove page from Modal Stack
                Cancelled = false;
                await Navigation.PopModalAsync();
            }
        }
        async private void Cancel_Button_Clicked(object sender, EventArgs e)
        {
            //Signal that user cancelled and remove page from Modal Stack
            Cancelled = true;
            await Navigation.PopModalAsync();
        }
        //Disable the Android Back Button
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}