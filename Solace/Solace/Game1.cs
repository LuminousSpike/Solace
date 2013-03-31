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
#if (ANDROID)
using Microsoft.Xna.Framework.Input.Touch;
using Android.Content.Res;
#endif

namespace Solace
{
    public enum GameState
    {
        StartMenu,
        OptionsMenu,
        Loading,
        Playing,
        Paused,
        GameOver,
        Exiting
    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // Global Varibles and Constants

#if (ANDROID)
        public static AssetManager assetManager = Android.App.Application.Context.Assets;

        public static GestureSample gesture;
#endif

        
        public static Keys MoveUpKey = Keys.W, MoveDownKey = Keys.S, MoveLeftKey = Keys.A, MoveRightKey = Keys.D;

        public static GameState gameState = new GameState();

        public static float ViewPortWidth = 1920, ViewPortHeight = 1080;

        private GameState LoadedGameState = new GameState();
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private StartMenuScreen startMenu;
        private OptionsMenuScreen optionsMenuScreen;
        private KeyboardState CurrentKeyboardState, PreviousKeyboardState;
        private GameScreen gameScreen;
        private GameOverScreen gameOverScreen;

        private Matrix SpriteScale;

        public Game1()
        {
#if (ANDROID)
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.FreeDrag;
#endif
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.ToggleFullScreen();
            graphics.ApplyChanges();
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
            gameState = GameState.StartMenu;
            LoadedGameState = GameState.StartMenu;

            startMenu = new StartMenuScreen(graphics);
            startMenu.Initialize();

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
            startMenu.LoadContent(Content, GraphicsDevice);
            float screenscaleX = graphics.GraphicsDevice.Viewport.Width / ViewPortWidth;
            float screenscaleY = graphics.GraphicsDevice.Viewport.Height / ViewPortHeight;
            SpriteScale = Matrix.CreateScale(screenscaleX, screenscaleY, 1);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            startMenu.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
#if (ANDROID)
            // Get touch input
            bool gesturing = false;
            while (TouchPanel.IsGestureAvailable)
            {
                gesture = TouchPanel.ReadGesture();
                gesturing = true;
            }

            // Start the game
            if (gesture.GestureType == GestureType.Tap)
            {
                gameState = GameState.Playing;
            }
#endif
            CurrentKeyboardState = Keyboard.GetState();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                gameState = GameState.StartMenu;

            // TODO: Add your update logic here
            if (gameState == GameState.StartMenu)
            {
                if (LoadedGameState != GameState.StartMenu)
                {
                    Content.Unload();
                    startMenu = new StartMenuScreen(graphics);
                    startMenu.Initialize();
                    startMenu.LoadContent(Content, GraphicsDevice);
                    LoadedGameState = GameState.StartMenu;
                }
                startMenu.Update(gameTime);
            }

            if (gameState == GameState.OptionsMenu)
            {
                if (LoadedGameState != GameState.OptionsMenu)
                {
                    Content.Unload();
                    optionsMenuScreen = new OptionsMenuScreen();
                    optionsMenuScreen.Initialize();
                    optionsMenuScreen.LoadContent(Content, GraphicsDevice);
                    LoadedGameState = GameState.OptionsMenu;
                }
                optionsMenuScreen.Update(gameTime);
            }

            if (CurrentKeyboardState.IsKeyUp(Keys.P) && PreviousKeyboardState.IsKeyDown(Keys.P))
            {
                if (gameState == GameState.Paused)
                {
                    gameState = GameState.Playing;
                }
                else if (gameState == GameState.Playing)
                {
                    gameState = GameState.Paused;
                }
            }

            if (gameState == GameState.Playing)
            {
                if (LoadedGameState != GameState.Playing)
                {
                    Content.Unload();
                    gameScreen = new GameScreen(graphics);
                    gameScreen.Initialize(Content, GraphicsDevice);
                    gameScreen.LoadContent(Content, GraphicsDevice);
                    LoadedGameState = GameState.Playing;
                }
                gameScreen.Update(gameTime, GraphicsDevice);
            }

            // Gameover goes to Start Menu for now
            if (gameState == GameState.GameOver)
            {
                if (LoadedGameState != GameState.GameOver)
                {
                    Content.Unload();
                    gameOverScreen = new GameOverScreen();
                    gameOverScreen.Initialize();
                    gameOverScreen.LoadContent(Content, GraphicsDevice);
                    LoadedGameState = GameState.GameOver;
                }
                gameOverScreen.Update(gameTime, GraphicsDevice);
            }

#if (ANDROID)
            if (gesturing == false)
                gesture = new GestureSample();
#endif

            if (gameState == GameState.Exiting)
                this.Exit();
            PreviousKeyboardState = Keyboard.GetState();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            // TODO: Add your drawing code here
            if (gameState == GameState.StartMenu && LoadedGameState == GameState.StartMenu)
                startMenu.Draw(gameTime, spriteBatch, SpriteScale);

            if (gameState == GameState.OptionsMenu && LoadedGameState == GameState.OptionsMenu)
                optionsMenuScreen.Draw(spriteBatch);

            if (gameState == GameState.Playing && LoadedGameState == GameState.Playing || gameState == GameState.Paused)
                gameScreen.Draw(gameTime, GraphicsDevice, spriteBatch, SpriteScale);

            if (gameState == GameState.GameOver && LoadedGameState == GameState.GameOver)
                gameOverScreen.Draw(gameTime, GraphicsDevice, spriteBatch, SpriteScale);

            base.Draw(gameTime);
        }
    }
}
