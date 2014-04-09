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
    public partial class ClipboardTracking : UserControl
    {



        private void button1_Click(object sender, EventArgs e)
        {
            //Example For using Filter!

            //Create a New Formart

            MainClass.Set_New_Filter("My Individual Filter Name");

            //Create a New Data Object and Fill it with something from a Object or else

            DataObject NewdataObj = new DataObject(MainClass.Get_Last_Filter().Name, TestClassForUsingFilter.MyObjectValue);

            //Set it to the Clipboard

            Clipboard.SetDataObject(NewdataObj);

            //
            // Do Something else or jump out of the Funktion
            //
            // If there an Objekt witch got a Filter name or ID you createt with this Funktion Set_New_Filter() 
            // The Programm will raise the event New_formatet_Content_Available and fill the Args with the Object 
            // that has the Same Filter name like an Filter
            //
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(TestClassForUsingFilter.MyObjectValue);
        }
    }
    [Serializable]
    static public class TestClassForUsingFilter
    {
        static private string myValue = "This is the value of the class";

        // Creates a default constructor for the class. 
        // Creates a property to retrieve or set the value. 
        static public string MyObjectValue
        {
            get
            {
                return myValue;
            }
            set
            {
                myValue = value;
            }
        }
    }

}
