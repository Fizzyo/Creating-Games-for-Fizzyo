using System;

namespace Fizzyo_Library
{
    public class FizzyoInput : IDisposable
    {
        private static SharpDX.DirectInput.DirectInput directInput;
        private static SharpDX.DirectInput.Joystick[] joysticks;
        private static int[] joysticksDeadzoneX;
        private static FizzyoInput instance;

        public FizzyoInput()
        {
            directInput = new SharpDX.DirectInput.DirectInput();
            //var devices = directInput.GetDevices();
            var devices = directInput.GetDevices(SharpDX.DirectInput.DeviceType.Supplemental, SharpDX.DirectInput.DeviceEnumerationFlags.AllDevices);
            joysticks = new SharpDX.DirectInput.Joystick[devices.Count];
            joysticksDeadzoneX = new int[devices.Count];
            for (int i = 0; i < devices.Count; i++)
            {
                var joystickGuid = devices[i].InstanceGuid;
                joysticks[i] = new SharpDX.DirectInput.Joystick(directInput, joystickGuid);
                joysticks[i].Acquire();
                var deadzone = joysticks[i].GetCurrentState();
                joysticksDeadzoneX[i] = deadzone.X - 500;
            }
        }

        public static Microsoft.Xna.Framework.Input.GamePadState GetState(Microsoft.Xna.Framework.PlayerIndex index)
        {
            if (instance == null) instance = new FizzyoInput();

            SharpDX.DirectInput.JoystickState joyState = new SharpDX.DirectInput.JoystickState();
            if ((int)index < joysticks.Length)
            {
                joysticks[(int)index].GetCurrentState(ref joyState);
                joyState.X -= joysticksDeadzoneX[(int)index];
                return joyState.ToXNAGamePadState();
            }

            return new Microsoft.Xna.Framework.Input.GamePadState();
        }

        public void Dispose()
        {
            directInput = null;
        }
    }

    public static class SharpDXJoystickExtension
    {
        public static Microsoft.Xna.Framework.Input.GamePadState ToXNAGamePadState(this SharpDX.DirectInput.JoystickState joyState)
        {
            Microsoft.Xna.Framework.Vector2 LeftStick = new Microsoft.Xna.Framework.Vector2((float)joyState.X / 45000, (float)joyState.Y);
            Microsoft.Xna.Framework.Input.Buttons buttons = joyState.Buttons[0] ? Microsoft.Xna.Framework.Input.Buttons.A : 0;

            Microsoft.Xna.Framework.Input.GamePadState outputState = new Microsoft.Xna.Framework.Input.GamePadState(LeftStick, Microsoft.Xna.Framework.Vector2.Zero, 0, 0, buttons);
            return outputState;
        }

    }
}
