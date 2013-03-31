using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Solace
{
    class Asteroid
    {
        public Texture2D Texture;
        public Vector2 Position, myOrigin;
        public bool Active, Splitting = false;
        public float Value;
        public int Width, Height, Size, Health;
#if (!ANDROID)
        public Color[] TextureData;
#endif
        public Matrix myTransform;
        public Rectangle CollisionBox;

        public Asteroid(Texture2D texture1, Texture2D texture2, Texture2D texture3, Texture2D texture4)
        {
            Texture1 = texture1;
            Texture2 = texture2;
            Texture3 = texture3;
            Texture4 = texture4;
        }

        private Texture2D Texture1 { get; set; }
        private Texture2D Texture2 { get; set; }
        private Texture2D Texture3 { get; set; }
        private Texture2D Texture4 { get; set; }

        private Random random = new Random();
        private float enemyMoveSpeed { get; set; }
        private int MaxHealth;
        private float Circle = (float)Math.PI * 2;
        private float Rotation;
        private float RotationRate;

        public void Initialize(int size)
        {
            Size = size;
            Value = (float)random.NextDouble();
            SetTexture();
            SetHealth();
            SetSpeed();
            SetRotation();
            Active = true;
        }

        public void Update(GameTime gametime)
        {
            Position.X -= enemyMoveSpeed;
            Rotation += RotationRate;
            Rotation = Rotation % Circle;
            myTransform = Matrix.CreateTranslation(new Vector3(-myOrigin, 0.0f)) * Matrix.CreateRotationZ(Rotation) * Matrix.CreateTranslation(new Vector3(Position, 0.0f));
            CollisionBox = CollisionManager.CalculateBoundingRectangle(new Rectangle(0, 0, Width, Height), myTransform);

            if (Position.X < -Width)
            {
                Active = false;
            }
            if (Health <= 0)
            {
                Split();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, Rotation, myOrigin, 1f, SpriteEffects.None, 0f);
        }

        private void Split()
        {
            if (Size <= 1)
            {
                Active = false;
                Player.Flamoca += Value;
            }
            else
            {
                MaxHealth = MaxHealth / 2;
                Health = MaxHealth;
                Size -= 1;
                SetTexture();
                SetHealth();
                SetSpeed();
                SetRotation();
                int splitChance = random.Next(0, 100);
                if (splitChance < Size * 10)
                    Splitting = true;
            }
        }

        private void SetTexture()
        {
            if (Size == 4)
                Texture = Texture4;
            else if (Size == 3)
                Texture = Texture3;
            else if (Size == 2)
                Texture = Texture2;
            else
                Texture = Texture1;

            Width = Texture.Width;
            Height = Texture.Height;
            myOrigin = new Vector2(Width / 2, Height / 2);
#if (!ANDROID)
            TextureData = new Color[Width * Height];
            Texture.GetData(TextureData);
#endif
        }

        private void SetHealth()
        {
            if (Size == 4)
                Health = 200;
            else if (Size == 3)
                Health = 150;
            else if (Size == 2)
                Health = 100;
            else
                Health = 50;
        }

        private void SetSpeed()
        {
            if (Size == 4)
                enemyMoveSpeed = 0.5f;
            else if (Size == 3)
                enemyMoveSpeed = 1f;
            else if (Size == 2)
                enemyMoveSpeed = 1.5f;
            else
                enemyMoveSpeed = 2f;
        }

        private void SetRotation()
        {
            do
            {
                if (Size == 4)
                    RotationRate = (float)random.Next(-2, 2) / 100;
                else if (Size == 3)
                    RotationRate = (float)random.Next(-4, 4) / 100;
                else if (Size == 2)
                    RotationRate = (float)random.Next(-6, 6) / 100;
                else
                    RotationRate = (float)random.Next(-8, 8) / 100;
            } while (RotationRate == 0);
        }
    }
}
