/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 4/September/2014
 * Date Moddified :- 18/January/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace VaultTech.Physics.CollisionDetection
{
    public class Circle
    {
        public Vector2 CenterPosition;
        public float Radius;

        public Circle()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CenterPosition">Center Position</param>
        /// <param name="Radius">Radius</param>
        public Circle(Vector2 CenterPosition, float Radius)
        {
            this.CenterPosition = CenterPosition;
            this.Radius = Radius;
        }

        public bool Intersect(Circle circle)
        {
            Vector2 Distance = new Vector2(circle.CenterPosition.X - CenterPosition.X, circle.CenterPosition.Y - CenterPosition.Y);
            float SqrRadius = circle.Radius + Radius;

            if ((Distance.X * Distance.X) + (Distance.Y * Distance.Y) < SqrRadius * SqrRadius)
                return true;
            else
                return false;
        }
    }
}