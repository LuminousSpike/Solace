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
using Solar;
using Solar.GUI;
using Solar.GUI.Containers;
using Solar.GUI.Controls;

namespace Solace
{
    class ChallengeMenuScreen
    {
        GuiSystem guiSystem = new GuiSystem();
        ScrollableList ChallengeSelectionList = new ScrollableList(100, 100, 10, 10);
        Button BtnTest;
        

        public void Initialize(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            guiSystem.Initialize();
            ChallengeSelectionList.Initialize();
            
        }

        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            BtnTest = new Button(new Vector2(10, 10), 100, 40, 0, Color.Black * 0.15f, Color.YellowGreen * 0.15f, Color.White, Color.White, "Testing...", @"Fonts\StartMenuButtonFont");
            ChallengeSelectionList.Add(BtnTest);
            guiSystem.Add(ChallengeSelectionList);
            guiSystem.LoadContent(Content, graphicsDevice);
        }

        public void UnloadContent()
        {
            guiSystem.UnloadContent();
        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            
            guiSystem.Update();

            // Update the Scrollable Lists
            
            
            ChallengeSelectionList.Update(gameTime);
        }

        public void Draw(GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Matrix spriteScale)
        {
            // So we know the screen is rendering
            graphicsDevice.Clear(Color.Orange);
            spriteBatch.Begin();
            guiSystem.Draw(spriteBatch);
            spriteBatch.End();

            // Draw Scrollable Lists
            ChallengeSelectionList.Draw(spriteBatch);
        }
    }
}
