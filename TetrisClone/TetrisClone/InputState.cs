//-----------------------------------------
// InputState.cs
//
// by Patrick Dean
//------------------------------------------
namespace InputStateController
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// This class handles Keyboard, Mouse and GamePad input.
    /// </summary>
    public class InputState
    {
        #region Properties
        /// <summary>
        /// Gets the current KeyboardState
        /// </summary>
        public KeyboardState CurrentKeyboardState { get; private set; }

        /// <summary>
        /// Gets the previous KeyboardState
        /// </summary>
        public KeyboardState PreviousKeyboardState { get; private set; }

        /// <summary>
        /// Gets the current MouseState
        /// </summary>
        public MouseState CurrentMouseState { get; private set; }

        /// <summary>
        /// Gets the previous MouseState
        /// </summary>
        public MouseState PreviousMouseState { get; private set; }

        /// <summary>
        /// Gets the current GamePadState
        /// </summary>
        public GamePadState CurrentGamePadState { get; private set; }

        /// <summary>
        /// Gets the previous GamePadState
        /// </summary>
        public GamePadState PreviousGamePadState { get; private set; }

        /// <summary>
        /// Keeps track of whether a gamepad has ever been connected.
        /// </summary>
        public bool GamePadWasConnected { get; private set; }
        
        #endregion

        private const float threshold = 0.5f;

        public InputState()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes InputState to its default values.
        /// </summary>
        public void Initialize()
        {
            CurrentKeyboardState = Keyboard.GetState();
            CurrentMouseState = Mouse.GetState();
            CurrentGamePadState = GamePad.GetState(PlayerIndex.One);
        }

        /// <summary>
        /// Sets previous states and updates to new states.
        /// </summary>
        public void Update()
        {
            PreviousKeyboardState = CurrentKeyboardState;
            PreviousMouseState = CurrentMouseState;
            PreviousGamePadState = CurrentGamePadState;

            CurrentKeyboardState = Keyboard.GetState();
            CurrentMouseState = Mouse.GetState();
            CurrentGamePadState = GamePad.GetState(PlayerIndex.One);

            if (CurrentGamePadState.IsConnected)
                GamePadWasConnected = true;
        }

        #region Keyboard

        /// <summary>
        /// Queries Keyboard key.
        /// </summary>
        /// <param name="key">The key to query.</param>
        /// <returns>Returns true if pressed.</returns>
        public bool IsKeyPressed(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Queries Keyboard key.
        /// </summary>
        /// <param name="key">The key to query.</param>
        /// <returns>Returns true if released.</returns>
        public bool IsKeyReleased(Keys key)
        {
            return CurrentKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Queries Keyboard key.
        /// </summary>
        /// <param name="key">The key to query.</param>
        /// <returns>Returns true if key is pressed and was previously released.</returns>
        public bool IsKeyTriggered(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key);
        }
        #endregion

        #region Mouse

        #region Mouse Pressed

        /// <summary>
        /// Queries LeftMouseButton
        /// </summary>
        /// <returns>Returns true if pressed.</returns>
        public bool IsMouseLeftButtonPressed()
        {
            return CurrentMouseState.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Queries RightMouseButton
        /// </summary>
        /// <returns>Returns true if pressed.</returns>
        public bool IsMouseRightButtonPressed()
        {
            return CurrentMouseState.RightButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Queries MiddleMouseButton
        /// </summary>
        /// <returns>Returns true if pressed.</returns>
        public bool IsMouseMiddleButtonPressed()
        {
            return CurrentMouseState.MiddleButton == ButtonState.Pressed;
        }
        #endregion

        #region Mouse Released

        /// <summary>
        /// Queries LeftMouseButton
        /// </summary>
        /// <returns>Returns true if released.</returns>
        public bool IsMouseLeftButtonReleased()
        {
            return CurrentMouseState.LeftButton == ButtonState.Released;
        }

        /// <summary>
        /// Queries RightMouseButton
        /// </summary>
        /// <returns>Returns true if released.</returns>
        public bool IsMouseRightButtonReleased()
        {
            return CurrentMouseState.RightButton == ButtonState.Released;
        }

        /// <summary>
        /// Queries MiddleMouseButton
        /// </summary>
        /// <returns>Returns true if released.</returns>
        public bool IsMouseMiddleButtonReleased()
        {
            return CurrentMouseState.MiddleButton == ButtonState.Released;
        }
        #endregion

        #region Mouse Triggered

        /// <summary>
        /// Queries LeftMouseButton
        /// </summary>
        /// <returns>Returns true if LeftMouseButton is pressed and was previously released.</returns>
        public bool IsMouseLeftButtonTriggered()
        {
            return CurrentMouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released;
        }

        /// <summary>
        /// Queries RightMouseButton
        /// </summary>
        /// <returns>Returns true if RightMouseButton is pressed and was previously released.</returns>
        public bool IsMouseRightButtonTriggered()
        {
            return CurrentMouseState.RightButton == ButtonState.Pressed && PreviousMouseState.RightButton == ButtonState.Released;
        }

        /// <summary>
        /// Queries MiddleMouseButton
        /// </summary>
        /// <returns>Returns true if MiddleMouseButton is pressed and was previously released.</returns>
        public bool IsMouseMiddleButtonTriggered()
        {
            return CurrentMouseState.MiddleButton == ButtonState.Pressed && PreviousMouseState.MiddleButton == ButtonState.Released;
        }

        #endregion

        #endregion

        #region Game Pad

        /// <summary>
        /// Queries GamePad button.
        /// </summary>
        /// <param name="button">The button to query.</param>
        /// <returns>Returns true if pressed.</returns>
        public bool IsButtonPressed(Buttons button)
        {
            return CurrentGamePadState.IsButtonDown(button);
        }

        /// <summary>
        /// Queries GamePad button.
        /// </summary>
        /// <param name="button">The button to query.</param>
        /// <returns>Returns true if released.</returns>
        public bool IsButtonReleased(Buttons button)
        {
            return CurrentGamePadState.IsButtonUp(button);
        }

        /// <summary>
        /// Queries GamePad button.
        /// </summary>
        /// <param name="button">The button to query.</param>
        /// <returns>Returns true if button is pressed and was previously released.</returns>
        public bool IsButtonTriggered(Buttons button)
        {
            return CurrentGamePadState.IsButtonDown(button) && PreviousGamePadState.IsButtonUp(button);
        }

        public bool IsLeftStickUpPressed()
        {
            return CurrentGamePadState.ThumbSticks.Left.Y > threshold;
        }

        public bool IsLeftStickDownPressed()
        {
            return CurrentGamePadState.ThumbSticks.Left.Y < -threshold;
        }

        public bool IsLeftStickLeftPressed()
        {
            return CurrentGamePadState.ThumbSticks.Left.X < -threshold;
        }

        public bool IsLeftStickRightPressed()
        {
            return CurrentGamePadState.ThumbSticks.Left.X > threshold;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsLeftStickUpTriggered()
        {
            return IsLeftStickUpPressed() && PreviousGamePadState.ThumbSticks.Left.Y < threshold;
        }

        public bool IsLeftStickDownTriggered()
        {
            return IsLeftStickDownPressed() && PreviousGamePadState.ThumbSticks.Left.Y > -threshold;
        }

        public bool IsLeftStickLeftTriggered()
        {
            return IsLeftStickLeftPressed() && PreviousGamePadState.ThumbSticks.Left.X > -threshold;
        }

        public bool IsLeftStickRightTriggered()
        {
            return IsLeftStickRightPressed() && PreviousGamePadState.ThumbSticks.Left.X < threshold;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Helper for checking if a key was newly pressed during this update.
        /// </summary>
        public bool IsNewKeyPress(Keys key)
        {
            return (CurrentKeyboardState.IsKeyDown(key) &&
                        PreviousKeyboardState.IsKeyUp(key));
        }
        
        /// <summary>
        /// Helper for checking if a button was newly pressed during this update.
        /// </summary>
        public bool IsNewButtonPress(Buttons button)
        {
            return (CurrentGamePadState.IsButtonDown(button) &&
                        PreviousGamePadState.IsButtonUp(button));
        }
        
        /// <summary>
        /// Checks for a "menu select" input action.
        /// </summary>
        public bool IsMenuSelect()
        {
            return IsNewKeyPress(Keys.Enter) ||
                   IsNewButtonPress(Buttons.A) ||
                   IsNewButtonPress(Buttons.Start);
        }


        /// <summary>
        /// Checks for a "menu cancel" input action.
        /// </summary>
        public bool IsMenuCancel()
        {
            return IsNewKeyPress(Keys.Escape) ||
                   IsNewButtonPress(Buttons.B) ||
                   IsNewButtonPress(Buttons.Back);
        }


        /// <summary>
        /// Checks for a "menu up" input action.
        /// </summary>
        public bool IsMenuUp()
        {
            return IsNewKeyPress(Keys.Up) ||
                   IsNewButtonPress(Buttons.DPadUp) ||
                   IsNewButtonPress(Buttons.LeftThumbstickUp);
        }


        /// <summary>
        /// Checks for a "menu down" input action.
        /// </summary>
        public bool IsMenuDown()
        {
            return IsNewKeyPress(Keys.Down) ||
                   IsNewButtonPress(Buttons.DPadDown) ||
                   IsNewButtonPress(Buttons.LeftThumbstickDown);
        }

        /// <summary>
        /// Checks for a "pause the game" input action.
        /// </summary>
        public bool IsPauseGame()
        {
            return IsNewKeyPress(Keys.Escape) ||
                   IsNewButtonPress(Buttons.Back) ||
                   IsNewButtonPress(Buttons.Start);
        }

        #endregion
    }
}
