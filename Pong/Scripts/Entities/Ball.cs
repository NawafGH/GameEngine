using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong.Entities
{
    public class Ball
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Texture2D Texture;
        private readonly int _windowHeight;


        public Ball(GraphicsDevice graphicsDevice, int windowHeight)
        {
            // Initialize the ball texture (a simple white circle)
            Texture = new Texture2D(graphicsDevice, 10, 10); // Size of the ball
            Color[] colorData = new Color[10 * 10];
            for (int i = 0; i < colorData.Length; ++i) colorData[i] = Color.White;
            Texture.SetData(colorData);

            _windowHeight = windowHeight;

            // Set initial position and velocity
            ResetPosition(graphicsDevice);
        }

        public void ResetPosition(GraphicsDevice graphicsDevice)
        {
            Position = new Vector2(graphicsDevice.Viewport.Width / 2f, graphicsDevice.Viewport.Height / 2f);

            // Set a starting velocity, adjust as necessary
            Velocity = new Vector2(200f, 200f); 
        }

        public void Update(GameTime gameTime)
        {
            // Update ball position based on velocity
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Check for collision with top and bottom window boundaries
            if (Position.Y < 0 || Position.Y > _windowHeight - Texture.Height)
            {
                Velocity.Y = -Velocity.Y; // Reverse the vertical direction
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
