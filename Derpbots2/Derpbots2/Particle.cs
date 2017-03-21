using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics; 

namespace Derpbots2
{
    public class Particle
    {
        public int TotalActiveTime;
        public int ActiveTime;
        public float speed;
        public float size;
        public float rotation;
        public Vector2 position;
        public Vector2 velocity;
        Vector2 direction; 
        public Texture2D texture;
        Color color; 

        public Particle(Texture2D texture, int activeTime, float speed, float size, float rotation, Vector2 position, Color color)
        {
            Random rand = new Random(); 
            this.texture = texture;
            ActiveTime = activeTime;
            this.speed = rand.Next(0, (int)speed);
            this.size = rand.Next(0, (int)size); 
            this.position = position;
            this.color = color;
            this.rotation = rotation;

            
            direction = new Vector2((float)Math.Cos(rotation),
                (float)Math.Sin(rotation));
            direction.Normalize();
        }

        public void Update(GameTime gameTime)
        {
            TotalActiveTime += gameTime.ElapsedGameTime.Milliseconds;
            Random rand = new Random(gameTime.ElapsedGameTime.Milliseconds);

            velocity = new Vector2((float)rand.NextDouble(), .025f);
            
            size += .005f;

            rotation += .025f; 

            direction = new Vector2((float)Math.Cos(rotation),
                (float)Math.Sin(rotation));
            direction.Normalize();
            
            float dampening = 1.0025f;

            speed *= dampening;
            position += velocity * direction * speed; 

        }

        public void Draw(SpriteBatch spriteBatch)
        {            
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, (int)size, (int)size), color * speed); 
        }
    }
}
