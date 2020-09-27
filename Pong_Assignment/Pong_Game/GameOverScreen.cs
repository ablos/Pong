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
    public class GameOverScreen
    {
        // Constructor
        public GameOverScreen()
        {
            // Make the mouse pointer visible
            PongGame.pongGame.IsMouseVisible = true;
        }

        // Deconstructor
        ~GameOverScreen()
        {
            // Make the mouse pointer invisible
            if (PongGame.pongGame != null)
                PongGame.pongGame.IsMouseVisible = false;
        }

        // Update Method
        public void Update()
        {

        }

        // Draw method
        public void Draw()
        {

        }
    }
}
