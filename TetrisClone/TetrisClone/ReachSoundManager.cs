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
    /// Handles the sound effects of the game for the Reach profile.
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ReachSoundManager : GameplayPiece
    {
        Song musicTrack;
        SoundEffect hitWall, drop, score, tetris;
        
        /// <summary>
        /// Gets or Sets the Volume for all sound effects.
        /// </summary>
        public float SoundEffectVolume { get; set; }

        /// <summary>
        /// Gets or Sets the Volume for the background music.
        /// </summary>
        public float MusicVolume { get; set; }

        /// <summary>
        /// Constructs an instance of ReachSoundManager.
        /// </summary>
        /// <param name="game">The game for which this is a component of.</param>
        /// <param name="musicVolume">Initial music volume.</param>
        /// <param name="soundEffectVolume">Initial sound effect volume.</param>
        public ReachSoundManager(float musicVolume, float soundEffectVolume)
        {
            SoundEffectVolume = soundEffectVolume;
            MusicVolume = musicVolume;
        }
        
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void LoadContent(ContentManager Content)
        {
            // TODO: Add your initialization code here

           
            hitWall = Content.Load<SoundEffect>("audio\\sfx\\HitWall");
            drop = Content.Load<SoundEffect>("audio\\sfx\\Drop");
            score = Content.Load<SoundEffect>("audio\\sfx\\score");
            tetris = Content.Load<SoundEffect>("audio\\sfx\\tetris");
           
            musicTrack = Content.Load<Song>("audio\\music\\music");

            MediaPlayer.Volume = MusicVolume;
        }
        
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
        }
        
        /// <summary>
        /// Starts the background music track.
        /// </summary>
        public void StartMusic()
        {
            MediaPlayer.Play(musicTrack);
            MediaPlayer.IsRepeating = true;
        }

        /// <summary>
        /// Pauses or resumes the music track, depending on
        /// current state.
        /// </summary>
        public void PauseMusic()
        {
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Pause();
        }

        public void ResumeMusic()
        {
            if (MediaPlayer.State == MediaState.Paused)
                MediaPlayer.Resume();
        }

        /// <summary>
        /// Stops the music track from playing.
        /// </summary>
        public void StopMusic()
        {
            MediaPlayer.Stop();
        }

        /// <summary>
        /// Plays the sound effect for hitting the wall.
        /// </summary>
        public void HitWall()
        {
            SoundEffectInstance instance = hitWall.CreateInstance();
            instance.Volume = SoundEffectVolume;
            instance.Play();
        }

        /// <summary>
        /// Plays the sound effect for dropping a piece.
        /// </summary>
        public void Drop()
        {
            SoundEffectInstance instance = drop.CreateInstance();
            instance.Volume = SoundEffectVolume;
            instance.Play();
        }
        
        /// <summary>
        /// Plays the sound effect corresponding to a given combo
        /// multiplier.
        /// </summary>
        /// <param name="combo">The combo multiplier.</param>
        public void Score(int combo)
        {
            float comboPitch;
            if (combo > 9)
                comboPitch = 1.0f;
            else
                comboPitch = 0.125f * (combo);

            SoundEffectInstance instance = score.CreateInstance();
            instance.Volume = SoundEffectVolume;
            instance.Pitch = comboPitch;
            instance.Play();
        }

        /// <summary>
        /// Plays the sound effect for scoring a Tetris.
        /// </summary>
        public void Tetris()
        {
            SoundEffectInstance instance = tetris.CreateInstance();
            instance.Volume = SoundEffectVolume;
            instance.Play();
        }
        
    }
}
