using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Solace
{
    class Projectile
    {
        public Vector2 Position;
        public int ProjectileDamage;
        public bool Active = true, EnemyProjectile;
        public Ship ShooterShip;
        public Matrix myTransform;
        public Rectangle CollisionBox;
#if (WINDOWS)
        public Color[] TextureData;
#endif

        private Texture2D ProjectileTexture;
        private string ProjectileType;
        private float ProjectileSpeed;

        public int Width
        {
            get { return ProjectileTexture.Width; }
        }

        public int Height
        {
            get { return ProjectileTexture.Height; }
        }

        public Projectile(Texture2D projectileTexture, string projectileType, float projectileSpeed, int projectileDamage, Vector2 projectilePostition, Ship ship, bool enemyProjectile = false)
        {
            ProjectileTexture = projectileTexture;
            ProjectileType = projectileType;
            ProjectileSpeed = projectileSpeed;
            ProjectileDamage = projectileDamage;
            Position = projectilePostition;
            ShooterShip = ship;
            EnemyProjectile = enemyProjectile;
#if (WINDOWS)
            TextureData = new Color[ProjectileTexture.Width * ProjectileTexture.Height];
            ProjectileTexture.GetData(TextureData);
#endif
        }

        public void Update()
        {
            Position.X += ProjectileSpeed;
            myTransform = Matrix.CreateTranslation(new Vector3(Position, 0.0f));
            CollisionBox = CollisionManager.CalculateBoundingRectangle(new Rectangle(0, 0, ProjectileTexture.Width, ProjectileTexture.Height), myTransform);
            if (Position.X - Width / 2 > Game1.ViewPortWidth || Position.X + Width / 2 < 0)
                Active = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ProjectileTexture, Position, null, Color.White, 0f, new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 0f);
        }
    }
}
