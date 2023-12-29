using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyPhysicsEngine.Entities;
using MyPhysicsEngine.Physics;
using MyPhysicsEngine.Graphics;
using System.Collections.Generic;
using ImGuiNET;
using ImGuiNET.SampleProgram.XNA;
using System;

namespace Physics2D;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _texture;
    private List<PhysicsObject> _physicsObjects;
    private KeyboardState _previousKeyboardState;
    private ImGuiRenderer _imGuiRenderer;
    private Dictionary<float, Texture2D> _circleTextures;
    private enum ObjectType { None, Circle, Square }
    private ObjectType _selectedObjectType = ObjectType.None;
    private float _objectMass = 1.0f;
    private float _objectWidth = 50f;
    private float _objectHeight = 50f;
    private float _objectRadius = 30f;
    private bool _isDynamic = true;
    private Color _objectColor = Color.Green;
    private bool _spacePressed = false;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _physicsObjects = new List<PhysicsObject>();
        _circleTextures = new Dictionary<float, Texture2D>();
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

        // Create a 1x1 white texture for rendering squares
        _texture = new Texture2D(GraphicsDevice, 1, 1);
        _texture.SetData(new[] { Color.White });

        // Initialize ImGui
        _imGuiRenderer = new ImGuiRenderer(this);
        _imGuiRenderer.RebuildFontAtlas();
    }

    protected override void Update(GameTime gameTime)
    {
        var currentKeyboardState = Keyboard.GetState();
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.Escape))
            Exit();

        // Start ImGui frame
        _imGuiRenderer.BeforeLayout(gameTime);

        // ImGui interface
        ImGui.Begin("Sidebar");
        if (ImGui.Button("Create Circle"))
        {
            _selectedObjectType = ObjectType.Circle;
        }
        if (ImGui.Button("Create Square"))
        {
            _selectedObjectType = ObjectType.Square;
        }
        ImGui.End();

        // Properties sidebar
        if (_selectedObjectType != ObjectType.None)
        {
            ImGui.Begin("Properties");

            ImGui.InputFloat("Mass", ref _objectMass);
            if (_selectedObjectType == ObjectType.Circle)
            {
                float newRadius = _objectRadius;
                if (ImGui.InputFloat("Radius", ref newRadius) && newRadius > 0)
                {
                    if (newRadius != _objectRadius)
                    {
                        _objectRadius = newRadius;
                        // Create new circle texture if it doesn't exist
                        if (!_circleTextures.ContainsKey(_objectRadius))
                        {
                            _circleTextures[_objectRadius] = Renderer.CreateCircleTexture(GraphicsDevice, (int)_objectRadius, Color.White);
                        }
                    }
                }
            }
            else
            {
                ImGui.InputFloat("Width", ref _objectWidth);
                ImGui.InputFloat("Height", ref _objectHeight);
            }

            ImGui.Checkbox("Dynamic", ref _isDynamic);

            if (ImGui.ColorButton("Green", new System.Numerics.Vector4(0, 1, 0, 1))) _objectColor = Color.Green;
            ImGui.SameLine();
            if (ImGui.ColorButton("Blue", new System.Numerics.Vector4(0, 0, 1, 1))) _objectColor = Color.Blue;
            ImGui.SameLine();
            if (ImGui.ColorButton("Red", new System.Numerics.Vector4(1, 0, 0, 1))) _objectColor = Color.Red;

            ImGui.End();
        }

        // Handle object creation on spacebar press
        MouseState mouseState = Mouse.GetState();
        if (!_spacePressed && currentKeyboardState.IsKeyDown(Keys.Space))
        {
            Vector2 position = new Vector2(mouseState.X, mouseState.Y);
            PhysicsObject newObject;

            if (_selectedObjectType == ObjectType.Circle)
            {
                if (!_circleTextures.ContainsKey(_objectRadius))
                {
                    _circleTextures[_objectRadius] = Renderer.CreateCircleTexture(GraphicsDevice, (int)_objectRadius, Color.White);
                }
                newObject = new PhysicsObject(position, _objectMass, _objectRadius, _objectColor) { IsDynamic = _isDynamic, CircleTexture = _circleTextures[_objectRadius] };
            }
            else // ObjectType.Square
            {
                Vector2 size = new Vector2(_objectWidth, _objectHeight);
                newObject = new PhysicsObject(position, _objectMass, size, _objectColor) { IsDynamic = _isDynamic };
            }

            _physicsObjects.Add(newObject);
            _spacePressed = true;
        }
        else if (_spacePressed && !currentKeyboardState.IsKeyDown(Keys.Space))
        {
            _spacePressed = false;
        }

        // Update physics for each object
        foreach (var obj in _physicsObjects)
        {
            if (obj.IsDynamic)
            {
                Simulate(obj, gameTime);
            }
        }

        _previousKeyboardState = currentKeyboardState;
        _imGuiRenderer.AfterLayout();
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
            Texture2D objectTexture = (obj.Shape == ShapeType.Circle && obj.CircleTexture != null) ? obj.CircleTexture : _texture;
            Renderer.DrawPhysicsObject(_spriteBatch, obj, _texture, objectTexture);
        }
        _spriteBatch.End();

        _imGuiRenderer.AfterLayout();

        base.Draw(gameTime);
    }
}
