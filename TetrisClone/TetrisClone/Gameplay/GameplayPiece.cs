// -----------------------------------------------------------------------
// <copyright file="GameplayPiece.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace TetrisClone
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Virtual class that will be inherited by game components.
    /// </summary>
    public class GameplayPiece
    {
        public virtual void LoadContent(ContentManager Content)
        {

        }
        public virtual void Initialize()
        {

        }
        public virtual void Update(GameTime gameTime)
        {

        }
        public virtual void Draw(GameTime gameTime, SpriteBatch sb)
        {

        }
    }
}
