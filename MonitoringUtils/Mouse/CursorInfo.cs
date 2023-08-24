using System.Text;
using Vanara.PInvoke;
using static Vanara.PInvoke.User32;
using System.Numerics;

namespace MonitoringUtils.Cursor
{
    public enum CursorStateEnum
    {
        Hidden = 0,
        Showing = 1,
        Suppressed = 2
    }

    public class CursorInfo
    {
        private CURSORINFO cursorInfo;
        public Vector2 Position { get; private set; }
        public CursorStateEnum CursorState { get; private set; }

        public CursorInfo()
        {
            InstantiateCursorInfo();
        }

        unsafe private void InstantiateCursorInfo()
        {
            CURSORINFO cInfo = new CURSORINFO();
            cInfo.cbSize = Convert.ToUInt32(sizeof(CURSORINFO));

            if (!GetCursorInfo(ref cInfo)) throw new InvalidOperationException("Failed to obtain cursor information");
            cursorInfo = cInfo;

            // Configure properties
            Position = new Vector2(cursorInfo.ptScreenPos.X, cursorInfo.ptScreenPos.Y);
            CursorState = GetCursorState();
        }

        private CursorStateEnum GetCursorState()
        {
            int enumValue = (int)cursorInfo.flags;

            return (CursorStateEnum)enumValue;
        }

        //public void UpdateCursorInfo() => InstantiateCursorInfo(); // Don't do this.

        public string GetReadableDescription()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"Pos: {Position.ToString()}");
            builder.AppendLine($"Cursor State: {CursorState.ToString()}");

            return builder.ToString();
        }
    }
}
