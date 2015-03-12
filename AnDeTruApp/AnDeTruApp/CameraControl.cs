using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Drawing;
using System.Diagnostics;
using AnDeTruSprites;

namespace AnDeTruApp
{

    public class GestureEventArgs : EventArgs
    {
        public Gesture Gesture { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
    public class CameraControl
    {
        private Size captureSize = new Size(848, 480);

        public Microsoft.Xna.Framework.Rectangle SpriteRectangle;
        public Texture2D SpriteTexture { get; set; }
        public System.Drawing.Bitmap SpriteBitmap { get; set; }
        public PXCMSenseManager SenseManager { get; set; }

        public Dictionary<Gesture, int> helloHello;
        List<int> Xs = new List<int>();
        List<int> Ys = new List<int>();

        public PXCMHandModule hand  { get; set; }

        public System.Drawing.Point HandLocation;
            
        private GraphicsDevice _gd;
        PXCMHandData handData;

        public event EventHandler<GestureEventArgs> GestureCapturedHandler;
        private int times;

        public CameraControl(GraphicsDevice d)
        {
            this.helloHello = new Dictionary<Gesture, int>();
            this.helloHello.Add(new Rock(), 0);
            this.helloHello.Add(new Scissors(), 0);
            this.helloHello.Add(new Paper(), 0);

            // Save the graphic device
            _gd = d;

            // Create new manager
            this.SenseManager = PXCMSenseManager.CreateInstance();
            this.HandLocation = new System.Drawing.Point();
            // Enable stream
            this.SenseManager.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_COLOR, captureSize.Width, captureSize.Height, 60);

            //Enable Hand
            SenseManager.EnableHand();//D

            // Create to sprite bitemap for copy
            this.SpriteBitmap = new System.Drawing.Bitmap(captureSize.Width, captureSize.Height);

            // set event handler to function
            PXCMSenseManager.Handler handler = new PXCMSenseManager.Handler();
            hand = SenseManager.QueryHand();
            handData = hand.CreateOutput();
            PXCMHandConfiguration handConfiguration = hand.CreateActiveConfiguration();//D
            handConfiguration.EnableGesture("fist", true);//D
            handConfiguration.EnableGesture("spreadfingers", true);//D
            handConfiguration.EnableGesture("v_sign", true);//D
            handConfiguration.EnableTrackedJoints(true);
            handConfiguration.SubscribeGesture(OnFiredGesture);
            handConfiguration.ApplyChanges();


            handler.onNewSample = OnNewSample;
            handler.onModuleProcessedFrame = new PXCMSenseManager.Handler.OnModuleProcessedFrameDelegate(onProcessedFrame);

            this.SenseManager.Init(handler);
        }

        private void OnFiredGesture(PXCMHandData.GestureData gestureData)
        {
            PXCMHandData.JointData[][] nodes = new PXCMHandData.JointData[][] { new PXCMHandData.JointData[0x20], new PXCMHandData.JointData[0x20] };
            PXCMHandData.IHand IHandData;
            int numOfHands = handData.QueryNumberOfHands();
            for (int i = 0; i < numOfHands; i++)
            {
                if (handData.QueryHandData(PXCMHandData.AccessOrderType.ACCESS_ORDER_BY_TIME, i, out IHandData) == pxcmStatus.PXCM_STATUS_NO_ERROR)
                {
                    if (handData != null)
                    {
                        PXCMHandData.JointData jointData;
                        IHandData.QueryTrackedJoint((PXCMHandData.JointType)1, out jointData);
                        nodes[i][1] = jointData;
                        //Debug.WriteLine(nodes[i][1].positionImage.x.ToString() + " " + nodes[i][1].positionImage.y.ToString() + " " + numOfHands.ToString());
                        this.HandLocation.X = (int)nodes[i][1].positionImage.x;
                        this.HandLocation.Y = (int)nodes[i][1].positionImage.y;
                    }
                }
            }
            PXCMHandData.GestureData GestureData;//D
            Gesture g = null;
            if (handData.IsGestureFired("fist", out GestureData))//D
            {
                g = new Rock();
                //iROCK++;
            }

            if (handData.IsGestureFired("spreadfingers", out GestureData))//D
            {
                g = new Paper();
                //iPaper++;
            }

            if (handData.IsGestureFired("v_sign", out GestureData))//D
            {
                g = new Scissors();
                //iScissor++;
            }

            EventHandler<GestureEventArgs> handler = GestureCapturedHandler;
            if (handler != null && g != null)
            {
                helloHello[g]++;
                Xs.Add(this.HandLocation.X);
                Ys.Add(this.HandLocation.Y);
                times++;

                if (times > 10)
                {
                    int xx = Xs[Xs.Count/2];
                    int yy = Ys[Ys.Count/2];
                    times = 0;
                    Xs.Clear();
                    Ys.Clear();
                    Gesture current = helloHello.Keys.Where(x => helloHello[x] == helloHello.Values.Max()).First();
                    Debug.WriteLine(String.Format("GONNA DO {0}", current));
                    helloHello.Keys.ToList().ForEach(gesture => helloHello[gesture] = 0);
                    handler(this, new GestureEventArgs() { Gesture = current, X = xx, Y = yy });
                }
            }

        }

        private pxcmStatus onProcessedFrame(int mid, PXCMBase module, PXCMCapture.Sample sample)
        {
            if (mid == PXCMHandModule.CUID)
            {
                handData.Update();
            }

            // return NO ERROR to continue, or any ERROR to exit the loop
            return pxcmStatus.PXCM_STATUS_NO_ERROR;
        }

        pxcmStatus OnNewSample(int mid, PXCMCapture.Sample sample)
        {

            // work on sample.color
            PXCMImage.ImageData data;
            pxcmStatus stt = sample.color.AcquireAccess(PXCMImage.Access.ACCESS_READ,
                                                        PXCMImage.PixelFormat.PIXEL_FORMAT_RGB32,
                                                        out data);
            // Check for errors
            if (stt != pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                return stt;
            }

            this.SpriteBitmap = data.ToBitmap(0, this.SpriteBitmap);
            sample.color.ReleaseAccess(data);

            this.SpriteTexture = ConversionServices.BitmapToTexture2D(_gd, this.SpriteBitmap);

            // return NO ERROR to continue, or any ERROR to exit the loop
            return pxcmStatus.PXCM_STATUS_NO_ERROR;
        }

        public void UnloadContent()
        {
            this.SenseManager.Dispose();
        }

        public void Update()
        {
            this.SenseManager.AcquireFrame(false);
        }

        /// <summary>
        /// Draws the image of the person on the background.
        /// </summary>
        public void Draw()
        {
            this.SenseManager.ReleaseFrame();
        }
    }
}
