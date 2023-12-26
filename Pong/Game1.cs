using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Entities; 


namespace Pong;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Game entities
    private Paddle _paddle1;
    private Paddle _paddle2;
    private Ball _ball;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _graphics.PreferredBackBufferWidth = 800;  // Set this value to the desired width of your window
        _graphics.PreferredBackBufferHeight = 600;   // Set this value to the desired height of your window
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        // Initialize paddles and ball
        _paddle1 = new Paddle(GraphicsDevice, _graphics.PreferredBackBufferHeight);
        _paddle2 = new Paddle(GraphicsDevice, _graphics.PreferredBackBufferHeight)
        {
            Position = new Vector2(_graphics.PreferredBackBufferWidth - 60, _graphics.PreferredBackBufferHeight / 2f) // Adjust the position for the second paddle
        };
        _ball = new Ball(GraphicsDevice, _graphics.PreferredBackBufferHeight);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        // Update game entities
        _paddle1.Update(gameTime);
        _paddle2.Update(gameTime); // You'll need to modify the control keys for the second paddle
        _ball.Update(gameTime);

        // Collision detection with paddles
        if (Collision(_ball, _paddle1) || Collision(_ball, _paddle2))
        {
            _ball.Velocity.X = -_ball.Velocity.X; // Reverse horizontal direction
        }

        // Scoring
        if (_ball.Position.X < 0 || _ball.Position.X > _graphics.PreferredBackBufferWidth)
        {
            // Update score
            // Reset ball position
            _ball.ResetPosition(GraphicsDevice);
        }

        base.Update(gameTime);
    }

    //Checking for Collisions
    private bool Collision(Ball ball, Paddle paddle)
    {
        // Create rectangles around the ball and paddle for collision detection
        Rectangle ballRect = new Rectangle((int)ball.Position.X, (int)ball.Position.Y, ball.Texture.Width, ball.Texture.Height);
        Rectangle paddleRect = new Rectangle((int)paddle.Position.X, (int)paddle.Position.Y, paddle.Texture.Width, paddle.Texture.Height);

        // Check if the ball rectangle intersects with the paddle rectangle
        return ballRect.Intersects(paddleRect);
    }


    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        _paddle1.Draw(_spriteBatch);
        _paddle2.Draw(_spriteBatch);
        _ball.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
