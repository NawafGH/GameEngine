using Microsoft.Xna.Framework;

namespace MyPhysicsEngine.Entities
{
    public enum ShapeType
    {
        Circle,
        Square
    }

    public class PhysicsObject
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public float Mass { get; set; }
        public Vector2 Size { get; set; }  // Used for Square (Width, Height)
        public float Radius { get; set; }  // Used for Circle
        public Color Color { get; set; }
        public ShapeType Shape { get; set; }

        // Constructor for Square
        public PhysicsObject(Vector2 position, float mass, Vector2 size, Color color)
        {
            Position = position;
            Mass = mass;
            Size = size;
            Color = color;
            Shape = ShapeType.Square;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
        }

        // Constructor for Circle
        public PhysicsObject(Vector2 position, float mass, float radius, Color color)
        {
            Position = position;
            Mass = mass;
            Radius = radius;
            Color = color;
            Shape = ShapeType.Circle;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
        }

        public void ApplyForce(Vector2 force)
        {
            Acceleration += force / Mass;
        }

        public void Update(GameTime gameTime)
        {
            Velocity += Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Acceleration = Vector2.Zero;
        }

        // Additional methods for physics calculations
    }
}
