using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pong_Game
{
    // Create an enum to determine where a player is allowed to play
    public enum PlayField { Left, Right, TopLeft, TopRight, BottomLeft, BottomRight };

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class PongGame : Game
    {
        GraphicsDeviceManager graphics;             // Variable to store the MonoGame Graphics Device Manager
        SpriteBatch spriteBatch;                    // Variable to store the MonoGame Sprite Batch

        private const int ballSpeed = 5;            // Variable to store the speed of the ball
        private bool fourPlayers = false;           // Variable to determine if game should be played with four or two players
        Player[] players;                           // Array to store all players created

        // Constructor of MonoGame game class
        public PongGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Execute MonoGame base Initialization method
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Create player list to store all created players (using list for easy adding without setting a size first)
            List<Player> _players = new List<Player>();

            // Create players one and two
            Player playerOne = new Player(Color.Red, PlayField.Left, GraphicsDevice, spriteBatch);
            Player playerTwo = new Player(Color.Yellow, PlayField.Right, GraphicsDevice, spriteBatch);

            // Add all players to the player list
            _players.Add(playerOne);
            _players.Add(playerTwo);

            // When four player mode is activated, create more players and update old players update field and start locations
            if (fourPlayers)
            {
                // Edit playfields of players one and two and update start locations
                playerOne.playField = PlayField.TopLeft;
                playerOne.MoveToStartLocation();
                playerTwo.playField = PlayField.TopRight;
                playerTwo.MoveToStartLocation();

                // Create players three and four
                Player playerThree = new Player(Color.LimeGreen, PlayField.BottomLeft, GraphicsDevice, spriteBatch);
                Player playerFour = new Player(Color.CornflowerBlue, PlayField.BottomRight, GraphicsDevice, spriteBatch);

                // Add all players to the player list
                _players.Add(playerThree);
                _players.Add(playerFour);
            }
            
            // Convert the player list to an array and store it (arrays are faster and use less memory)
            players = _players.ToArray();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Exit game when escape is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // When key S is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                players[0].Move(false);
            
            // When key W is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                players[0].Move(true);

            // When arrow down is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                players[1].Move(false);

            // When arrow up is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                players[1].Move(true);

            // Execute MonoGame base Update method
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Set background of screen
            GraphicsDevice.Clear(Color.Black);

            // Start the spriteBatch to allow drawing
            spriteBatch.Begin();

            // Draw all players
            foreach (Player p in players)
                p.Draw();

            // End the spritebatch
            spriteBatch.End();

            // Execute MonoGame base draw method
            base.Draw(gameTime);
        }
    }

    // Player class
    public class Player
    {
        public int lives = 3;                                   // Variable to store lives for player
        private const int speed = 5;                            // Set speed of player (is for every player)
        private readonly Point size = new Point(20, 80);        // Set size of player (is for every player, readonly to prevent accedental edits)
        public Point location;                                  // Variable to store player location
        private Texture2D texture;                              // Variable to store the texture of the player
        private Color color;                                    // Variable to store the color of the player
        private GraphicsDevice gDevice;                         // Variable to store the GrapicsDevice from game
        private SpriteBatch spriteBatch;                        // Variable to store spriteBatch
        public PlayField playField;                             // Variable to store playfield for player -> where is he allowed to play

        // Constructor for the player class
        public Player(Color color, PlayField playField, GraphicsDevice gDevice, SpriteBatch spriteBatch)
        {
            this.playField = playField;     // Copy the given value to the local playField variable
            this.color = color;             // Set color of player to the color given
            this.gDevice = gDevice;         // Store graphics device for later use
            this.spriteBatch = spriteBatch; // Store spriteBatch for later drawing of player

            // Move the player to the start location
            MoveToStartLocation();

            // Create texture for player and set color data
            texture = new Texture2D(gDevice, size.X, size.Y);
            SetColor();
        }

        // Move the player to the start location
        public void MoveToStartLocation()
        {
            location = DetermineStartLocation();
        }

        // Function to determine start location of player (for multiplayer use)
        private Point DetermineStartLocation()
        {
            int rightSideX = gDevice.Viewport.Bounds.Right - size.X;                                // Calculate the X coördinate for players on the right side of the screen
            int halfScreenY = (gDevice.Viewport.Bounds.Height / 2) - (size.Y / 2);                  // Calculate the Y coördinate if player is allowed to use the whole side of the screen
            int topHalfScreenY = (gDevice.Viewport.Bounds.Height / 4) - (size.Y / 2);               // Calculate the Y coördinate if player is allowed to use only the top of the screen
            int bottomHalfScreenY = (int)(gDevice.Viewport.Bounds.Height * 0.75) - (size.Y / 2);    // Calculate the Y coördinate if player is allowed to use only the bottom of the screen

            // Set location of player depending on the PlayField the player is allowed to play on
            switch (playField)
            {
                // If playfield is left side of the screen, place player in the middle of the left side of the screen
                case PlayField.Left:
                    return new Point(0, halfScreenY);
                // If playfield is right side of the screen, place player in the middle of the right side of the screen
                case PlayField.Right:
                    return new Point(rightSideX, halfScreenY);
                // If playfield is top left of the screen, place player in the middle of the top half of the screen on the left side
                case PlayField.TopLeft:
                    return new Point(0, topHalfScreenY);
                // If playfield is bottom left of the screen, place player in the middle of the bottom half of the screen on the left side
                case PlayField.BottomLeft:
                    return new Point(0, bottomHalfScreenY);
                // If playfield is top right of the screen, place player in the middle of the top half of the screen on the right side
                case PlayField.TopRight:
                    return new Point(rightSideX, topHalfScreenY);
                // If playfield is bottom right of the screen, place player in the middle of the bottom half of the screen on the right side
                case PlayField.BottomRight:
                    return new Point(rightSideX, bottomHalfScreenY);
                // If playfield isn't set somehow, throw an exception killing the program.
                default:
                    throw new Exception("No player field given! Please contact developers.");
            }
        }

        // Set the color data of the texture of the player
        private void SetColor()
        {
            // Make color array same size as there are pixels in the texture
            Color[] data = new Color[size.X * size.Y];
            // Set each color to the desired player color
            for (int i = 0; i < data.Length; i++)
                data[i] = color;

            // Apply the color array to the texture pixels
            texture.SetData(data);
        }

        // Draw player on the screen
        public void Draw()
        {
            spriteBatch.Draw(texture, new Rectangle(location, size), color);
        }

        // Move the player - Arg. moveUp to determine if player should move up or down
        public void Move(bool moveUp)
        {
            int _speed = moveUp ? speed : (speed * -1); // Set speed positive or negative depending on the moveUp arg., then move player
            int _locationY = location.Y;                // Create temporary Y location variable (to prevent player from going out of bound temporarily)
            _locationY -= _speed;                       // Change the location depending on the speed

            // If player can reach the bottom of the screen (depending on playfield), prevent it from going out of bounds
            if (playField == PlayField.Left || playField == PlayField.Right || playField == PlayField.BottomLeft || playField == PlayField.BottomRight)
            {
                // Prevent the player from going out of the screen on the bottom
                if ((_locationY + size.Y) > gDevice.Viewport.Bounds.Height)
                    _locationY = gDevice.Viewport.Bounds.Bottom - size.Y;
            }

            // If player can reach the top of the screen (depending on playfield), prevent it from going out of bounds
            if (playField == PlayField.Left || playField == PlayField.Right || playField == PlayField.TopLeft || playField == PlayField.TopRight)
            {
                // Prevent the player from going out of the screen on the top
                if (_locationY < 0)
                    _locationY = 0;
            }
            // If the player is allowed to use only the top half of the screen (depending on playfield), prevent it from going out of bounds
            if (playField == PlayField.TopLeft || playField == PlayField.TopRight)
            {
                // Prevent player from going under half of the screen.
                if ((_locationY + size.Y) > (gDevice.Viewport.Bounds.Height / 2))
                    _locationY = (gDevice.Viewport.Bounds.Height / 2) - size.Y;
            }
            // If the player is allowed to use only the bottom half of the screen (depending of playfield), prevent it from going out of bounds
            else if (playField == PlayField.BottomRight || playField == PlayField.BottomLeft)
            {
                // Prevent player from going above half of the screen.
                if (_locationY < (gDevice.Viewport.Bounds.Height / 2))
                    _locationY = (gDevice.Viewport.Bounds.Height / 2);
            }

            // Set the actual location value to the temporary location value
            location.Y = _locationY;
        }
    }
}
