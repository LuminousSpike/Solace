using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Solace
{
    class ParallaxBackground
    {
        // The image
        public Texture2D texture;

        // Array of positions of the parallax background
        public Vector2[] positions;

        // The speed
        public float speed, textureAlpha;

        private int viewHeight;

        public virtual void Initialize(ContentManager content, String texturePath, float speed, float alpha)
        {
            // Load the texture
            texture = content.Load<Texture2D>(texturePath);

            // Set the alpha
            textureAlpha = alpha;
            // Set the speed of the background
            this.speed = speed;

            // Divide the screen width to determine how many tiles needed
            // Add 1 so there is no gap in the tiling
            positions = new Vector2[(int)Game1.ViewPortWidth / texture.Width + 2];
            viewHeight = (int)(Game1.ViewPortHeight / texture.Height) + 1;

            // Set the inital positions
            for (int i = 0; i < positions.Length; i++)
            {
                // The tiles need to be side by side to create a tiling effect
                positions[i] = new Vector2(i * texture.Width, 0);
            }
        }

        public virtual void Update()
        {
            // Update the positions of the background
            for (int i = 0; i < positions.Length; i++)
            {
                // Update the position of the screen by adding the speed
                positions[i].X += speed;

                // If the speed has the background moving to the left
                if (speed <= 0)
                {
                    // Check the texture is out of view then put that texture at the end of the screen
                    if (positions[i].X <= -texture.Width)
                    {
                        positions[i].X = texture.Width * (positions.Length - 1);
                    }
                }

                // If the speed has the background moving to the right
                else
                {
                    // Check if the texture is out of view then position it to the start of the screen
                    if (positions[i].X >= texture.Width * (positions.Length - 1))
                    {
                        positions[i].X = -texture.Width;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                for (int ii = 0; ii < viewHeight; ii++)
                {
                    positions[i].Y = texture.Height * ii;
                    spriteBatch.Draw(texture, positions[i], Color.White * textureAlpha);
                }
            }
        }
    }
}
