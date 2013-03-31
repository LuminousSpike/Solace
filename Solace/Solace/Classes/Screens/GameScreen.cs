using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Solace
{
    class GameScreen
    {
        GraphicsDeviceManager graphics;
        XMLEngine xmlEngine = new XMLEngine();
        BackgroundManager backgroundManager = new BackgroundManager(true);
        SpawnManager spawnManager;
        ProjectileManager projectileManager = new ProjectileManager();
        CollisionManager collisionManager = new CollisionManager();
        Player player;
        HudManager hudManager;

        public GameScreen(GraphicsDeviceManager graphicsDevice)
        {
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public void Initialize(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            // TODO: Add your initialization logic here
            spawnManager = new SpawnManager(xmlEngine);
            player = new Player(xmlEngine);
            xmlEngine.PhraseWeaponXML();
            xmlEngine.PhraseShipXML();
            spawnManager.Initialize(graphicsDevice, Content);
            projectileManager.Initialize();
            player.Initialize();
            hudManager = new HudManager(player);
            hudManager.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent(ContentManager Content, GraphicsDevice graphicsDevice)
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            backgroundManager.LoadContent(Content, graphicsDevice, @"Backgrounds\Clouds\Cloud-Plasma-3", @"Backgrounds\Clouds\Cloud-Plasma-4");
            foreach (Ship ship in xmlEngine.PlayerShips)
            {
                ship.ShipTexture = (Content.Load<Texture2D>(ship.ShipTexturePath));
                foreach (Weapon weapon in ship.myHardpoints)
                {
                    weapon.ProjectileTexture = (Content.Load<Texture2D>(weapon.ProjectileTexturePath));
                }
            }

            foreach (Ship ship in xmlEngine.EnemyShips)
            {
                ship.ShipTexture = (Content.Load<Texture2D>(ship.ShipTexturePath));
                foreach (Weapon weapon in ship.myHardpoints)
                {
                    weapon.ProjectileTexture = (Content.Load<Texture2D>(weapon.ProjectileTexturePath));
                }
            }

            

            player.LoadContent(Content);
            hudManager.LoadContent(Content, graphicsDevice);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        public void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            hudManager.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            // Allows the game to exit
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                

            // TODO: Add your update logic here
            backgroundManager.Update();
            spawnManager.Update(gameTime, projectileManager);
            player.Update(gameTime, projectileManager);
            projectileManager.Update();
            collisionManager.Update(player, spawnManager.enemyManager.Enemies, spawnManager.enemyManager.Asteroids, projectileManager.Projectiles);
            hudManager.Update(graphicsDevice);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Matrix spriteScale)
        {
            // TODO: Add your drawing code here
            backgroundManager.Draw(spriteBatch, spriteScale);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None,
                RasterizerState.CullCounterClockwise, null, spriteScale);
            projectileManager.Draw(spriteBatch);
            spawnManager.Draw(spriteBatch);
            player.Draw(spriteBatch);
            hudManager.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
