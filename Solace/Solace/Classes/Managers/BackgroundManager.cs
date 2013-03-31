using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Solace
{
    class BackgroundManager
    {
        private ParallaxBackground[] Backgrounds = new ParallaxBackground[6];
        private ParallaxPlanet Planet = new ParallaxPlanet();
        private bool PlanetEnabled;

        public BackgroundManager(bool planetEnabled)
        {
            PlanetEnabled = planetEnabled;
        }

        public void LoadContent(ContentManager content, GraphicsDevice graphics, String Cloud1, String Cloud2)
        {
            Create(content, @"Backgrounds\Stars\Stars1", Game1.ViewPortWidth, -0.1f, 0, 1f);
            Create(content, @"Backgrounds\Stars\Stars2", Game1.ViewPortWidth, -0.3f, 1, 1f);
            Create(content, @"Backgrounds\Stars\Stars3", Game1.ViewPortWidth, -1f, 2, 1f);
            Create(content, Cloud1, Game1.ViewPortWidth, -0.2f, 3, 0.6f);
            Create(content, Cloud2, Game1.ViewPortWidth, -0.6f, 4, 0.4f);
            if (PlanetEnabled)
                Planet.Initialize(content, @"Backgrounds\Planets\Planet3-game", -0.1f, 1f);
        }

        public void Create(ContentManager content, String texturePath, float screenWidth, float speed, int Layer, float alpha)
        {
            ParallaxBackground NewPB = new ParallaxBackground();
            NewPB.Initialize(content, texturePath, speed, alpha);
            Add(NewPB, Layer);
        }

        public void Update()
        {
            for (int i = 0; i < 5; i++)
            {
                Backgrounds[i].Update();
            }
            if (PlanetEnabled)
                Planet.Update();
        }

        public void Draw(SpriteBatch spriteBatch, Matrix spriteScale)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None,
                RasterizerState.CullCounterClockwise, null, spriteScale);
            Backgrounds[0].Draw(spriteBatch);
            if(PlanetEnabled)
                Planet.Draw(spriteBatch);
            for (int i = 3; i < 5; i++)
            {
                Backgrounds[i].Draw(spriteBatch);
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.None,
                RasterizerState.CullCounterClockwise, null, spriteScale);
            for (int i = 1; i < 3; i++)
            {
                Backgrounds[i].Draw(spriteBatch);
            }
            spriteBatch.End();

        }

        private void Add(ParallaxBackground PB, int Layer)
        {
            Backgrounds[Layer] = PB;
        }
    }
}
