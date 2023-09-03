using static Vanara.PInvoke.User32;
using MonitoringUtils.ButtonInfo;
using System;

namespace MonitoringUtils.Mouse
{
    public enum MouseButtonsEnum
    {
        LeftButton = 1,
        RightButton = 2,
        MiddleButton = 4,
        ExtraButton1 = 5,
        ExtraButton2 = 6,
    }

    public class MouseButtonInfo : IButtonInfo
    {
        public MouseButtonsEnum Button { get; set; }
        public bool IsPressed { get; set; }
        public bool WasPressed { get; set; }

        public MouseButtonInfo(MouseButtonsEnum button) : this(button, GetAsyncKeyState((int)button)) { }
        public MouseButtonInfo(MouseButtonsEnum button, short statusCode)
        {
            Button = button;

            bool isPressed = BitHelper.IsBitOn(BitConverter.GetBytes(statusCode), 15);
            bool wasPressed = BitHelper.IsBitOn(BitConverter.GetBytes(statusCode), 0);

            IsPressed = isPressed;
            WasPressed = wasPressed;
        }

        public string GetReadableDescription()
        {
            return $"({Button}): IsPressed? {IsPressed} ; WasPressed? {WasPressed}";
        }
    }
}
