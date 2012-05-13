namespace TetrisClone
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Graphics;
    using InputStateController;
    using TetrisClone;

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Soft,
        Hard
    };

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Piece
    {
        Block[] block;
        Point[] tempMove;
        Board theBoard;

        public bool Swapped { get; set; }
        public BlockType Type { get; set; }
        Piece swapPiece;
        public bool HasBlock { get; protected set; }
        public bool IsGameOver { get; protected set; }
        public bool UseGhostPiece { get; set; }
        public bool FirstSwap { get; protected set; }
        public ReachSoundManager Sounds { get; protected set; }
        public Scoring ScoreKeeper { get; protected set; }
        public double TimeSinceLastAutoDrop { get; set; }

        public Board.ScoringEvent Drop;

        public static Random randomNumber = new Random();

        public Piece()
        {
            block = new Block[4];
            tempMove = new Point[4];
        }

        public Piece(Board theBoard)
            : this()
        {
            IsGameOver = false;
            UseGhostPiece = true;
            FirstSwap = false;
            this.theBoard = theBoard;
        }

        public Piece(Board theBoard, Piece swapPiece, ReachSoundManager sounds, Scoring scoreKeeper)
            : this(theBoard)
        {
            this.swapPiece = swapPiece;
            Sounds = sounds;
            ScoreKeeper = scoreKeeper;
            Drop = new Board.ScoringEvent(ScoreKeeper.Drop);
            TimeSinceLastAutoDrop = 0;
        }

        /// <summary>
        /// Updates this piece. Does nothing if the player still has control
        /// of the piece.
        /// </summary>
        public void Update()
        {
            if (!HasBlock)
            {
                if (!FirstSwap && Swapped)
                    FirstSwap = true;
                else
                {
                    foreach (Block b in block)
                    {
                        theBoard.Bake(b);
                    }
                    theBoard.IsMoveMade = true;
                }
            }
        }

        /// <summary>
        /// Rotates the current piece, if valid.
        /// </summary>
        public void rotate()
        {
            bool MoveIsValid = false;

            if (HasBlock)
            {
                #region Rotate Blue Block (L Shape)
                if (block[0].Type == BlockType.L)
                {
                    if (block[0].Rotate == 0)
                    {
                        tempMove[0] = new Point(block[0].Position.X, block[0].Position.Y + 2);
                        tempMove[1] = new Point(block[1].Position.X - 1, block[1].Position.Y + 1);
                        tempMove[2] = block[2].Position;
                        tempMove[3] = new Point(block[3].Position.X + 1, block[3].Position.Y - 1);
                    }
                    else if (block[0].Rotate == 1)
                    {
                        tempMove[0] = new Point(block[0].Position.X - 2, block[0].Position.Y);
                        tempMove[1] = new Point(block[1].Position.X - 1, block[1].Position.Y - 1);
                        tempMove[2] = block[2].Position;
                        tempMove[3] = new Point(block[3].Position.X + 1, block[3].Position.Y + 1);
                    }
                    else if (block[0].Rotate == 2)
                    {
                        tempMove[0] = new Point(block[0].Position.X, block[0].Position.Y - 2);
                        tempMove[1] = new Point(block[1].Position.X + 1, block[1].Position.Y - 1);
                        tempMove[2] = block[2].Position;
                        tempMove[3] = new Point(block[3].Position.X - 1, block[3].Position.Y + 1);
                    }
                    else if (block[0].Rotate == 3)
                    {
                        tempMove[0] = new Point(block[0].Position.X + 2, block[0].Position.Y);
                        tempMove[1] = new Point(block[1].Position.X + 1, block[1].Position.Y + 1);
                        tempMove[2] = block[2].Position;
                        tempMove[3] = new Point(block[3].Position.X - 1, block[3].Position.Y - 1);
                    }
                }
                #endregion

                #region Rotate Green Block (J Shape)
                else if (block[0].Type == BlockType.J)
                {
                    if (block[0].Rotate == 0)
                    {
                        tempMove[0] = new Point(block[0].Position.X + 2, block[0].Position.Y);
                        tempMove[1] = new Point(block[1].Position.X + 1, block[1].Position.Y - 1);
                        tempMove[2] = block[2].Position;
                        tempMove[3] = new Point(block[3].Position.X - 1, block[3].Position.Y + 1);
                    }
                    else if (block[0].Rotate == 1)
                    {
                        tempMove[0] = new Point(block[0].Position.X, block[0].Position.Y + 2);
                        tempMove[1] = new Point(block[1].Position.X + 1, block[1].Position.Y + 1);
                        tempMove[2] = block[2].Position;
                        tempMove[3] = new Point(block[3].Position.X - 1, block[3].Position.Y - 1);
                    }
                    else if (block[0].Rotate == 2)
                    {
                        tempMove[0] = new Point(block[0].Position.X - 2, block[0].Position.Y);
                        tempMove[1] = new Point(block[1].Position.X - 1, block[1].Position.Y + 1);
                        tempMove[2] = block[2].Position;
                        tempMove[3] = new Point(block[3].Position.X + 1, block[3].Position.Y - 1);
                    }
                    else if (block[0].Rotate == 3)
                    {
                        tempMove[0] = new Point(block[0].Position.X, block[0].Position.Y - 2);
                        tempMove[1] = new Point(block[1].Position.X - 1, block[1].Position.Y - 1);
                        tempMove[2] = block[2].Position;
                        tempMove[3] = new Point(block[3].Position.X + 1, block[3].Position.Y + 1);
                    }
                }
                #endregion

                #region Rotate Indigo Block (I Shape)
                else if (block[0].Type == BlockType.I)
                {
                    if (block[0].Rotate == 0)
                    {
                        tempMove[0] = new Point(block[0].Position.X + 2, block[0].Position.Y - 1);
                        tempMove[1] = new Point(block[1].Position.X + 1, block[1].Position.Y);
                        tempMove[2] = new Point(block[2].Position.X, block[2].Position.Y + 1);
                        tempMove[3] = new Point(block[3].Position.X - 1, block[3].Position.Y + 2);
                    }
                    else if (block[0].Rotate == 1)
                    {
                        tempMove[0] = new Point(block[0].Position.X + 1, block[0].Position.Y + 2);
                        tempMove[1] = new Point(block[1].Position.X, block[1].Position.Y + 1);
                        tempMove[2] = new Point(block[2].Position.X - 1, block[2].Position.Y);
                        tempMove[3] = new Point(block[3].Position.X - 2, block[3].Position.Y - 1);
                    }
                    else if (block[0].Rotate == 2)
                    {
                        tempMove[0] = new Point(block[0].Position.X - 2, block[0].Position.Y + 1);
                        tempMove[1] = new Point(block[1].Position.X - 1, block[1].Position.Y);
                        tempMove[2] = new Point(block[2].Position.X, block[2].Position.Y - 1);
                        tempMove[3] = new Point(block[3].Position.X + 1, block[3].Position.Y - 2);
                    }
                    else if (block[0].Rotate == 3)
                    {
                        tempMove[0] = new Point(block[0].Position.X - 1, block[0].Position.Y - 2);
                        tempMove[1] = new Point(block[1].Position.X, block[1].Position.Y - 1);
                        tempMove[2] = new Point(block[2].Position.X + 1, block[2].Position.Y);
                        tempMove[3] = new Point(block[3].Position.X + 2, block[3].Position.Y + 1);
                    }
                }
                #endregion

                #region Rotate Orange Block (T Shape)
                else if (block[0].Type == BlockType.T)
                {
                    if (block[0].Rotate == 0)
                    {
                        tempMove[0] = new Point(block[0].Position.X + 1, block[0].Position.Y + 1);
                        tempMove[1] = new Point(block[1].Position.X + 1, block[1].Position.Y - 1);
                        tempMove[2] = block[2].Position;
                        tempMove[3] = new Point(block[3].Position.X - 1, block[3].Position.Y + 1);
                    }
                    else if (block[0].Rotate == 1)
                    {
                        tempMove[0] = new Point(block[0].Position.X - 1, block[0].Position.Y + 1);
                        tempMove[1] = new Point(block[1].Position.X + 1, block[1].Position.Y + 1);
                        tempMove[2] = block[2].Position;
                        tempMove[3] = new Point(block[3].Position.X - 1, block[3].Position.Y - 1);
                    }
                    else if (block[0].Rotate == 2)
                    {
                        tempMove[0] = new Point(block[0].Position.X - 1, block[0].Position.Y - 1);
                        tempMove[1] = new Point(block[1].Position.X - 1, block[1].Position.Y + 1);
                        tempMove[2] = block[2].Position;
                        tempMove[3] = new Point(block[3].Position.X + 1, block[3].Position.Y - 1);
                    }
                    else if (block[0].Rotate == 3)
                    {
                        tempMove[0] = new Point(block[0].Position.X + 1, block[0].Position.Y - 1);
                        tempMove[1] = new Point(block[1].Position.X - 1, block[1].Position.Y - 1);
                        tempMove[2] = block[2].Position;
                        tempMove[3] = new Point(block[3].Position.X + 1, block[3].Position.Y + 1);
                    }
                }
                #endregion

                #region Rotate Violet Block (S Shape)
                else if (block[0].Type == BlockType.S)
                {
                    if (block[0].Rotate == 0)
                    {
                        tempMove[0] = new Point(block[0].Position.X, block[0].Position.Y + 2);
                        tempMove[1] = new Point(block[1].Position.X + 1, block[1].Position.Y + 1);
                        tempMove[2] = block[2].Position;
                        tempMove[3] = new Point(block[3].Position.X + 1, block[3].Position.Y - 1);
                    }
                    else if (block[0].Rotate == 1)
                    {
                        tempMove[0] = new Point(block[0].Position.X - 2, block[0].Position.Y);
                        tempMove[1] = new Point(block[1].Position.X - 1, block[1].Position.Y + 1);
                        tempMove[2] = block[2].Position;
                        tempMove[3] = new Point(block[3].Position.X + 1, block[3].Position.Y + 1);
                    }
                    else if (block[0].Rotate == 2)
                    {
                        tempMove[0] = new Point(block[0].Position.X, block[0].Position.Y - 2);
                        tempMove[1] = new Point(block[1].Position.X - 1, block[1].Position.Y - 1);
                        tempMove[2] = block[2].Position;
                        tempMove[3] = new Point(block[3].Position.X - 1, block[3].Position.Y + 1);
                    }
                    else if (block[0].Rotate == 3)
                    {
                        tempMove[0] = new Point(block[0].Position.X + 2, block[0].Position.Y);
                        tempMove[1] = new Point(block[1].Position.X + 1, block[1].Position.Y - 1);
                        tempMove[2] = block[2].Position;
                        tempMove[3] = new Point(block[3].Position.X - 1, block[3].Position.Y - 1);
                    }
                }
                #endregion

                #region Rotate Red Block (O Shape)
                else if (block[0].Type == BlockType.O)
                {
                    //square block, do nothing
                    tempMove[0] = block[0].Position;
                    tempMove[1] = block[1].Position;
                    tempMove[2] = block[2].Position;
                    tempMove[3] = block[3].Position;
                }
                #endregion

                #region Rotate Yellow Block (Z Shape)
                else if (block[0].Type == BlockType.Z)
                {
                    if (block[0].Rotate == 0)
                    {
                        tempMove[0] = new Point(block[0].Position.X + 2, block[0].Position.Y);
                        tempMove[1] = new Point(block[1].Position.X + 1, block[1].Position.Y + 1);
                        tempMove[2] = block[2].Position;
                        tempMove[3] = new Point(block[3].Position.X - 1, block[3].Position.Y + 1);
                    }
                    else if (block[0].Rotate == 1)
                    {
                        tempMove[0] = new Point(block[0].Position.X, block[0].Position.Y + 2);
                        tempMove[1] = new Point(block[1].Position.X - 1, block[1].Position.Y + 1);
                        tempMove[2] = block[2].Position;
                        tempMove[3] = new Point(block[3].Position.X - 1, block[3].Position.Y - 1);
                    }
                    else if (block[0].Rotate == 2)
                    {
                        tempMove[0] = new Point(block[0].Position.X - 2, block[0].Position.Y);
                        tempMove[1] = new Point(block[1].Position.X - 1, block[1].Position.Y - 1);
                        tempMove[2] = block[2].Position;
                        tempMove[3] = new Point(block[3].Position.X + 1, block[3].Position.Y - 1);
                    }
                    else if (block[0].Rotate == 3)
                    {
                        tempMove[0] = new Point(block[0].Position.X, block[0].Position.Y - 2);
                        tempMove[1] = new Point(block[1].Position.X + 1, block[1].Position.Y - 1);
                        tempMove[2] = block[2].Position;
                        tempMove[3] = new Point(block[3].Position.X + 1, block[3].Position.Y + 1);
                    }
                }
                #endregion

                int count = 0;
                int sign = 1;

                for (int i = 0; i < 4; i++)
                {
                    if (tempMove[i].X >= 6)
                        sign = -1;
                }

                while (!theBoard.IsMoveValid(tempMove) && count < 3)
                {
                    for (int i = 0; i < 4; i++)
                        tempMove[i] = new Point(tempMove[i].X + sign, tempMove[i].Y);
                    count++;
                }

                if (theBoard.IsMoveValid(tempMove))
                    MoveIsValid = true;

                if (MoveIsValid)
                {
                    block[0].Position = tempMove[0];
                    block[1].Position = tempMove[1];
                    block[2].Position = tempMove[2];
                    block[3].Position = tempMove[3];


                    block[0].NextRotate();
                    block[1].NextRotate();
                    block[2].NextRotate();
                    block[3].NextRotate();
                }
            }
        }


        /// <summary>
        /// Moves piece one row to the left, if valid.
        /// </summary>
        public void moveLeft()
        {
            if (HasBlock)
            {
                tempMove[0] = new Point(block[0].Position.X - 1, block[0].Position.Y);
                tempMove[1] = new Point(block[1].Position.X - 1, block[1].Position.Y);
                tempMove[2] = new Point(block[2].Position.X - 1, block[2].Position.Y);
                tempMove[3] = new Point(block[3].Position.X - 1, block[3].Position.Y);

                if (theBoard.IsMoveValid(tempMove))
                {
                    block[0].Position = tempMove[0];
                    block[1].Position = tempMove[1];
                    block[2].Position = tempMove[2];
                    block[3].Position = tempMove[3];
                }
                else
                    Sounds.HitWall();
            }
        }

        /// <summary>
        /// Moves piece one row to the right, if valid.
        /// </summary>
        public void moveRight()
        {
            if (HasBlock)
            {
                tempMove[0] = new Point(block[0].Position.X + 1, block[0].Position.Y);
                tempMove[1] = new Point(block[1].Position.X + 1, block[1].Position.Y);
                tempMove[2] = new Point(block[2].Position.X + 1, block[2].Position.Y);
                tempMove[3] = new Point(block[3].Position.X + 1, block[3].Position.Y);

                if (theBoard.IsMoveValid(tempMove))
                {
                    block[0].Position = tempMove[0];
                    block[1].Position = tempMove[1];
                    block[2].Position = tempMove[2];
                    block[3].Position = tempMove[3];
                }
                else
                    Sounds.HitWall();
            }
        }

        /// <summary>
        /// Moves this piece down one row if valid.
        /// </summary>
        /// <param name="dir">Hard or Soft drop, for scoring purposes.</param>
        public void moveDown(Direction dir)
        {
            tempMove[0] = new Point(block[0].Position.X, block[0].Position.Y + 1);
            tempMove[1] = new Point(block[1].Position.X, block[1].Position.Y + 1);
            tempMove[2] = new Point(block[2].Position.X, block[2].Position.Y + 1);
            tempMove[3] = new Point(block[3].Position.X, block[3].Position.Y + 1);

            if (theBoard.IsMoveValid(tempMove))
            {
                block[0].Position = tempMove[0];
                block[1].Position = tempMove[1];
                block[2].Position = tempMove[2];
                block[3].Position = tempMove[3];
            }
            else HasBlock = false;

            switch (dir)
            {
                case Direction.Hard: Drop(LineClear.HardDrop); TimeSinceLastAutoDrop = 0; break;
                case Direction.Soft: Drop(LineClear.SoftDrop); TimeSinceLastAutoDrop = 0; break;
            }
        }

        /// <summary>
        /// Called when player drops a piece.
        /// Moves piece down until until it reaches the bottom or an obstacle.
        /// </summary>
        public void drop()
        {
            while (HasBlock)
            {
                moveDown(Direction.Hard);
            }
            Sounds.Drop();
        }

        /// <summary>
        /// Creates the appropriate shape corresponding to the color.
        /// </summary>
        /// <param name="color">The color of the block.</param>
        public void CreateShape(BlockType color)
        {
            this.Type = color;

            if (color == BlockType.L) // L Shape
            {
                block[0] = new Block(color, 5, 1);
                block[1] = new Block(color, 5, 2);
                block[2] = new Block(color, 4, 2);
                block[3] = new Block(color, 3, 2);
            }
            else if (color == BlockType.J) // J Shape
            {
                block[0] = new Block(color, 3, 1);
                block[1] = new Block(color, 3, 2);
                block[2] = new Block(color, 4, 2);
                block[3] = new Block(color, 5, 2);
            }
            else if (color == BlockType.I) // I Shape
            {
                block[0] = new Block(color, 3, 1);
                block[1] = new Block(color, 4, 1);
                block[2] = new Block(color, 5, 1);
                block[3] = new Block(color, 6, 1);
            }
            else if (color == BlockType.T) // T Shape
            {
                block[0] = new Block(color, 4, 1);
                block[1] = new Block(color, 3, 2);
                block[2] = new Block(color, 4, 2);
                block[3] = new Block(color, 5, 2);

            }
            else if (color == BlockType.S) // S Shape
            {
                block[0] = new Block(color, 5, 1);
                block[1] = new Block(color, 4, 1);
                block[2] = new Block(color, 4, 2);
                block[3] = new Block(color, 3, 2);
            }
            else if (color == BlockType.O) // O Shape
            {
                block[0] = new Block(color, 4, 1);
                block[1] = new Block(color, 5, 1);
                block[2] = new Block(color, 4, 2);
                block[3] = new Block(color, 5, 2);

            }
            else if (color == BlockType.Z) // Z shape
            {
                block[0] = new Block(color, 3, 1);
                block[1] = new Block(color, 4, 1);
                block[2] = new Block(color, 4, 2);
                block[3] = new Block(color, 5, 2);
            }

            tempMove[0] = block[0].Position;
            tempMove[1] = block[1].Position;
            tempMove[2] = block[2].Position;
            tempMove[3] = block[3].Position;

            if (!theBoard.IsMoveValid(tempMove))
                IsGameOver = true;


            HasBlock = true;
        }


        /// <summary>
        /// Draws the current piece to the game board.
        /// </summary>
        /// <param name="gameTime">The gameTime.</param>
        /// <param name="sb">The spritebatch.</param>
        public void drawShapeToBoard(GameTime gameTime, SpriteBatch sb)
        {
            foreach (Block b in block)
                b.Draw(sb, theBoard.GetTexture(Type), Color.White, theBoard.Offset, theBoard.Scale);
            if (UseGhostPiece)
                DrawGhostPiece(gameTime, sb);
        }

        /// <summary>
        /// Draws the preview of the next piece.
        /// </summary>
        /// <param name="gameTime">The gameTime.</param>
        /// <param name="sb">The spriteBatch.</param>
        /// <param name="drawLoc">The screen location to draw the next piece at.</param>
        public void drawNextPiece(GameTime gameTime, SpriteBatch sb, Vector2 drawLoc)
        {
            if (Type == BlockType.L) // L Shape
            {
                block[0] = new Block(Type, 1, 2);
                block[1] = new Block(Type, 0, 0);
                block[2] = new Block(Type, 0, 1);
                block[3] = new Block(Type, 0, 2);
            }
            else if (Type == BlockType.J) // J Shape
            {
                block[0] = new Block(Type, 0, 2);
                block[1] = new Block(Type, 1, 0);
                block[2] = new Block(Type, 1, 1);
                block[3] = new Block(Type, 1, 2);
            }
            else if (Type == BlockType.I) // I Shape
            {
                block[0] = new Block(Type, 1, 0);
                block[1] = new Block(Type, 1, 1);
                block[2] = new Block(Type, 1, 2);
                block[3] = new Block(Type, 1, 3);
            }
            else if (Type == BlockType.T) // T Shape
            {
                block[0] = new Block(Type, 0, 0);
                block[1] = new Block(Type, 0, 1);
                block[2] = new Block(Type, 0, 2);
                block[3] = new Block(Type, 1, 1);
            }
            else if (Type == BlockType.S) // S Shape
            {
                block[0] = new Block(Type, 0, 0);
                block[1] = new Block(Type, 0, 1);
                block[2] = new Block(Type, 1, 1);
                block[3] = new Block(Type, 1, 2);
            }
            else if (Type == BlockType.O) // O Shape
            {
                block[0] = new Block(Type, 0, 0);
                block[1] = new Block(Type, 1, 0);
                block[2] = new Block(Type, 0, 1);
                block[3] = new Block(Type, 1, 1);

            }
            else if (Type == BlockType.Z) // Z Shape
            {
                block[0] = new Block(Type, 1, 0);
                block[1] = new Block(Type, 1, 1);
                block[2] = new Block(Type, 0, 1);
                block[3] = new Block(Type, 0, 2);
            }

            foreach (Block b in block)
                b.Draw(sb, theBoard.GetTexture(Type), Color.White, drawLoc, theBoard.Scale);
        }

        /// <summary>
        /// Draws the Ghost Piece.
        /// </summary>
        /// <param name="gameTime">The gameTime.</param>
        /// <param name="sb">The spritebatch.</param>
        public void DrawGhostPiece(GameTime gameTime, SpriteBatch sb)
        {
            Block[] ghostBlock = new Block[4];

            for (int i = 0; i < 4; i++)
                ghostBlock[i] = block[i];

            bool done = false;
            while (!done)
            {

                tempMove[0] = new Point(ghostBlock[0].Position.X, ghostBlock[0].Position.Y + 1);
                tempMove[1] = new Point(ghostBlock[1].Position.X, ghostBlock[1].Position.Y + 1);
                tempMove[2] = new Point(ghostBlock[2].Position.X, ghostBlock[2].Position.Y + 1);
                tempMove[3] = new Point(ghostBlock[3].Position.X, ghostBlock[3].Position.Y + 1);

                if (theBoard.IsMoveValid(tempMove))
                {
                    ghostBlock[0].Position = tempMove[0];
                    ghostBlock[1].Position = tempMove[1];
                    ghostBlock[2].Position = tempMove[2];
                    ghostBlock[3].Position = tempMove[3];
                }
                else done = true;
            }

            foreach (Block b in ghostBlock)
                b.Draw(sb, theBoard.GetTexture(BlockType.None), Color.White, theBoard.Offset, theBoard.Scale);
        }

        /// <summary>
        /// Swapped the current piece with the hold piece
        /// if a piece hasn't already been swapped since last play.
        /// </summary>
        public void swap()
        {
            if (!FirstSwap)
            {
                swapPiece.Type = this.Type;
                Swapped = true;
                HasBlock = false;
            }
            else
            {
                BlockType tempColor = this.Type;
                this.Type = swapPiece.Type;
                swapPiece.Type = tempColor;
                Swapped = true;
                CreateShape(this.Type);
            }

        }
    }
}
