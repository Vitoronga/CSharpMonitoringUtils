using System;
using System.Collections.Generic;
using System.Numerics;
using Vanara.PInvoke;
using static Vanara.PInvoke.User32;

namespace MonitoringUtils.Mouse
{
    public static class MouseFuncs
    {
        /// <summary>
        /// <para>
        /// Retrieves information about the cursor in a persistent manner.
        /// </para>
        /// </summary>
        /// <returns></returns>
        public static CursorInfo GetCursorInfo() => new CursorInfo();

        // Cursor Position Tracker Stuff:
        private static Vector2 _last_cursor_pos_;
        /// <summary>
        /// Detects if the cursor position changed since it was last checked
        /// </summary>
        /// <returns></returns>
        public static bool HasCursorPositionChanged()
        {
            CursorInfo cursorInfo = GetCursorInfo();
            bool posChanged = false;

            if (_last_cursor_pos_ != cursorInfo.Position) posChanged = true;

            _last_cursor_pos_ = cursorInfo.Position;
            return posChanged;
        }


        /// <summary>
        /// Detects if a certain button is currently being pressed down.
        /// </summary>
        /// <param name="button">The mouse button defined by the MouseButtonsEnum</param>
        /// <returns></returns>
        public static bool IsButtonPressed(MouseButtonsEnum button)
        {
            short statusCode = GetKeyState((int)button);

            bool isPressed = BitHelper.IsBitOn(BitConverter.GetBytes(statusCode), 15);

            return isPressed;
        }

        /// <summary>
        /// Detects if a button state has changed since it was last checked (by calling this method or any other that may perform said check).
        /// </summary>
        /// <param name="button">The mouse button defined by the MouseButtonsEnum</param>
        /// <returns></returns>
        public static bool HasButtonChanged(MouseButtonsEnum button)
        {
            short statusCode = GetAsyncKeyState((int)button);

            bool isPressed = BitHelper.IsBitOn(BitConverter.GetBytes(statusCode), 15);
            bool wasPressed = BitHelper.IsBitOn(BitConverter.GetBytes(statusCode), 0);

            return isPressed || wasPressed;
        }

        /// <summary>
        /// <para>
        /// Detects if ANY button state has changed since it was last checked.
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
        public static bool HasAnyButtonChanged()
        {
            bool m1 = HasButtonChanged(MouseButtonsEnum.LeftButton);
            bool m2 = HasButtonChanged(MouseButtonsEnum.RightButton);
            bool m3 = HasButtonChanged(MouseButtonsEnum.MiddleButton);
            bool m4 = HasButtonChanged(MouseButtonsEnum.ExtraButton1);
            bool m5 = HasButtonChanged(MouseButtonsEnum.ExtraButton2);

            return m1 || m2 || m3 || m4 || m5;
        }
        
        /// <summary>
        /// Retrieves information about a certain button's states.
        /// </summary>
        /// <param name="button">The mouse button defined by the MouseButtonsEnum</param>
        /// <returns></returns>
        public static MouseButtonInfo GetButtonStates(MouseButtonsEnum button)
        {
            return new MouseButtonInfo(button);
        }

        /// <summary>
        /// Retrieves information about ALL buttons' states.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<MouseButtonsEnum, MouseButtonInfo> GetAllButtonStates()
        {
            Array enumValues = Enum.GetValues(typeof(MouseButtonsEnum));
            Dictionary<MouseButtonsEnum, MouseButtonInfo> keyStatusMap = new Dictionary<MouseButtonsEnum, MouseButtonInfo>(enumValues.Length);

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
