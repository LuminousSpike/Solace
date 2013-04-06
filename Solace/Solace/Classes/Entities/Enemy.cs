using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Solace
{
    class Enemy
    {
        public Ship myShip;
        public Vector2 Position;
        public bool Active;

        public Enemy(Ship ship)
        {
            myShip = ship.ShallowCopy();
        }

        public Enemy ShallowCopy()
        {
            Enemy other = (Enemy)this.MemberwiseClone();
            other.myShip = this.myShip.ShallowCopy();
            return other;
        }

        public void Initialize(Vector2 position)
        {
            myShip.TextureData = new Color[myShip.ShipTexture.Width * myShip.ShipTexture.Height];
            myShip.ShipTexture.GetData(myShip.TextureData);
            Position = position;
            Active = true;
        }

        public void LoadContent(ContentManager Content)
        {
            myShip.LoadContent(Content);
            
        }

        public void Update(GameTime gametime, ProjectileManager projectilemanager)
        {
            Position.X -= myShip.Speed;
            myShip.myTransform = Matrix.CreateTranslation(new Vector3(Position, 0.0f));
            myShip.CollisionBox = CollisionManager.CalculateBoundingRectangle(new Rectangle(0, 0, myShip.ShipTexture.Width, myShip.ShipTexture.Height), myShip.myTransform);
            if (Position.X < -myShip.ShipTexture.Width)
            {
                Active = false;
            }
            if (myShip.Health <= 0)
            {
                Active = false;
                Player.Score += myShip.Points;
            }
            myShip.FireWeapon(gametime, projectilemanager, Position, false);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(myShip.ShipTexture, Position, Color.White);
        }
    }
}
