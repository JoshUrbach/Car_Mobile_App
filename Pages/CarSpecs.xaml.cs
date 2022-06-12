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
    public partial class CarSpecs : ContentPage
    {
        public CarSpecs(Car car)
        {
            //Error thrown when there is no car 
            if (car == null) throw new ArgumentNullException();

            //This is what allows the screen to display the correct car specs (passed in argument)
            BindingContext = car;
            InitializeComponent();
        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}