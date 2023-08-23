using Vanara.PInvoke;
using static Vanara.PInvoke.User32;

namespace MonitoringUtils.Cursor
{
    public enum MouseButtonsEnum
    {
        Mouse1 = 1,
        Mouse2 = 2,
        Mouse3 = 4,
        Mouse4 = 5,
        Mouse5 = 6,
    }


    public static class CursorFuncs
    {
        public static CursorInfo GetStaticCursorInfo() => new CursorInfo();

        // FIX THIS: the bit is switched by the click, this can be used to track the HasChangedButton
        public static bool IsButtonPressed(MouseButtonsEnum button)
        {
            short returnCode = GetKeyState((int)button);

            bool isPressed = BitHelper.IsBitOn(BitConverter.GetBytes(returnCode), 0);

            return isPressed;
        }

        public static bool HasButtonChanged(MouseButtonsEnum button)
        {
            short returnCode = GetAsyncKeyState((int)button);

            bool isPressed = BitHelper.IsBitOn(BitConverter.GetBytes(returnCode), 15);
            bool wasPressed = BitHelper.IsBitOn(BitConverter.GetBytes(returnCode), 0);

            return isPressed || wasPressed;
        }

        public static bool HasAnyButtonChanged()
        {
            bool m1 = HasButtonChanged(MouseButtonsEnum.Mouse1);
            bool m2 = HasButtonChanged(MouseButtonsEnum.Mouse2);
            bool m3 = HasButtonChanged(MouseButtonsEnum.Mouse3);
            bool m4 = HasButtonChanged(MouseButtonsEnum.Mouse4);
            bool m5 = HasButtonChanged(MouseButtonsEnum.Mouse5);

            return m1 || m2 || m3 || m4 || m5;
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
