using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TiledSharp; 

namespace Derpbots2
{
    public class MapRenderer
    {

        private int tileWidth;
        private int tileHeight;
        public int tilesetTilesWide;
        public int tilesetTilesHigh;
        public TmxMap map;
        Texture2D tileset;
         
        public MapRenderer(string name)
        {
            map = new TmxMap("Maps/" + name+".tmx"); 
        }

        public void LoadContent(ContentManager content)
        {
            tileset = content.Load<Texture2D>(map.Tilesets[0].Name.ToString());
            tileWidth = map.Tilesets[0].TileWidth;
            tileHeight = map.Tilesets[0].TileHeight;

            tilesetTilesWide = tileset.Width / tileWidth;
            tilesetTilesHigh = tileset.Height / tileHeight;

        }

        public void Update()
        {
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            for (var i = 0; i < map.Layers[0].Tiles.Count; i++)
            {
                int gid = map.Layers[0].Tiles[i].Gid;

                // Empty tile, do nothing
                if (gid == 0)
                {

                }
                else
                {
                    int tileFrame = gid - 1;
                    int column = tileFrame % tilesetTilesWide;
                    int row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide);

                    float x = (i % map.Width) * map.TileWidth;
                    float y = (float)Math.Floor(i / (double)map.Width) * map.TileHeight;

                    Rectangle tilesetRec = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);

                    spriteBatch.Draw(tileset, new Rectangle((int)x, (int)y, tileWidth, tileHeight), tilesetRec, Color.White);
                }
            }

        }
    }
}
