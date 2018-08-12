using MyTripCountdown.Models;
using MyTripCountdown.ViewModels.Base;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyTripCountdown.ViewModels
{
    public class MyTripCountdownViewModel : BaseViewModel
    {
        private Trip _trip;
        private Countdown _countdown;
        private int _days;
        private int _hours;
        private int _minutes;
        private double _progress;

        public MyTripCountdownViewModel()
        {
            _countdown = new Countdown();
        }

        public Trip MyTrip
        {
            get => _trip;
            set => SetProperty(ref _trip, value);
        }

        public int Days
        {
            get => _days;
            set => SetProperty(ref _days, value);
        }

        public int Hours
        {
            get => _hours;
            set => SetProperty(ref _hours, value);
        }

        public int Minutes
        {
            get => _minutes;
            set => SetProperty(ref _minutes, value);
        }

        public double Progress
        {
            get => _progress;
            set => SetProperty(ref _progress, value);
        }

        public ICommand RestartCommand => new Command(Restart);

        public override Task LoadAsync()
        {
            LoadTrip();

            _countdown.EndDate = MyTrip.Date;
            _countdown.Start();

            _countdown.Ticked += OnCountdownTicked;
            _countdown.Completed += OnCountdownCompleted;

            return base.LoadAsync();
        }

        public override Task UnloadAsync()
        {
            _countdown.Ticked -= OnCountdownTicked;
            _countdown.Completed -= OnCountdownCompleted;

            return base.UnloadAsync();
        }

        void OnCountdownTicked()
        {
            Days = _countdown.RemainTime.Days;
            Hours = _countdown.RemainTime.Hours;
            Minutes = _countdown.RemainTime.Minutes;

            var totalSeconds = (MyTrip.Date - MyTrip.Creation).TotalSeconds;
            var remainSeconds = _countdown.RemainTime.TotalSeconds;
            Progress = remainSeconds / totalSeconds;
        }

        void OnCountdownCompleted()
        {
            Days = 0;
            Hours = 0;
            Minutes = 0;

            Progress = 0;
        }

        void LoadTrip()
        {
            var trip = new Trip()
            {
                Picture = "trip",
                Date = DateTime.Now + new TimeSpan(1, 2, 42, 15),
                Creation = DateTime.Now.AddHours(-8)
            };

            MyTrip = trip;
        }

        void Restart()
        {
            Debug.WriteLine("Restart");
        }
    }
}