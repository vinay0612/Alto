using System;
using System.ComponentModel;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using TestSliderSample;
using TestSliderSample.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomSlider), typeof(MySliderRenderer))]
namespace TestSliderSample.Droid.Renderer
{
    [Obsolete]
    public class MySliderRenderer : SliderRenderer
    {
        private CustomSlider view;
        int currentSliderValue = 0;
        bool IsDragging = false;
        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || e.NewElement == null)
                return;
            view = (CustomSlider)Element;
            if (!string.IsNullOrEmpty(view.ThumbImageIcon))
            {    // Set Thumb Icon

                var percentage = Convert.ToDecimal((Control.Progress / 10).ToString());
                var img = Resources.GetDrawable(view.ThumbImageIcon);
                Bitmap bitmap = ((BitmapDrawable)img).Bitmap;
                Bitmap newBitMap = DrawText(Convert.ToInt32(percentage).ToString() + "%", bitmap);
                newBitMap = Bitmap.CreateScaledBitmap(newBitMap, 150, 150, true);
                Drawable newImg = new BitmapDrawable(newBitMap);
                Control.SetThumb(newImg);

            }
            else if (view.ThumbColor != Xamarin.Forms.Color.Default ||
                view.MaxColor != Xamarin.Forms.Color.Default ||
                view.MinColor != Xamarin.Forms.Color.Default)
            Control.Thumb.SetColorFilter(view.ThumbColor.ToAndroid(), PorterDuff.Mode.SrcIn);
            Control.ProgressTintList = Android.Content.Res.ColorStateList.ValueOf(view.MinColor.ToAndroid());
            Control.ProgressTintMode = PorterDuff.Mode.SrcIn;
            //this is for Maximum Slider line Color  
            Control.ProgressBackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(view.MaxColor.ToAndroid());
            Control.ProgressBackgroundTintMode = PorterDuff.Mode.SrcIn;
            currentSliderValue = Control.Progress;
            Control.Background = null;
            //Control.SetOnTouchListener();
        }

        public override bool OnTouchEvent(MotionEvent e)
        {

            return true;
        }

        private void Control_Touch(object sender, TouchEventArgs e)
        {
            float prevX = float.MinValue;
            float eps = 0.001f;
            Drawable drawable = Control.Thumb;
            if (e.Event.Action == MotionEventActions.Down)
            {
                if (e.Event.GetAxisValue(Axis.X) >= drawable.Bounds.Left && e.Event.GetAxisValue(Axis.X) <= drawable.Bounds.Right && e.Event.GetAxisValue(Axis.Y) >= drawable.Bounds.Top && e.Event.GetAxisValue(Axis.Y) <= drawable.Bounds.Bottom)
                {
                    prevX = e.Event.GetAxisValue(Axis.X);
                    IsDragging = true;
                }
            }
            if (e.Event.Action == MotionEventActions.Up)
            {
                if (e.Event.GetAxisValue(Axis.X) >= drawable.Bounds.Left && e.Event.GetAxisValue(Axis.X) <= drawable.Bounds.Right && e.Event.GetAxisValue(Axis.Y) >= drawable.Bounds.Top && e.Event.GetAxisValue(Axis.Y) <= drawable.Bounds.Bottom)
                {
                    if (Math.Abs(e.Event.GetAxisValue(Axis.X) - prevX) < eps)
                    {
                        Console.WriteLine("Clicked on place");
                        IsDragging = false;
                    }

                    prevX = float.MinValue;
                    return;
                }
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBean)
            {
                if (Control == null)
                { return; }
                SeekBar ctrl = Control;
                Drawable thumb = ctrl.Thumb;
                if (thumb != null)
                {
                    int thumbTop = ctrl.Height / 2 - thumb.IntrinsicHeight / 2;
                    thumb.SetBounds(thumb.Bounds.Left, thumbTop,
                                    thumb.Bounds.Left + thumb.IntrinsicWidth, thumbTop + thumb.IntrinsicHeight);
                }
            }
            
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            view = (CustomSlider)Element;
            if (!string.IsNullOrEmpty(view.ThumbImageIcon))
            {    // Set Thumb Icon  
                var percentage = Convert.ToDecimal((Control.Progress/10).ToString());
                var img = Resources.GetDrawable(view.ThumbImageIcon);
                Bitmap bitmap = ((BitmapDrawable)img).Bitmap;
                Bitmap newBitMap = DrawText(Convert.ToInt32(percentage).ToString() + "%", bitmap);
                newBitMap = Bitmap.CreateScaledBitmap(newBitMap, 150, 150, true);
                Drawable newImg = new BitmapDrawable(newBitMap);
                Control.SetThumb(newImg);
            }
            //IsDragging = true;
            if (IsDragging == false)
            {
                Control.Progress = currentSliderValue;
            }
            currentSliderValue = Control.Progress;
        }

        Bitmap DrawText(string text,Bitmap bitmap)
        {
            Bitmap output = bitmap.Copy(Bitmap.Config.Argb8888, true);
            Canvas canvas = new Canvas(output);
            Console.WriteLine(canvas.Height);
            Paint paint = new Paint(PaintFlags.LinearText);
            var bound = new Rect();
            paint.GetTextBounds(text, 0, text.Length, bound);
            paint.Color = Android.Graphics.Color.White;
            paint.TextSize = 18f;
            paint.TextAlign = Paint.Align.Center;
            //paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcOver));
            canvas.DrawText(text, 35, 35, paint);
            return output;
        }
   }

    public class MyOnTouchListener : Java.Lang.Object, View.IOnTouchListener
    {
        
    }

}
