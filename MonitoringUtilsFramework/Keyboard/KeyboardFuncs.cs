using System;
using System.Collections.Generic;
using System.Linq;
using Vanara.PInvoke;
using static Vanara.PInvoke.User32;

namespace MonitoringUtils.Keyboard
{
    public static class KeyboardFuncs
    {
        //public static KeyboardInfo GetStaticKeyboardInfo() => new KeyboardInfo(); // Improve naming, as it retrieves data in a persistent manner, not actual exclusive static info

        /// <summary>
        /// Detects if a certain key is currently being pressed down.
        /// </summary>
        /// <param name="button">The keyboard button defined by the KeyboardButtonsEnum</param>
        /// <returns></returns>
        public static bool IsKeyPressed(KeyboardButtonsEnum button)
        {
            short returnCode = GetKeyState((int)button);

            bool isPressed = BitHelper.IsBitOn(BitConverter.GetBytes(returnCode), 15);

            return isPressed;
        }

        /// <summary>
        /// Detects if a key is toggled on (such as the CAPS LOCK button).
        /// </summary>
        /// <param name="button">The keyboard button defined by the KeyboardButtonsEnum</param>
        /// <returns></returns>
        public static bool IsKeyToggled(KeyboardButtonsEnum button)
        {
            short returnCode = GetKeyState((int)button);

            bool isToggled = BitHelper.IsBitOn(BitConverter.GetBytes(returnCode), 0);

            return isToggled;
        }

        /// <summary>
        /// Detects if a key is currently being pressed down or is toggled on.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Both checks are done with a single system call, so there is no relevant performance impact.
        /// </para>
        /// </remarks>
        /// <param name="button">The keyboard button defined by the KeyboardButtonsEnum</param>
        /// <returns></returns>
        public static bool IsKeyPressedOrToggled(KeyboardButtonsEnum button)
        {
            short returnCode = GetKeyState((int)button);

            bool isPressed = BitHelper.IsBitOn(BitConverter.GetBytes(returnCode), 15);
            bool isToggled = BitHelper.IsBitOn(BitConverter.GetBytes(returnCode), 0);

            return isPressed || isToggled;
        }

        /// <summary>
        /// Detects if a key state has changed since it was last checked (by calling this method or any other that may perform said check).
        /// </summary>
        /// <param name="button">The keyboard button defined by the KeyboardButtonsEnum</param>
        /// <returns></returns>
        public static bool HasKeyChanged(KeyboardButtonsEnum button)
        {
            short returnCode = GetAsyncKeyState((int)button);

            bool isPressed = BitHelper.IsBitOn(BitConverter.GetBytes(returnCode), 15);
            bool wasPressed = BitHelper.IsBitOn(BitConverter.GetBytes(returnCode), 0);

            return isPressed || wasPressed;
        }

        // WARNING: ONLY USE THIS METHOD IF YOU DON'T NEED THE STATES DETAILS!!! Once the state is captured, it is "cleared out" of the system, so subsequent calls won't receive the current state. 
        /// <summary>
        /// <para>
        /// Detects if ANY key state has changed since it was last checked.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// WARNING: ONLY USE THIS METHOD IF YOU DON'T NEED THE STATES DETAILS!!!
        /// </para>
        /// <para>
        /// Once the state is captured, it is "cleared out" of the system, so subsequent calls won't receive the same received data. 
        /// </para>
        /// </remarks>
        /// <returns></returns>
        public static bool HasAnyKeyChanged()
        {
            IEnumerable<KeyboardButtonsEnum> array = Enum.GetValues(typeof(KeyboardButtonsEnum)).Cast<KeyboardButtonsEnum>();

            foreach(KeyboardButtonsEnum key in array)
            {
                if (HasKeyChanged(key)) return true;
            }

            return false;
        }

        /// <summary>
        /// Retrieves the states of a certain key.
        /// </summary>
        /// <param name="button">The keyboard button defined by the KeyboardButtonsEnum</param>
        /// <returns></returns>
        public static KeyboardButtonInfo GetKeyStates(KeyboardButtonsEnum button)
        {
            return new KeyboardButtonInfo(button);
        }

        /// <summary>
        /// Retrieves the states of ALL keys.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<KeyboardButtonsEnum, KeyboardButtonInfo> GetAllKeyStates()
        {
            Array enumValues = Enum.GetValues(typeof(KeyboardButtonsEnum));
            Dictionary<KeyboardButtonsEnum, KeyboardButtonInfo> keyStatusMap = new Dictionary<KeyboardButtonsEnum, KeyboardButtonInfo>(enumValues.Length);

            foreach (KeyboardButtonsEnum mouseButton in enumValues)
            {
                keyStatusMap[mouseButton] = GetKeyStates(mouseButton);
            }

            return keyStatusMap;
        }
    }
}
