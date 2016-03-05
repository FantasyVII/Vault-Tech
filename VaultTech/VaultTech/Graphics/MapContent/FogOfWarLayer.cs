/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 24/March/2014
 * Date Moddified :- 9/February/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VaultTech.Graphics.MapContent
{
    public class FogOfWarLayer
    {
        SpriteBatch spriteBatch;

        Map map;
        int[,] Array;
        public bool[,] Explored;
        public bool[,] UnitInExploredArea;
        public Rectangle[,] CollitionRectangle;

        public Texture2D Texture;


        public FogOfWarLayer(Map map)
        {
            this.map = map;
        }

        public void InitializeArray()
        {
            Array = new int[(int)map.ArraySize.X, (int)map.ArraySize.Y];
            Explored = new bool[(int)map.ArraySize.X, (int)map.ArraySize.Y];
            UnitInExploredArea = new bool[(int)map.ArraySize.X, (int)map.ArraySize.Y];
            CollitionRectangle = new Rectangle[(int)map.ArraySize.X, (int)map.ArraySize.Y];

            for (int x = 0; x < map.ArraySize.X; x++)
            {
                for (int y = 0; y < map.ArraySize.Y; y++)
                {
                    Explored[x, y] = false;
                    UnitInExploredArea[x, y] = false;
                }
            }
        }

        public void UpdateOnce(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        public void Draw()
        {
            for (int x = 0; x < map.ArraySize.X; x++)
            {
                for (int y = 0; y < map.ArraySize.Y; y++)
                {
                    Vector2 TilePosition = new Vector2((float)x * map.tileBank.TileSize.X, (float)y * map.tileBank.TileSize.Y);
                    CollitionRectangle[x, y] = new Rectangle((int)TilePosition.X, (int)TilePosition.Y, (int)map.tileBank.TileSize.X, (int)map.tileBank.TileSize.Y);

                    if (!Explored[x, y] && Camera.rectangle.Intersects(CollitionRectangle[x, y]))
                        spriteBatch.Draw(Texture, TilePosition, Color.White);

                    if (Explored[x, y] && !UnitInExploredArea[x, y] && Camera.rectangle.Intersects(CollitionRectangle[x, y]))
                        spriteBatch.Draw(Texture, TilePosition, Color.White * 0.6f);
                }
            }
        }
    }
}