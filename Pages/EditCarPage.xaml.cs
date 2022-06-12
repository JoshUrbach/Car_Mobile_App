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
    public partial class EditCarPage : ContentPage
    {
        //Member variable for returning contact info to main page
        public Car carEdit { get; set; }
        private Car copyCar = new Car();

        //Member variable for returning OK/Cancel selection to main page
        public bool CancelledEdit { get; set; }
        public EditCarPage(Car car)
        {
            //Set the Binding Car to the car to be edited 
            carEdit = car;

            //Create a copy
            
            copyCar.Brand = car.Brand;
            copyCar.Model = car.Model;
            copyCar.Price = car.Price;
            copyCar.Horsepower = car.Horsepower;
            copyCar.Torque = car.Torque;
            copyCar.Engine = car.Engine;

            BindingContext = copyCar;
            InitializeComponent();
        }

        async private void OK_Button_Clicked(object sender, EventArgs e)
        {
            //Validate input, Name is required
            if (carEdit.Brand == null || carEdit.Brand.Length == 0)
            {
                //Signify Error to User and reset the focus to correct control
                await DisplayAlert("Error", "You Must Enter a Brand", "OK");
                enteredBrand.Focus();
            }
            else
            {
                //Signal that user accepted new value and remove page from Modal Stack, also update the car to edit to match the temporary values after confirmation that user wants to make change
                carEdit.Brand = copyCar.Brand;
                carEdit.Model = copyCar.Model;
                carEdit.Price = copyCar.Price;
                carEdit.Horsepower = copyCar.Horsepower;
                carEdit.Torque = copyCar.Torque;
                carEdit.Engine = copyCar.Engine;

                CancelledEdit = false;
                await Navigation.PopModalAsync();
            }
        }
        async private void Cancel_Button_Clicked(object sender, EventArgs e)
        {
            if(await DisplayAlert("Confirmation", "Are you sure?","Yes", "No"))
            {
                //Signal that user cancelled and remove page from Modal Stack
                CancelledEdit = true;
                await Navigation.PopModalAsync();
            }
                
            else
            {
                return;
            }
        }
        //Disable the Android Back Button
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}