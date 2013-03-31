using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Solar.GUI;
using Solar.GUI.Controls;

namespace Solace
{
    class HudManager
    {
        Player playerRef;

        GuiSystem guiSystem = new GuiSystem();

        Box boxLifeBars;

        TextBox txtBoxFlamoca;
        TextBox txtBoxScore;
        TextBox txtBoxWaves;

        LifeBar lfbHealthBar;
        LifeBar lfbShieldBar;

        public HudManager(Player player)
        {
            playerRef = player;
        }

        public void Initialize()
        {
            guiSystem.Initialize();
            txtBoxScore = new TextBox(new Vector2(10, 10), 137, 29, 1, Color.Black * 0.39f, new Color(98, 0, 0), "Score: " + Player.Score, @"Fonts\HUDFont");
            txtBoxFlamoca = new TextBox(new Vector2(10, 49), 137, 29, 1, Color.Black * 0.39f, new Color(98, 0, 0), "Flamoca: " + Player.Flamoca, @"Fonts\HUDFont");
        }

        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            txtBoxWaves = new TextBox(new Vector2(Game1.ViewPortWidth - 147, Game1.ViewPortHeight - 39), 137, 29, 1, Color.Black * 0.39f, new Color(98, 0, 0), "Waves: " + SpawnManager.WaveCount, @"Fonts\HUDFont");
            boxLifeBars = new Box(new Vector2(10, Game1.ViewPortHeight - 54), 162, 44, 1, Color.Black * 0.39f, new Color(98, 0, 0), graphicsDevice);
            lfbHealthBar = new LifeBar(new Vector2(28, Game1.ViewPortHeight - 45), playerRef.myShip.Health, @"Sprites\GUI\Health-Bar");
            lfbShieldBar = new LifeBar(new Vector2(28, Game1.ViewPortHeight - 29), playerRef.myShip.Shield, @"Sprites\GUI\Shield-Bar");

            guiSystem.Add(txtBoxScore);
            guiSystem.Add(txtBoxFlamoca);
            guiSystem.Add(txtBoxWaves);
            guiSystem.Add(boxLifeBars);
            guiSystem.Add(lfbHealthBar);
            guiSystem.Add(lfbShieldBar);

            guiSystem.LoadContent(content, graphicsDevice);
        }

        public void UnloadContent()
        {
            guiSystem.UnloadContent();
        }

        public void Update(GraphicsDevice graphicsDevice)
        {
            txtBoxScore.UpdateText("Score: " + Player.Score, graphicsDevice);
            txtBoxFlamoca.UpdateText("Flamoca: " + Math.Round((double)Player.Flamoca, 2), graphicsDevice);
            txtBoxWaves.UpdateText("Wave: " + SpawnManager.WaveCount, graphicsDevice);
            lfbHealthBar.Update(playerRef.myShip.Health, playerRef.myShip.MaxHealth);
            lfbShieldBar.Update((int)playerRef.myShip.Shield, (int)playerRef.myShip.MaxShield);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            guiSystem.Draw(spritebatch);
        }
    }
}
