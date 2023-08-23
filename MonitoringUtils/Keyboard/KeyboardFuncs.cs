using Vanara.PInvoke;
using static Vanara.PInvoke.User32;

namespace MonitoringUtils.Keyboard
{
    public static class KeyboardFuncs
    {
        //public static KeyboardInfo GetStaticKeyboardInfo() => new KeyboardInfo(); // Improve naming, as it retrieves data in a persistent manner, not actual exclusive static info

        public static bool IsKeyPressed(KeyboardButtonsEnum button)
        {
            short returnCode = GetKeyState((int)button);

            bool isPressed = BitHelper.IsBitOn(BitConverter.GetBytes(returnCode), 15);

            return isPressed;
        }

        public static bool IsKeyToggled(KeyboardButtonsEnum button)
        {
            short returnCode = GetKeyState((int)button);

            bool isToggled = BitHelper.IsBitOn(BitConverter.GetBytes(returnCode), 0);

            return isToggled;
        }

        public static bool IsKeyPressedOrToggled(KeyboardButtonsEnum button)
        {
            short returnCode = GetKeyState((int)button);

            bool isPressed = BitHelper.IsBitOn(BitConverter.GetBytes(returnCode), 15);
            bool isToggled = BitHelper.IsBitOn(BitConverter.GetBytes(returnCode), 0);

            return isPressed || isToggled;
        }

        public static bool HasKeyChanged(KeyboardButtonsEnum button)
        {
            short returnCode = GetAsyncKeyState((int)button);

            bool isPressed = BitHelper.IsBitOn(BitConverter.GetBytes(returnCode), 15);
            bool wasPressed = BitHelper.IsBitOn(BitConverter.GetBytes(returnCode), 0);

            return isPressed || wasPressed;
        }

        // WARNING: ONLY USE THIS METHOD IF YOU DON'T NEED THE STATES DETAILS!!! Once the state is captured, it is "cleared out" of the system, so subsequent calls won't receive the current state. 
        public static bool HasAnyKeyChanged()
        {
            IEnumerable<KeyboardButtonsEnum> array = Enum.GetValues(typeof(KeyboardButtonsEnum)).Cast<KeyboardButtonsEnum>();

            foreach(KeyboardButtonsEnum key in array)
            {
                if (HasKeyChanged(key)) return true;
            }

            return false;
        }

        public static KeyboardButtonInfo GetKeyStates(KeyboardButtonsEnum button)
        {
            return new KeyboardButtonInfo(button);
        }

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
