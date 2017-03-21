using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.GamerServices;
using System.Collections.Generic; 

using Lidgren.Network; 

namespace Derpbots2
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        

        MapRenderer map1;
        public Player player;
        public Camera camerea; 
        public int SCREEN_WIDTH = 900;
        public int SCREEN_HEIGHT;
        public static Physics physics;

        Texture2D texture;
        Multiplayer muliplayer;
        SpriteFont spriteFont; 

        enum GameStates
        {
            Title,
            Game
        }
        GameStates gameState; 
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            SCREEN_HEIGHT = (SCREEN_WIDTH / 16) * 9;  
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            IsMouseVisible = true;
            muliplayer = new Multiplayer(); 
            
        }
        
        protected override void Initialize()
        {
            
            base.Initialize();            
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ProjectileManager projectileManager = new ProjectileManager(this, spriteBatch);
             
            map1 = new MapRenderer("map"); 
            map1.LoadContent(Content);

            camerea = new Camera(); 
            player = new Player();
            player.LoadContent(Content);
            physics = new Physics(map1.map , map1.tilesetTilesWide, map1.tilesetTilesHigh);

            muliplayer.LoadContent(Content);

            spriteFont = Content.Load<SpriteFont>("SpriteFont1");
            gameState = GameStates.Game; 
        }
        
        protected override void Update(GameTime gameTime)
        {

            if (gameState == GameStates.Game)
            {
                
                player.Update(this, gameTime);
                muliplayer.Update(player);
                Game1 game = this;
                camerea.Update(ref game);
            }

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (gameState == GameStates.Game)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null,
                    null, null, null, camerea.transform);

                map1.Draw(spriteBatch);


                muliplayer.Draw(spriteBatch);

                player.Draw(spriteBatch); 
                base.Draw(gameTime);
                spriteBatch.End();

                spriteBatch.Begin();
                spriteBatch.DrawString(spriteFont, player.health.ToString(), new Vector2(20, 20), Color.Green);
                spriteBatch.DrawString(spriteFont, player.particleEmiter.particles.Count.ToString(), new Vector2(20, 40), Color.Green);
                spriteBatch.End();
            }

        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            //Console.WriteLine("Exiting Server"); 

            muliplayer.Shutdown(); 

            base.OnExiting(sender, args);
        }

    }
}