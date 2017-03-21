using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;
using Microsoft.Xna.Framework; 

namespace Derpbots2
{
    public class Physics
    {
        TmxMap map;
        private int tileWidth;
        private int tileHeight;
        private int tilesetTilesWide;
        private int tilesetTilesHigh;

        public Physics(TmxMap _map, int tilesetTilesWide, int tilesetTilesHigh)
        {
            map = _map;

            tileWidth = map.Tilesets[0].TileWidth;
            tileHeight = map.Tilesets[0].TileHeight;

            this.tilesetTilesWide = tilesetTilesWide; 
            this.tilesetTilesHigh = tilesetTilesHigh; 

        }
        
        public bool Collision(Rectangle Object)
        {
            for (var i = 0; i < map.Layers[0].Tiles.Count; i++)
            {
                int gid = map.Layers[0].Tiles[i].Gid;

                // Empty tile, do nothing
                if (gid == 0)
                {

                }
                else if(gid == 1)
                {
                    int tileFrame = gid - 1;
                    int column = tileFrame % tilesetTilesWide;
                    int row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide);

                    float x = (i % map.Width) * map.TileWidth;
                    float y = (float)Math.Floor(i / (double)map.Width) * map.TileHeight;

                    Rectangle tilesetRec = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);

                    if((new Rectangle((int)x, (int)y, tileWidth, tileHeight)).Intersects(Object))
                    {
                        return true; 
                    }

                }
            }

            return false; 
        }

        public bool BulletCollision(Rectangle object1)
        {
            
            for(int i = 0; i < ProjectileManager.bullets.Count(); i++)
            {
                if (object1.Intersects(ProjectileManager.bullets[i].rectangle) && ProjectileManager.bullets[i].type == Projectile.BulletType.Enemy)
                {
                    //Console.WriteLine("hit: " + i);v

                    ProjectileManager.bullets[i].Destroy(); 
                    return true;
                }
               
            }
            return false; 
        }
    }
}
