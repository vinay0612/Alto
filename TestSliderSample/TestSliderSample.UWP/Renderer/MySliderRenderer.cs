using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSliderSample;
using TestSliderSample.UWP.Renderer;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomSlider), typeof(MySliderRenderer))]
namespace TestSliderSample.UWP.Renderer
{
    public class MySliderRenderer : SliderRenderer
    {
        public CustomSlider View { get; set; }
        public double currentValue = 0;
        bool isTapped = false;
        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || e.NewElement == null)
            {
                return;
            }

            View = (CustomSlider)Element;
            if (!string.IsNullOrEmpty(View.ThumbImageIcon))
            {    //
                var thumbImage = View.ThumbImageIcon;

                if (thumbImage == null)
                {
                    Control.ThumbImageSource = null;
                    return;
                }

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.DecodePixelHeight = 150;
                bitmapImage.DecodePixelWidth = 100;
                bitmapImage.UriSource = new Uri("ms-appx:///Assets/" + View.ThumbImageIcon + ".png");
                Control.ThumbImageSource = bitmapImage;
                currentValue = Control.Value;
               
            }
            Control.IsThumbToolTipEnabled = true;
            this.SetText(View.ThumbImageIcon, (Control.Value * 100).ToString()+ "%");
        }


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri("ms-appx:///Assets/" + View.ThumbImageIcon + ".png");
            Control.ThumbImageSource = bitmapImage;
            if(isTapped == true)
            {
                Control.Value = currentValue;

            }
            else
            {
                currentValue = Control.Value;
            }
            Console.WriteLine(Control.Value.ToString());
            this.SetText(View.ThumbImageIcon, (Control.Value*100).ToString() + "%");
            Control.IsThumbToolTipEnabled = true;
            Control.IsDoubleTapEnabled = false;
            Control.IsTapEnabled = false;
            Control.IsRightTapEnabled = false;

        }

        private async void SetText(string img,string text)
        {

            Uri imageuri = new Uri("ms-appx:///Assets/" + img + ".png");
            StorageFile inputFile = await StorageFile.GetFileFromApplicationUriAsync(imageuri);
            BitmapDecoder imagedecoder;
            using (var imagestream = await inputFile.OpenAsync(FileAccessMode.Read))
            {
                imagedecoder = await BitmapDecoder.CreateAsync(imagestream);
            }
            CanvasDevice device = CanvasDevice.GetSharedDevice();
            CanvasRenderTarget renderTarget = new CanvasRenderTarget(device, imagedecoder.PixelWidth, imagedecoder.PixelHeight, 100);
            using (var ds = renderTarget.CreateDrawingSession())
            {
                ds.Clear(Colors.White);
                CanvasBitmap image = await CanvasBitmap.LoadAsync(device, inputFile.Path);
                ds.DrawImage(image);
                var formater = new CanvasTextFormat();
                formater.FontSize = 18;
                ds.DrawText(text, new System.Numerics.Vector2(10, 15), Colors.White, formater);
            }

            using (var stream = new InMemoryRandomAccessStream())
            {
                stream.Seek(0);
                await renderTarget.SaveAsync(stream, CanvasBitmapFileFormat.Png);
                BitmapImage image = new BitmapImage();
                image.SetSource(stream);
                Control.ThumbImageSource = image;
            }
            
        }
    }

    
}
