/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 4/April/2014
 * Date Moddified :- 24/February/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace VaultTech.Graphics.ParticleSystem
{
    class OldParticle
    {
        SpriteBatch spriteBatch;

        //Time time, RedColorTime, GreenColorTime, BlueColorTime, AlphaColorTime;

        Texture2D Texture;
        Vector2 Position, Position2, Size, Direction, Velocity;
        Rectangle rectangle;

        bool Rdone, Gdone, Bdone, Adone;
        bool DOonce;
        Color CurrentColor, StartingColor, EndingColor;
        Vector4 ColorDifference;

        float Angle, Speed, Life;
        int radius = 5;
        bool IsDead;

        public OldParticle(Vector2 Position, Vector2 Size, float Angle, float Speed, float Life, Color StartingColor, Color EndingColor)
        {
            this.Position = Position;
            this.Size = Size;
            this.Angle = Angle;
            this.Speed = Speed;
            this.Life = Life;
            this.StartingColor = StartingColor;
            this.EndingColor = EndingColor;

            ColorDifference = new Vector4(EndingColor.R - StartingColor.R, EndingColor.G - StartingColor.G, EndingColor.B - StartingColor.B, EndingColor.A - StartingColor.A);
            CurrentColor = StartingColor;
            DOonce = true;
            IsDead = false;
            Rdone = false;
            Gdone = false;
            Bdone = false;
            Adone = false;

            //time = new Time();
            //RedColorTime = new Time();
            //GreenColorTime = new Time();
            //BlueColorTime = new Time();
            //AlphaColorTime = new Time();

            //time.Start = true;
            //RedColorTime.Start = true;
            //GreenColorTime.Start = true;
            //BlueColorTime.Start = true;
            //AlphaColorTime.Start = true;
        }

        public void LoadContent(ContentManager Content)
        {
            Texture = Content.Load<Texture2D>("particle");
        }

        public void UpdateOnce(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        public void FindPosition2()
        {
            Position2.X = (float)(Position.X + radius * Math.Cos(Angle * Math.PI / 180));
            Position2.Y = (float)(Position.Y + radius * Math.Sin(Angle * Math.PI / 180));
        }

        public void CalculateVelocity()
        {
            // Calculate direction vector
            Direction.X = Position2.X - Position.X;
            Direction.Y = Position2.Y - Position.Y;

            // Normalize direction vector to unit length
            float Length = (float)Math.Sqrt(Direction.X * Direction.X + Direction.Y * Direction.Y);
            Direction.X /= (float)Length;
            Direction.Y /= (float)Length;

            // Apply movement vector to move
            Velocity.X += (float)Direction.X * (Speed / 100);
            Velocity.Y += (float)Direction.Y * (Speed / 100);

            rectangle = new Rectangle((int)Velocity.X, (int)Velocity.Y, (int)Size.X, (int)Size.Y);
        }

        void ChangeColor(GameTime gameTime)
        {
            //RedColorTime.Update(gameTime);
            //GreenColorTime.Update(gameTime);
            //BlueColorTime.Update(gameTime);
            //AlphaColorTime.Update(gameTime);


            //If the color difference value is negative, then make it positive.
            if (DOonce)
            {
                if (ColorDifference.X < 0)
                    ColorDifference.X -= ColorDifference.X * 2;

                if (ColorDifference.Y < 0)
                    ColorDifference.Y -= ColorDifference.Y * 2;

                if (ColorDifference.Z < 0)
                    ColorDifference.Z -= ColorDifference.Z * 2;

                if (ColorDifference.W < 0)
                    ColorDifference.W -= ColorDifference.W * 2;
                DOonce = false;
            }


            if (/*RedColorTime.TotalMilliseconds >= (Life / ColorDifference.X) &&*/ !Rdone)
            {
                if (StartingColor.R > EndingColor.R)
                {
                    CurrentColor.R -= 1;
                }
                else if (StartingColor.R < EndingColor.R)
                {
                    CurrentColor.R += 1;
                }

                if (CurrentColor.R == EndingColor.R)
                    Rdone = true;

                //RedColorTime.Restart = true;
            }

            if (/*GreenColorTime.TotalMilliseconds >= (Life / ColorDifference.Y) &&*/ !Gdone)
            {
                if (StartingColor.G > EndingColor.G)
                {
                    CurrentColor.G -= 1;

                }
                else if (StartingColor.G < EndingColor.G)
                {
                    CurrentColor.G += 1;


                }
                if (CurrentColor.G == EndingColor.G)
                    Gdone = true;


                //GreenColorTime.Restart = true;
            }

            if (/*BlueColorTime.TotalMilliseconds >= (Life / ColorDifference.Z) &&*/ !Bdone)
            {
                if (StartingColor.B > EndingColor.B)
                {
                    CurrentColor.B -= 1;


                }
                else if (StartingColor.B < EndingColor.B)
                {
                    CurrentColor.B += 1;


                }

                if (CurrentColor.B == EndingColor.B)
                    Bdone = true;

                //BlueColorTime.Restart = true;
            }

            if (/*AlphaColorTime.TotalMilliseconds >= (Life / ColorDifference.W) &&*/ !Adone)
            {
                if (StartingColor.A > EndingColor.A)
                {
                    CurrentColor.A -= 1;

                }
                else if (StartingColor.A < EndingColor.A)
                {
                    CurrentColor.A += 1;


                }

                if (CurrentColor.A == EndingColor.A)
                    Adone = true;

                //AlphaColorTime.Restart = true;
            }
        }

        void isDead()
        {
            /*if (Life <= time.TotalMilliseconds)
            {
                time.Restart = true;
                IsDead = true;
            }*/
        }

        public void Update(GameTime gameTime)
        {
            //time.Update(gameTime);

            if (!IsDead)
            {
                FindPosition2();
                CalculateVelocity();
            }

            ChangeColor(gameTime);

            isDead();
        }

        public void Draw()
        {
            spriteBatch.Draw(Texture, rectangle, CurrentColor);
        }
    }
}