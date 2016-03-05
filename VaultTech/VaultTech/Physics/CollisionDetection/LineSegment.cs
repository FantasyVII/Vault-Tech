/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 27/December/2014
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
    public class LineSegment
    {
        Point A, B;

        public LineSegment(Point A, Point B)
        {
            this.A = A;
            this.B = B;
        }

        public bool Intersect(LineSegment lineSegment)
        {
            //Create the first Vector with its points A and B.
            Vector2 s1 = new Vector2(B.X, B.Y) - new Vector2(A.X, A.Y);

            //Create the second Vector with its points A and B.
            Vector2 s2 = new Vector2(lineSegment.B.X, lineSegment.B.Y) - new Vector2(lineSegment.A.X, lineSegment.A.Y);


            Vector2 u = new Vector2(A.X, A.Y) - new Vector2(lineSegment.A.X, lineSegment.A.Y);

            float ip = 1f / (-s2.X * s1.Y + s1.X * s2.Y);

            float s = (-s1.Y * u.X + s1.X * u.Y) * ip;
            float t = (s2.X * u.Y - s2.Y * u.X) * ip;

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
                return true;

            return false;
        }

        public bool Intersect(Rectangle rectangle)
        {
            LineSegment TopLineSegment = new LineSegment(new Point(rectangle.X, rectangle.Y), new Point(rectangle.X + rectangle.Width, rectangle.Y));
            LineSegment RightLineSegment = new LineSegment(new Point(rectangle.X + rectangle.Width, rectangle.Y), new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height));
            LineSegment BottomLineSegment = new LineSegment(new Point(rectangle.X, rectangle.Y + rectangle.Height), new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height));
            LineSegment LeftLineSegment = new LineSegment(new Point(rectangle.X, rectangle.Y), new Point(rectangle.X, rectangle.Y + rectangle.Height));

            if (Intersect(TopLineSegment) || Intersect(RightLineSegment) || Intersect(BottomLineSegment) || Intersect(LeftLineSegment))
                return true;

            return false;
        }

        public Vector2 PointOfIntersection(LineSegment lineSegment)
        {
            //The point of intersection if found
            Vector2 i = Vector2.Zero;

            //Create the first Vector with its points A and B.
            Vector2 s1 = new Vector2(B.X, B.Y) - new Vector2(A.X, A.Y);

            //Create the second Vector with its points A and B.
            Vector2 s2 = new Vector2(lineSegment.B.X, lineSegment.B.Y) - new Vector2(lineSegment.A.X, lineSegment.A.Y);


            Vector2 u = new Vector2(A.X, A.Y) - new Vector2(lineSegment.A.X, lineSegment.A.Y);

            float ip = 1f / (-s2.X * s1.Y + s1.X * s2.Y);

            float s = (-s1.Y * u.X + s1.X * u.Y) * ip;
            float t = (s2.X * u.Y - s2.Y * u.X) * ip;

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
                return i = new Vector2(A.X, A.Y) + (s1 * t);

            return Vector2.Zero;
        }
    }
}