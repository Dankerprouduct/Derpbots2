using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Derpbots2
{
    public class ProjectileManager: DrawableGameComponent
    {
        public static List<Projectile> bullets = new List<Projectile>();

        SpriteBatch spriteBatch;
        static Texture2D laser;
        public ProjectileManager(Game1 game, SpriteBatch spriteBatch) : base(game)
        {
            game.Components.Add(this);
            laser = game.Content.Load<Texture2D>("Sprites/laser_projectile");

            this.spriteBatch = spriteBatch;
        }

        public override void Update(GameTime gameTime)
        {
            for(int i = 0; i < bullets.Count(); i++)
            {
                bullets[i].Update(gameTime);
                if(bullets[i].TotalActiveTime > bullets[i].ActiveTime)
                {
                    bullets.RemoveAt(0); 
                }
                //Console.WriteLine(bullets.Count); 
            }
        }

        public static void AddBullet(Vector2 position, Vector2 direction, float rotation, int activeTime, Projectile.BulletType bulletType)
        {

            bullets.Add(new Projectile(laser, position, direction, rotation, activeTime, bulletType));
            
        }
        
        public override void Draw(GameTime gameTime)
        {
            for(int i =0; i < bullets.Count(); i++)
            {
                bullets[i].Draw(spriteBatch); 
            }
            
            base.Draw(gameTime);
        }
    }
}
