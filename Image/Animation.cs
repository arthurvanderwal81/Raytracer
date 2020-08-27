using System;
using System.Timers;
using System.Drawing;

namespace Raytracer.Image
{
    public class Animation
    {
        public delegate void FrameUpdateDelegate(int frameNumber, Bitmap frame);
        public delegate void AnimationStopDelegate();

        public FrameUpdateDelegate FrameUpdate;
        public AnimationStopDelegate AnimationStop;

        public ImageStack ImageStack { get; set; }

        public int CurrentImageIndex { get; set; }

        public bool Bounce { get; set; }
        public bool Loop { get; set; }

        private int _direction = 1;
        private Timer _timer;

        public Bitmap CurrentImage
        {
            get
            {
                return ImageStack[CurrentImageIndex];
            }
        }

        public Animation()
        {
            ImageStack = new ImageStack();
            Loop = true;
        }

        public void Start(int frameTime)
        {
            if (ImageStack.Bitmaps.Count < 2)
            {
                throw new ArgumentException("ImageStack has to contain at least 2 images");
            }

            _timer = new Timer(frameTime);
            _timer.Elapsed += _timer_Elapsed;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CurrentImageIndex += _direction;

            if (CurrentImageIndex < 0 || CurrentImageIndex == ImageStack.Bitmaps.Count)
            {
                if (Loop)
                {
                    if (Bounce)
                    {
                        _direction = -_direction;
                        CurrentImageIndex += _direction;
                    }
                    else
                    {
                        CurrentImageIndex = CurrentImageIndex < 0 ? 0 : ImageStack.Bitmaps.Count - 1;
                    }
                }
                else
                {
                    Stop();
                }
            }

            /*
            if (Loop)
            {
                CurrentImageIndex = (CurrentImageIndex + 1) % ImageStack.Bitmaps.Count;
            }
            else
            {
                CurrentImageIndex += 1;

                if (CurrentImageIndex == ImageStack.Bitmaps.Count)
                {
                    Stop();
                }
            }
            */

            FrameUpdate?.Invoke(CurrentImageIndex, CurrentImage);
        }

        public void Stop()
        {
            _timer.Stop();
            _timer.Dispose();

            AnimationStop?.Invoke();
        }
    }
}