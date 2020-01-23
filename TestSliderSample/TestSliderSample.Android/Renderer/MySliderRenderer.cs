using System;
using System.ComponentModel;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using TestSliderSample;
using TestSliderSample.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.Views.View;

[assembly: ExportRenderer(typeof(CustomSlider), typeof(MySliderRenderer))]
namespace TestSliderSample.Droid.Renderer
{
    [Obsolete]
    public class MySliderRenderer : SliderRenderer
    {
        public CustomSlider view;
        public int currentSliderValue = 0; 
        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);
            var seekBar = new ExtendedSlider(this.Context);
            seekBar.mySliderRenderer = this;
            SetNativeControl(seekBar);
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
            Control.StopTrackingTouch += Control_StopTrackingTouch;
            currentSliderValue = Control.Progress;
            Control.Background = null;
                     
        }

        private void Control_StopTrackingTouch(object sender, SeekBar.StopTrackingTouchEventArgs e)
        {
            var percentage = Convert.ToDecimal((currentSliderValue).ToString());
            var img = Resources.GetDrawable("VolumeKnob");
            Bitmap bitmap = ((BitmapDrawable)img).Bitmap;
            Bitmap newBitMap = DrawText(Convert.ToInt32(percentage).ToString() + "%", bitmap);
            newBitMap = Bitmap.CreateScaledBitmap(newBitMap, 150, 150, true);
            Drawable newImg = new BitmapDrawable(newBitMap);
            Control.SetThumb(newImg);
        }

        public void ChangeProgressValue()
        {
            var percentage = Convert.ToDecimal((currentSliderValue).ToString());
            var img = Resources.GetDrawable("VolumeKnobH");
            Bitmap bitmap = ((BitmapDrawable)img).Bitmap;
            Bitmap newBitMap = DrawText(Convert.ToInt32(percentage).ToString() + "%", bitmap);
            newBitMap = Bitmap.CreateScaledBitmap(newBitMap, 150, 150, true);
            Drawable newImg = new BitmapDrawable(newBitMap);
            Control.SetThumb(newImg);
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
           
            currentSliderValue = Control.Progress;
        }

       public Bitmap DrawText(string text,Bitmap bitmap)
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
            canvas.DrawText(text, 35, 35, paint);
            return output;
        }

        void SetSliderHieght(string pColor,string bgColor)
        {
            GradientDrawable p = new GradientDrawable();
            p.SetCornerRadius(10);
            p.SetColor(Android.Graphics.Color.DarkRed);
            ClipDrawable progress = new ClipDrawable(p, GravityFlags.Left, ClipDrawable.Horizontal);
            GradientDrawable background = new GradientDrawable();
            background.SetColor(Android.Graphics.Color.Blue);
            background.SetCornerRadius(10);
            LayerDrawable pd = new LayerDrawable(new Drawable[] { background, progress});
            Control.SetProgressDrawableTiled(pd);
        }
   }

    public class ExtendedSlider : SeekBar
    {
        Drawable _drawable;
        public MySliderRenderer mySliderRenderer;
        public ExtendedSlider(Context context) : base(context)
        {
           
        }
        public override void SetThumb(Drawable thumb)
        {
            base.SetThumb(thumb);
            _drawable = thumb;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            Drawable drawable = _drawable;
            
            if (e.Action == MotionEventActions.Down)
            {
                Console.WriteLine(this.Progress);
                if (e.GetAxisValue(Axis.X) <= drawable.Bounds.Left || e.GetAxisValue(Axis.X) >= drawable.Bounds.Right || e.GetAxisValue(Axis.Y) <= drawable.Bounds.Top || e.GetAxisValue(Axis.Y) >= drawable.Bounds.Bottom)
                {
                    //base.OnTouchEvent(e);
                    return false;
                }
                
            }
            var progressValue = this.Min + (int)(this.Max * e.GetAxisValue(Axis.X) / this.Width);
            if (progressValue < 0)
            {
                progressValue = 0;
            }
            if (progressValue > 100)
            {
                progressValue = 100;
            }
            mySliderRenderer.currentSliderValue = progressValue;//Convert.ToInt32(e.GetAxisValue(Axis.X)/10);
            this.Progress = mySliderRenderer.currentSliderValue;
            mySliderRenderer.ChangeProgressValue();
            return  base.OnTouchEvent(e);
        }
    }
}
