using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyPhysicsEngine.Entities;

namespace MyPhysicsEngine.Graphics
{
    public static class Renderer
    {
        public static void DrawPhysicsObject(SpriteBatch spriteBatch, PhysicsObject obj, Texture2D texture, Texture2D circleTexture)
        {
            if (obj.Shape == ShapeType.Circle)
            {
                DrawCircle(spriteBatch, obj, circleTexture);
            }
            else if (obj.Shape == ShapeType.Square)
            {
                DrawSquare(spriteBatch, obj, texture);
            }
        }

        private static void DrawCircle(SpriteBatch spriteBatch, PhysicsObject obj, Texture2D circleTexture)
        {
            Vector2 origin = new Vector2(circleTexture.Width / 2, circleTexture.Height / 2);
            spriteBatch.Draw(circleTexture, obj.Position, null, obj.Color, 0f, origin, 1f, SpriteEffects.None, 0f);
        }

        private static void DrawSquare(SpriteBatch spriteBatch, PhysicsObject obj, Texture2D texture)
        {
            Rectangle rect = new Rectangle((int)obj.Position.X, (int)obj.Position.Y, (int)obj.Size.X, (int)obj.Size.Y);
            spriteBatch.Draw(texture, rect, obj.Color);
        }

        public static Texture2D CreateCircleTexture(GraphicsDevice graphicsDevice, float radius, Color color)
        {
            int diameter =  ((int) radius) * 2;
            Texture2D texture = new Texture2D(graphicsDevice, diameter, diameter);
            Color[] colorData = new Color[diameter * diameter];

            float radiussquared = radius * radius;

            for (int x = 0; x < diameter; x++)
            {
                for (int y = 0; y < diameter; y++)
                {
                    int index = x * diameter + y;
                    Vector2 position = new Vector2(x - radius, y - radius);
                    if (position.LengthSquared() <= radiussquared)
                    {
                        colorData[index] = color;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }

    }
}
