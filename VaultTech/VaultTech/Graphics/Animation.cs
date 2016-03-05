/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 13/July/2014
 * Date Moddified :- 23/January/2016
 * </Copyright>
 */

using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VaultTech.Contents;

namespace VaultTech.Graphics
{
    class Sequence
    {
        public int Row;
        public string Name;
        public List<Frame> Frames;

        public Sequence(int Row, string Name, List<Frame> Frames)
        {
            this.Row = Row;
            this.Name = Name;
            this.Frames = Frames;
        }
    };

    class Frame
    {
        public int Index, TimeLength;

        public Frame(int Index, int TimeLength)
        {
            this.Index = Index;
            this.TimeLength = TimeLength;
        }
    };

    public class Animation
    {
        #region MonoGame Properties
        GraphicsDeviceManager Graphics;
        SpriteBatch spriteBatch;
        #endregion
        #region Public variables
        public Vector2 Position, SingleSpriteSize;
        public bool Pause, Reset;
        #endregion
        #region Private variables
        Stopwatch Timer;

        Texture2D SpriteSheet;
        Rectangle SourceRectangle;

        int CurrentFrame, TotalFrames;
        float TotalFrameTime;
        Vector2 DefaultFrame;
        #endregion

        public int SequenceIndex, frameIndex;
        bool PlaySequence = false;
        List<Sequence> Sequences = new List<Sequence>();
        bool loop, Backward;

        public Animation(Vector2 DefaultFrame)
        {
            this.DefaultFrame = DefaultFrame;
            Timer = new Stopwatch();
            Timer.Start();
        }

        public void Initialize(GraphicsDeviceManager Graphics)
        {
            this.Graphics = Graphics;
        }

        public void LoadContent(string AnimationFile)
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(FileManager.ContentFolder + AnimationFile);

            if (xmlDoc.SelectSingleNode("Animation/File") != null)
            {
                string TexturePath = xmlDoc.SelectSingleNode("/Animation/File").Attributes.GetNamedItem("Location").Value;
                SpriteSheet = StreamTexture.LoadTextureFromStream(Graphics, TexturePath);
            }

            if (xmlDoc.SelectSingleNode("Animation/SingleSpriteSize") != null)
                SingleSpriteSize = new Vector2(int.Parse(xmlDoc.SelectSingleNode("Animation/SingleSpriteSize").Attributes.GetNamedItem("Width").Value),
                                                int.Parse(xmlDoc.SelectSingleNode("Animation/SingleSpriteSize").Attributes.GetNamedItem("Height").Value));

            foreach (XmlNode SequenceNode in xmlDoc.SelectNodes("Animation/Sequence"))
            {
                List<Frame> Frames = new List<Frame>();

                foreach (XmlNode FrameNode in SequenceNode.ChildNodes)
                    Frames.Add(new Frame(int.Parse(FrameNode.Attributes.GetNamedItem("Index").Value), int.Parse(FrameNode.Attributes.GetNamedItem("Length").Value)));

                Sequences.Add(new Sequence(int.Parse(SequenceNode.Attributes.GetNamedItem("Row").Value), SequenceNode.Attributes.GetNamedItem("Name").Value, Frames));
            }
        }

        public void UpdateOnce(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        void PlayAnimation()
        {
            if (PlaySequence)
            {
                if (SequenceIndex <= Sequences.Count - 1)
                {
                    if (Timer.ElapsedMilliseconds >= Sequences[SequenceIndex].Frames[frameIndex].TimeLength)
                    {
                        Timer.Restart();

                        if (!Backward)
                        {
                            if (frameIndex >= Sequences[SequenceIndex].Frames.Count - 1)
                            {
                                if(loop)
                                    frameIndex = 0;
                                else
                                    frameIndex = Sequences[SequenceIndex].Frames.Count - 1;
                            }
                            else
                                frameIndex++;
                        }
                        else
                        {
                            if (frameIndex <= 0)
                            {
                                if (loop)
                                    frameIndex = Sequences[SequenceIndex].Frames.Count - 1;
                                else
                                    frameIndex = 0;
                            }
                            else
                                frameIndex--;
                        }
                    }
                    else if (Timer.ElapsedMilliseconds <= Sequences[SequenceIndex].Frames[frameIndex].TimeLength)
                        SourceRectangle = new Rectangle((int)(Sequences[SequenceIndex].Frames[frameIndex].Index * SingleSpriteSize.X), (int)(Sequences[SequenceIndex].Row * SingleSpriteSize.Y), (int)SingleSpriteSize.X, (int)SingleSpriteSize.Y);
                }
            }
        }

        public void Play(int SequenceIndex, bool loop, bool Backward)
        {
            this.SequenceIndex = SequenceIndex;
            this.loop = loop;
            this.Backward = Backward;
            PlaySequence = true;
        }

        public void Stop(int StopAtSequenceIndex, int StopAtFrame)
        {
            SequenceIndex = StopAtSequenceIndex;
            frameIndex = StopAtFrame;
            SourceRectangle = new Rectangle((int)(Sequences[SequenceIndex].Frames[frameIndex].Index * SingleSpriteSize.X), (int)(Sequences[SequenceIndex].Row * SingleSpriteSize.Y), (int)SingleSpriteSize.X, (int)SingleSpriteSize.Y);
            PlaySequence = false;
        }

        public void Update(GameTime gameTime)
        {
            PlayAnimation();
        }

        public void Draw()
        {
            spriteBatch.Draw(SpriteSheet, Position, SourceRectangle, Color.White);
        }
    }
}