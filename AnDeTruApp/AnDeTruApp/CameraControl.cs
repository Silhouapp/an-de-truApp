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
    enum enmGesture
    {
        Fist,
        SpreadHand,
        VShape
    }
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
            handConfiguration.EnableGesture("fist", true);//D
            handConfiguration.EnableGesture("spreadfingers", true);//D
            handConfiguration.EnableGesture("v_sign", true);//D
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
                if (handData.QueryHandData(PXCMHandData.AccessOrderType.ACCESS_ORDER_BY_ID, i, out IHandData) == pxcmStatus.PXCM_STATUS_NO_ERROR)
                {
                    if (handData != null)
                    {
                        PXCMHandData.JointData jointData;
                        IHandData.QueryTrackedJoint(PXCMHandData.JointType.JOINT_CENTER, out jointData);
                        nodes[i][1] = jointData;
                        //Debug.WriteLine(nodes[i][1].positionImage.x.ToString() + " " + nodes[i][1].positionImage.y.ToString() + " " + numOfHands.ToString());
                        arrX[i][nHandGestureCount[i]] = nodes[i][1].positionImage.x;
                        arrY[i][nHandGestureCount[i]] = nodes[i][1].positionImage.y;
                        nHandGestureCount[i]++;
                        PXCMHandData.GestureData GestureData;//D

                        if (handData.IsGestureFired("fist", out GestureData))//D
                        {
                            arrGestureTypeCount[i][(int)enmGesture.Fist]++;
                        }
                        else if (handData.IsGestureFired("spreadfingers", out GestureData))//D
                        {
                            arrGestureTypeCount[i][(int)enmGesture.SpreadHand]++;
                        }
                        else if (handData.IsGestureFired("v_sign", out GestureData))//D
                        {
                            arrGestureTypeCount[i][(int)enmGesture.VShape]++;
                        }
                    }

                }

            }
            if (nFrameCount >= 7)
            {
                inzOutPut();
            }
        }

        private void inzOutPut()
        {
            Gesture g = calcDominintGesture(0);
            int nXMedian = (int)calcMedian(arrX[0], nHandGestureCount[0]);
            int nYMedian = (int)calcMedian(arrY[0], nHandGestureCount[0]);

            EventHandler<GestureEventArgs> handler = GestureCapturedHandler;
            if (handler != null && g != null)
            {
                handler(this, new GestureEventArgs() { Gesture = g, X = nXMedian, Y = nYMedian });
            }
            if(handData.QueryNumberOfHands() == 2)
            {
                g = calcDominintGesture(1);
                nXMedian = (int)calcMedian(arrX[1], nHandGestureCount[1]);
                nYMedian = (int)calcMedian(arrY[1], nHandGestureCount[1]);
                
                if (handler != null && g != null)
                {
                    this.HandLocation.X = nXMedian;
                    this.HandLocation.Y = nYMedian;
                    handler(this, new GestureEventArgs() { Gesture = g, X = nXMedian, Y = nYMedian });
                }
            }
            Array.Clear(arrGestureTypeCount[0],0,3);
            Array.Clear(arrGestureTypeCount[1], 0, 3);
            Array.Clear(nHandGestureCount,0,2);
            nFrameCount = 0;
            Array.Clear(arrX[0],0,60);
            Array.Clear(arrX[1], 0, 60);
            Array.Clear(arrY[0], 0, 60);
            Array.Clear(arrY[1], 0, 60);
        }

        private float calcMedian(float[] array,int HandGestureCount)
        {
            Array.Sort(array, 0, HandGestureCount);
            return array[HandGestureCount / 2]; ;
        }

        private Gesture calcDominintGesture(int ihand)
        {
            Gesture DominintGesture = null;
            if ((arrGestureTypeCount[ihand][(int)enmGesture.Fist] > arrGestureTypeCount[ihand][(int)enmGesture.SpreadHand])
                && (arrGestureTypeCount[ihand][(int)enmGesture.Fist] > arrGestureTypeCount[ihand][(int)enmGesture.VShape]))
            {
                DominintGesture = new Rock();
            }
            else if ((arrGestureTypeCount[ihand][(int)enmGesture.SpreadHand] > arrGestureTypeCount[ihand][(int)enmGesture.Fist])
                && (arrGestureTypeCount[ihand][(int)enmGesture.SpreadHand] > arrGestureTypeCount[ihand][(int)enmGesture.VShape]))
            {
                DominintGesture = new Paper();
            }
            else if ((arrGestureTypeCount[ihand][(int)enmGesture.VShape] > arrGestureTypeCount[ihand][(int)enmGesture.Fist])
                && (arrGestureTypeCount[ihand][(int)enmGesture.VShape] > arrGestureTypeCount[ihand][(int)enmGesture.SpreadHand]))
            {
                DominintGesture = new Scissors();
            }
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
