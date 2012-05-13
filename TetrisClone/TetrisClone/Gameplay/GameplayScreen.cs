// -----------------------------------------------------------------------
// <copyright file="GameplayScreen.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace TetrisClone
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using InputStateController;
    using TetrisClone;

    /// <summary>
    /// GameplayScreen handles the rendering and updating of actual Tetris playing.
    /// </summary>
    public class GameplayScreen : GameScreen
    {
        Board board;
        float scale;
        Texture2D background;
        Piece currentPiece, nextPiece, swapPiece;
        Vector2 backgroundPosition;
        public static ReachSoundManager sounds;
        public double TimeSinceLastAutoDrop { get; set; }
        public static Scoring scoreKeeper;
        RandomGenerator theGenerator;
        
        int x, y;

        ContentManager content;
        SpriteFont gameFont;

        List<GameplayPiece> pieceList=new List<GameplayPiece>();

        float pauseAlpha;

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            #if XBOX
            scale = 0.5f;
            x = 245; y = 150;
            #elif WINDOWS
            scale = 1.0f; 
            x = 135; y = 150;
            #endif

            sounds = new ReachSoundManager(0.50f, 0.50f);
            scoreKeeper = new Scoring(x, 0, sounds);
            board = new Board(10, 22, scale, new Vector2(x, y), scoreKeeper);
            nextPiece = new Piece(board);
            swapPiece = new Piece(board);
            currentPiece = new Piece(board, swapPiece, sounds, scoreKeeper);
            theGenerator = new RandomGenerator();

            pieceList.Add(sounds);
            pieceList.Add(scoreKeeper);
            pieceList.Add(board);
            pieceList.Add(theGenerator);
                        

            Initialize();
        }

        public void Initialize()
        {
            backgroundPosition = (new Vector2(x, y));
                        

            TimeSinceLastAutoDrop = 0;
            
            foreach (GameplayPiece g in pieceList)
                g.Initialize();
        }
        
        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            #if WINDOWS
            gameFont = content.Load<SpriteFont>("fonts\\WinFont");
            #elif XBOX
            gameFont = content.Load<SpriteFont>("fonts\\XboxFont");
            #endif
 
            background = content.Load<Texture2D>("images\\background");
            currentPiece.CreateShape(theGenerator.NextPiece());

            ScreenManager.Game.ResetElapsedTime();

            foreach (GameplayPiece g in pieceList)
                g.LoadContent(content);

            sounds.StartMusic();
        }

        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            
            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
            
            // TODO: Add your update logic here
            if (IsActive)
            {
                nextPiece.Type = theGenerator.Peek();
                currentPiece.Update();

                TimeSinceLastAutoDrop += gameTime.ElapsedGameTime.Milliseconds;

                if (!currentPiece.HasBlock && !currentPiece.IsGameOver)
                {
                    currentPiece.CreateShape(theGenerator.NextPiece());
                    currentPiece.Swapped = false;
                }

                if (TimeSinceLastAutoDrop > scoreKeeper.LockDelay)
                {
                    currentPiece.moveDown(Direction.Down);
                    TimeSinceLastAutoDrop = 0;
                }

                if (currentPiece.IsGameOver)
                    GameOver();
            }
            
            currentPiece.TimeSinceLastAutoDrop = this.TimeSinceLastAutoDrop;
            
            foreach (GameplayPiece g in pieceList)
                g.Update(gameTime);
        }

        public override void HandleInput(InputStateController.InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardState;
            GamePadState gamePadState = input.CurrentGamePadState;

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected;

            if (input.IsPauseGame() || gamePadDisconnected)
            {
                sounds.PauseMusic();
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                sounds.ResumeMusic();
                // handle input...move from Piece.cs
                if (input.IsKeyPressed(Keys.Right) && input.IsKeyPressed(Keys.Left))
                {
                    //do nothing
                }
                else if (input.IsKeyTriggered(Keys.Right) || input.IsLeftStickRightTriggered())
                {
                   currentPiece.moveRight();
                }
                else if (input.IsKeyTriggered(Keys.Left) || input.IsLeftStickLeftTriggered())
                {
                    currentPiece.moveLeft();
                }
                else if (input.IsKeyTriggered(Keys.Down) || input.IsLeftStickDownTriggered())
                {
                    currentPiece.moveDown(Direction.Soft);
                }
                else if (input.IsKeyTriggered(Keys.Up) || input.IsLeftStickUpTriggered())
                {
                    currentPiece.rotate();
                }
                else if (input.IsKeyTriggered(Keys.Space) || input.IsButtonTriggered(Buttons.A))
                {
                    currentPiece.drop();
                }
                else if (input.IsKeyTriggered(Keys.C) || input.IsButtonTriggered(Buttons.X))
                {
                    if (!currentPiece.Swapped)
                        currentPiece.swap();
                }

            }
        }

        public void GameOver()
        {
            sounds.StopMusic();
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            spriteBatch.Draw(background, backgroundPosition, null, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
            
            currentPiece.drawShapeToBoard(gameTime, spriteBatch);

            nextPiece.drawNextPiece(gameTime, spriteBatch, new Vector2(470, 0));

            if (currentPiece.FirstSwap)
                swapPiece.drawNextPiece(gameTime, spriteBatch, Vector2.Zero);

            spriteBatch.End();

            foreach (GameplayPiece g in pieceList)
                g.Draw(gameTime, spriteBatch);

        }

    }
}
