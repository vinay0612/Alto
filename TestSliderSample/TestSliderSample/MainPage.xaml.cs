using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestSliderSample
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            slider.DragStarted += Slider_DragStarted;
            slider.DragCompleted += Slider_DragCompleted;
        }

        private void Slider_DragCompleted(object sender, EventArgs e)
        {
            slider.ThumbImageIcon = "VolumeKnob";
        }

        private void Slider_DragStarted(object sender, EventArgs e)
        {
            slider.ThumbImageIcon = "VolumeKnobH";
        }


    }
}
