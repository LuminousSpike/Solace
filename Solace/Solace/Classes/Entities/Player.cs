using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#if (ANDROID)
using Microsoft.Xna.Framework.Input.Touch;
#endif

namespace Solace
{
    class Player
    {
        public static int Score = 0;
        public static float Flamoca = 0f;
        public Ship myShip;
        
        private Vector2 screenSize;
        private XMLEngine xmlEngine;

        public Player(XMLEngine xmlengine)
        {
            xmlEngine = xmlengine;
        }
        
        public void Initialize()
        {
            Score = 0;
            Flamoca = 0f;
        }

        public void LoadContent(ContentManager content)
        {
            myShip = xmlEngine.PlayerShips[0].ShallowCopy();
            myShip.LoadContent();
            myShip.Position = new Vector2(Game1.ViewPortWidth / 2 - myShip.ShipTexture.Width / 2, Game1.ViewPortHeight / 2 - myShip.ShipTexture.Height / 2);
            screenSize = new Vector2(Game1.ViewPortWidth, Game1.ViewPortHeight);
        }

        public void Update(GameTime gameTime, ProjectileManager projectileManager)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                myShip.Move(0f, -myShip.Speed);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                myShip.Move(0f, myShip.Speed);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                myShip.Move(-myShip.Speed, 0f);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                myShip.Move(myShip.Speed, 0f);
            }

            // Android Controls

#if(ANDROID)
            if (Game1.gesture.GestureType == GestureType.FreeDrag)
            {
                myShip.Position.X += Game1.gesture.Delta.X * 0.5f;
                myShip.Position.Y += Game1.gesture.Delta.Y * 0.5f;
            }
#endif

            myShip.Update(gameTime);
            myShip.FireWeapon(gameTime, projectileManager, myShip.Position, true);

            // Make sure that the player does not go out of bounds
            myShip.Position.X = MathHelper.Clamp(myShip.Position.X, 0, screenSize.X - myShip.ShipTexture.Width);
            myShip.Position.Y = MathHelper.Clamp(myShip.Position.Y, 0, screenSize.Y - myShip.ShipTexture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(myShip.ShipTexture, myShip.Position, Color.White);
        }
    }
}
