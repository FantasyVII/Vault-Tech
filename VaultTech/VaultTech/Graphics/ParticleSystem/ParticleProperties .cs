using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaultTech.Graphics.ParticleSystem
{
    public struct ParticleProperties
    {
        public Texture2D texture;
        public Color color;
        public Vector2 Position, Size, Velocity, MaxVelocity, Acceleration;
        public double TotalLifeTimeMillisecond, ElapsedTimeMillisecond;
        public Rectangle rectangle;
    }
}