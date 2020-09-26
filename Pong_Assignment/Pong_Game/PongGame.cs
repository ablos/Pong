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
        SpriteBatch spriteBatch;                            // Variable to store the MonoGame Sprite Batch

        public bool fourPlayers;                            // Variable to determine if game should be played with four or two players
        public Player[] players;                            // Array to store all players created
        public Ball ball;                                   // Variable to store ball in

        public Texture2D lifeTexture;                       // Variable to store life texture in
        private Texture2D ballTexture;                      // Variable to store ball texture in

        public GameState gameState = GameState.Playing;     // Variable to store gamestate in

        public static PongGame pongGame;                    // Variable to store instance of this class

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

        // Create a new ball in the middle of the screen, with a new random start direction (removes the old ball)
        private void CreateBall()
        {
            ball = new Ball(ballTexture, GraphicsDevice, spriteBatch);
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

    // Ball class
    public class Ball
    {
        private const float speed = 400;                        // Variable to store speed of the ball
        public Vector2 location;                                // Variable to store location of the ball
        private readonly Point size = new Point(18, 18);        // Variable to store size of the ball
        public Color color = Color.White;                       // Variable to store color of the ball
        private GraphicsDevice gDevice;                         // Variable to store graphics device from monogame (used for boundaries)
        private SpriteBatch spriteBatch;                        // Variable to store sprite batch from monogame (used for drawing)
        private Texture2D texture;                              // Variable to store the texture of the ball
        private Vector2 direction;                              // Variable to store the direction of the ball
        private Random random = new Random();                   // Variable used to create random numbers
        private float minimumAngle = 0.4f;                      // Variable to store minimum angle of the start direction of the ball
        private bool allowBounceRight = false;                  // Variable to store if ball is allowed to bounce against players on the right
        private bool allowBounceLeft = false;                   // Variable to store if ball is allowed to bounce against players on the left

        // Constructor of the ball class
        public Ball(Texture2D texture, GraphicsDevice gDevice, SpriteBatch spriteBatch)
        {
            this.texture = texture;             // Copy the given texture value to the local texture variable
            this.gDevice = gDevice;             // Save the GraphicsDevice in a local variable
            this.spriteBatch = spriteBatch;     // Save the Sprite Batch in a local variable

            // Position the ball in the middle of the screen
            location = new Vector2((gDevice.Viewport.Bounds.Width / 2) - (size.X / 2), (gDevice.Viewport.Bounds.Height / 2) - (size.Y / 2));

            // Get a random angle and make sure it is above the minimum angle
            float r = (float)random.NextDouble();
            if (r < minimumAngle)
                r = minimumAngle;

            // Set the direction according to the angle
            direction = new Vector2((random.Next(1, 3) == 1 ? 1 : -1), (random.Next(1, 3) == 1 ? 1 : -1) * r);

            // Set restrictions for bouncing
            if (direction.X < 0)
                allowBounceLeft = true;
            else
                allowBounceRight = true;
        }

        // Draw the ball
        public void Draw()
        {
            spriteBatch.Draw(texture, new Rectangle(location.ToPoint(), size), color);
        }

        // Move the ball
        public void Move(float elapsedTime)
        {
            // Move the ball according to the direction, speed and time
            location += direction * speed * elapsedTime;

            // Make sure the ball doesn't go out of bounds, when it hits the left or right side of the screen, take a live.
            if (location.X > (gDevice.Viewport.Width - size.X) || location.X < 0)
            {
                // Is the ball on the left side of the screen?
                if (location.X < gDevice.Viewport.Width / 2)
                {
                    // If there is a player that plays on the left side, kill it
                    if (!PongGame.pongGame.FindPlayerToKill(PlayField.Left))
                    {
                        // If there is a player on the bottom left, and the ball is there as well, kill it
                        if (location.Y >= gDevice.Viewport.Height / 2)
                            PongGame.pongGame.FindPlayerToKill(PlayField.BottomLeft);
                        // If there is a player on the top left, and the ball is there as well, kill it
                        else
                            PongGame.pongGame.FindPlayerToKill(PlayField.TopLeft);
                    }
                }
                // The ball is on the right side of the screen
                else
                {
                    // If there is a player that plays the right side, kill it
                    if (!PongGame.pongGame.FindPlayerToKill(PlayField.Right))
                    {
                        // If there is a player on the bottom right, and the ball is there as well, kill it
                        if (location.Y >= gDevice.Viewport.Height / 2)
                            PongGame.pongGame.FindPlayerToKill(PlayField.BottomRight);
                        // If there is a player on the top right, and the ball is there as well, kill it
                        else
                            PongGame.pongGame.FindPlayerToKill(PlayField.TopRight);
                    }
                }
            }

            // Make sure the ball doesn't go out of bounds, when it hits the top or the bottom, invert the Y direction
            if (location.Y > (gDevice.Viewport.Height - size.Y) || location.Y < 0)
                direction.Y *= -1;

            // Create a rectangle around the ball to detect collision
            Rectangle ballRect = new Rectangle(location.ToPoint(), size);
            foreach (Player p in PongGame.pongGame.players)
            {
                // Create a rectangle around the player to detect collision
                Rectangle playerRect = new Rectangle(p.location, p.size);

                // If both rectangles intersect, decide what to do with the ball
                if (playerRect.Intersects(ballRect))
                {
                    // If the ball is on the left side, but it isn't allowed to bounce against the player (to prevent the ball getting stuck in the player), stop here
                    if (location.X < gDevice.Viewport.Bounds.Width / 2 && !allowBounceLeft)
                        return;

                    // If the ball is on the right side, but it isn't allowed to bounce against the player (to prevent the ball getting stuck in the player), stop here
                    if (location.X > gDevice.Viewport.Bounds.Width / 2 && !allowBounceRight)
                        return;

                    // Invert the restrictions
                    allowBounceRight = !allowBounceRight;
                    allowBounceLeft = !allowBounceLeft;

                    // Invert the X direction
                    direction.X *= -1;

                    // If the ball hits the player on the top 1/5th of the player, bounce up
                    if (((int)location.Y + size.Y / 2) <= (p.location.Y + p.size.Y / 5))
                    {
                        direction.Y = -1;
                    }
                    // If the ball hits the player on the second to top 1/5th of the player, bounce slightly up
                    else if (((int)location.Y + size.Y / 2) > (p.location.Y + p.size.Y / 5)
                        && ((int)location.Y + size.Y / 2) <= p.location.Y + 2 * p.size.Y / 5)
                    {
                        direction.Y = -0.5f;
                    }
                    // If the ball hits the player in the middle, bounce straight
                    else if (((int)location.Y + size.Y / 2) > p.location.Y + 2 * p.size.Y / 5
                        && ((int)location.Y + size.Y / 2) <= p.location.Y + 3 * p.size.Y / 5)
                    {
                        direction.Y = 0;
                    }
                    // If the ball hits the player on the second to bottom 1/5th of the player, bounce slightly down
                    else if (((int)location.Y + size.Y / 2) > p.location.Y + 3 * p.size.Y / 5
                        && ((int)location.Y + size.Y / 2) <= p.location.Y + 4 * p.size.Y / 5)
                    {
                        direction.Y = 0.5f;
                    }
                    // If the ball hits the player on the bottom 1/5th of the player, bounce down
                    else if (((int)location.Y + size.Y / 2) > p.location.Y + 4 * p.size.Y / 5)
                    {
                        direction.Y = 1;
                    }
                }
            }
        }
    }

    // Player class
    public class Player
    {
        public int lives = 3;                                           // Variable to store lives for player
        private int livesTextureOffset = 5;                             // Offset for the space between the textures of the lives
        private readonly Point lifeTextureSize = new Point(20, 20);     // Size of the life textures
        private const int speed = 10;                                   // Set speed of player (is for every player)
        public readonly Point size = new Point(20, 100);                // Set size of player (is for every player, readonly to prevent accedental edits)
        public Point location;                                          // Variable to store player location
        private Texture2D texture;                                      // Variable to store the texture of the player
        public Color color;                                             // Variable to store the color of the player
        private GraphicsDevice gDevice;                                 // Variable to store the GrapicsDevice from game
        private SpriteBatch spriteBatch;                                // Variable to store spriteBatch
        public PlayField playField;                                     // Variable to store playfield for player -> where is he allowed to play

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

        // Draw player and lives on the screen
        public void Draw()
        {
            // Draw the player
            spriteBatch.Draw(texture, new Rectangle(location, size), color);

            // Draw the lives
            // Variable to store the position of the lives
            Point pos = new Point(0, livesTextureOffset);

            // Determine the X position of the lives for players on the right side of the screen
            if (playField == PlayField.Right || playField == PlayField.TopRight || playField == PlayField.BottomRight)
                pos.X = gDevice.Viewport.Bounds.Width - (lives * (livesTextureOffset + lifeTextureSize.X)) - livesTextureOffset;

            // Determine the Y position of the lives for the players on the bottom side of the screen
            if (playField == PlayField.BottomRight || playField == PlayField.BottomLeft)
                pos.Y = gDevice.Viewport.Bounds.Height - lifeTextureSize.Y - livesTextureOffset;

            // Draw all the lives
            for (int i = 0; i < lives; i++)
            {
                pos.X += livesTextureOffset;
                spriteBatch.Draw(PongGame.pongGame.lifeTexture, new Rectangle(pos, lifeTextureSize), Color.White);
                pos.X += lifeTextureSize.X;
            }
        }

        // Action when player fails to hit ball
        public void Die()
        {
            // Remove life
            lives--;

            // When player doesn't have any lives left, show game over screen, or in case of four players, let the remaining player play the whole half of the screen
            if (lives <= 0)
            {
                if (PongGame.pongGame.fourPlayers)
                {
                    // Remove one player from the field, unless all players from one half are dead
                    bool hasTeammate = false;
                    foreach (Player p in PongGame.pongGame.players)
                    {
                        // Try to find a teammate, if one is found, edit the playfield for this player
                        switch (playField)
                        {
                            case PlayField.TopLeft:
                                if (p.playField == PlayField.BottomLeft && p.lives > 0)
                                {
                                    p.playField = PlayField.Left;
                                    hasTeammate = true;
                                }
                                break;
                            case PlayField.BottomLeft:
                                if (p.playField == PlayField.TopLeft && p.lives > 0)
                                {
                                    p.playField = PlayField.Left;
                                    hasTeammate = true;
                                }
                                break;
                            case PlayField.TopRight:
                                if (p.playField == PlayField.BottomRight && p.lives > 0)
                                {
                                    p.playField = PlayField.Right;
                                    hasTeammate = true;
                                }
                                break;
                            case PlayField.BottomRight:
                                if (p.playField == PlayField.TopRight && p.lives > 0)
                                {
                                    p.playField = PlayField.Right;
                                    hasTeammate = true;
                                }
                                break;
                        }

                        // If a teammate was found, break out of the loop
                        if (hasTeammate)
                            break;
                    }

                    // If there is no teammate: game over
                    if (!hasTeammate)
                    {
                        PongGame.pongGame.gameState = GameState.GameOver;
                        return;
                    }

                    // If there is a teammate, remove this player from game
                    List<Player> players = PongGame.pongGame.players.ToList();
                    players.Remove(this);
                    PongGame.pongGame.players = players.ToArray();
                }else
                {
                    // Game over
                    PongGame.pongGame.gameState = GameState.GameOver;
                }
            }
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
