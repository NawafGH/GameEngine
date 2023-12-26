using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong.Entities
{
    public class Paddle
    {
        public Vector2 Position;
        public Texture2D Texture;
        private readonly float _speed = 200f; // Speed of the paddle
        private readonly int _windowHeight;

        public Paddle(GraphicsDevice graphicsDevice, int windowHeight)
        {
            // Initialize the paddle texture (a simple white rectangle)
            Texture = new Texture2D(graphicsDevice, 10, 60); // Width and height of the paddle
            Color[] colorData = new Color[10 * 60];
            for (int i = 0; i < colorData.Length; ++i) colorData[i] = Color.White;
            Texture.SetData(colorData);

            _windowHeight = windowHeight;

            // Set initial position
            Position = new Vector2(50, windowHeight / 2f); // Adjust as needed
        }

        public void Update(GameTime gameTime)
        {
            // Get the current state of the keyboard
            var keyboardState = Keyboard.GetState();

            // Move the paddle up and down based on input
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                Position.Y -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                Position.Y += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            // Prevent the paddle from moving off the screen
            Position.Y = Math.Clamp(Position.Y, 0, _windowHeight - Texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
