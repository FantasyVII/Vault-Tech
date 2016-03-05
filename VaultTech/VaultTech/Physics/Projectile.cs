/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 10/June/2014
 * Date Moddified :- 8/March/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using VaultTech.Contents;

namespace VaultTech.Physics
{
    public class Projectile
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch spriteBatch;

        Texture2D Texture;
        Vector2 Size;
        public Rectangle rectangle { set; get; }

        Vector2 StartingPosition, DeltaPosition, Direction, Velocity;

        public bool IsAlive { set; get; }
        float Speed;

        public Projectile()
        {
            Size = new Vector2(32, 32);
            IsAlive = true;
        }

        public void Initialize(GraphicsDeviceManager Graphics)
        {
            this.Graphics = Graphics;
        }

        public void LoadContent(string TextureLocation)
        {
            Texture = StreamTexture.LoadTextureFromStream(Graphics, TextureLocation);
        }

        public void UpdateOnce(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        public void CalculateVelocity(Vector2 StartingPosition, Vector2 Destination, float Speed)
        {
            this.StartingPosition = StartingPosition;
            this.Speed = Speed;

            DeltaPosition.X = Destination.X - StartingPosition.X;
            DeltaPosition.Y = Destination.Y - StartingPosition.Y;

            double VectorLength = Math.Sqrt((DeltaPosition.X * DeltaPosition.X) + (DeltaPosition.Y * DeltaPosition.Y));

            Direction.X = (float)DeltaPosition.X / (float)VectorLength;
            Direction.Y = (float)DeltaPosition.Y / (float)VectorLength;
        }

        public void Update(GameTime gameTime)
        {
            float ElapsedGameTimeSpeed = (float)(Speed * gameTime.ElapsedGameTime.TotalMilliseconds);

            Velocity.X += Direction.X * ElapsedGameTimeSpeed;
            Velocity.Y += Direction.Y * ElapsedGameTimeSpeed;

            rectangle = new Rectangle((int)(Velocity.X + StartingPosition.X), (int)(Velocity.Y + StartingPosition.Y), 32, 32);
        }

        public void Draw()
        {
            if (IsAlive)
                spriteBatch.Draw(Texture, rectangle, Color.White);
        }
    }
}