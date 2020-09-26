using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Pong_Game
{
    // Create an enum for gamestates
    public enum GameState { InMenu, Playing, GameOver};

    // Create an enum to determine where a player is allowed to play
    public enum PlayField { Left, Right, TopLeft, TopRight, BottomLeft, BottomRight };

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class PongGame : Game
    {
        GraphicsDeviceManager graphics;                     // Variable to store the MonoGame Graphics Device Manager
        public SpriteBatch spriteBatch;                     // Variable to store the MonoGame Sprite Batch

        public bool fourPlayers;                            // Variable to determine if game should be played with four or two players
        public Player[] players;                            // Array to store all players created
        public Ball ball;                                   // Variable to store ball in

        public Texture2D lifeTexture;                       // Variable to store life texture in
        private Texture2D ballTexture;                      // Variable to store ball texture in

        public GameState gameState = GameState.Playing;     // Variable to store gamestate in

        public static PongGame pongGame;                    // Variable to store instance of this class

        // Get property to get screensize
        public Vector2 ScreenSize
        {
            get { return new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height); }
        }

        // Get property to get access to graphics device
        public GraphicsDevice gDevice
        {
            get { return GraphicsDevice; }
        }

        // Constructor of MonoGame game class
        public PongGame()
        {
            // MonoGame required lines
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Set window name
            this.Window.Title = "Pong";

            // Create a static instance of this class for accessibility from other classes
            pongGame = this;
        }

        // Main function, this function is called when the program is started
        [STAThread]
        static void Main()
        {
            // Run the pong game
            using (var game = new PongGame())
                game.Run();
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

            // Load the life texture
            lifeTexture = Content.Load<Texture2D>("life");

            // Load the ball texture
            ballTexture = Content.Load<Texture2D>("ball");

            // Create players
            CreatePlayers(false);

            // Create the ball
            CreateBall();
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

            // Only execute this code when game is running
            if (gameState == GameState.Playing)
            {
                // Check if playing with four or two players
                if (!fourPlayers)
                {
                    // When playing with two players

                    // When key S is pressed
                    if (Keyboard.GetState().IsKeyDown(Keys.S))
                        players[0].Move(false);
                    // When key W is pressed
                    else if (Keyboard.GetState().IsKeyDown(Keys.W))
                        players[0].Move(true);

                    // When arrow down is pressed
                    if (Keyboard.GetState().IsKeyDown(Keys.Down))
                        players[1].Move(false);
                    // When arrow up is pressed
                    else if (Keyboard.GetState().IsKeyDown(Keys.Up))
                        players[1].Move(true);
                }
                else
                {
                    // When playing with four players

                    // Loop trough all players
                    foreach (Player p in players)
                    {
                        // Key checks for top left player
                        if (p.playField == PlayField.TopLeft || p.playField == PlayField.Left)
                        {
                            // When S key is pressed
                            if (Keyboard.GetState().IsKeyDown(Keys.S))
                                p.Move(false);
                            // When W key is pressed
                            else if (Keyboard.GetState().IsKeyDown(Keys.W))
                                p.Move(true);
                        }

                        // Key checks for bottom left player
                        if (p.playField == PlayField.BottomLeft || p.playField == PlayField.Left)
                        {
                            // When C key is pressed
                            if (Keyboard.GetState().IsKeyDown(Keys.C))
                                p.Move(false);
                            // When F key is pressed
                            else if (Keyboard.GetState().IsKeyDown(Keys.F))
                                p.Move(true);
                        }

                        // Key checks for top right player
                        if (p.playField == PlayField.TopRight || p.playField == PlayField.Right)
                        {
                            // When arrow down key is pressed
                            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                                p.Move(false);
                            // When arrow up key is pressed
                            else if (Keyboard.GetState().IsKeyDown(Keys.Up))
                                p.Move(true);
                        }

                        // Key checks for bottom right player
                        if (p.playField == PlayField.BottomRight || p.playField == PlayField.Right)
                        {
                            // When ? key is pressed
                            if (Keyboard.GetState().IsKeyDown(Keys.OemQuestion))
                                p.Move(false);
                            // When ' key is pressed
                            else if (Keyboard.GetState().IsKeyDown(Keys.OemQuotes))
                                p.Move(true);
                        }
                    }
                }

                // Move ball
                ball.Move((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

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

            // Only execute this code when game is running
            if (gameState == GameState.Playing)
            {
                // Draw all players
                foreach (Player p in players)
                    p.Draw();

                // Draw the ball
                ball.Draw();
            }

            // End the spritebatch
            spriteBatch.End();

            // Execute MonoGame base draw method
            base.Draw(gameTime);
        }

        // Create players
        private void CreatePlayers(bool _fourPlayers)
        {
            // Store the bool in the class variable
            fourPlayers = _fourPlayers;

            // Create player list to store all created players (using list for easy adding without setting a size first)
            List<Player> _players = new List<Player>();

            // Create players one and two
            Player playerOne = new Player(Color.Red, PlayField.Left);
            Player playerTwo = new Player(Color.Yellow, PlayField.Right);

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
                Player playerThree = new Player(Color.LimeGreen, PlayField.BottomLeft);
                Player playerFour = new Player(Color.CornflowerBlue, PlayField.BottomRight);

                // Add all players to the player list
                _players.Add(playerThree);
                _players.Add(playerFour);
            }

            // Convert the player list to an array and store it (arrays are faster and use less memory)
            players = _players.ToArray();
        }

        // Create a new ball in the middle of the screen, with a new random start direction (removes the old ball)
        private void CreateBall()
        {
            ball = new Ball(ballTexture);
        }

        // Find a player to kill with the same playfield the ball is in, when found, kill the player and reset the ball
        public bool FindPlayerToKill(PlayField field)
        {
            foreach (Player p in players)
            {
                if (p.playField == field)
                {
                    p.Die();
                    CreateBall();
                    return true;
                }
            }

            return false;
        }
    }
}
