using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Derpbots2
{
    public class Projectile
    {
        Vector2 position;
        Vector2 direction;
        Texture2D texture;
        float rotation;
        public int ActiveTime;
        public int TotalActiveTime;
        public Rectangle rectangle; 
        public enum BulletType
        {
            Friendly,
            Enemy
        }
        public BulletType type; 
        
        public Projectile(Texture2D texture,
            Vector2 position, Vector2 direction, float rotation, int activeTime, BulletType type)
        {
            this.texture = texture;
            this.position = position;
            this.direction = direction;
            this.rotation = rotation;
            this.ActiveTime = activeTime;
            this.TotalActiveTime = 0;
            this.type = type; 
        }

        public void Update(GameTime gameTime)
        {
            position = new Vector2((int)position.X, (int)position.Y);
            position += direction * 15;
            this.TotalActiveTime += gameTime.ElapsedGameTime.Milliseconds;
            Physics();
        }
        
        public void Physics()
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y,
                (int)(Math.Cos(rotation)* texture.Width), 
                (int)(Math.Sin(rotation) * texture.Height));

            if(Game1.physics.Collision(rectangle))
            {
                rotation = (float)Math.PI - rotation;
                if(rotation < 0)
                {
                    rotation = (float)MathHelper.ToRadians(180 - MathHelper.ToDegrees(rotation) + 360 + 180) ; 
                    direction.Y *= -1;   
                    
                }
                direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
                direction.Normalize(); 
            }


        }

        public void Destroy()
        {
            this.TotalActiveTime = ActiveTime + 1; 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height), new Rectangle (0,0, texture.Width, texture.Height),
                Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 1f);
            
        }
    }
}
