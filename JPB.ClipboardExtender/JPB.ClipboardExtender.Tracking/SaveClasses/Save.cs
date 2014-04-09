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
    class SaveClass
    {
        private SaveClass()
        { 
        
        }

        static SaveClass _instance = new SaveClass();

        public static SaveClass Instance
        {
            get { return _instance; }
        }

        private List<IDataObject> _GDGesamter_Clipboard_Content = new List<IDataObject>();

        public List<IDataObject> GDGesamter_Clipboard_Content
        {
            get { return _GDGesamter_Clipboard_Content; }
            set { _GDGesamter_Clipboard_Content = value; }
        }


        private List<DataFormats.Format> _GDFormarte = new List<DataFormats.Format>();
        private IDataObject _objNewestContent;

        public IDataObject ObjNewestContent
        {
            get { return _objNewestContent; }
            set { _objNewestContent = value; }
        }
        /// <summary>
        /// All saved Formats
        /// </summary>
        public List<DataFormats.Format> GDFormarte
        {
            get { return _GDFormarte; }
            set { _GDFormarte = value; }
        }
    }
}
