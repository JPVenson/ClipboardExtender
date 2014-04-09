using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace ClipbordTrackingDll
{
    public class MyUnFormatetArgs : EventArgs
    {
        private IDataObject strEventInfo;
        public MyUnFormatetArgs(IDataObject idobj)
        {
            strEventInfo = idobj;
        }

        public MyUnFormatetArgs()
        {
            // TODO: Complete member initialization
        }

        public IDataObject GetInfo()
        {
            return strEventInfo;
        }
    }
    public class MyFormatetArgs : EventArgs
    {
        private List<Object> strEventInfo;
        public MyFormatetArgs(List<Object> idobj)
        {
            strEventInfo = idobj;
        }

        public MyFormatetArgs()
        {
            // TODO: Complete member initialization
        }

        public List<Object> GetInfo()
        {
            return strEventInfo;
        }
    }
}
