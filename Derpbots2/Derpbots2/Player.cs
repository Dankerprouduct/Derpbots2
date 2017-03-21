using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content; 

namespace Derpbots2
{
    public class Player
    {

        private Rectangle rect;
        private Vector2 position;
        private Vector2 old_position;
        private Vector2 velocity;
        public string name; 
        
        KeyboardState keyboardState;
        MouseState mouseState = Mouse.GetState();
        MouseState oldMouseState; 
        public Texture2D texture; 
        float speed = 8;
        public bool facingRight;
        public bool shooting;
        public float rotation;
        public Vector2 direction;
        int shotTimer;
        public int health = 100;

        public Emitter particleEmiter; 
        public Player()
        {
            position = new Vector2(100, 100);
            rect = new Rectangle(100, 100, 100, 100);
            List<string> names = new List<string>()
            {
                "Jerome",
                "Bill",
                "Turtle",
                "Hare",
                "Karly",
                "Carly",
                "Bobby",
                "Hank",
                "Dale",
                "Redhorn",
                "BoomHauer"
            };
            Random random = new Random();
            int index = random.Next(0, names.Count());
            name = names[index]; 
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Sprites/Derpbot");
            particleEmiter = new Emitter(0, 6000);
            particleEmiter.LoadContent(content);
            particleEmiter.AddColor(Color.White);
            particleEmiter.AddColor(Color.Blue);
            particleEmiter.AddColor(Color.LightBlue); 
            particleEmiter.AddColor(Color.LightCyan);
            particleEmiter.AddColor(Color.LightPink);
            particleEmiter.AddColor(Color.Purple);
            particleEmiter.AddColor(Color.HotPink);
            particleEmiter.AddColor(Color.Lavender);
        }

        public void Update(Game1 game, GameTime gameTime)
        {
            
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState(); 
            Movement();
            Weapons(game, gameTime );
            Damage(); 
            oldMouseState = mouseState;

            particleEmiter.Update(gameTime);

            particleEmiter.position = position;
        }

        void Weapons(Game1 game, GameTime gameTime)
        {
            rotation = (float)Math.Atan2(
              (double)MouseDirection(game).Y,
                (double)MouseDirection(game).X);

            if (mouseState.LeftButton == ButtonState.Pressed )
            {
                
                shotTimer += gameTime.ElapsedGameTime.Milliseconds; 
                if(shotTimer > 250)
                {
                    shooting = true;
                    shotTimer = 0; 
                }
                else
                {
                    shooting = false; 
                }
                //ProjectileManager.AddBullet(new Vector2(position.X + (texture.Width / 2), position.Y), MouseDirection(game), rotation); 
            }
            else
            {
                shooting = false;
            }
        }

        Vector2 MouseDirection(Game1 game)
        {         
               
            Vector2 mousePosition = Vector2.Transform(new Vector2(mouseState.X, mouseState.Y), Matrix.Invert(game.camerea.transform));
            direction = mousePosition - position;
            direction.Normalize();
            return direction;
        }

        void Movement()
        {

            float decayRate = 1;
            velocity *= decayRate * 1;
            old_position = position;
            position.X += (int)velocity.X;
            rect = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            if (Game1.physics.Collision(rect))
            {
                position = old_position;
            }

            position.Y += (int)velocity.Y;
            rect = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            if (Game1.physics.Collision(rect))
            {
                position = old_position;
            }

            keyboardState = Keyboard.GetState();
            
            if (keyboardState.IsKeyDown(Keys.W))
            {
                velocity.Y = -speed;
            }
            else if (keyboardState.IsKeyDown(Keys.S))
            {
                velocity.Y = speed;
            }
            else
            {
                velocity.Y = 0;
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                velocity.X = -speed;
                facingRight = false;
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                velocity.X = speed;
                facingRight = true;
            }
            else
            {
                velocity.X = 0;
            }
        }

        void Damage()
        {
            rect = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            if (Game1.physics.BulletCollision(rect))
            {
                Console.WriteLine("DAMAGE");
                TakeDamage(3); 
                
            }
        }
        
        public void TakeDamage(int ammount)
        {
            health -= ammount;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            particleEmiter.Draw(spriteBatch); 

        }
        public Vector2 GetVelocity()
        {
            return velocity; 
        }

        public Vector2 GetPosition()
        {
            return position; 
        }
        public Rectangle GetRect()
        {
            return rect; 
        }
    }
}
