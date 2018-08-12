using MyTripCountdown.ViewModels;
using MyTripCountdown.ViewModels.Base;
using Xamarin.Forms;

namespace MyTripCountdown.Views
{
    public partial class MyTripCountdownView : ContentPage
	{
		public MyTripCountdownView ()
		{
			InitializeComponent ();

            BindingContext = new MyTripCountdownViewModel();
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var vm = BindingContext as BaseViewModel;
            await vm?.LoadAsync();
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            var vm = BindingContext as BaseViewModel;
            await vm?.UnloadAsync();
        }
    }
}