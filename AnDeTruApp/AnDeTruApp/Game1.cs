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

        // Store some information about the sprite's motion.
        Vector2 spriteSpeed = new Vector2(50.0f, 50.0f);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //this.graphics.IsFullScreen = true;
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

            Gestures.loadGestures(
                new KeyValuePair<Gesture, Texture2D>(new Paper(), this.Content.Load<Texture2D>("Paper2048"))
            );
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

            EmphesizeSquareByNumber(4, Color.Red);

            base.Draw(gameTime);
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
            int rows = 3;

            int height = GraphicsDevice.PresentationParameters.Bounds.Height;
            int width = GraphicsDevice.PresentationParameters.Bounds.Width;

            Texture2D texture1px = new Texture2D(graphics.GraphicsDevice, 1, 1);
            texture1px.SetData(new Color[] { Color.White });

            for (float x = 0; x < cols - 1; x++)
            {
                Rectangle rectangleX = new Rectangle((int)((width / 3) * (x + 1)), 0, 3, height);
                Rectangle rectangleY = new Rectangle(0, (int)((height / 3) * (x + 1)), width, 3);

                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
                spriteBatch.Draw(texture1px, rectangleX, Color.White);
                spriteBatch.Draw(texture1px, rectangleY, Color.White);
                spriteBatch.End();
            }
        }

        /// <summary>
        /// Emphesize a specific square on the screen
        /// </summary>
        /// <param name="squareId">a number from 0 - 8</param>
        private void EmphesizeSquareByNumber(int squareId, Color color)
        {
            if (squareId != -1)
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
    }
}
