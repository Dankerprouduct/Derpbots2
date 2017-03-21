using System;
using System.Threading;
using Lidgren.Network; 

namespace Derpbots2Server
{
    class Program
    {
        static void Main(string[] args)
        {
            string version = "1.12"; 
            Console.Title = "DERPBOTS 2 SERVER " + "( VER: "+version+" )";
            Console.ForegroundColor = ConsoleColor.Yellow;


            Console.WriteLine(@"
  _____  ______ _____  _____  ____   ____ _______ _____       ___               
 |  __ \|  ____|  __ \|  __ \|  _ \ / __ \__   __/ ____|  _  |__ \              
 | |  | | |__  | |__) | |__) | |_) | |  | | | | | (___   (_)    ) |             
 | |  | |  __| |  _  /|  ___/|  _ <| |  | | | |  \___ \        / /         _    
 | |__| | |____| | \ \| |    | |_) | |__| | | |  ____) |  _   / /_      /\| |/\ 
 |_____/|______|_|__\_\_|____|____/_\____/_ |_|_|_____/__(_)_|____|____ \ ` ' / 
 |______|______|______|______|______|______|______|______|______|______|_     _|
  Danker Games                                                          / , . \ 
                                                                        \/|_|\/                                                                                                                                                                 
");
            Console.WriteLine(""); 
            Console.WriteLine("\n\n\n");
            
            Console.ResetColor();


            //Console.Write("Please Enter Server Name: ");
            //string hostName = Console.ReadLine();


            int port = 8181; 
            bool portSet = false;
            
            while (portSet == false)
            {
                Console.WriteLine("Default: " + port); 
                Console.Write("please enter port: ");
                
                if (!Int32.TryParse(Console.ReadLine(), out port))
                {
                    if(port == 0)
                    {
                        port = 8181;
                        portSet = true;
                        break;
                    }

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Numbers only please.");
                    Console.ResetColor(); 
                }
                else
                {
                    portSet = true;
                }
            }
            Console.WriteLine("Port set to: " + port); 

            NetPeerConfiguration config = new NetPeerConfiguration("Derpbots");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.Port = port;
            

            // create and start server
            NetServer server = new NetServer(config);
            server.Start();

            // schedule initial sending of position updates
            double nextSendUpdates = NetTime.Now;

            // run until escape is pressed
            while (!Console.KeyAvailable || Console.ReadKey().Key != ConsoleKey.Escape)
            {
                NetIncomingMessage msg;
                while ((msg = server.ReadMessage()) != null)
                {
                    switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.DiscoveryRequest:
                            //
                            // Server received a discovery request from a client; send a discovery response (with no extra data attached)
                            //
                            server.SendDiscoveryResponse(null, msg.SenderEndPoint);
                            break;
                        case NetIncomingMessageType.VerboseDebugMessage:
                        case NetIncomingMessageType.DebugMessage:
                            Console.WriteLine(msg.ReadString());
                            break;
                        case NetIncomingMessageType.WarningMessage:
                            WriteWarning(msg.ToString());
                            break;
                        case NetIncomingMessageType.ErrorMessage:
                            WriteError(msg.ReadString());
                            break;
                        case NetIncomingMessageType.StatusChanged:
                             
                            NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                            Console.WriteLine(status); 
                            if(status == NetConnectionStatus.Disconnected)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("[DISCONNECTED]");
                                Console.ResetColor(); 
                                Console.WriteLine("Player " + msg.ReadString() + " Disconnected");
                            }
                            if (status == NetConnectionStatus.Connected)
                            {
                                //
                                // A new player just connected!
                                //

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("[CONNECTED] ");
                                Console.ResetColor();
                                //NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier)
                                Console.WriteLine(NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier) + " connected!");

                                // randomize his position and store in connection tag
                                
                                // player given random bool and position
                                msg.SenderConnection.Tag = new PlayerData(
                                    "NULL",
                                    false,
                                    false,
                                    0,
                                    0,
                                    0,
                                    NetRandom.Instance.Next(10, 100),
                                    NetRandom.Instance.Next(10, 100),
                                    NetRandom.Instance.Next(10, 100),
                                    NetRandom.Instance.Next(10, 100)); 
                            }

                            break;
                        case NetIncomingMessageType.Data:
                            //
                            // The client sent input to the server
                            //
                            string name = msg.ReadString(); 
                            bool facingRight = msg.ReadBoolean();
                            bool shooting = msg.ReadBoolean();
                            float rotation = msg.ReadFloat();
                            int activeTime = msg.ReadInt32();
                            int type = msg.ReadInt32();
                            float dirX = msg.ReadFloat();
                            float dirY = msg.ReadFloat();                      
                            int xinput = msg.ReadInt32();
                            int yinput = msg.ReadInt32();



                            PlayerData player = msg.SenderConnection.Tag as PlayerData;

                            player.name = name; 
                            player.facigRight = facingRight;
                            player.shooting = shooting;
                            player.rotation = rotation;
                            player.ActiveTime = activeTime; 
                            player.type = type;
                            player.dirX = dirX;
                            player.dirY = dirY;
                            player.X = xinput;
                            player.Y = yinput; 
                            break;
                    }

                    //
                    // send position updates 60 times per second
                    //
                    double now = NetTime.Now;
                    if (now > nextSendUpdates)
                    {
                        // Yes, it's time to send position updates

                        // for each player...
                        foreach (NetConnection player in server.Connections)
                        {
                            // ... send information about every other player (actually including self)
                            foreach (NetConnection otherPlayer in server.Connections)
                            {
                                // send position update about 'otherPlayer' to 'player'
                                NetOutgoingMessage om = server.CreateMessage();

                                // write who this position is for
                                om.Write(otherPlayer.RemoteUniqueIdentifier);

                                if (otherPlayer.Tag == null)
                                    msg.SenderConnection.Tag = new PlayerData(
                                    "NULL",
                                    false,
                                    false,
                                    0,
                                    0,
                                    0,
                                    NetRandom.Instance.Next(10, 100),
                                    NetRandom.Instance.Next(10, 100),
                                    NetRandom.Instance.Next(10, 100),
                                    NetRandom.Instance.Next(10, 100));


                                PlayerData _player = otherPlayer.Tag as PlayerData;

                                om.Write(_player.name); 
                                om.Write(_player.facigRight);
                                om.Write(_player.shooting);
                                om.Write(_player.rotation);
                                om.Write(_player.ActiveTime); 
                                om.Write(_player.type);
                                om.Write(_player.dirX);
                                om.Write(_player.dirY);  
                                om.Write(_player.X);
                                om.Write(_player.Y);
                                

                                // send message
                                server.SendMessage(om, player, NetDeliveryMethod.Unreliable);
                            }
                        }

                        // schedule next update
                        nextSendUpdates += (1.0 / 60.0);
                    }
                }

                // sleep to allow other processes to run smoothly
                Thread.Sleep(1);
            }

            server.Shutdown("app exiting");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Goodbye!");
            Console.ReadKey(); 


        }

        public static void WriteWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[Warning] "); 
            Console.ResetColor();
            Console.WriteLine(message); 
        }
        public static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Error] ");            
            Console.ResetColor();
            Console.WriteLine(message);
        }
    }
}
