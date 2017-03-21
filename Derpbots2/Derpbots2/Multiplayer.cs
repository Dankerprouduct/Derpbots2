using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content; 

namespace Derpbots2
{
    public class Multiplayer
    {
        Dictionary<long, Vector2> oldpPositions = new Dictionary<long, Vector2>();
        Dictionary<long, Vector2> positions = new Dictionary<long, Vector2>();
        Dictionary<long, bool> facing = new Dictionary<long, bool>();
        Dictionary<long, bool> shooting = new Dictionary<long, bool>();
        Dictionary<long, string> playerNames = new Dictionary<long, string>();

        NetClient client;

        Texture2D texture;

        float lag = 1; 
        public Multiplayer()
        {

            NetPeerConfiguration config = new NetPeerConfiguration("Derpbots");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            client = new NetClient(config);
            client.Start();

            client.DiscoverLocalPeers(8181);
        }

        public void Update(Player player)
        {
            SendMessages(player);
            RecieveMessages(player); 

            
        }

        public  void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Sprites/Derpbot");
        }

        public void Shutdown()
        {
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
            client.Shutdown(names[index]);
        }

        void RecieveMessages(Player player)
        {
            // read messages
            NetIncomingMessage msg;
            if(client.ServerConnection == null)
            {
                Console.WriteLine("NO SERVER");
                client.DiscoverLocalPeers(8181); 
            }
            while ((msg = client.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryResponse:
                        // just connect to first server discovered
                        client.Connect(msg.SenderEndPoint);
                        break;
                    case NetIncomingMessageType.Data:
                        // server sent a position update
                        long who = msg.ReadInt64();
                        string name = msg.ReadString(); 
                        bool facingRight = msg.ReadBoolean();
                        bool shooting = msg.ReadBoolean();
                        float rotation = msg.ReadFloat();
                        int activeTime = msg.ReadInt32(); 
                        int type = msg.ReadInt32();
                        float dirX = msg.ReadFloat();
                        float dirY = msg.ReadFloat();
                        int x = msg.ReadInt32();
                        int y = msg.ReadInt32();

                        facing[who] = facingRight;
                        
                        positions[who] = new Vector2(x, y);
                        this.shooting[who] = shooting;
                        playerNames[who] = name; 

                        
                        if (shooting)
                        {
                            if (playerNames[who] == player.name)
                            {
                                
                                ProjectileManager.AddBullet(new Vector2(positions[who].X + (59 / 2), positions[who].Y), new Vector2(dirX, dirY), rotation, activeTime, Projectile.BulletType.Friendly);
                            }
                            else
                            {
                                
                                ProjectileManager.AddBullet(new Vector2(positions[who].X + (59 / 2), positions[who].Y), new Vector2(dirX, dirY), rotation, activeTime, Projectile.BulletType.Enemy);
                            }
                        }
                        break;
                }
            }
        }

        void SendMessages(Player player)
        {
            //
            // If there's input; send it to server
            //
            NetOutgoingMessage om = client.CreateMessage();

            om.Write(player.name);
            om.Write(player.facingRight);
            om.Write(player.shooting);
            om.Write(player.rotation);
            om.Write(3000); // active Time
            om.Write(0); // bullet type
            om.Write(player.direction.X);
            om.Write(player.direction.Y);
            om.Write((int)player.GetPosition().X); // very inefficient to send a full Int32 (4 bytes) but we'll use this for simplicity
            om.Write((int)player.GetPosition().Y);
            client.SendMessage(om, NetDeliveryMethod.Unreliable);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var kvp in positions)
            {
                if (facing[kvp.Key])
                {
                    spriteBatch.Draw(texture, kvp.Value, Color.White);
                }
                else
                {
                    spriteBatch.Draw(texture, new Rectangle((int)kvp.Value.X, (int)kvp.Value.Y, texture.Width, texture.Height),
                        new Rectangle(0, 0, texture.Width, texture.Height),
                        Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                }

            }
        }

    }
}
