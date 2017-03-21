using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content; 

namespace Derpbots2
{
    public class Emitter
    {
        public Vector2 position;
        public Texture2D texture; 
        public List<Particle> particles = new List<Particle>();
        List<Color> colors = new List<Color>(); 

        int min, max; 
        public Emitter(int min, int max)
        {
            this.min = min; 
            this.max = max; 
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Sprites/Particles/Particle1"); 
        }

        public void Update(GameTime gameTime)
        {
            Random random = new Random(gameTime.TotalGameTime.Milliseconds);
            int index = random.Next(0, colors.Count);

            for (int i = 0; i < 5; i++)
            {
                
                particles.Add(new Particle(texture, 10000, 30, 10, (float)random.NextDouble(), position, colors[index]));

                if (particles.Count() > max)
                {
                    particles.RemoveAt(0);
                }
            }

            
            

            for (int i = 0; i < particles.Count(); i++)
            {
                
                particles[i].Update(gameTime); 

                if(particles[i].TotalActiveTime > particles[i].ActiveTime)
                {
                    particles.RemoveAt(i); 
                }
                if(particles[i].speed <= .1f)
                {
                    particles.RemoveAt(i); 
                }
            }

        }

        public void AddColor(Color color)
        {
            colors.Add(color); 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < particles.Count(); i++)
            {
                particles[i].Draw(spriteBatch); 
            }
        }

    }
}
