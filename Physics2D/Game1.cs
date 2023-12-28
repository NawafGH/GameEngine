using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyPhysicsEngine.Entities;
using MyPhysicsEngine.Physics;
using MyPhysicsEngine.Graphics;
using System.Collections.Generic;

namespace Physics2D;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _texture;
    private Texture2D circleTexture;
    private List<PhysicsObject> _physicsObjects;
    private KeyboardState _previousKeyboardState;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _physicsObjects = new List<PhysicsObject>();
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 1000; // Increase width
        _graphics.PreferredBackBufferHeight = 600;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Create a 1x1 white texture for rendering
        _texture = new Texture2D(GraphicsDevice, 1, 1);
        _texture.SetData(new[] { Color.White });

        int circleRadius = 40; // Example radius
        circleTexture = Renderer.CreateCircleTexture(GraphicsDevice, circleRadius, Color.White);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        KeyboardState currentKeyboardState = Keyboard.GetState();
        MouseState mouseState = Mouse.GetState();

        // Check for new key press
        if (currentKeyboardState.IsKeyDown(Keys.C) && !_previousKeyboardState.IsKeyDown(Keys.C))
        {
            // Add a new circle
            _physicsObjects.Add(new PhysicsObject(new Vector2(mouseState.X, mouseState.Y), 1f, 15f, Color.Green));
        }
        if (currentKeyboardState.IsKeyDown(Keys.S) && !_previousKeyboardState.IsKeyDown(Keys.S))
        {
            // Add a new square
            _physicsObjects.Add(new PhysicsObject(new Vector2(mouseState.X, mouseState.Y), 1f, new Vector2(40, 40), Color.Blue));
        }

        // Simulate physics for each object
        foreach (var obj in _physicsObjects)
        {
            Simulate(obj, gameTime);
        }

        // Update previous keyboard state
        _previousKeyboardState = currentKeyboardState;

        base.Update(gameTime);
    }

    private void Simulate(PhysicsObject obj, GameTime gameTime)
    {
        Gravity.ApplyGravity(obj);
        obj.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        foreach (var obj in _physicsObjects)
        {
            // Pass both square and circle textures to the renderer
            Renderer.DrawPhysicsObject(_spriteBatch, obj, _texture, circleTexture);
        }
        _spriteBatch.End();

        base.Draw(gameTime);
    }

}
