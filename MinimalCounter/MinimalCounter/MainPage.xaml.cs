using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Diagnostics;

namespace MinimalCounter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        bool doIncrement = false;
        bool isPressed = false;
        Stopwatch stopwatch = new Stopwatch();

        private string counterValueString;
        private int counterValue = 0;
        private DisplayInfo mainDisplayInfo;

        private double width = 0;
        private double height = 0;

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height); //must be called
            if (this.width != width || this.height != height)
            {
                this.width = width;
                this.height = height;
                HeightString = ((int)height).ToString();
                WidthString = ((int)width).ToString();
            }
        }

        public string CounterValueString
        {
            get { return counterValueString; }
            set
            {
                // OnPropertyChanged should not be called if the property value
                // does not change.
                if (counterValueString == value)
                    return;
                counterValueString = value;
                OnPropertyChanged(nameof(CounterValueString)); // Notify that there was a change on this property
            }
        }

        private string heightString;
        public string HeightString
        {
            get { return heightString; }
            set
            {
                if (heightString == value)
                    return;
                heightString = value;
                OnPropertyChanged(nameof(HeightString)); // Notify that there was a change on this property
            }
        }

        private string widthString;
        public string WidthString
        {
            get { return widthString; }
            set
            {
                if (widthString == value)
                    return;
                widthString = value;
                OnPropertyChanged(nameof(WidthString)); // Notify that there was a change on this property
            }
        }

        public MainPage()
        {
            InitializeComponent();
            DeviceDisplay.KeepScreenOn = true;
            CounterValueString = counterValue.ToString();
            BindingContext = this;

            // Get Metrics
            mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            // Width (in pixels)
            WidthString = mainDisplayInfo.Width.ToString();

            // Height (in pixels)
            HeightString = mainDisplayInfo.Height.ToString();
        }

        private void IncrementButton_Pressed(object sender, EventArgs e)
        {
            stopwatch.Start();
            isPressed = true;
            doIncrement = true;

            try
            {
                // Use default vibration length
                Vibration.Vibrate();

                // Or use specified time
                var duration = TimeSpan.FromMilliseconds(150);
                Vibration.Vibrate(duration);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }

            int j = 0;
            Device.StartTimer(TimeSpan.FromMilliseconds(1000), () =>
            {
                if (isPressed && 2 < j++)
                {
                    doIncrement = false;
                    counterValue = 0;
                    CounterValueString = counterValue.ToString();
                    stopwatch.Reset();
                }
                return isPressed;
            });
        }

        private void IncrementButton_Released(object sender, EventArgs e)
        {
            isPressed = false;
            if (doIncrement)
            {
                counterValue++;
                CounterValueString = counterValue.ToString();
            }
        }
    }
}
