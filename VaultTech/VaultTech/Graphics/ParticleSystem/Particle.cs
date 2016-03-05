/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 26/November/2015
 * Date Moddified :- 28/November/2015
 * </Copyright>
 */

using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VaultTech.Contents;

namespace VaultTech.Graphics.ParticleSystem
{
    public class Particle
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch spriteBatch;

        Texture2D texture;
        public Color color;
        Vector2 Velocity;
        public Vector2 Position, Size, MaxVelocity, Acceleration;
        public double TotalLifeTimeMillisecond, ElapsedTimeMillisecond;

        Rectangle rectangle;

        public bool IsAlive
        {
            get
            {
                if (ElapsedTimeMillisecond >= TotalLifeTimeMillisecond)
                    return false;
                else
                    return true;
            }

            private set { }
        }

        public Particle(Vector2 Position, Vector2 Size, Vector2 MaxVelocity, Vector2 Acceleration, Color color, double TotalLifeTimeMillisecond)
        {
            this.Position = Position;
            this.Size = Size;
            this.MaxVelocity = MaxVelocity;
            this.Acceleration = Acceleration;
            this.color = color;
            this.TotalLifeTimeMillisecond = TotalLifeTimeMillisecond;
        }

        public void Initialize(GraphicsDeviceManager Graphics)
        {
            this.Graphics = Graphics;
        }

        public void LoadContent(string TexturePath)
        {
            texture = StreamTexture.LoadTextureFromStream(Graphics, TexturePath);
        }

        public void UpdateOnce(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        void CalculateVelocity(GameTime gameTime)
        {
            Velocity += Acceleration * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Position += Velocity;

            SetMaxVelocity();
        }

        void SetMaxVelocity()
        {
            if (Velocity.X > 0 || Velocity.Y > 0)
                Velocity = new Vector2(Math.Min(Velocity.X, MaxVelocity.X), Math.Min(Velocity.Y, MaxVelocity.Y));

            if (Velocity.X < 0 || Velocity.Y < 0)
                Velocity = new Vector2(Math.Max(Velocity.X, -MaxVelocity.X), Math.Max(Velocity.Y, -MaxVelocity.Y));
        }

        public void Update(GameTime gameTime)
        {
            if (IsAlive)
            {
                CalculateVelocity(gameTime);
                rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

                ElapsedTimeMillisecond += (double)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        public void Draw()
        {
            if (IsAlive)
                spriteBatch.Draw(texture, rectangle, color);
        }
    }
}