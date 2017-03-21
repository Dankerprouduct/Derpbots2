using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Derpbots2
{
    public class Camera
    {

        public Matrix transform;
        public Vector2 center;
        Viewport view;
        float scale = 1f;

        public Camera()
        {

            
        }

        public void Update(ref Game1 game)
        {
            center = new Vector2((int)(game.player.GetPosition().X - (game.player.GetRect().Width / 2) - (int)(game.SCREEN_WIDTH / 2)),
                ((int)game.player.GetPosition().Y - (int)(game.player.GetRect().Height / 2) - (int)(game.SCREEN_HEIGHT / 2)));

            transform = Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0)) * Matrix.CreateScale(new Vector3(scale, scale, 1)); 
            //Matrix.CreateTranslation(new Vector3(game.GraphicsDevice.Viewport.Width * 0.5f, game.GraphicsDevice.Viewport.Height * 0.5f, 0));
        }
    }
}
