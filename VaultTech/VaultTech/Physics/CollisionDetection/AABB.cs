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

namespace VaultTech.Physics.CollisionDetection
{
    public class AABB
    {
        public bool CoolideRight = false, CoolideLeft = false, CoolideUp = false, CoolideDown = false;

        public void CalculateCollision(PhysicsObject MovingObject, PhysicsObject StaticObject)
        {
            float rightEdgeDistance = StaticObject.Position.X - (MovingObject.Position.X + MovingObject.Size.X);
            float leftEdgeDistance = StaticObject.Position.X + StaticObject.Size.X - MovingObject.Position.X;

            float TopEdgeDistance = StaticObject.Position.Y - (MovingObject.Position.Y + MovingObject.Size.Y);
            float BottomEdgeDistance = StaticObject.Position.Y + StaticObject.Size.Y - MovingObject.Position.Y;


            float Left_Right_SmallerDistance = Math.Min(Math.Abs(rightEdgeDistance), Math.Abs(leftEdgeDistance));
            float Top_Bottom_SmallerDistance = Math.Min(Math.Abs(TopEdgeDistance), Math.Abs(BottomEdgeDistance));

            float smallerDistance = Math.Min(Math.Abs(Left_Right_SmallerDistance), Math.Abs(Top_Bottom_SmallerDistance));

            if (MovingObject.Rectangle.Intersects(StaticObject.Rectangle))
            {
                if (smallerDistance == Math.Abs(rightEdgeDistance))
                {

                   // MovingObject.Position.X = StaticObject.Position.X - MovingObject.Size.X;
                    CoolideRight = true;

                }

                if (smallerDistance == Math.Abs(leftEdgeDistance))
                {

                   // MovingObject.Position.X = StaticObject.Position.X + StaticObject.Size.X;
                    CoolideLeft = true;
                }

                if (smallerDistance == Math.Abs(BottomEdgeDistance))
                {
                   // MovingObject.Position.Y = StaticObject.Position.Y + StaticObject.Size.Y;
                    CoolideDown = true;
                }

                if (smallerDistance == Math.Abs(TopEdgeDistance))
                {
                   // MovingObject.Position.Y = StaticObject.Position.Y - MovingObject.Size.Y;
                    CoolideUp = true;
                }
            }
        }
    }
}