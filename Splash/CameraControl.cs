using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using AnDeTruApp;
using System.Threading;

namespace Splash
{
    public class CameraControl
    {
        private Size captureSize = new Size(320, 240);

        public System.Drawing.Bitmap SpriteBitmap { get; set; }
        public PXCMSenseManager SenseManager { get; set; }
        public PXCMHandModule hand { get; set; }

        PXCMHandData handData;

        public CameraControl()
        {

            // Create new manager
            this.SenseManager = PXCMSenseManager.CreateInstance();

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

            handler.onModuleProcessedFrame = new PXCMSenseManager.Handler.OnModuleProcessedFrameDelegate(onProcessedFrame);

            this.SenseManager.Init(handler);

            this.SenseManager.StreamFrames(false);
        }

        private void OnFiredGesture(PXCMHandData.GestureData gestureData)
        {
            PXCMHandData.JointData[][] nodes = new PXCMHandData.JointData[][] { new PXCMHandData.JointData[0x20], new PXCMHandData.JointData[0x20] };
            PXCMHandData.IHand IHandData;
            int numOfHands = handData.QueryNumberOfHands();

            PXCMHandData.GestureData GestureData;//D

            if (handData.IsGestureFired("spreadfingers", out GestureData))//D
            {
                Game1 game = new AnDeTruApp.Game1();
                game.Run();
                //this.SenseManager.Dispose();
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

    }
}