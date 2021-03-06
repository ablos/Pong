﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

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

        public bool fourPlayers = false;                    // Variable to determine if game should be played with four or two players
        public Player[] players;                            // Array to store all players created
        public Ball ball;                                   // Variable to store ball in

        public bool sfxOn = true;                           // Variable to store if sound effects should be played or not

        public Texture2D lifeTexture;                       // Variable to store life texture in
        public Texture2D ballTexture;                       // Variable to store ball texture in
        public Texture2D quitButtonTexture;                 // Variable to store quit button texture in
        public Texture2D twoPlayersButtonTexture;           // Variable to store two players button texture in
        public Texture2D fourPlayersButtonTexture;          // Variable to store four players button texture in
        public Texture2D menuButtonTexture;                 // Variable to store menu button texture in
        public Texture2D newGameButtonTexture;              // Variable to store new game button texture in
        public Texture2D menuPongTexture;                   // Variable to store menu title texture in
        public Texture2D gameOverTexture;                   // Variable to store game over title texture in
        public Texture2D sfxOnButtonTexture;                // Variable to store sfx on button texture in
        public Texture2D sfxOffButtonTexture;               // Variable to store sfx off button texture in

        public SoundEffect paddleHitSound;                  // Variable to store paddle hit sound in
        public SoundEffect wallHitSound;                    // Variable to store wall hit sound in
        public SoundEffect dieSound;                        // Variable to store game over sound in

        public SpriteFont pixelFont;                        // Variable to store custom over font in

        public GameState gameState = GameState.InMenu;      // Variable to store gamestate in

        public static PongGame pongGame;                    // Variable to store instance of this class

        public MenuScreen menuScreen;                       // Variable to store instance off MenuScreen
        public GameHandler gameHandler;                     // Variable to store instance of PlayerHandler
        public GameOverScreen gameOverScreen;               // Variable to store instance of GameOverScreen

        public Random random = new Random();                // Create a random variable

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
            // Set window name
            Window.Title = "Pong";

            // Create a static instance of this class for accessibility from other classes
            pongGame = this;

            // Execute MonoGame base Initialization method
            base.Initialize();

            // Create menu screen object
            menuScreen = new MenuScreen();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load textures
            lifeTexture = Content.Load<Texture2D>("life");
            ballTexture = Content.Load<Texture2D>("ball");
            fourPlayersButtonTexture = Content.Load<Texture2D>("four-player-button");
            menuButtonTexture = Content.Load<Texture2D>("menu-button");
            menuPongTexture = Content.Load<Texture2D>("menu-pong");
            newGameButtonTexture = Content.Load<Texture2D>("new-game-button");
            quitButtonTexture = Content.Load<Texture2D>("quit-button");
            twoPlayersButtonTexture = Content.Load<Texture2D>("two-players-button");
            gameOverTexture = Content.Load<Texture2D>("game-over-button");
            sfxOnButtonTexture = Content.Load<Texture2D>("sound-on");
            sfxOffButtonTexture = Content.Load<Texture2D>("sound-off");

            // Load sound effects
            paddleHitSound = Content.Load<SoundEffect>("sfx-paddle-hit");
            wallHitSound = Content.Load<SoundEffect>("sfx-wall-hit");
            dieSound = Content.Load<SoundEffect>("sfx-die");

            // Load font
            pixelFont = Content.Load<SpriteFont>("pixel-font");
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Only execute this code when in menu
            if (gameState == GameState.InMenu)
                menuScreen.Update();
            else if (menuScreen != null)
                menuScreen = null;      // Destroy menu screen instance

            // Only execute this code when game is running
            if (gameState == GameState.Playing)
                gameHandler.Update(gameTime);
            // Only execute when gamestate is not playing or game over
            else if (gameState != GameState.GameOver && gameHandler != null)
                gameHandler = null;     // Delete instance of game handler

            // Only execute this code when gamestate is game over
            if (gameState == GameState.GameOver)
                gameOverScreen.Update();
            else
                gameOverScreen = null;  // Delete instance of game over screen

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

            // Decide what to draw depending on the game state
            switch (gameState)
            {
                case GameState.InMenu:
                    if (menuScreen != null) menuScreen.Draw();
                    break;
                case GameState.Playing:
                    if (gameHandler != null) gameHandler.Draw();
                    break;
                case GameState.GameOver:
                    if (gameOverScreen != null) gameOverScreen.Draw();
                    break;
            }

            // End the spritebatch
            spriteBatch.End();

            // Execute MonoGame base draw method
            base.Draw(gameTime);
        }
    }
}
