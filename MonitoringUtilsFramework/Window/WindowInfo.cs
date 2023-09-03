using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Vanara.PInvoke;
using static Vanara.PInvoke.User32;

namespace MonitoringUtils.Window
{
    public class WindowInfo
    {
        private WINDOWINFO windowInfo;
        public HWND Handle { get; private set; }
        public Process MainProcess { get; private set; }

        public WindowInfo(HWND safeHwnd)
        {
            InstantiateWindowInfo(safeHwnd);
            //SwitchToThisWindow
            //SetForegroundWindow
            //SetFocus
            //ScrollWindow
            //RegisterHotKey
            //OpenIcon
            //MoveWindow
            //LockSetForegroundWindow
            //IsWindowEnabled
            //IsWindow
            //IsIconic
            //IsChild
            //GetWindowThreadProcessId
            //GetWindowRect // or obtain it directly from WINDOWINFO
            //GetWindowPlacement

            //WINDOWPLACEMENT // Conselhos importantes sobre usar a função certa para posicionar a janela

            // Window control: normal window, with title bar and content
            // Window edit control: dialog-box-like rectangle window for input operations.

            //GetWindowModuleFileName // PARECE MUITO BOM
            //GetWindow // Talvez seja útil para obter qual janela está à frente (z order)
            //GetUserObjectInformation // ??
            //GetTopWindow // Obtém a janela mais à frente entre as janelas associadas com uma janela mãe
            //GetTitleBarInfo // Talvez n seja útil, precisa do handle da própria barra
            //GetNextWindow // Obter próxima janela de acordo com o enum de cmd lá (ácima, abaixo, primeira, última, filha, etc.)

            //GetProp // property?

            // Keyboard
            //GetKeyState
            //GetKeyboardType // ?
            //GetKeyboardState // overkill mas pode ser bom pra uma verificação geral direta
            //GetKeyboardLayout // ABNT por ex (eu acho) (só o layout ativo no momento)
            //GetKeyboardLayoutList // Obter todos os layouts disponíveis (ativo e os inativos)


            //GetForegroundWindow
            //GetFocus // ?
            //GetDesktopWindow // a pseudo janela chamada desktop?


            //GetCursorPos // pode ser bueno
            //GetCursorInfo
            //GetClassInfoEx // window class handle + icon handle (?)
            //GetAncestor // Get direct parent to the specified window
            //GetActiveWindow // Deve ser útil?
            //FindWindow // procura janela com class name igual ao argumento (não procura janelas-filhas)
            //EnumWindows // parece bom e mt complicado ao msm tempo
            //EnumDesktopWindows
            //EnumDesktops
            //EnumChildWindows
            //EndTask // ??? parece perigoso

            //EnableWindow // parece ruim usar por si só, acho que usar o foreground faz mais sentido (ou os 2+ em conjunto?)
            //DragDetect // específico, provavelmente n será útil (mas é interessante)
            //CloseWindow // MINIMIZES a window
            //CascadeWindows // seems cool but useless
            //BringWindowToTop // probably will be useful
            //BlockInput // wow
            //AnyPopup // read the desc, it differentiates between general screen and application area
            //AllowSetForegroundWindow // hmm... ... why?

        }
        public WindowInfo(IntPtr handle) : this(new HWND(handle)) { }

        unsafe private void InstantiateWindowInfo(HWND hwnd)
        {
            Handle = hwnd;

            WINDOWINFO wInfo = new WINDOWINFO();
            wInfo.cbSize = Convert.ToUInt32(sizeof(WINDOWINFO));

            if (!GetWindowInfo(Handle, ref wInfo)) throw new ArgumentException("Couldn't get window info from provided handle (hwnd)", "safeHwnd");

            windowInfo = wInfo;

            MainProcess = Process.GetProcessById((int)GetProcessId());
        }



        // PROPERTIES



        public bool IsVisible => IsWindowVisible(Handle);
        public bool IsMinimized => IsIconic(Handle);
        public bool IsMaximized => IsZoomed(Handle);
        public bool IsEnabledForInput => IsWindowEnabled(Handle);
        public uint ThreadId => GetWindowThreadProcessId(Handle, out uint _);
        public int ProcessId => MainProcess.Id;
        public string ProcessName => MainProcess.ProcessName;
        public string MainModuleName => MainProcess.MainModule.ModuleName;
        public string MainModulePath => MainProcess.MainModule.FileName;
        public string TitleText => GetWindowTitleText();



        // DATA GETTERS



        //public string GetFileName()
        //{
        //    StringBuilder builder = new StringBuilder(256);

        //    GetWindowModuleFileName(Handle, builder, Convert.ToUInt32(builder.Capacity));

        //    return builder.ToString();
        //}





        // PRIVATE GETTERS

        private string GetWindowTitleText()
        {
            StringBuilder builder = new StringBuilder(256);

            GetWindowText(Handle, builder, builder.Capacity);

            return builder.ToString();
        }

        private uint GetProcessId()
        {
            uint processId;

            GetWindowThreadProcessId(Handle, out processId);

            return processId;
        }



        // OTHER



        public string GetReadableDescription()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Window title text: {TitleText}");
            sb.AppendLine($"Origin file name: {MainModuleName}");
            sb.AppendLine($"Origin file path: {MainModulePath}");
            sb.AppendLine($"Thread Id: {ThreadId}");
            sb.AppendLine($"Process Name: {ProcessName}");
            sb.AppendLine($"Process Id: {ProcessId}");
            sb.AppendLine($"Is visible: {IsVisible}");
            sb.AppendLine($"Is minimized: {IsMinimized}");
            sb.AppendLine($"Is maximized: {IsMaximized}");
            sb.AppendLine($"Can receive input: {IsEnabledForInput}");

            return sb.ToString();
        }

        //~WindowInfo() {
        //    Handle.Dispose(); // When destroying the object call this I think
        //}
    }
}
