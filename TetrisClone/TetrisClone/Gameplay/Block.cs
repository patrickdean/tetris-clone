// Block.cs
// Patrick Dean

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TetrisClone
{
    public enum BlockType
    {
        Z, S, L, O, I, J, T,
        None
    };
    
    /// <summary>
    /// A data structure representing one block.
    /// </summary>
    public struct Block
    {
        /// <summary>
        /// The type of block.
        /// </summary>
        public BlockType Type { get; set; }

        /// <summary>
        /// The point position of the block on the game board.
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// The rotate value of the block.
        /// Used to determine which rotate to use.
        /// </summary>
        public int Rotate { get; set; }

        public Block(BlockType type, int x, int y)
            : this()
        {
            Position = new Point(x, y);
            Type = type;
            Rotate = 0;
        }

        /// <summary>
        /// Sets the next rotation value.
        /// </summary>
        public void NextRotate()
        {
            Rotate = (Rotate + 1) % 4;
        }

        /// <summary>
        /// Draws the block.
        /// </summary>
        /// <param name="sb">The spritebatch.</param>
        /// <param name="texture">The block texture.</param>
        /// <param name="drawColor">The draw color. Color.White for default.</param>
        /// <param name="drawPos">The draw position of the block.</param>
        /// <param name="scale">The scale.</param>
        public void Draw(SpriteBatch sb, Texture2D texture, Color drawColor, Vector2 drawPos, float scale)
        {
            Vector2 drawLoc = new Vector2(((Position.X + 1) * 30.0f * scale) + drawPos.X, ((Position.Y + 1) * 30.0f * scale) + drawPos.Y);
            sb.Draw(texture, drawLoc, null, drawColor, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }
    }
}
