using Vanara.PInvoke;
using static Vanara.PInvoke.User32;

namespace MonitoringUtils.Cursor
{
    public static class CursorFuncs
    {
        public static CursorInfo GetStaticCursorInfo() => new CursorInfo(); // Improve naming, as it retrieves data in a persistent manner, not actual exclusive static info

        public static bool IsButtonPressed(MouseButtonsEnum button)
        {
            short statusCode = GetKeyState((int)button);

            bool isPressed = BitHelper.IsBitOn(BitConverter.GetBytes(statusCode), 15);

            return isPressed;
        }

        public static bool HasButtonChanged(MouseButtonsEnum button)
        {
            short statusCode = GetAsyncKeyState((int)button);

            bool isPressed = BitHelper.IsBitOn(BitConverter.GetBytes(statusCode), 15);
            bool wasPressed = BitHelper.IsBitOn(BitConverter.GetBytes(statusCode), 0);

            return isPressed || wasPressed;
        }

        // WARNING: ONLY USE THIS METHOD IF YOU DON'T NEED THE STATES DETAILS!!! Once the state is captured, it is "cleared out" of the system, so subsequent calls won't receive the current state. 
        public static bool HasAnyButtonChanged()
        {
            bool m1 = HasButtonChanged(MouseButtonsEnum.LeftButton);
            bool m2 = HasButtonChanged(MouseButtonsEnum.RightButton);
            bool m3 = HasButtonChanged(MouseButtonsEnum.MiddleButton);
            bool m4 = HasButtonChanged(MouseButtonsEnum.ExtraButton1);
            bool m5 = HasButtonChanged(MouseButtonsEnum.ExtraButton2);

            return m1 || m2 || m3 || m4 || m5;
        }
        
        public static CursorButtonInfo GetButtonStates(MouseButtonsEnum button)
        {
            return new CursorButtonInfo(button);
        }

        public static Dictionary<MouseButtonsEnum, CursorButtonInfo> GetAllButtonStates()
        {
            Array enumValues = Enum.GetValues(typeof(MouseButtonsEnum));
            Dictionary<MouseButtonsEnum, CursorButtonInfo> keyStatusMap = new Dictionary<MouseButtonsEnum, CursorButtonInfo>(enumValues.Length);

            foreach (MouseButtonsEnum mouseButton in enumValues)
            {
                keyStatusMap[mouseButton] = GetButtonStates(mouseButton);
            }

            return keyStatusMap;
        }

        //unsafe public static Vector2 GetPos()
        //{
        //    CURSORINFO cInfo = new CURSORINFO();
        //    cInfo.cbSize = Convert.ToUInt32(sizeof(CURSORINFO));

        //    if (!User32.GetCursorInfo(ref cInfo)) throw new InvalidOperationException("Failed to obtain cursor information");

        //    return new Vector2(cInfo.ptScreenPos.x, cInfo.ptScreenPos.y);
        //}
    }
}
