using System;
using System.ComponentModel;
using CoreGraphics;
using TestSliderSample;
using TestSliderSample.iOS.Renderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomSlider), typeof(MySliderRenderer))]
namespace TestSliderSample.iOS.Renderer
{

    public class MySliderRenderer : SliderRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Slider> e)
        {
            base.OnElementChanged(e);
            SetNativeControl(new MySlideriOS());
            if (e.OldElement != null || e.NewElement == null)
                return;

            var view = (CustomSlider)Element;
            if (!string.IsNullOrEmpty(view.ThumbImageIcon))
            {
                //Assigns a thumb image to the specified control states.
                var percentage = Convert.ToDecimal(Control.Value.ToString("0.00")) * 100;
                var img = DrawText(UIImage.FromBundle(view.ThumbImageIcon), Convert.ToInt32(percentage).ToString()+"%", UIColor.White, 8);
                Control.SetThumbImage(img, UIControlState.Normal);
            }
            else if (view.ThumbColor != Xamarin.Forms.Color.Default ||
                view.MaxColor != Xamarin.Forms.Color.Default ||
                view.MinColor != Xamarin.Forms.Color.Default)
                // Set Progress bar Thumb color  
                Control.ThumbTintColor = view.ThumbColor.ToUIColor();
            //this is for Minimum Slider line Color  
            Control.MinimumTrackTintColor = view.MinColor.ToUIColor();
            //this is for Maximum Slider line Color  
            Control.MaximumTrackTintColor = view.MaxColor.ToUIColor();
            
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var view = (CustomSlider)Element;
            if (!string.IsNullOrEmpty(view.ThumbImageIcon))
            {
                //Assigns a thumb image to the specified control states.
                var percentage = Convert.ToDecimal(Control.Value.ToString("0.00")) * 100;
                var img = DrawText(UIImage.FromBundle(view.ThumbImageIcon), Convert.ToInt32(percentage).ToString() + "%", UIColor.White, 12);
                Control.SetThumbImage(img, UIControlState.Normal);
            }
        }

        public static UIImage DrawText(UIImage uiImage, string sText, UIColor textColor, int iFontSize)
        {
            nfloat fWidth = uiImage.Size.Width;
            nfloat fHeight = uiImage.Size.Height;

            CGColorSpace colorSpace = CGColorSpace.CreateDeviceRGB();

            using (CGBitmapContext ctx = new CGBitmapContext(IntPtr.Zero, (nint)fWidth, (nint)fHeight, 8, 4 * (nint)fWidth, CGColorSpace.CreateDeviceRGB(), CGImageAlphaInfo.PremultipliedFirst))
            {
                ctx.DrawImage(new CGRect(0, 0, (double)fWidth, (double)fHeight), uiImage.CGImage);

                ctx.SelectFont("Helvetica", iFontSize, CGTextEncoding.MacRoman);

                //Measure the text's width - This involves drawing an invisible string to calculate the X position difference
                float start, end, textWidth;

                //Get the texts current position
                start = (float)ctx.TextPosition.X+2;
                //Set the drawing mode to invisible
                ctx.SetTextDrawingMode(CGTextDrawingMode.Invisible);
                //Draw the text at the current position
                ctx.ShowText(sText);
                //Get the end position
                end = (float)ctx.TextPosition.X;
                //Subtract start from end to get the text's width
                textWidth = end - start;

                nfloat fRed;
                nfloat fGreen;
                nfloat fBlue;
                nfloat fAlpha;
                //Set the fill color to black. This is the text color.
                textColor.GetRGBA(out fRed, out fGreen, out fBlue, out fAlpha);
                ctx.SetFillColor(fRed, fGreen, fBlue, fAlpha);

                //Set the drawing mode back to something that will actually draw Fill for example
                ctx.SetTextDrawingMode(CGTextDrawingMode.FillClip);

                //Draw the text at given coords.
                ctx.ShowTextAtPoint(fWidth/2-10, (fHeight/2), sText);

                return UIImage.FromImage(ctx.ToImage());
            }
        }
    }

    public class MySlideriOS : UISlider
    {
        public MySlideriOS()
        {
           
        }

        //public override CGRect TrackRectForBounds(CGRect forBounds)
        //{
        //    CGRect rect = base.TrackRectForBounds(forBounds);
        //    return new CGRect(rect.X, rect.Y, rect.Width, 20);
        //}
    }

}
