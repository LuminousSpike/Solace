using Microsoft.Xna.Framework;

namespace Solace
{
    class ParallaxPlanet : ParallaxBackground
    {
        public override void Initialize(Microsoft.Xna.Framework.Content.ContentManager content, string texturePath, float speed, float alpha)
        {
            base.Initialize(content, texturePath, speed, 100f);
            positions = new Vector2[1];
            positions[0] = new Vector2(Game1.ViewPortWidth - 20, 200);
        }

        public override void Update()
        {
            positions[0].X += speed;
        }
    }
}
