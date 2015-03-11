using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace AnDeTruApp
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        Texture2D spriteTexture;
        Rectangle spriteRect;
        System.Drawing.Bitmap bmpSprite;
        SpriteBatch spriteBatch;
        PXCMSenseManager sm;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Create a SenseManager instance
            sm = PXCMSenseManager.CreateInstance();

          
            // Enable depth stream at 320x240x60fps
            sm.EnableStream(PXCMCapture.StreamType.STREAM_TYPE_COLOR, 320, 240, 60);
            this.bmpSprite = new System.Drawing.Bitmap(320, 240);

            // Initialize my event handler
            PXCMSenseManager.Handler handler = new PXCMSenseManager.Handler();

            handler.onNewSample = OnNewSample;
            sm.Init(handler);

        }

        pxcmStatus OnNewSample(int mid, PXCMCapture.Sample sample)
        {
            // work on sample.color
            PXCMImage.ImageData data;
            pxcmStatus stt = sample.color.AcquireAccess(PXCMImage.Access.ACCESS_READ, PXCMImage.PixelFormat.PIXEL_FORMAT_RGB32, out data);
            this.bmpSprite = data.ToBitmap(0, this.bmpSprite);
            sample.color.ReleaseAccess(data);
            this.spriteTexture = ConversionServices.BitmapToTexture2D(GraphicsDevice, this.bmpSprite);

            // return NO ERROR to continue, or any ERROR to exit the loop

            return pxcmStatus.PXCM_STATUS_NO_ERROR;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Clean up
            sm.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            sm.AcquireFrame(false);
            sm.ReleaseFrame();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            if (this.spriteTexture != null)
            {
                this.spriteRect.Height = GraphicsDevice.PresentationParameters.Bounds.Height;
                this.spriteRect.Width = GraphicsDevice.PresentationParameters.Bounds.Width;

                spriteBatch.Begin();
                spriteBatch.Draw(this.spriteTexture, this.spriteRect, Color.White);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
