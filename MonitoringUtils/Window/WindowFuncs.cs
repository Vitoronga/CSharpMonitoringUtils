using Vanara.PInvoke;
using static Vanara.PInvoke.User32;

namespace MonitoringUtils.Window
{
    public class WindowFuncs
    {
        public static bool MinimizeWindow(HWND handle) => CloseWindow(handle);
        public static bool MinimizeWindow(WindowInfo wInfo) => MinimizeWindow(wInfo.Handle);

        public static WindowInfo GetForegroundWindow() => new WindowInfo(User32.GetForegroundWindow());

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

        
        
        // Get All Windows Stuff

        private static TaskCompletionSource windowsEnumerated;

        private static List<WindowInfo> _windows_buffer_;

        public static async Task<List<WindowInfo>> GetAllWindows(bool getHiddenWindows = false)
        {
            // Preparar espera ao task completion source (mix loko de task com evento)
            _windows_buffer_ = new List<WindowInfo>();
            windowsEnumerated = new TaskCompletionSource();

            if (getHiddenWindows) EnumWindows(new EnumWindowsProc(EnumWindowsProcHandler), default);
            else EnumChildWindows(GetDesktopWindow(), new EnumWindowsProc(EnumWindowsProcHandler), default); // Doesn't quite work (for now)

            // Esperar término dos callbacks
            await windowsEnumerated.Task;

            return _windows_buffer_;
        }

        // É tipo um delegate a ser chamado pelo evento de enumeração de chamadas! (do SO provavelmente)
        private static bool EnumWindowsProcHandler(HWND hwnd, IntPtr lParam)
        {
            if (windowsEnumerated == null) throw new InvalidOperationException("Task completion source isn't instantiated.");

            WindowInfo wInfo = new WindowInfo(hwnd);

            // FILTRANDO JANELA
            if (!wInfo.IsEnabledForInput || (!wInfo.IsVisible && !wInfo.IsMinimized) || string.IsNullOrWhiteSpace(wInfo.MainModulePath)) return true;

            _windows_buffer_.Add(wInfo);

            // Detecta se é a última janela (a mais atrás de todas)
            bool isLast = hwnd == GetWindow(User32.GetForegroundWindow(), GetWindowCmd.GW_HWNDLAST); // Foreground window because whatever...

            if (isLast && !windowsEnumerated.TrySetResult() &&
                !windowsEnumerated.TrySetException(new InvalidOperationException("Failed to set task completion source value")))
                Console.WriteLine("FAILED ENUMERATING WINDOWS AT: " + wInfo.GetReadableDescription());

            // O valor de retorno serve para interromper (se for false) a enumeração das janelas (aparentemente, de alguma forma misteriosa)
            // Retorna true pra dll chamar novamente (caso tenha mais janelas pra mostrar)
            return true;
        }
    }
}
