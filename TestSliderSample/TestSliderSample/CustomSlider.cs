using System;
using Xamarin.Forms;

namespace TestSliderSample
{
    public class CustomSlider : Slider
    {
        public static readonly BindableProperty MaxColorProperty = BindableProperty.Create(nameof(MaxColor),
        typeof(Color), typeof(CustomSlider), Color.Default);

        public Color MaxColor
        {
            get { return (Color)GetValue(MaxColorProperty); }
            set { SetValue(MaxColorProperty, value); }
        }

        public static readonly BindableProperty MinColorProperty = BindableProperty.Create(nameof(MinColor),
            typeof(Color), typeof(CustomSlider), Color.Default);

        public Color MinColor
        {
            get { return (Color)GetValue(MinColorProperty); }
            set { SetValue(MinColorProperty, value); }
        }

        public static readonly BindableProperty ThumbColorProperty = BindableProperty.Create(nameof(ThumbColor),
            typeof(Color), typeof(CustomSlider), Color.Default);

        public Color ThumbColor
        {
            get { return (Color)GetValue(ThumbColorProperty); }
            set { SetValue(ThumbColorProperty, value); }
        }

        public static readonly BindableProperty ThumbImageIconProperty = BindableProperty.Create(nameof(ThumbImageIcon),
              typeof(string), typeof(CustomSlider), string.Empty);

        public string ThumbImageIcon
        {
            get { return (string)GetValue(ThumbImageIconProperty); }
            set { SetValue(ThumbImageIconProperty, value); }
        }
    }
}
