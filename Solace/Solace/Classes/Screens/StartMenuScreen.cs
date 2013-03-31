using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Solar.GUI;
using Solar.GUI.Controls;

namespace Solace
{
    // All the stuff that a player expects to be on a start menu: New Game, Continue, Options, and Exit Game.
    class StartMenuScreen
    {
        private GraphicsDeviceManager graphics;

        private BackgroundManager backgroundManager = new BackgroundManager(false);

        private GuiSystem guiSystem = new GuiSystem();
        private Box boxHeaderLine;
        private Button btnNewGame, btnContinue, btnOptions, btnCredits, btnExit;

        private SpriteFont HeaderFont, HUDFont;
        private Vector2 HeaderPosition, MadeByPosition;
        private KeyboardState CurrentKeyboardState, PreviousKeyboardState;
        private string MenuTitle = "Solace";

        public StartMenuScreen(GraphicsDeviceManager graphicsDeviceManager)
        {
            graphics = graphicsDeviceManager;
        }

        // Set up anything which does not load content
        public void Initialize()
        {
            guiSystem.Initialize();
        }

        // Loading the content and setting up said content
        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            backgroundManager.LoadContent(Content, graphicsDevice, @"Backgrounds\Clouds\Cloud-Plasma-1", @"Backgrounds\Clouds\Cloud-Plasma-2");

            HeaderFont = Content.Load<SpriteFont>(@"Fonts\StartMenuHeaderFont");
            HUDFont = Content.Load<SpriteFont>(@"Fonts\HUDFont");

            HeaderPosition = new Vector2(Game1.ViewPortWidth / 2 - (HeaderFont.MeasureString(MenuTitle).X / 2), Game1.ViewPortHeight / 20);
            MadeByPosition = new Vector2(10, Game1.ViewPortHeight - HUDFont.MeasureString("Made by Nathan Todd").Y);

            boxHeaderLine = new Box(new Vector2(Game1.ViewPortWidth / 2 - (float)((
                HeaderFont.MeasureString(MenuTitle).X * 1.3) / 2), Game1.ViewPortHeight / 6),
                (int)(HeaderFont.MeasureString(MenuTitle).X * 1.3), 2, 0, Color.White * 0.4f, Color.White, graphicsDevice);

            btnNewGame = new Button(new Vector2(Game1.ViewPortWidth / 2 - 168, Game1.ViewPortHeight / 3), 336, 69, 0, Color.Black * 0.3f, Color.YellowGreen * 0.3f,
                Color.White, Color.White * 0.6f, "New Game", @"Fonts\StartMenuButtonFont");

            btnOptions = new Button(new Vector2(Game1.ViewPortWidth / 2 - 168, Game1.ViewPortHeight / 3 + 69), 336, 69, 0, Color.Black * 0.15f, Color.YellowGreen * 0.15f,
                Color.White, Color.White * 0.6f, "Options", @"Fonts\StartMenuButtonFont");

            btnExit = new Button(new Vector2(Game1.ViewPortWidth / 2 - 168, Game1.ViewPortHeight / 3 + 138), 336, 69, 0, Color.Black * 0.3f, Color.YellowGreen * 0.3f,
                Color.White, Color.White * 0.6f, "Exit", @"Fonts\StartMenuButtonFont");
            
            guiSystem.Add(boxHeaderLine);
            guiSystem.Add(btnNewGame);
            guiSystem.Add(btnOptions);
            guiSystem.Add(btnExit);

            guiSystem.LoadContent(Content, graphicsDevice);
            guiSystem.ButtonIndexUpdate(0);
        }

        public void UnloadContent()
        {
            guiSystem.UnloadContent();
        }

        // Manage the GUI here, all the buttons and player input.
        public void Update(GameTime gameTime)
        {
            backgroundManager.Update();

            CurrentKeyboardState = Keyboard.GetState();
            // Button actions go here
            if ((CurrentKeyboardState.IsKeyUp(Keys.Enter) && PreviousKeyboardState.IsKeyDown(Keys.Enter)) || (CurrentKeyboardState.IsKeyUp(Keys.E)
                && PreviousKeyboardState.IsKeyDown(Keys.E)))
            {
                if (btnNewGame.IsSelected)
                    Game1.gameState = GameState.Playing;

                //if (btnOptions.IsSelected)
                    //Game1.gameState = Game1.GameState.OptionsMenu;

                if (btnExit.IsSelected)
                    Game1.gameState = GameState.Exiting;
            }
            PreviousKeyboardState = Keyboard.GetState();
            guiSystem.Update();
            
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix spriteScale)
        {
            backgroundManager.Draw(spriteBatch, spriteScale);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None,
                RasterizerState.CullCounterClockwise, null, spriteScale);
            spriteBatch.DrawString(HeaderFont, MenuTitle, HeaderPosition, Color.White * 0.6f);
            spriteBatch.DrawString(HUDFont, "Made by Nathan Todd", MadeByPosition, Color.White * 0.6f);
            guiSystem.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
