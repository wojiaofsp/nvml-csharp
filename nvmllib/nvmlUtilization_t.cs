using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace nvmllib.vs2015
{
    [StructLayout(LayoutKind.Sequential)]
    public struct nvmlUtilization_t
    {
        public uint gpu { get; set; }
        public uint memory { get; set; }

    }
}
