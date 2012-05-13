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
    /// This class generates a queue of all 7 tetris pieces in a
    /// random order.
    /// 
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class RandomGenerator : GameplayPiece
    {
        public static Random randomNumber = new Random();

        /// <summary>
        /// The queue of next pieces.
        /// </summary>
        public Queue<BlockType> pieces { get; protected set; }
                
        List<BlockType> pieceList;

        public RandomGenerator()
        {
            // TODO: Construct any child components here
            pieces = new Queue<BlockType>();
            pieceList = new List<BlockType>(7);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            ResetList();

            CreateFirstBag();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if (pieces.Count == 0)
            {
                ResetList();
                CreateNewBag();
            }
        }
        
        /// <summary>
        /// Returns the next Piece in the current bag and
        /// dequeues it.
        /// </summary>
        /// <returns>The next Piece.</returns>
        public BlockType NextPiece()
        {
            return pieces.Dequeue();
        }

        /// <summary>
        /// Creates a bag whose first piece is not S or Z.
        /// </summary>
        private void CreateFirstBag()
        {
            pieces.Clear();
            int i = 6;
            int j;

            j = randomNumber.Next(2, 7);
            pieces.Enqueue(pieceList.ElementAt(j));
            pieceList.RemoveAt(j);
            
            while (i > 0)
            {
                j = randomNumber.Next(0, i);
                pieces.Enqueue(pieceList.ElementAt(j));
                pieceList.RemoveAt(j);
                i--;
            }
        }

        /// <summary>
        /// Populates the queue with each piece in a random order.
        /// </summary>
        private void CreateNewBag()
        {
            pieces.Clear();
            int i = 7;
            int j;
            while (i > 0)
            {
                j = randomNumber.Next(0, i);
                pieces.Enqueue(pieceList.ElementAt(j));
                pieceList.RemoveAt(j);
                i--;
            }
        }

        /// <summary>
        /// Returns the next piece without dequeueing it.
        /// </summary>
        /// <returns>The next piece.</returns>
        public BlockType Peek()
        {
            return pieces.Peek();
        }

        /// <summary>
        /// Resets the pieceList to its initial state.
        /// </summary>
        private void ResetList()
        {
            pieceList.Clear();
            for (int i = 0; i < 7; i++)
                pieceList.Add((BlockType)i);
        }
    }
}
