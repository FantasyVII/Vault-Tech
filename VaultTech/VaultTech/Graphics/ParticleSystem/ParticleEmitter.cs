/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 26/November/2015
 * Date Moddified :- 28/November/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VaultTech.Graphics.ParticleSystem
{
    public class ParticleEmitter
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch spriteBatch;

        List<Particle> Particles;

        int MaxParticles;
        string FilePath;

        Random random;

        bool Run, addParticles, SpawnOverTime;
        int SpawnTime;
        double CurrentSpawnTime;

        public void Start()
        {
            Run = true;
            addParticles = true;
            SpawnOverTime = false;
        }

        public void Start(int SpawnTime)
        {
            this.SpawnTime = SpawnTime;
            Run = true;
            addParticles = true;
            SpawnOverTime = true;
        }

        public void Stop()
        {
            Run = false;
            addParticles = false;
            SpawnOverTime = false;
        }

        public void StopWhenAllDead()
        {
            addParticles = false;
            SpawnOverTime = false;
        }

        public void Reset()
        {
            Particles.Clear();
            CurrentSpawnTime = 0;
        }

        public ParticleEmitter(int MaxParticles)
        {

            this.MaxParticles = MaxParticles;

            Particles = new List<Particle>();
            random = new Random();
        }

        public void Initialize(GraphicsDeviceManager Graphics)
        {
            this.Graphics = Graphics;
        }

        public void LoadContent(string FilePath)
        {
            this.FilePath = FilePath;
        }

        public void UpdateOnce(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        void AddParticles(GameTime gameTime)
        {
            if (addParticles)
            {
                if (SpawnOverTime)
                {
                    CurrentSpawnTime += gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (CurrentSpawnTime >= SpawnTime)
                    {
                        if (Particles.Count <= MaxParticles)
                        {
                            Particles.Add(new Particle(new Vector2(random.Next(200, 500), random.Next(400, 500)), new Vector2(16, 16), new Vector2(0.01f, 0.01f), new Vector2(0.00f, -0.0005f), Color.White, random.Next(5000, 7000)));

                            Particles[Particles.Count - 1].Initialize(Graphics);
                            Particles[Particles.Count - 1].LoadContent(FilePath);
                            Particles[Particles.Count - 1].UpdateOnce(spriteBatch);
                        }

                        CurrentSpawnTime = 0;
                    }
                }
                else
                {
                    while (Particles.Count <= MaxParticles)
                    {
                        Particles.Add(new Particle(new Vector2(random.Next(200, 500), random.Next(400, 500)), new Vector2(16, 16), new Vector2(0.01f, 0.01f), new Vector2(0.00f, -0.0005f), Color.White, random.Next(5000, 7000)));

                        Particles[Particles.Count - 1].Initialize(Graphics);
                        Particles[Particles.Count - 1].LoadContent(FilePath);
                        Particles[Particles.Count - 1].UpdateOnce(spriteBatch);
                    }
                }
            }
        }

        public void Update(GameTime gameTime, string FilePath)
        {
            if (Run)
            {
                AddParticles(gameTime);

                for (int i = 0; i < Particles.Count; i++)
                {
                    Particles[i].Update(gameTime);

                    if (!Particles[i].IsAlive)
                        Particles.RemoveAt(i);
                }
            }

            if (!addParticles && Particles.Count <= 0)
                Run = false;
        }

        public void Draw()
        {
            for (int i = 0; i < Particles.Count; i++)
                Particles[i].Draw();
        }
    }
}