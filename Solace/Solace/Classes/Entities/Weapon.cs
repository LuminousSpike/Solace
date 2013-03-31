using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Solace
{
    class Weapon
    {
        public string WeaponName = null, ProjectileTexturePath = null;
        public float OffsetX { get; set; }
        public float OffsetY { get; set; }
        public Texture2D ProjectileTexture { get; set; }

        private string WeaponType = null, ProjectileType = null;
        private int WeaponClass = 0, ProjectileDamage = 0, fired = 0;
        private float WeaponRefireRate = 0f, ProjectileSpeed = 0f;
        private TimeSpan FireTime;
        private TimeSpan PreviousFireTime = new TimeSpan();
        
        public Weapon(string weaponType, string weaponName, int weaponClass, float weaponRefireRate, string projectileTexture,
            string projectileType, int projectileDamage, float projectileSpeed)
        {
            WeaponType = weaponType;
            WeaponName = weaponName;
            WeaponClass = weaponClass;
            WeaponRefireRate = weaponRefireRate;

            ProjectileTexturePath = projectileTexture;
            ProjectileType = projectileType;
            ProjectileDamage = projectileDamage;
            ProjectileSpeed = projectileSpeed;

            FireTime = TimeSpan.FromSeconds((double)WeaponRefireRate);
        }

        public Weapon ShallowCopy()
        {
            Weapon other = (Weapon)this.MemberwiseClone();
            return other;
        }


        public void fire(GameTime gameTime, ProjectileManager projectileManager, Vector2 postition, bool isplayer, Ship ship)
        {
            if (gameTime.TotalGameTime - this.PreviousFireTime > FireTime)
            {
                float projectileSpeed;
                if (isplayer)
                    projectileSpeed = ProjectileSpeed;
                else
                    projectileSpeed = -ProjectileSpeed;
                Projectile myProjectile = new Projectile(ProjectileTexture, ProjectileType, projectileSpeed, ProjectileDamage, postition, ship, !isplayer);
                myProjectile.Position.X += OffsetX;
                myProjectile.Position.Y += OffsetY;
                this.PreviousFireTime = gameTime.TotalGameTime;
                projectileManager.AddProjectile(myProjectile);
                fired++;
            }
        }
    }
}
