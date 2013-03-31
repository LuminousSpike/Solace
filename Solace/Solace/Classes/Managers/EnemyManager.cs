using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Solace
{
    class EnemyManager
    {
        public List<Texture2D> EnemyTexture;
        public List<Enemy> Enemies;
        public List<Asteroid> Asteroids;

        private GraphicsDevice graphicsDevice { get; set; }
        private Texture2D AsteroidTexture1, AsteroidTexture2, AsteroidTexture3, AsteroidTexture4;

        public EnemyManager(GraphicsDevice graphicsdevice)
        {
            graphicsDevice = graphicsdevice;
        }

        public void Initialize(ContentManager content)
        {
            LoadContent(content);
            EnemyTexture = new List<Texture2D>();

            Enemies = new List<Enemy>();
            Asteroids = new List<Asteroid>();
        }

        public void Update(GameTime gameTime, ProjectileManager projectilemanager)
        {
            UpdateEnemies(gameTime, projectilemanager);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Asteroids.Count; i++)
            {
                Asteroids[i].Draw(spriteBatch);
            }

            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].Draw(spriteBatch);
            }
        }

        public void AddEnemy(Random random, Ship ship)
        {
            Enemy enemy = new Enemy(ship);
            Vector2 position = new Vector2(Game1.ViewPortWidth + enemy.myShip.ShipTexture.Width / 2, random.Next(100, (int)Game1.ViewPortHeight - 100));
            enemy.LoadContent(position);
            Enemies.Add(enemy);
        }

        public void AddAsteroid(Random random)
        {
            Asteroid astroid = new Asteroid(AsteroidTexture1, AsteroidTexture2, AsteroidTexture3, AsteroidTexture4);
            astroid.Initialize(random.Next(1, 5));
            astroid.Position = new Vector2(Game1.ViewPortWidth + astroid.Texture.Width / 2, random.Next(100, (int)Game1.ViewPortHeight - 100));
            Asteroids.Add(astroid);
        }

        public void SplitAsteroid(int size, Vector2 position)
        {
            Asteroid astroid = new Asteroid(AsteroidTexture1, AsteroidTexture2, AsteroidTexture3, AsteroidTexture4);
            astroid.Initialize(size);
            astroid.Position = position;
            Asteroids.Add(astroid);
        }

        private void UpdateEnemies(GameTime gameTime, ProjectileManager projectilemanager)
        {
            for (int i = Asteroids.Count - 1; i >= 0; i--)
            {
                if (Asteroids[i].Splitting == true)
                {
                    Random random = new Random();
                    SplitAsteroid(Asteroids[i].Size, Asteroids[i].Position + new Vector2(0, -25 * Asteroids[i].Size));
                    Asteroids[i].Position += new Vector2(0, 25 * Asteroids[i].Size);
                    Asteroids[i].Splitting = false;
                }

                Asteroids[i].Update(gameTime);

                if (Asteroids[i].Active == false)
                {
                    Asteroids.RemoveAt(i);
                }

            }

            for (int i = Enemies.Count - 1; i >= 0; i--)
            {
                Enemies[i].Update(gameTime, projectilemanager);

                if (Enemies[i].Active == false)
                {
                    Enemies.RemoveAt(i);
                }
            }
        }

        public void LoadContent(ContentManager content)
        {
            AsteroidTexture1 = content.Load<Texture2D>(@"Sprites\Asteroids\Asteroid-1");
            AsteroidTexture2 = content.Load<Texture2D>(@"Sprites\Asteroids\Asteroid-2");
            AsteroidTexture3 = content.Load<Texture2D>(@"Sprites\Asteroids\Asteroid-3");
            AsteroidTexture4 = content.Load<Texture2D>(@"Sprites\Asteroids\Asteroid-4");
        }
    }
}
