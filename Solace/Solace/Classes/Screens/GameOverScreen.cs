using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Solar.GUI;
using Solar.GUI.Containers;
using Solar.GUI.Controls;

namespace Solace
{
    class GameOverScreen
    {
        GuiSystem guiSystem = new GuiSystem();
        Button btnBack;
        private KeyboardState CurrentKeyboardState, PreviousKeyboardState;
        private BackgroundManager backgroundManager = new BackgroundManager(false);

        public void Initialize()
        {
            guiSystem.Initialize();
        }

        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            backgroundManager.LoadContent(Content, graphicsDevice, @"Backgrounds\Clouds\Cloud-Plasma-5", @"Backgrounds\Clouds\Cloud-Plasma-6");
            btnBack = new Button(new Vector2(Game1.ViewPortWidth / 2 - 168, Game1.ViewPortHeight - 89), 336, 69, 0, Color.Black * 0.15f, Color.YellowGreen * 0.15f,
                Color.White, Color.White, "Continue", @"Fonts\StartMenuButtonFont");
            btnBack.IsSelected = true;
            guiSystem.Add(btnBack);
            guiSystem.LoadContent(Content, graphicsDevice);
        }

        public void UnloadContent()
        {
            guiSystem.UnloadContent();
        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            backgroundManager.Update();
            CurrentKeyboardState = Keyboard.GetState();
            // Button actions go here
            if ((CurrentKeyboardState.IsKeyUp(Keys.Enter) && PreviousKeyboardState.IsKeyDown(Keys.Enter)) || (CurrentKeyboardState.IsKeyUp(Keys.E)
                && PreviousKeyboardState.IsKeyDown(Keys.E)))
            {
                if (btnBack.IsSelected)
                    Game1.gameState = GameState.StartMenu;
            }
            PreviousKeyboardState = Keyboard.GetState();

            guiSystem.Update();
        }

        public void Draw(GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Matrix spriteScale)
        {
            // So we know the screen is rendering
            backgroundManager.Draw(spriteBatch, spriteScale);
            spriteBatch.Begin();
            guiSystem.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
