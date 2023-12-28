using Microsoft.Xna.Framework;
using MyPhysicsEngine.Entities;

namespace MyPhysicsEngine.Physics
{
    public static class Gravity
    {
        private static readonly Vector2 DownwardForce = new Vector2(0, 500f);

        public static void ApplyGravity(PhysicsObject obj)
        {
            obj.ApplyForce(DownwardForce * obj.Mass);
        }
    }
}
