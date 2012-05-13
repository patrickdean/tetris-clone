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
using TetrisClone;


namespace TetrisClone
{
    public enum LineClear
    {
        None,
        SoftDrop,
        HardDrop,
        Single,
        Double,
        TSpin,
        Triple,
        Tetris,
        TSpinSingle,
        TSpinDouble,
        TSpinTriple
    };


    /// <summary>
    /// This is the game component handles the game's scoring.
    /// </summary>
    public class Scoring : GameplayPiece
    {
        private enum PointValues
        {
            Single = 100,
            Double = 300,
            TSpin = 400,
            Triple = 500,
            Tetris = 800,
            TSpinSingle = 800,
            TSpinDouble = 1200,
            TSpinTriple = 1600
        };

        SpriteFont sf;

        LineClear LastPlay = LineClear.None;
        LineClear LastScore = LineClear.None;

        public int Level { get; protected set; }
        public int LinesToClear { get; protected set; }
        public int TotalPoints { get; protected set; }
        public int TotalLinesCleared { get; protected set; }
        public int LockDelay { get; protected set; }
        public ReachSoundManager Sounds { get; protected set; }

        private const int DEFAULT_LOCK_DELAY = 1100;

        int linesThisLevel = 0;

        string score, lines, level;
        Vector2 position;
        private int combo = 0;

        public Scoring(int x, int y, ReachSoundManager sounds)
        {
            // TODO: Construct any child components here
            position = new Vector2(x, y);
            Sounds = sounds;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            TotalPoints = 0;
            TotalLinesCleared = 0;
            LinesToClear = 5;
            Level = 1;
            LockDelay = DEFAULT_LOCK_DELAY - 100;
            score = "Score: " + TotalPoints;
            lines = "Lines To Go: " + (Level * 5 - linesThisLevel);
            level = "Level: " + Level;
        }

        /// <summary>
        /// Loads the sprite batch for this component and the font
        /// for the score display.
        /// </summary>
        public override void LoadContent(ContentManager Content)
        {
            #if WINDOWS
            sf = Content.Load<SpriteFont>("fonts\\WinFont");
            #elif XBOX
            sf = Content.Load<SpriteFont>("fonts\\XboxFont");
            #endif
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if (linesThisLevel > Level * 5)
                LevelUp();

            score = "Score: " + TotalPoints;
            lines = "Lines To Go: " + (Level * 5 - linesThisLevel);
            level = "Level: " + Level;
        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();

            sb.DrawString(sf, score, position, Color.White);
            sb.DrawString(sf, lines, new Vector2(position.X, position.Y + 30.0f), Color.White);
            sb.DrawString(sf, level, new Vector2(position.X, position.Y + 60.0f), Color.White);

            sb.End();
        }

        public void reset()
        {
            Level = 1;
            linesThisLevel = 0;
            TotalPoints = 0;
            TotalLinesCleared = 0;
            LastPlay = LineClear.None;
            LastScore = LineClear.None;
        }

        private void Single()
        {
            TotalPoints += (int)PointValues.Single * Level + Combo();
            TotalLinesCleared++;
            linesThisLevel++;

            LastPlay = LineClear.Single;
            LastScore = LineClear.Single;
            Sounds.Score(combo);
        }

        private void Double()
        {
            TotalPoints += (int)PointValues.Double * Level + Combo();
            TotalLinesCleared += 2;
            linesThisLevel += 3;

            LastPlay = LineClear.Double;
            LastScore = LineClear.Double;
            Sounds.Score(combo);
        }

        private void Triple()
        {
            TotalPoints += (int)PointValues.Triple * Level + Combo();
            TotalLinesCleared += 3;
            linesThisLevel += 5;

            LastPlay = LineClear.Triple;
            LastScore = LineClear.Triple;
            Sounds.Score(combo);
        }

        private void Tetris()
        {
            if (LastScore == LineClear.Tetris)
            {
                TotalPoints += (int)PointValues.Tetris * 3 * Level / 2 + Combo();
                linesThisLevel += 12;
            }
            else
            {
                TotalPoints += (int)PointValues.Tetris * Level + Combo();
                linesThisLevel += 8;
            }
            TotalLinesCleared += 4;

            LastPlay = LineClear.Tetris;
            LastScore = LineClear.Tetris;
            Sounds.Tetris();
        }

        private void TSpin(LineClear move)
        {

        }

        public void Drop(LineClear move)
        {
            if (move == LineClear.SoftDrop)
                TotalPoints++;
            else if (move == LineClear.HardDrop)
                TotalPoints += 2;
        }

        private int Combo()
        {
            if (IsScoringPlay(LastPlay))
                combo = Math.Min(combo + 1, 20);
            else
                combo = 0;

            return 50 * combo * Level;
        }


        private bool IsScoringPlay(LineClear play)
        {
            switch (play)
            {
                case LineClear.None:
                case LineClear.TSpin: return false;
            }
            return true;
        }

        /// <summary>
        /// Increases the game's level.
        /// </summary>
        private void LevelUp()
        {
            linesThisLevel = 0;
            Level++;
            LinesToClear = 5 * Level;
            UpdateLockDelay();
        }

        private void UpdateLockDelay()
        {
            if (LockDelay > 100)
                LockDelay = DEFAULT_LOCK_DELAY - Level * 100;
        }

        public void LastMove(LineClear move)
        {
            switch (move)
            {
                case LineClear.None: LastPlay = move; break;
                case LineClear.Single: Single(); break;
                case LineClear.Double: Double(); break;
                case LineClear.Triple: Triple(); break;
                case LineClear.Tetris: Tetris(); break;
                case LineClear.TSpin: TSpin(move); break;
                case LineClear.TSpinSingle: TSpin(move); break;
                case LineClear.TSpinDouble: TSpin(move); break;
                case LineClear.TSpinTriple: TSpin(move); break;
            }
        }
    }
}
