using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace TetrisClone
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Board : GameplayPiece
    {
        public Block[,] gameBoard;

        public int Height { get; protected set; }
        public int Width { get; protected set; }
        public float Scale { get; protected set; }
        public Vector2 Offset { get; protected set; }
        public Scoring ScoreKeeper { get; protected set; }
        int linesCleared;
        Texture2D blueBlock, redBlock, violetBlock, greenBlock,
            yellowBlock, indigoBlock, orangeBlock, ghostBlock;
        public bool IsMoveMade { get; set; }
        Vector2 boardLoc;

        public delegate void ScoringEvent(LineClear move);

        public ScoringEvent Move;

        public Board(int x, int y, float scale, Vector2 boardLoc, Scoring scoreKeeper)
        {
            Width = x;
            Height = y;
            Scale = scale;
            ScoreKeeper = scoreKeeper;
            this.boardLoc = boardLoc;

            IsMoveMade = false;

            Offset = new Vector2(boardLoc.X - 30.0f * scale, boardLoc.Y - 90.0f * scale);//60.0f * scale);

            Move = new ScoringEvent(ScoreKeeper.LastMove);

            gameBoard = new Block[Width, Height];
        }
        #region Initialization
        /// <summary>
        /// Loads Content for Board.
        /// </summary>
        public override void LoadContent(ContentManager Content)
        {
            redBlock = Content.Load<Texture2D>("images\\redBlock");
            orangeBlock = Content.Load<Texture2D>("images\\orangeBlock");
            yellowBlock = Content.Load<Texture2D>("images\\yellowBlock");
            greenBlock = Content.Load<Texture2D>("images\\greenBlock");
            blueBlock = Content.Load<Texture2D>("images\\blueBlock");
            indigoBlock = Content.Load<Texture2D>("images\\indigoBlock");
            violetBlock = Content.Load<Texture2D>("images\\violetBlock");
            ghostBlock = Content.Load<Texture2D>("images\\ghostBlock");
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    gameBoard[i, j] = new Block(BlockType.None, i, j);
                }
            }
        }
        #endregion

        #region Draw and Update
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

            linesCleared = 0;

            for (int i = 0; i < Height; i++)
                if (RowCount(i) == Width)
                {
                    ClearRow(i);
                    Optimize(i);
                    linesCleared++;
                }

            if (IsMoveMade)
            {
                switch (linesCleared)
                {
                    case 0: Move(LineClear.None); break;
                    case 1: Move(LineClear.Single); break;
                    case 2: Move(LineClear.Double); break;
                    case 3: Move(LineClear.Triple); break;
                    case 4: Move(LineClear.Tetris); break;
                }
                IsMoveMade = false;
            }
        }

        /// <summary>
        /// Draws the game board.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="sb">The spritebatch</param>
        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (gameBoard[i, j].Type != BlockType.None)
                        gameBoard[i, j].Draw(sb, (GetTexture(gameBoard[i, j].Type)), Color.White, Offset, Scale);
                }
            }

            sb.End();

        }
        #endregion

        #region Board Utilities
        /// <summary>
        /// Returns the appropriate texture for a given Block.
        /// </summary>
        /// <param name="blockColor">The Block color.</param>
        /// <returns>The texture.</returns>
        public Texture2D GetTexture(BlockType blockColor)
        {
            switch (blockColor)
            {
                case BlockType.Z: return redBlock;
                case BlockType.L: return orangeBlock;
                case BlockType.O: return yellowBlock;
                case BlockType.S: return greenBlock;
                case BlockType.I: return blueBlock;
                case BlockType.J: return indigoBlock;
                case BlockType.T: return violetBlock;
                case BlockType.None: return ghostBlock;
            }
            return null;
        }
                
        /// <summary>
        /// Clears a row of blocks from the game board.
        /// </summary>
        /// <param name="rowNum">The number of the row to clear.</param>
        protected void ClearRow(int rowNum)
        {
            for (int i = 0; i < Width; i++)
                gameBoard[i, rowNum].Type = BlockType.None;
        }

        /// <summary>
        /// Shifts the entire board down.
        /// </summary>
        /// <param name="rowNum">Row number to shift from.</param>
        protected void Optimize(int rowNum)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = rowNum; j > 0; j--)
                    gameBoard[i, j].Type = gameBoard[i, j - 1].Type;
                gameBoard[i, 0].Type = BlockType.None;
            }
        }

        /// <summary>
        /// Counts the number of blocks in a row.
        /// </summary>
        /// <param name="rowNum">The number of the row to count.</param>
        /// <returns>The count of rowNum.</returns>
        protected int RowCount(int rowNum)
        {
            int count = 0;
            for (int i = 0; i < Width; i++)
                if (gameBoard[i, rowNum].Type != BlockType.None)
                    count++;
            return count;
        }

        /// <summary>
        /// Check if an intended move is a valid one by checking if 
        /// the spaces on the game board are occupied.
        /// </summary>
        /// <param name="move">An array of points representing the intended move.</param>
        /// <returns>True if move is valid.</returns>
        public bool IsMoveValid(Point[] move)
        {
            foreach (Point m in move)
            {
                if (m.X >= Width || m.X < 0 || m.Y < 0 || m.Y >= Height)
                    return false;
                if (gameBoard[m.X, m.Y].Type != BlockType.None)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Permanently attaches to the game board.
        /// Called when current piece is placed.
        /// </summary>
        /// <param name="b"></param>
        public void Bake(Block b)
        {
            gameBoard[b.Position.X, b.Position.Y] = b;
        }
        #endregion
    }
}
