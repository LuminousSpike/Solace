using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Solace
{
    class CollisionManager
    {
        public void Update(Player player, List<Enemy> Enemies, List<Asteroid> Astroids, List<Projectile> Projectiles)
        {
            for (int i = 0; i < Projectiles.Count; i++)
            {
                // Enemies
                for (int ii = 0; ii < Enemies.Count; ii++)
                {
                    if (Projectiles[i].CollisionBox.Intersects(Enemies[ii].myShip.CollisionBox) && Projectiles[i].ShooterShip != Enemies[ii].myShip && Projectiles[i].EnemyProjectile == false)
                    {
#if(!ANDROID)
                        if (IntersectPixels(Projectiles[i].myTransform, Projectiles[i].Width, Projectiles[i].Height, Projectiles[i].TextureData, Enemies[ii].myShip.myTransform, Enemies[ii].myShip.ShipTexture.Width, Enemies[ii].myShip.ShipTexture.Height, Enemies[ii].myShip.TextureData))
                        {
                            Projectiles[i].Active = false;
                            DamageShip(Projectiles[i].ProjectileDamage, Enemies[ii].myShip);
                        }
#else
                        Projectiles[i].Active = false;
                        DamageShip(Projectiles[i].ProjectileDamage, Enemies[ii].myShip);
#endif
                    }
                }

                // Player
                if (Projectiles[i].CollisionBox.Intersects(player.myShip.CollisionBox) && Projectiles[i].ShooterShip != player.myShip)
                {
#if(!ANDROID)
                    if (IntersectPixels(Projectiles[i].myTransform, Projectiles[i].Width, Projectiles[i].Height, Projectiles[i].TextureData, player.myShip.myTransform, player.myShip.ShipTexture.Width, player.myShip.ShipTexture.Height, player.myShip.TextureData))
                    {
                        Projectiles[i].Active = false;
                        DamageShip(Projectiles[i].ProjectileDamage, player.myShip);
                    }
#else
                    Projectiles[i].Active = false;
                    DamageShip(Projectiles[i].ProjectileDamage, player.myShip);
#endif
                }

                // Astroids
                for (int ii = 0; ii < Astroids.Count; ii++)
                {
                    if (Projectiles[i].CollisionBox.Intersects(Astroids[ii].CollisionBox) && Projectiles[i].EnemyProjectile == false)
                    {
#if(!ANDROID)
                        if (IntersectPixels(Projectiles[i].myTransform, Projectiles[i].Width, Projectiles[i].Height, Projectiles[i].TextureData, Astroids[ii].myTransform, Astroids[ii].Width, Astroids[ii].Height, Astroids[ii].TextureData))
                        {
                            Projectiles[i].Active = false;
                            Astroids[ii].Health -= Projectiles[i].ProjectileDamage;
                        }
#else
                        Projectiles[i].Active = false;
                        Astroids[ii].Health -= Projectiles[i].ProjectileDamage;
#endif
                    }
                }
            }

            for (int i = 0; i < Enemies.Count; i++)
            {
                if (player.myShip.CollisionBox.Intersects(Enemies[i].myShip.CollisionBox))
                {
#if(!ANDROID)
                    if (IntersectPixels(player.myShip.myTransform, player.myShip.ShipTexture.Width, player.myShip.ShipTexture.Height, player.myShip.TextureData, Enemies[i].myShip.myTransform, Enemies[i].myShip.ShipTexture.Width, Enemies[i].myShip.ShipTexture.Height, Enemies[i].myShip.TextureData))
                    {
                        Enemies[i].myShip.Health = 0;
                        DamageShip(50, player.myShip); // Replace hard coded value with a variable
                    }
#else
                    Enemies[i].myShip.Health = 0;
                    DamageShip(50, player.myShip); // Replace hard coded value with a variable
#endif
                }
            }

            for (int i = 0; i < Astroids.Count; i++)
            {
                if (player.myShip.CollisionBox.Intersects(Astroids[i].CollisionBox))
                {
#if(!ANDROID)
                    if (IntersectPixels(player.myShip.myTransform, player.myShip.ShipTexture.Width, player.myShip.ShipTexture.Height, player.myShip.TextureData, Astroids[i].myTransform, Astroids[i].Width, Astroids[i].Height, Astroids[i].TextureData))
                    {
                        Astroids[i].Health = 0;
                        DamageShip(25 * Astroids[i].Size, player.myShip); // Replace hard coded value with a variable
                    }
#else
                    Astroids[i].Health = 0;
#endif
                }
            }

            // Check for game over
            if (player.myShip.Health <= 0)
                Game1.gameState = GameState.GameOver;
        }

        private void DamageShip(int damageDealt, Ship ship)
        {
            float damage = damageDealt - ship.Shield;
            ship.Shield -= damageDealt;
            if (damage > 0)
            {
                ship.Health -= (int)damage;
            }
        }

        public static Rectangle CalculateBoundingRectangle(Rectangle rectangle, Matrix transform)
        {
            // Get all four corners in local space
            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum anf maximum extents of the rectanggle in world space
            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop), Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop), Vector2.Max(leftBottom, rightBottom));

            // Return as a rectangle
            return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        private bool IntersectPixels(Matrix transformA, int widthA, int heightA, Color[] dataA, Matrix transformB, int widthB, int heightB, Color[] dataB)
        {
            // Calculate a matrix which reansforms from A's local space into world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++)
            {
                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++)
                {
                    // Calculate this pixel's location in B
                    Vector2 positionInB = Vector2.Transform(new Vector2(xA, yA), transformAToB);

                    // Round to the nearest pixel
                    int xB = (int)Math.Round(positionInB.X);
                    int yB = (int)Math.Round(positionInB.Y);

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < widthB && 0 <= yB && yB < heightB)
                    {
                        // Get the colors of the overlapping pixels
                        Color colorA = dataA[xA + yA * widthA];
                        Color colorB = dataB[xB + yB * widthB];

                        // If both pixels are not completely transparent
                        if (colorA.A != 0 && colorB.A != 0)
                            return true;
                    }
                }
            }

            // No intersection has been found
            return false;
        }
    }
}
