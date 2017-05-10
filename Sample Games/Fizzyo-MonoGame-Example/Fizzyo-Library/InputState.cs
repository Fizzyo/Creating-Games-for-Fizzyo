#region File Description
//-----------------------------------------------------------------------------
// InputState.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
#endregion

namespace Fizzyo_Library
{
    /// <summary>
    /// Helper for reading input from keyboard, gamepad, and touch input. This class 
    /// tracks both the current and previous state of the input devices, and implements 
    /// query methods for high level input actions such as "move up through the menu"
    /// or "pause the game".
    /// </summary>
    public class InputState : GameComponent
    {
        #region Fields

        public const int MaxGamePadInputs = 4;

        public KeyboardState CurrentKeyboardState;
        public readonly GamePadState[] CurrentGamePadStates;

        public KeyboardState LastKeyboardState;
        public readonly GamePadState[] LastGamePadStates;

        public readonly bool[] GamePadWasConnected;

        public bool GamePadsAvailable = false;

        public TouchCollection TouchState;

        public readonly List<GestureSample> Gestures = new List<GestureSample>();

        #endregion

        #region Initialization


        /// <summary>
        /// Constructs a new input state.
        /// </summary>
        public InputState(Game game) : base (game)
        {
            CurrentKeyboardState = new KeyboardState();
            CurrentGamePadStates = new GamePadState[MaxGamePadInputs];

            LastKeyboardState = new KeyboardState();
            LastGamePadStates = new GamePadState[MaxGamePadInputs];

            GamePadWasConnected = new bool[MaxGamePadInputs];
        }


        #endregion

        #region Update
        /// <summary>
        /// Reads the latest state of the keyboard and gamepad.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            LastKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
            for (int i = 0; i < MaxGamePadInputs; i++)
            {
                LastGamePadStates[i] = CurrentGamePadStates[i];

                CurrentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);

                GamePadsAvailable = false;

                // Keep track of whether a gamepad has ever been
                // connected, so we can detect if it is unplugged.
                if (CurrentGamePadStates[i].IsConnected)
                {
                    GamePadsAvailable = true;
                    GamePadWasConnected[i] = true;
                }
            }

            TouchState = TouchPanel.GetState();

            Gestures.Clear();
            while (TouchPanel.IsGestureAvailable)
            {
                Gestures.Add(TouchPanel.ReadGesture());
            }

            base.Update(gameTime);

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Helper for checking if a key was pressed during this update. The
        /// controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a keypress
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsKeyPressed(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Helper for checking if a key was newly pressed during this update,
        /// by the specified player.
        /// </summary>
        public bool IsNewKeyPress(Keys key)
        {
            return (CurrentKeyboardState.IsKeyDown(key) &&
                    LastKeyboardState.IsKeyUp(key));
        }

        /// <summary>
        /// Helper for checking if a key was held down during this update,
        /// by the specified player.
        /// </summary>
        public bool IsKeyHeld(Keys key)
        {
            return (CurrentKeyboardState.IsKeyDown(key) &&
                    LastKeyboardState.IsKeyDown(key));
        }

        /// <summary>
        /// Helper for checking if a key was released during this update,
        /// by the specified player.
        /// </summary>
        public bool IsKeyReleased(Keys key)
        {
            return (CurrentKeyboardState.IsKeyUp(key) &&
                    LastKeyboardState.IsKeyDown(key));
        }

        /// <summary>
        /// Helper for checking if a key was newly pressed during this update. The
        /// controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a keypress
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsNewKeyPress(Keys key, PlayerIndex? controllingPlayer,
                                            out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentKeyboardState.IsKeyDown(key) &&
                        LastKeyboardState.IsKeyUp(key));
            }
            else
            {
                // Accept input from any player.
                return (IsNewKeyPress(key, PlayerIndex.One, out playerIndex) ||
                        IsNewKeyPress(key, PlayerIndex.Two, out playerIndex) ||
                        IsNewKeyPress(key, PlayerIndex.Three, out playerIndex) ||
                        IsNewKeyPress(key, PlayerIndex.Four, out playerIndex));
            }
        }

        /// <summary>
        /// Helper for checking if a button was pressed during this update.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a button press
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsButtonPressed(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return CurrentGamePadStates[i].IsButtonDown(button);
            }
            else
            {
                // Accept input from any player.
                return (IsButtonPressed(button, PlayerIndex.One, out playerIndex) ||
                        IsButtonPressed(button, PlayerIndex.Two, out playerIndex) ||
                        IsButtonPressed(button, PlayerIndex.Three, out playerIndex) ||
                        IsButtonPressed(button, PlayerIndex.Four, out playerIndex));
            }
        }

        /// <summary>
        /// Helper for checking the state of the left thumbstick during this update.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a button press
        /// is detected, the output playerIndex reports which player pressed it.
        /// If no connected gamepad found, it will return Vector2.Zero
        /// </summary>
        public Vector2 GetThumbStickLeft(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;
            return GetThumbStickLeft(controllingPlayer, out playerIndex);
        }

            /// <summary>
            /// Helper for checking the state of the left thumbstick during this update.
            /// The controllingPlayer parameter specifies which player to read input for.
            /// If this is null, it will accept input from any player. When a button press
            /// is detected, the output playerIndex reports which player pressed it.
            /// If no connected gamepad found, it will return Vector2.Zero
            /// </summary>
            public Vector2 GetThumbStickLeft(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return CurrentGamePadStates[i].ThumbSticks.Left;
            }
            else
            {
                for (int i = 0; i < MaxGamePadInputs; i++)
                {
                    if (CurrentGamePadStates[i].IsConnected)
                    {
                        playerIndex = (PlayerIndex)i;
                        return CurrentGamePadStates[i].ThumbSticks.Left;
                    }
                }
                playerIndex = PlayerIndex.One;
                return Vector2.Zero;
            }
        }

        /// <summary>
        /// Helper for checking the state of the left thumbstick during this update.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a button press
        /// is detected, the output playerIndex reports which player pressed it.
        /// If no connected gamepad found, it will return Vector2.Zero
        /// </summary>
        public Vector2 GetThumbStickRight(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;
            return GetThumbStickRight(controllingPlayer, out playerIndex);
        }

        /// <summary>
        /// Helper for checking the state of the right thumbstick during this update.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a button press
        /// is detected, the output playerIndex reports which player pressed it.
        /// If no connected gamepad found, it will return Vector2.Zero
        /// </summary>
        public Vector2 GetThumbStickRight(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        { 
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return CurrentGamePadStates[i].ThumbSticks.Right;
            }
            else
            {
                for (int i = 0; i < MaxGamePadInputs; i++)
                {
                    if (CurrentGamePadStates[i].IsConnected)
                    {
                        playerIndex = (PlayerIndex)i;
                        return CurrentGamePadStates[i].ThumbSticks.Right;
                    }
                }
                playerIndex = PlayerIndex.One;
                return Vector2.Zero;
            }
        }

        /// <summary>
        /// Helper for checking the state of the left trigger during this update.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a button press
        /// is detected, the output playerIndex reports which player pressed it.
        /// If no connected gamepad found, it will return Vector2.Zero
        /// </summary>
        public float GetTriggerLeft(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;
            return GetTriggerLeft(controllingPlayer, out playerIndex);
        }

        /// <summary>
        /// Helper for checking the state of the left trigger during this update.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a button press
        /// is detected, the output playerIndex reports which player pressed it.
        /// If no connected gamepad found, it will return Vector2.Zero
        /// </summary>
        public float GetTriggerLeft(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return CurrentGamePadStates[i].Triggers.Left;
            }
            else
            {
                for (int i = 0; i < MaxGamePadInputs; i++)
                {
                    if (CurrentGamePadStates[i].IsConnected)
                    {
                        playerIndex = (PlayerIndex)i;
                        return CurrentGamePadStates[i].Triggers.Left;
                    }
                }
                playerIndex = PlayerIndex.One;
                return 0;
            }
        }

        /// <summary>
        /// Helper for checking the state of the right trigger during this update.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a button press
        /// is detected, the output playerIndex reports which player pressed it.
        /// If no connected gamepad found, it will return Vector2.Zero
        /// </summary>
        public float GetTriggerRight(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;
            return GetTriggerRight(controllingPlayer, out playerIndex);
        }

        /// <summary>
        /// Helper for checking the state of the right trigger during this update.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a button press
        /// is detected, the output playerIndex reports which player pressed it.
        /// If no connected gamepad found, it will return Vector2.Zero
        /// </summary>
        public float GetTriggerRight(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return CurrentGamePadStates[i].Triggers.Right;
            }
            else
            {
                for (int i = 0; i < MaxGamePadInputs; i++)
                {
                    if (CurrentGamePadStates[i].IsConnected)
                    {
                        playerIndex = (PlayerIndex)i;
                        return CurrentGamePadStates[i].Triggers.Right;
                    }
                }
                playerIndex = PlayerIndex.One;
                return 0;
            }
        }

        /// <summary>
        /// Helper for checking if a button was newly pressed during this update.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a button press
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsNewButtonPress(Buttons button, PlayerIndex? controllingPlayer,
                                                     out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentGamePadStates[i].IsButtonDown(button) &&
                        LastGamePadStates[i].IsButtonUp(button));
            }
            else
            {
                // Accept input from any player.
                return (IsNewButtonPress(button, PlayerIndex.One, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Two, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Three, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Four, out playerIndex));
            }
        }

        /// <summary>
        /// Helper for checking if a button was newly pressed during this update.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a button press
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsButtonHeld(Buttons button, PlayerIndex? controllingPlayer,
                                                     out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentGamePadStates[i].IsButtonDown(button) &&
                        LastGamePadStates[i].IsButtonDown(button));
            }
            else
            {
                // Accept input from any player.
                return (IsNewButtonPress(button, PlayerIndex.One, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Two, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Three, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Four, out playerIndex));
            }
        }

        /// <summary>
        /// Helper for checking if a button was newly pressed during this update.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a button press
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsButtonReleased(Buttons button, PlayerIndex? controllingPlayer,
                                                     out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentGamePadStates[i].IsButtonUp(button) &&
                        LastGamePadStates[i].IsButtonDown(button));
            }
            else
            {
                // Accept input from any player.
                return (IsNewButtonPress(button, PlayerIndex.One, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Two, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Three, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Four, out playerIndex));
            }
        }


        /// <summary>
        /// Checks for a "menu select" input action.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When the action
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsMenuSelect(PlayerIndex? controllingPlayer,
                                 out PlayerIndex playerIndex)
        {
            return IsNewKeyPress(Keys.Space, controllingPlayer, out playerIndex) ||
                   IsNewKeyPress(Keys.Enter, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.A, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
        }


        /// <summary>
        /// Checks for a "menu cancel" input action.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When the action
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsMenuCancel(PlayerIndex? controllingPlayer,
                                 out PlayerIndex playerIndex)
        {
            return IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.B, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.Back, controllingPlayer, out playerIndex);
        }


        /// <summary>
        /// Checks for a "menu up" input action.
        /// The controllingPlayer parameter specifies which player to read
        /// input for. If this is null, it will accept input from any player.
        /// </summary>
        public bool IsMenuUp(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Up, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.DPadUp, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.LeftThumbstickUp, controllingPlayer, out playerIndex);
        }


        /// <summary>
        /// Checks for a "menu down" input action.
        /// The controllingPlayer parameter specifies which player to read
        /// input for. If this is null, it will accept input from any player.
        /// </summary>
        public bool IsMenuDown(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Down, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.DPadDown, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.LeftThumbstickDown, controllingPlayer, out playerIndex);
        }


        /// <summary>
        /// Checks for a "pause the game" input action.
        /// The controllingPlayer parameter specifies which player to read
        /// input for. If this is null, it will accept input from any player.
        /// </summary>
        public bool IsPauseGame(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.Back, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
        }

        /// <summary>
        /// Checks for a player doing the Fire input action.
        /// The controllingPlayer parameter specifies which player to read
        /// input for. If this is null, it will accept input from any player.
        /// </summary>
        public bool PlayerPressedFire(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Space, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.A, controllingPlayer, out playerIndex);
        }


        #endregion

        #region Disposing
        /// <summary>
        /// Clean up the component when it is disposing.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            GamePadsAvailable = false;
            Gestures.Clear();

            base.Dispose(disposing);
        }
        #endregion
    }
}
