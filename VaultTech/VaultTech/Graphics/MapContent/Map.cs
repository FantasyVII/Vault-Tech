/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 16/March/2014
 * Date Moddified :- 23/January/2016
 * </Copyright>
 */

using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VaultTech.Physics;
using VaultTech.Contents;

namespace VaultTech.Graphics.MapContent
{
    public class Map
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch spriteBatch;

        public string MapFilePath;

        MapReader maploader;
        public TileBank tileBank;

        public List<Layer> Layers;
        public List<PhysicsObject> CollisionObjects;

        public Vector2 ArraySize;
        public Vector2 Size;
        public string Name;

        //public Texture2D texture;

        public enum RenderMode { SingleTile, LargeTexture }
        internal RenderMode renderMode;

        public static int TotalDrawCalls;

        public Map()
        {
            maploader = new MapReader(this);
            tileBank = new TileBank();
            Layers = new List<Layer>();

            CollisionObjects = new List<PhysicsObject>();
        }

        public void Initialize(GraphicsDeviceManager Graphics)
        {
            this.Graphics = Graphics;
            maploader.Initialize(Graphics);
        }

        public void LoadContent(string MapFilePath)
        {
            MapFilePath = MapFilePath.Replace('\\', '/');
            this.MapFilePath = MapFilePath;

            maploader.LoadContent(MapFilePath);
            tileBank.LoadContent();

            //texture = StreamTexture.LoadTextureFromStream(Graphics, "a.png");

            //ReduceNumberOfCollisionRectangle();
        }

        public void LoadContentFromArchive(string MapFilePath)
        {
            MapFilePath = MapFilePath.Replace('\\', '/');
            this.MapFilePath = MapFilePath;

            maploader.LoadContentFromArchive(MapFilePath);
            tileBank.LoadContent();

            //texture = StreamTexture.LoadTextureFromStream(Graphics, "a");

            //ReduceNumberOfCollisionRectangle();
        }

        public void UpdateOnce(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;

            for (int i = 0; i < Layers.Count; i++)
            {
                Layers[i].Initialize(Graphics);
                Layers[i].UpdateOnce(spriteBatch);
            }
        }

        /// <summary>
        /// It takes the collision Rectangle list and it reduces the amount of rectangles for better performance
        /// </summary>
        public void ReduceNumberOfCollisionRectangle()
        {
            int[,] CollisionArray = new int[(int)ArraySize.X, (int)ArraySize.Y];

            // Convert the Collision List back to 2D array.
            for (int i = 0; i < CollisionObjects.Count; i++)
                CollisionArray[(int)CollisionObjects[i].Position.X, (int)CollisionObjects[i].Position.Y] = 1;

            CollisionObjects = new List<PhysicsObject>();

            for (int y = 0; y < ArraySize.Y; y++)
            {
                for (int x = 0; x < ArraySize.X; x++)
                {
                    if (CollisionArray[x, y] == 1)
                    {
                        CollisionObjects.Add(new PhysicsObject(new Vector2(x, y), new Vector2(tileBank.TileSize.X, tileBank.TileSize.Y)));

                        for (int j = x; j < ArraySize.X - 1; j++)
                        {
                            if (CollisionArray[j + 1, y] == 1)
                            {
                                CollisionObjects[CollisionObjects.Count - 1] = new PhysicsObject(CollisionObjects[CollisionObjects.Count - 1].Position, new Vector2((int)(CollisionObjects[CollisionObjects.Count - 1].Size.X + tileBank.TileSize.X), (int)tileBank.TileSize.Y));
                                x = j + 1;
                            }
                            else
                            {
                                x = j + 1;
                                break;
                            }
                        }
                    }
                }
            }

            // Convert Collision List array positions to in game pixel positions
            for (int i = 0; i < CollisionObjects.Count; i++)
                CollisionObjects[i] = new PhysicsObject(CollisionObjects[i].Position * tileBank.TileSize, CollisionObjects[i].Size);
        }

        internal int[,] ConvertCollisionListTo2DArray()
        {
            int[,] CollisionArray = new int[(int)ArraySize.X, (int)ArraySize.Y];

            for (int i = 0; i < CollisionObjects.Count; i++)
            {
                for (int y = 0; y < (int)(CollisionObjects[i].Rectangle.Height / tileBank.TileSize.Y); y++)
                {
                    for (int x = 0; x < (int)(CollisionObjects[i].Rectangle.Width / tileBank.TileSize.X); x++)
                    {
                        CollisionArray[(int)(CollisionObjects[i].Rectangle.X / tileBank.TileSize.X) + x, (int)(CollisionObjects[i].Rectangle.Y / tileBank.TileSize.Y) + y] = 1;
                    }
                }
            }

            return CollisionArray;
        }

        public void Draw()
        {
            TotalDrawCalls = 0;

            for (int i = 0; i < Layers.Count; i++)
            {
                Layers[i].Draw(Color.White);
                TotalDrawCalls += Layers[i].DrawCalls;
            }
            /*
      Layers[0].Draw(Color.White);
      Layers[1].Draw(Color.White * 0.75f);*/

            
            /*for (int i = 0; i < CollisionObjects.Count; i++)
            {
                spriteBatch.Draw(texture, new Rectangle(CollisionObjects[i].Rectangle.X * 16, CollisionObjects[i].Rectangle.Y * 16,
                                                        CollisionObjects[i].Rectangle.Width * 16, CollisionObjects[i].Rectangle.Height * 16), Color.White * 0.4f);
            }*/

            //fogOfWarLayer.Draw(spriteBatch);
        }

        public void Draw(int LayerIndex)
        {
            TotalDrawCalls = 0;

            Layers[LayerIndex].Draw(Color.White);
            TotalDrawCalls += Layers[LayerIndex].DrawCalls;
            /*
            for (int i = 0; i < CollisionObjects.Count; i++)
            {
                spriteBatch.Draw(texture, new Rectangle(CollisionObjects[i].Rectangle.X * 16, CollisionObjects[i].Rectangle.Y * 16,
                                                        CollisionObjects[i].Rectangle.Width * 16, CollisionObjects[i].Rectangle.Height * 16), Color.White * 0.4f);
            }*/
        }
    }
}