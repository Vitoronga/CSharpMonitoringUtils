using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringUtils.ButtonInfo
{
    public interface IButtonInfo
    {
        public bool IsPressed { get; init; }
        public bool WasPressed { get; init; }
    }
}
