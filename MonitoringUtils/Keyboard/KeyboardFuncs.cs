using System.Reflection.Metadata.Ecma335;
using Vanara.PInvoke;
using static Vanara.PInvoke.User32;

namespace MonitoringUtils.Keyboard
{
    public enum KeysEnum
    {
        // 1 mouse1 (left button)
        // 2 mouse2 (right button)
        // 3 cancel
        // 4 mouse3 (middle button)
        // 5 mouse4 (x1)
        // 6 mouse5 (x2)
        // 7 undefined
        Backspace = 8,
        Tab = 9,
        // 10 reserved
        // 11 reserved
        // 12 clear key (special oldschool key)
        Enter = 13,
        // 14 undefined
        // 15 undefined
        Shift = 16,
        Control = 17,
        Alt = 18,
        Pause = 19,
        Caps = 20,
        // 21 ime kana/hanguel/hangul
        // 22 ime on
        // 23 ime junja
        // 24 ime final
        // 25 ime hanja
        // 25 ime kanji
        // 26 ime off
        Esc = 27,
        // 28 ime convert
        // 29 ime nonconvert
        // 30 ime accept
        // 31 ime mode change request
        Spacebar = 32,
        PageUp = 33,
        PageDown = 34,
        End = 35,
        Home = 36,
        LeftArrow = 37,
        UpArrow = 38,
        RightArrow = 39,
        DownArrow = 40,
        Select = 41,
        //Print = 42,
        //Execute = 43,
        PrintScreen = 44,
        Insert = 45,
        Delete = 46,
        Help = 47,
        Zero = 48,
        One = 49,
        Two = 50,
        Three = 51,
        Four = 52,
        Five = 53,
        Six = 54,
        Seven = 55,
        Eight = 56,
        Nine = 57,
        // 58 undefined
        // 59 undefined
        // 60 undefined
        // 61 undefined
        // 62 undefined
        // 63 undefined
        // 64 undefined
        A = 65,
        B = 66,
        C = 67,
        D = 68,
        E = 69,
        F = 70,
        G = 71,
        H = 72,
        I = 73,
        J = 74,
        K = 75,
        L = 76,
        M = 77,
        N = 78,
        O = 79,
        P = 80,
        Q = 81,
        R = 82,
        S = 83,
        T = 84,
        U = 85,
        V = 86,
        W = 87,
        X = 88,
        Y = 89,
        Z = 90,
        LeftWindowsKey = 91,
        RightWindowsKey = 92,
        ApplicationsKey = 93,
        // 94 reserved
        ComputerSleep = 95,
        Numpad0 = 96,
        Numpad1 = 97,
        Numpad2 = 98,
        Numpad3 = 99,
        Numpad4 = 100,
        Numpad5 = 101,
        Numpad6 = 102,
        Numpad7 = 103,
        Numpad8 = 104,
        Numpad9 = 105,
        Multiply = 106,
        Add = 107,
        Separator = 108,
        Subtract = 109,
        Decimal = 110,
        Divide = 111,
        F1 = 112,
        F2 = 113,
        F3 = 114,
        F4 = 115,
        F5 = 116,
        F6 = 117,
        F7 = 118,
        F8 = 119,
        F9 = 120,
        F10 = 121,
        F11 = 122,
        F12 = 123,
        F13 = 124,
        F14 = 125,
        F15 = 126,
        F16 = 127,
        F17 = 128,
        F18 = 129,
        F19 = 130,
        F20 = 131,
        F21 = 132,
        F22 = 133,
        F23 = 134,
        F24 = 135,
        // 136 unassigned
        // 137 unassigned
        // 138 unassigned
        // 139 unassigned
        // 140 unassigned
        // 141 unassigned
        // 142 unassigned
        // 143 unassigned
        NumLock = 144,
        ScrollLock = 145,
        // 146 oem specific
        // 147 oem specific
        // 148 oem specific
        // 149 oem specific
        // 150 oem specific
        // 151 unassigned
        // 152 unassigned
        // 153 unassigned
        // 154 unassigned
        // 155 unassigned
        // 156 unassigned
        // 157 unassigned
        // 158 unassigned
        // 159 unassigned
        LeftShift = 160,
        RightShift = 161,
        LeftControl = 162,
        RightControl = 163,
        LeftAlt = 164,
        RightAlt = 165,
        // 166 Browser back key
        // 167 Browser forward key
        // 168 Browser refresh key
        // 169 Browser stop key
        // 170 Browser search key
        // 171 Browser favorites key
        // 172 Browser start and home key
        VolumeMute = 173,
        VolumeDown = 174,
        VolumeUp = 175,
        NextTrack = 176,
        PreviousTrack = 177,
        StopMedia = 178,
        Play_PauseMedia = 179,
        // 180 start mail key
        // 181 select media key
        // 182 start application 1 key
        // 183 start application 2 key
        // 184 reserved
        // 185 reserved
        SemiColon_Colon = 186,
        Plus = 187,
        Comma = 188,
        Minus = 189,
        Period = 190,
        Slash_InterrogationKey = 191,
        TildeKey = 192,
        // 193 reserved
        // 194 reserved
        // 195 reserved
        // 196 reserved
        // 197 reserved
        // 198 reserved
        // 199 reserved
        // 200 reserved
        // 201 reserved
        // 202 reserved
        // 203 reserved
        // 204 reserved
        // 205 reserved
        // 206 reserved
        // 207 reserved
        // 208 reserved
        // 209 reserved
        // 210 reserved
        // 211 reserved
        // 212 reserved
        // 213 reserved
        // 214 reserved
        // 215 reserved
        // 216 unassigned
        // 217 unassigned
        // 218 unassigned
        OpenBracket = 219,
        Backslash_Pipe = 220,
        CloseBracket = 221,
        SingleDoubleQuote = 222,
        // 223 MiscKey
        // 224 reserved
        // 225 oem specific
        BiggerSmallerThan = 226,
        // 227 oem specific
        // 228 oem specific
        // 229 ime process key
        // 230 oem specific
        // 231 strange key
        // 232 unassigned
        // 233 oem specific
        // 234 oem specific
        // 235 oem specific
        // 236 oem specific
        // 237 oem specific
        // 238 oem specific
        // 239 oem specific
        // 240 oem specific
        // 241 oem specific
        // 242 oem specific
        // 243 oem specific
        // 244 oem specific
        // 245 oem specific
        //Attn = 246,
        //CrSel = 247,
        //ExSel = 248,
        //EraseEOF = 249,
        //PlayKey = 250,
        //ZoomKey = 251,
        // 252 reserved
        //PA1Key = 253,
        //OEMClearKey = 254
    }

    public static class KeyboardFuncs
    {
        public static bool IsKeyPressed(KeysEnum button)
        {
            short returnCode = GetKeyState((int)button);

            bool isPressed = BitHelper.IsBitOn(BitConverter.GetBytes(returnCode), 15);

            return isPressed;
        }

        public static bool IsKeyToggled(KeysEnum button)
        {
            short returnCode = GetKeyState((int)button);

            bool isToggled = BitHelper.IsBitOn(BitConverter.GetBytes(returnCode), 0);

            return isToggled;
        }

        public static bool IsKeyPressedOrToggled(KeysEnum button)
        {
            short returnCode = GetKeyState((int)button);

            bool isPressed = BitHelper.IsBitOn(BitConverter.GetBytes(returnCode), 15);
            bool isToggled = BitHelper.IsBitOn(BitConverter.GetBytes(returnCode), 0);

            return isPressed || isToggled;
        }

        public static bool HasKeyChanged(KeysEnum button)
        {
            short returnCode = GetAsyncKeyState((int)button);

            bool isPressed = BitHelper.IsBitOn(BitConverter.GetBytes(returnCode), 15);
            bool wasPressed = BitHelper.IsBitOn(BitConverter.GetBytes(returnCode), 0);

            return isPressed || wasPressed;
        }

        public static bool HasAnyKeyChanged()
        {
            IEnumerable<KeysEnum> array = Enum.GetValues(typeof(KeysEnum)).Cast<KeysEnum>();

            foreach(KeysEnum key in array)
            {
                if (HasKeyChanged(key)) return true;
            }

            return false;
        }
    }
}
