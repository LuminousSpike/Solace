using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Solace
{
    class ProjectileManager
    {
        public List<Projectile> Projectiles;

        public void AddProjectile(Projectile projectile)
        {
            Projectiles.Add(projectile);
        }

        public void Initialize()
        {
            Projectiles = new List<Projectile>();
        }

        public void Update()
        {
            for (int i = Projectiles.Count - 1; i >= 0; i--)
            {
                Projectiles[i].Update();
                if (Projectiles[i].Active == false)
                {
                    Projectiles.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Projectiles.Count; i++)
            {
                Projectiles[i].Draw(spriteBatch);
            }
        }
    }
}
