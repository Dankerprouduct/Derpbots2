using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derpbots2Server
{
    public class PlayerData
    {
        public string name; 
        public bool facigRight;
        public bool shooting;
        public float rotation;
        public int type;
        public int ActiveTime;
        public float dirX;
        public float dirY;
        public int X;
        public int Y;

        public PlayerData(
            string name,
            bool fRight,
            bool shooting,
            float _rotation,
            int _ActiveTime,
            int _type,
            float _dirX,
            float _dirY,
            int _X, 
            int _Y)
        {
            this.name = name; 
            facigRight = fRight;
            this.shooting = shooting;
            this.rotation = _rotation;
            this.ActiveTime = _ActiveTime; 
            this.type = _type;
            this.dirX = _dirX;
            this.dirY = _dirY;
            this.X = _X;
            this.Y = _Y; 
        }

    }
}
