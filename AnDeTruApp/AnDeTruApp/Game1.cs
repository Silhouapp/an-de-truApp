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
using AnDeTruSprites;

namespace AnDeTruApp
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        CameraControl _camera;
        SpriteBatch spriteBatch;
        GameBoard _board;
        bool bIsDetectingHand = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            # if !DEBUG
            this.graphics.IsFullScreen = true;
            #endif
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this._camera = new CameraControl(GraphicsDevice);
            this._board = new GameBoard();
            this._camera.GestureCapturedHandler += _camera_GestureCapturedHandler;

            base.Initialize();
        }

        void _camera_GestureCapturedHandler(object sender, GestureEventArgs e)
        {
            var colWidth = GraphicsDevice.Viewport.Bounds.Width / 3;
            var rowHeight = GraphicsDevice.Viewport.Bounds.Height / 3;
            this._board.throwGesture(e.Gesture, ConversionServices.FromLocationToIndex(e.X, e.Y, colWidth,rowHeight));
            this.bIsDetectingHand = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Gestures.InitGestures(this.Content);
            this._board.addGestureView();

        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Clean up
            this._camera.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            // Camera update needs to be closest to base.Update
            this._camera.Update();
            this._board.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Must be the first method to be called.
            this.DrawBackground();
            this.DrawGestures();

            this.FillBox();

            base.Draw(gameTime);
        }

        private void FillBox()
        {
            if (this.bIsDetectingHand)
            {
                int height = GraphicsDevice.PresentationParameters.Bounds.Height / 3;
                int width = GraphicsDevice.PresentationParameters.Bounds.Width / 3;

                this.EmphesizeSquareByNumber(ConversionServices.FromLocationToIndex(this._camera.HandLocation.X + 80,
                                                                                    this._camera.HandLocation.Y,
                                                                                    width,
                                                                                    height).OneDimensional,
                                             Color.Green);
                this.bIsDetectingHand = false;
            }
        }

        private void DrawGestures()
        {
            var colWidth = GraphicsDevice.Viewport.Bounds.Width / 3;
            var rowHeight = GraphicsDevice.Viewport.Bounds.Height / 3;

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
            //this._board.CurrentGestureViews.ForEach(g => spriteBatch.Draw(g.Sprite.Texture, g.Sprite.destRect, g.Sprite.sourceRect, Color.Transparent));
            this._board.CurrentGestureViews.ForEach(gv =>
            {
                var sprite = gv.Sprite;
                var destRect = sprite.destRect;

                var x = gv.Point.X;
                x *= colWidth;
                x += colWidth / 2;
                x -= destRect.Width / 2;

                var y = gv.Point.Y;
                y *= rowHeight;
                y += rowHeight / 2;
                y -= destRect.Height / 2;

                destRect.Offset(x, y);
                spriteBatch.Draw(sprite.Texture, destRect, sprite.sourceRect, Color.White);
            }
            );

            spriteBatch.End();
        }

        /// </summary>
        private void DrawBackground()
        {
            // make sure not to die if null
            if (this._camera.SpriteTexture != null)
            {
                this._camera.SpriteRectangle.Height = GraphicsDevice.PresentationParameters.Bounds.Height;
                this._camera.SpriteRectangle.Width = GraphicsDevice.PresentationParameters.Bounds.Width;

                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
                spriteBatch.Draw(this._camera.SpriteTexture, this._camera.SpriteRectangle, Color.White);
                spriteBatch.End();
            }

            // Drawing lines
            this.InitializeBackgroundTexture();

            // this line needs to be last of method
            this._camera.Draw();
        }

        private void InitializeBackgroundTexture()
        {
            if (this._camera.SpriteTexture == null) return;

            int cols = 3;

            int height = GraphicsDevice.PresentationParameters.Bounds.Height;
            int width = GraphicsDevice.PresentationParameters.Bounds.Width;

            Color color = Color.White;
            color.A = 60;

            Texture2D texture1px = new Texture2D(graphics.GraphicsDevice, 1, 1);
            texture1px.SetData(new Color[] { Color.White });

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);

            for (float x = 0; x < cols - 1; x++)
            {
                Rectangle rectangleX = new Rectangle((int)((width / 3) * (x + 1)), 0, 3, height);
                Rectangle rectangleY = new Rectangle(0, (int)((height / 3) * (x + 1)), width, 3);

                spriteBatch.Draw(texture1px, rectangleX, color);
                spriteBatch.Draw(texture1px, rectangleY, color);
            }

            spriteBatch.End();

        }

        /// <summary>
        /// Emphesize a specific square on the screen
        /// </summary>
        /// <param name="squareId">a number from 0 - 8</param>
        private void EmphesizeSquareByNumber(int squareId, Color color)
        {
            if (squareId != -1 && this._camera.HandLocation.X != 0 && this._camera.HandLocation.Y != 0)
            {
                color.A = 60;

                int row = squareId / 3;
                int col = squareId % 3;

                int height = GraphicsDevice.PresentationParameters.Bounds.Height / 3 - 2;
                int width = GraphicsDevice.PresentationParameters.Bounds.Width / 3 - 2;

                int offsetX = col * width;
                int offsetY = row * height;

                Texture2D texture1px = new Texture2D(graphics.GraphicsDevice, 1, 1);
                texture1px.SetData(new Color[] { Color.White });

                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
                Rectangle rectangle = new Rectangle(offsetX + 4, offsetY + 4, width, height);
                spriteBatch.Draw(texture1px, rectangle, color);
                spriteBatch.End();
            }
        }

        private void PaintByHandPosition()
        {
            int boundHeight;
            int boundWidth;

            if (this._camera.HandLocation.X != 0 && this._camera.HandLocation.Y != 0)
            {
                boundHeight = this._camera.SpriteBitmap.Height;
                boundWidth = this._camera.SpriteBitmap.Width;

                int x = (int)((boundWidth - (this._camera.HandLocation.X + 80)) / (boundWidth / 3));
                int y = (int)((this._camera.HandLocation.Y) / (boundHeight / 3));
                int i = new AnDeTruSprites.Point { X = x, Y = y }.OneDimensional;

                this.EmphesizeSquareByNumber(i, Color.Green);
            }
        }
    }
}
