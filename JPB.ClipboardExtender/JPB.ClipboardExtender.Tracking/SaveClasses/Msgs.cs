using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClipbordTrackingDll
{
    public enum Msgs
    {
        WM_DRAWCLIPBOARD = 0x0308,
        WM_CLIPBOARDUPDATE = 0x031D,
        WM_CHANGECBCHAIN = 0x030D
    }
}
