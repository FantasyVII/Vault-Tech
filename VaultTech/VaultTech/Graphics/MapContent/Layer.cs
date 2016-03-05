/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 16/March/2014
 * Date Moddified :- 06/October/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VaultTech.Graphics.MapContent
{
    /// <summary>
    /// This struct will store the layer textures that are created from CreateTextures() function.
    /// </summary>
    struct LayerTexture
    {
        public Texture2D Texture;
        public Vector2 Position;
        public Vector2 Size;
        public Rectangle rectangle;

        public LayerTexture(Texture2D Texture, Vector2 Position, Vector2 Size)
        {
            this.Texture = Texture;
            this.Position = Position;
            this.Size = Size;
            rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }
    }

    /// <summary>
    /// This class is responsible for drawing the tiles on a single layer then all these layers are drawn on the map.
    /// </summary>
    public class Layer
    {
        /// <summary>
        /// MonoGame GraphicsDeviceManager.
        /// </summary>
        GraphicsDeviceManager Graphics;
        /// <summary>
        /// MonoGame SpriteBatch.
        /// </summary>
        SpriteBatch spriteBatch;

        /// <summary>
        /// Map class.
        /// </summary>
        Map map;

        /// <summary>
        /// 2D array that will store the uniqe tiles ID numbers.
        /// </summary>
        public int[,] Array;

        /// <summary>
        /// List of 
        /// </summary>
        List<LayerTexture> MapTextures;

        public int DrawCalls;

        /// <summary>
        /// Layer constructor that initialize the layer array that holdes the uniqe tiles ID numbers.
        /// </summary>
        /// <param name="map">Mpa class</param>
        public Layer(Map map)
        {
            this.map = map;

            MapTextures = new List<LayerTexture>();

            Array = new int[(int)map.ArraySize.X, (int)map.ArraySize.Y];

            //Sets the layer array to -1 which mean this array is empty and it will not draw anything.
            for (int y = 0; y < map.ArraySize.Y; y++)
                for (int x = 0; x < map.ArraySize.X; x++)
                    Array[x, y] = -1;
        }

        /// <summary>
        /// Initialize the layer class.
        /// </summary>
        /// <param name="Graphics">MonoGame GraphicsDeviceManager.</param>
        public void Initialize(GraphicsDeviceManager Graphics)
        {
            this.Graphics = Graphics;
        }

        /// <summary>
        /// Create the large textures if the RenderMode is set to LargeTexture. It also passes the SpriteBatch parameter to render the layer.
        /// </summary>
        /// <param name="spriteBatch">MonoGame SpriteBatch.</param>
        public void UpdateOnce(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;

            if (map.renderMode == Map.RenderMode.LargeTexture)
                CreateTextures();
        }

        /// <summary>
        /// This function will draw every tile on a texture that has the size of the current window size (viewport)
        /// then it will sort all these big textures to draw the full map on the screen.
        /// </summary>
        void CreateTextures()
        {
            Vector2 NumberOfMapTextures = map.Size / new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
            Vector2 MapTextureArraySize = new Vector2((float)Math.Ceiling(NumberOfMapTextures.X), (float)Math.Ceiling(NumberOfMapTextures.Y));

            for (int y = 0; y < MapTextureArraySize.Y; y++)
            {
                for (int x = 0; x < MapTextureArraySize.X; x++)
                {
                    CombineTextures.Begin(Graphics);
                    DrawTiles();
                    CombineTextures.End();

                    Texture2D MapTexture = CombineTextures.GetFinalTexture();

                    MapTextures.Add(new LayerTexture(MapTexture, new Vector2(x * Graphics.PreferredBackBufferWidth, y * Graphics.PreferredBackBufferHeight),
                                                               new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight)));
                }
            }
        }

        /// <summary>
        /// This function is responsible for drawing all the tiles on the screen. 
        /// But its used to draw everything on a backbuffer so the CreateTextures() 
        /// function would create a big texture out of all these tiles.
        /// </summary>
        void DrawTiles()
        {
            spriteBatch.Begin();

            for (int y = 0; y < map.ArraySize.Y; y++)
            {
                for (int x = 0; x < map.ArraySize.X; x++)
                {
                    Rectangle rectangle = new Rectangle((int)(x * map.tileBank.TileSize.X), (int)(y * map.tileBank.TileSize.Y), (int)map.tileBank.TileSize.X, (int)map.tileBank.TileSize.Y);

                    for (int i = 0; i < map.tileBank.Tiles.Count; i++)
                    {
                        if (Array[x, y] == map.tileBank.Tiles[i].ID)
                            spriteBatch.Draw(map.tileBank.TileSheets[map.tileBank.Tiles[i].TileSheetID].Texture, rectangle, map.tileBank.Tiles[i].SourceRectangle, Color.White);
                    }
                }
            }

            spriteBatch.End();
        }

        /// <summary>
        /// This is the main draw function that will render the map in one of two ways depending on the RenderMode that is set in the map file.
        /// 1- It will render everything on a big texture and then render that texture on the screen. This is optimal for drawing a log of small tiles on the screen at one.
        /// 2- It will render every tile directly on the screen. This is optimal for rendering large size tiles or small number of tiles.
        /// </summary>
        /// <param name="color">Layer color.</param>
        internal void Draw(Color color)
        {
            if (map.renderMode == Map.RenderMode.SingleTile)
            {
                DrawCalls = 0;

                for (int y = (int)Math.Floor(Camera.rectangle.Y / map.tileBank.TileSize.Y); y < (int)Math.Ceiling((Camera.rectangle.Height + Camera.rectangle.Y) / map.tileBank.TileSize.Y); y++)
                {
                    for (int x = (int)Math.Floor(Camera.rectangle.X / map.tileBank.TileSize.X); x < (int)Math.Ceiling((Camera.rectangle.Width + Camera.rectangle.X) / map.tileBank.TileSize.X); x++)
                    {
                        Rectangle rectangle = new Rectangle((int)(x * map.tileBank.TileSize.X), (int)(y * map.tileBank.TileSize.Y), (int)map.tileBank.TileSize.X, (int)map.tileBank.TileSize.Y);

                        if ((x >= 0 && x < map.ArraySize.X) && (y >= 0 && y < map.ArraySize.Y))
                        {
                            int TileID = Array[x, y];

                            if (TileID >= 0)
                            {
                                if (rectangle.Intersects(Camera.rectangle))
                                {
                                    spriteBatch.Draw(map.tileBank.TileSheets[map.tileBank.Tiles[TileID].TileSheetID].Texture, rectangle, map.tileBank.Tiles[TileID].SourceRectangle, color);
                                    DrawCalls++;
                                }
                            }
                        }
                    }
                }
            }

            else if (map.renderMode == Map.RenderMode.LargeTexture)
                for (int i = 0; i < MapTextures.Count; i++)
                    if (Camera.rectangle.Intersects(MapTextures[i].rectangle))
                        spriteBatch.Draw(MapTextures[i].Texture, MapTextures[i].Position, Color.White);
        }
    }
}