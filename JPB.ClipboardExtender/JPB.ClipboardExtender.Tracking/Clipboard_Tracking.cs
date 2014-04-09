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
using System.IO;
using System.Runtime.ConstrainedExecution;

namespace ClipbordTrackingDll
{
    /// <summary>
    /// Set a Instance of the Object and give him the handel of the Window
    /// Than Add too new Events like:
    ///  test.NewFormartetContent += new Clipbord_TrackingDll.NewformatetContent(New_formatet_Content_Available);
    ///  test.NewUnFormartetContent += new Clipbord_TrackingDll.NewUnformatetContent(New_Unformatet_Content_Available);
    /// 
    /// Than is it importent to override the WndProc method and add and rewrite
    ///     if (test != null)
    ///         test.NewMessage(ref m);
    ///     base.WndProc(ref m);
    /// </summary>
    public class ClipbordTracking : CriticalFinalizerObject, IDisposable
    {
        ////Example For using Filter!
        //
        ////Create a New Formart
        //
        //MainClass.Set_New_Filter("My Individual Filter Name");
        //
        ////Create a New Data Object and Fill it with something from a Object or else
        //
        //DataObject NewdataObj = new DataObject(MainClass.Get_Last_Filter().Name, TestClassForUsingFilter.MyObjectValue);
        //
        ////Set it to the Clipboard
        //
        //Clipboard.SetDataObject(NewdataObj);
        //
        ////
        //// Do Something else or jump out of the Funktion
        ////
        //// If there an Objekt witch got a Filter name or ID you createt with this Funktion Set_New_Filter() 
        //// The Programm will raise the event New_formatet_Content_Available and fill the Args with the Object 
        //// that has the Same Filter name like an Filter
        ////
        /// <summary>
        /// Construktor for the Clipboard_TrackingDll
        /// </summary>
        /// <param name="handel">The Handel of an Windows Window</param>
        public ClipbordTracking(IntPtr handel)
        {
            AddClipboardFormatListener(_handle = handel);
            NewFormartetContent += new NewformatetContent(New_formatet_Content_Available);
            NewUnFormartetContent += new NewUnformatetContent(New_Unformatet_Content_Available);
        }

        private readonly IntPtr _handle;

        #region User32.Dll Imports
        //[DllImport("user32.dll")]
        //static extern IntPtr GetClipboardViewer();

        //[DllImport("User32.dll", CharSet = CharSet.Auto)]
        //public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        /// <summary>
        /// Places the given window in the system-maintained clipboard format listener list.
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AddClipboardFormatListener(IntPtr hwnd);

        /// <summary>
        /// Removes the given window from the system-maintained clipboard format listener list.
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        #endregion

        #region Events

        public delegate void NewformatetContent(object source, MyFormatetArgs e);

        public static event NewformatetContent NewFormartetContent;

        private void RaiseNewFormartedContentEvent(MyFormatetArgs eventargs)
        {
            if (NewFormartetContent != null)
            {
                NewFormartetContent(this, eventargs);
            }
        }

        public delegate void NewUnformatetContent(object source, MyUnFormatetArgs e);

        public static event NewUnformatetContent NewUnFormartetContent;

        private void RaiseNewUnFormartedContentEvent(MyUnFormatetArgs eventargs)
        {
            if (NewUnFormartetContent != null)
            {
                NewUnFormartetContent(this, eventargs);
            }
        }

        #endregion

        void Set_New_Content_and_start_event()
        {
            SaveClass.Instance.GDGesamter_Clipboard_Content.Add(Clipboard.GetDataObject());
            List<Object> ObjecetsFromFilter = new List<Object>();
            foreach (DataFormats.Format item in SaveClass.Instance.GDFormarte)
            {
                if (Clipboard.GetDataObject().GetData(item.Name) != null)
                {
                    ObjecetsFromFilter.Add(Clipboard.GetDataObject().GetData(item.Name));
                }
            }
            if (ObjecetsFromFilter.Count != 0)
            {
                RaiseNewFormartedContentEvent(new MyFormatetArgs(ObjecetsFromFilter));
            }
            else
            {
                RaiseNewUnFormartedContentEvent(new MyUnFormatetArgs(Clipboard.GetDataObject()));
            }
        }

        public void NewMessage(ref Message m)
        {
            switch ((Msgs)m.Msg)
            {
                case Msgs.WM_CLIPBOARDUPDATE:
                    if (NewUnFormartetContent != null)
                    {
                        Set_New_Content_and_start_event();                        
                    }

                    break;

                default:
                    // unhandled window message
                    break;
            }
        }
        /// <summary>
        /// Rise if any Unfiltert content was loadet into the Clipboard
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void New_Unformatet_Content_Available(object source, MyUnFormatetArgs e)
        {

        }
        /// <summary>
        /// Event to get lists of Formartet Content
        /// Use Set Filter to Create new Filters
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void New_formatet_Content_Available(object source, MyFormatetArgs e)
        {

        }

        private bool _isDisposing;

        public void Dispose()
        {
            if (!_isDisposing)
            {
                _isDisposing = true;
                RemoveClipboardFormatListener(_handle);
            }
        }

        ~ClipbordTracking()
        {
            Dispose();
        }
    }

    public static class MainClass
    {
        /// <summary>
        /// Get The last Content
        /// </summary>
        /// <returns>Object with the last content was Saved</returns>
        /// 
        public static Object GetLastData()
        {
            return SaveClass.Instance.ObjNewestContent;
        }

        public static DataFormats.Format GetLastFilter()
        {
            return SaveClass.Instance.GDFormarte.Last();
        }

        public static void SetNewFilter(int Filter_Id)
        {
            DataFormats.Format NewFormart = DataFormats.GetFormat(Filter_Id);
            SaveClass.Instance.GDFormarte.Add(NewFormart);
        }

        public static void SetNewFilter(string Filter_Name)
        {
            DataFormats.Format NewFormart = DataFormats.GetFormat(Filter_Name);
            SaveClass.Instance.GDFormarte.Add(NewFormart);
        }

        static public IEnumerable<IDataObject> GetDataByFilter()
        {
            foreach (DataFormats.Format Formarte in SaveClass.Instance.GDFormarte)
            {
                foreach (var Contents in SaveClass.Instance.GDGesamter_Clipboard_Content)
                {
                    for (int i = 0; i < ((IDataObject)Contents).GetFormats().Length; i++)
                    {
                        if (Formarte.Name == ((IDataObject)Contents).GetFormats()[i])
                        {
                            yield return Contents;
                        }
                    }
                }
            }
        }
    }
}
