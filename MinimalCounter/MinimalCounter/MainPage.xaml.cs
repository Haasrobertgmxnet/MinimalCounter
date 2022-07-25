using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace MinimalCounter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private DateTime timeStamp;
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
                HeightString = (mainDisplayInfo.Density * height).ToString();
                WidthString = (mainDisplayInfo.Density * width).ToString();
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
            timeStamp = DateTime.Now;
            try
            {
                // Use default vibration length
                Vibration.Vibrate();

                // Or use specified time
                var duration = TimeSpan.FromMilliseconds(200);
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
            counterValue++;
            CounterValueString = counterValue.ToString();
        }

        private void IncrementButton_Released(object sender, EventArgs e)
        {
            if (timeStamp.AddSeconds(3) < DateTime.Now)
            {
                counterValue = 0;
                CounterValueString = counterValue.ToString();
            }
        }
    }
}
