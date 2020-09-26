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
    public class MenuScreen
    {

        private Texture2D quitButton;                       
        private Texture2D twoPlayerButton;                  
        private Texture2D fourPlayerButton;

        private Vector2 quitPosition;
        private Vector2 twoPlayerPosition;
        private Vector2 fourPlayerPosition;

        MouseState mouseState;
        MouseState previousMouseState;

        public MenuScreen()
        {
            //make the mouse pointer visible
            PongGame.pongGame.IsMouseVisible = true;    
        }
        public void Update()
        {

        }

        public void Draw()
        {

        }
    }
}
