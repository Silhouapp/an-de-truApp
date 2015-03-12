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
using System.Timers;

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
        const string fist_gesture = "fist";
        const string v_gesture = "v_sign";
        const string spread_gesture = "spreadfingers";

        private Size captureSize = new Size(848, 480);

        public Microsoft.Xna.Framework.Rectangle SpriteRectangle;
        public Texture2D SpriteTexture { get; set; }
        public System.Drawing.Bitmap SpriteBitmap { get; set; }
        public PXCMSenseManager SenseManager { get; set; }

        public PXCMHandModule hand  { get; set; }

        public System.Drawing.Point HandLocation;
            
        private GraphicsDevice _gd;
        PXCMHandData handData;

        public event EventHandler<GestureEventArgs> GestureCapturedHandler;
        private float[][] arrX;
        private float[][] arrY;
        private int[][] arrGestureTypeCount;
        private int nFrameCount = 0;
        private int[] nHandGestureCount;

        private Dictionary<string, int> gestureCount = new Dictionary<string, int>();

        public CameraControl(GraphicsDevice d)
        {
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
            handConfiguration.EnableGesture(fist_gesture, true);//D
            handConfiguration.EnableGesture(spread_gesture, true);//D
            handConfiguration.EnableGesture(v_gesture, true);//D
            handConfiguration.EnableTrackedJoints(true);
            handConfiguration.SubscribeGesture(OnFiredGesture);
            handConfiguration.ApplyChanges();
            arrX = new float[2][];
            arrY = new float[2][];
            arrX[0] = new float[60];
            arrY[0] = new float[60];
            arrX[1] = new float[60];
            arrY[1] = new float[60];
            nHandGestureCount = new int[2];
            arrGestureTypeCount = new int[2][];
            arrGestureTypeCount[0] = new int[3];
            arrGestureTypeCount[1] = new int[3];

            this.gestureCount.Add(fist_gesture, 0);
            this.gestureCount.Add(v_gesture, 0);
            this.gestureCount.Add(spread_gesture, 0);

            handler.onNewSample = OnNewSample;
            handler.onModuleProcessedFrame = new PXCMSenseManager.Handler.OnModuleProcessedFrameDelegate(onProcessedFrame);

            this.SenseManager.Init(handler);
        }

        private void OnFiredGesture(PXCMHandData.GestureData gestureData)
        {
            PXCMHandData.JointData[][] nodes = new PXCMHandData.JointData[][] { new PXCMHandData.JointData[0x20], new PXCMHandData.JointData[0x20] };
            PXCMHandData.IHand IHandData;
            int numOfHands = handData.QueryNumberOfHands();
            //for (int i = 0; i < numOfHands; i++)
            //{
            if (handData.QueryHandData(PXCMHandData.AccessOrderType.ACCESS_ORDER_BY_ID, 0, out IHandData) == pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                if (handData != null)
                {
                    PXCMHandData.JointData jointData;
                    IHandData.QueryTrackedJoint(PXCMHandData.JointType.JOINT_CENTER, out jointData);
                    nodes[0][1] = jointData;
                    //Debug.WriteLine(nodes[i][1].positionImage.x.ToString() + " " + nodes[i][1].positionImage.y.ToString() + " " + numOfHands.ToString());
                    this.HandLocation.X = (int)nodes[0][1].positionImage.x;
                    this.HandLocation.Y = (int)nodes[0][1].positionImage.y;
                    nHandGestureCount[0]++;

                    this.gestureCount[gestureData.name]++;
                }
                //}
            }

            if (nFrameCount >= 7)
            {
                inzOutPut();
            }
        }

        private void inzOutPut()
        {
            Gesture g = calcDominintGesture(0);
            //int nXMedian = (int)calcMedian(arrX[0], nHandGestureCount[0]);
            //int nYMedian = (int)calcMedian(arrY[0], nHandGestureCount[0]);

            EventHandler<GestureEventArgs> handler = GestureCapturedHandler;
            if (handler != null && g != null)
            {
                handler(this, new GestureEventArgs() { Gesture = g, X = this.HandLocation.X, Y = this.HandLocation.Y });
            }
        }

        private float calcMedian(float[] array,int HandGestureCount)
        {
            Array.Sort(array, 0, HandGestureCount);
            return array[HandGestureCount / 2]; ;
        }


        private Gesture calcDominintGesture(int ihand)
        {
            Gesture DominintGesture = null;

            List<KeyValuePair<string, int>> myList = this.gestureCount.ToList();

            myList.Sort((first, next) =>
            {
                return first.Value.CompareTo(next.Value);
            });

            if (myList[2].Key == v_gesture) return new Scissors();
            else if (myList[2].Key == fist_gesture) return new Rock();
            else if (myList[2].Key == spread_gesture) return new Paper();

            this.gestureCount[v_gesture] = 0;
            this.gestureCount[fist_gesture] = 0;
            this.gestureCount[spread_gesture] = 0;

            return DominintGesture;
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
            nFrameCount++;
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
            this.SenseManager.AcquireFrame(true);
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
