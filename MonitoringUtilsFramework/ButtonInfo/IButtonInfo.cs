using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringUtils.ButtonInfo
{
    public interface IButtonInfo
    {
        bool IsPressed { get; set; }
        bool WasPressed { get; set; }
    }
}
