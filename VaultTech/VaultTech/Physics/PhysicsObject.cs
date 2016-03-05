/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 27/December/2014
 * Date Moddified :- 25/November/2015
 * </Copyright>
 */

using System;

using Microsoft.Xna.Framework;

namespace VaultTech.Physics
{
    public class PhysicsObject
    {
        public Vector2 Position, Size, Velocity, MaxVelocity, Acceleration;
        public float Speed, Resistance;
        public Rectangle Rectangle { get; set; }
        public bool AcceleratingX, AcceleratingY;

        float ResistanceOverTime;

        public PhysicsObject()
        {
        }

        public PhysicsObject(Vector2 Position, Vector2 Size)
        {
            this.Position = Position;
            this.Size = Size;

            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }

        public PhysicsObject(Vector2 Position, Vector2 Size, Vector2 MaxVelocity, float Speed, float Resistance)
        {
            this.Position = Position;
            this.Size = Size;
            this.MaxVelocity = MaxVelocity;
            this.Speed = Speed;
            this.Resistance = Resistance;

            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }

        void SetMaxVelocity()
        {
            if (Velocity.X > 0 || Velocity.Y > 0)
                Velocity = new Vector2(Math.Min(Velocity.X, MaxVelocity.X), Math.Min(Velocity.Y, MaxVelocity.Y));

            if (Velocity.X < 0 || Velocity.Y < 0)
                Velocity = new Vector2(Math.Max(Velocity.X, -MaxVelocity.X), Math.Max(Velocity.Y, -MaxVelocity.Y));
        }

        void Decelerate()
        {
            if (!AcceleratingX)
            {
                if (Velocity.X > 0)
                {
                    Acceleration.X = 0;
                    Velocity.X -= ResistanceOverTime;

                    if (Velocity.X < 0)
                        Velocity.X = 0;
                }

                if (Velocity.X < 0f)
                {
                    Acceleration.X = 0;
                    Velocity.X += ResistanceOverTime;

                    if (Velocity.X > 0)
                        Velocity.X = 0;
                }
            }

            if (!AcceleratingY)
            {
                if (Velocity.Y > 0f)
                {
                    Acceleration.Y = 0;
                    Velocity.Y -= ResistanceOverTime;

                    if (Velocity.Y < 0)
                        Velocity.Y = 0;
                }

                if (Velocity.Y < 0f)
                {
                    Acceleration.Y = 0;
                    Velocity.Y += ResistanceOverTime;

                    if (Velocity.Y > 0)
                        Velocity.Y = 0;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            ResistanceOverTime = Resistance * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            Velocity += Acceleration * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Position += Velocity;

            SetMaxVelocity();
            Decelerate();

            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }
    }
}