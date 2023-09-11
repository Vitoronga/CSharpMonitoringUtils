using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text;
using Vanara.PInvoke;
using static Vanara.PInvoke.User32;

namespace MonitoringUtils.Window
{
    public class WindowFuncs
    {
        /// <summary>
        /// Minimizes a window.
        /// </summary>
        /// <param name="wInfo">The desired window's WindowInfo object</param>
        /// <returns></returns>
        public static bool MinimizeWindow(WindowInfo wInfo) => CloseWindow(wInfo.Handle);

        /// <summary>
        /// Returns the foremost window - the one in front of all others
        /// </summary>
        /// <remarks>
        /// <para>
        /// The foreground window can be <c>NULL</c> in certain cases, such as
		/// when a window is losing activation. 
        /// </para>
        /// </remarks>
        /// <returns></returns>
        public static WindowInfo GetForegroundWindow() => new WindowInfo(User32.GetForegroundWindow());

        /// <summary>
        /// Tries to get the foremost window in an exception free manner.
        /// </summary>
        /// <param name="wInfo">The returning WindowInfo object</param>
        /// <returns>A boolean representing the success in the retrieval</returns>
        public static bool TryGetForegroundWindow(out WindowInfo? wInfo)
        {
            wInfo = null;

            try
            {
                wInfo = GetForegroundWindow();
            }
            catch (InvalidOperationException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                // Unexpected exception
                return false;
            }

            return true;
        }

        public static async Task<string> SendMessageWindowGetText(WindowInfo windowInfo)
        {
            StringBuilder builder = new StringBuilder(256);
            await (Task.Run(() => SendMessage(windowInfo.Handle, WindowMessage.WM_GETTEXT, builder.Capacity, builder)));
            return builder.ToString();
        }



        // Get All Windows Stuff

        private static TaskCompletionSource windowsEnumerated;

        private static List<WindowInfo> _windowsBuffer_;
        private static bool _getOnlyMainWindow_ = true;
        private static bool _getHiddenWindows_ = false;
        //private static HWND _lastWindowHandle_; // Idea for optimizing checks

        /// <summary>
        /// Returns all existing windows asynchronously
        /// </summary>
        /// <param name="getOnlyMainWindow">Ignore secondary windows?</param>
        /// <param name="getHiddenWindows">Include hidden windows? (windows that doesn't show to the user and can't receive inputs)</param>
        /// <returns></returns>
        public static async Task<List<WindowInfo>> GetAllWindows(bool getOnlyMainWindow = true, bool getHiddenWindows = false)
        {
            // Preparar espera ao task completion source (mix loko de task com evento)
            _windowsBuffer_ = new List<WindowInfo>();
            _getOnlyMainWindow_ = getOnlyMainWindow;
            _getHiddenWindows_ = getHiddenWindows;
            windowsEnumerated = new TaskCompletionSource();

            if (getHiddenWindows) EnumWindows(new EnumWindowsProc(EnumWindowsProcHandler), default);
            else EnumChildWindows(GetDesktopWindow(), new EnumWindowsProc(EnumWindowsProcHandler), default); // Doesn't quite work (for now)

            // Esperar término dos callbacks
            await windowsEnumerated.Task;

            return _windowsBuffer_;
        }

        // É tipo um delegate a ser chamado pelo evento de enumeração de chamadas! (do SO provavelmente)
        private static bool EnumWindowsProcHandler(HWND hwnd, IntPtr lParam)
        {
            if (windowsEnumerated == null) throw new InvalidOperationException("Task completion source isn't instantiated.");

            WindowInfo wInfo = new WindowInfo(hwnd);

            // FILTRANDO JANELA:

            // Janelas ocultas:
            bool isHidden = !wInfo.IsEnabledForInput || string.IsNullOrWhiteSpace(wInfo.MainModulePath);
            bool passesHiddenCheck = _getHiddenWindows_ || (!_getHiddenWindows_ && !isHidden);

            // Janelas principais:
            Process p = Process.GetProcessById(wInfo.ProcessId);
            bool isMainWindow = p.MainWindowHandle == wInfo.Handle;
            bool passesMainWindowCheck = !_getOnlyMainWindow_ || (_getOnlyMainWindow_ && isMainWindow);

            // Detecta se é a última janela (a mais atrás de todas)
            bool isLast = hwnd == GetWindow(User32.GetForegroundWindow(), GetWindowCmd.GW_HWNDLAST); // Foreground window because whatever...

            // Realiza filtro:
            if (!(passesHiddenCheck && passesMainWindowCheck)) {
                if (!isLast) return true;
            }
            else _windowsBuffer_.Add(wInfo);

            if (isLast && !windowsEnumerated.TrySetResult() &&
                !windowsEnumerated.TrySetException(new InvalidOperationException("Failed to set task completion source value")))
                Console.WriteLine("FAILED ENUMERATING WINDOWS AT: " + wInfo.GetReadableDescription());

            // O valor de retorno serve para interromper (se for false) a enumeração das janelas (aparentemente, de alguma forma misteriosa)
            // Retorna true pra dll chamar novamente (caso tenha mais janelas pra mostrar)
            return true;
        }





        // Get All Child Windows Stuff

        private static TaskCompletionSource childWindowsEnumerated;

        private static List<WindowInfo> _childWindowsBuffer_;
        private static HWND? _lastChildWindowHandle_;

        public static async Task<List<WindowInfo>> GetAllChildWindows(WindowInfo parentWindow)
        {
            // Preparar espera ao task completion source
            _childWindowsBuffer_ = new List<WindowInfo>();
            childWindowsEnumerated = new TaskCompletionSource();

            EnumChildWindows(parentWindow.Handle, new EnumWindowsProc(EnumChildWindowsProcHandler), default); // Doesn't quite work (for now)

            // Esperar término dos callbacks
            await childWindowsEnumerated.Task;

            _lastChildWindowHandle_ = null; // critical

            return _childWindowsBuffer_;
        }

        private static bool EnumChildWindowsProcHandler(HWND hwnd, IntPtr lParam)
        {
            if (childWindowsEnumerated == null) throw new InvalidOperationException("Task completion source isn't instantiated.");

            WindowInfo wInfo = new WindowInfo(hwnd);

            if (_lastChildWindowHandle_ == null) _lastChildWindowHandle_ = GetWindow(wInfo.Handle, GetWindowCmd.GW_HWNDLAST);

            // Detecta se é a última janela (a mais atrás de todas)
            bool isLast = hwnd == _lastChildWindowHandle_;

            _childWindowsBuffer_.Add(wInfo);

            if (isLast && !childWindowsEnumerated.TrySetResult() &&
                !childWindowsEnumerated.TrySetException(new InvalidOperationException("Failed to set task completion source value")))
                Console.WriteLine("FAILED ENUMERATING WINDOWS AT: " + wInfo.GetReadableDescription());

            // O valor de retorno serve para interromper (se for false) a enumeração das janelas (aparentemente, de alguma forma misteriosa)
            // Retorna true pra dll chamar novamente (caso tenha mais janelas pra mostrar)
            return true;
        }
    }
}
