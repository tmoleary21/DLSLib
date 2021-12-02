using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Dymo;

namespace DLSLib
{
	public class LabelWriter
	{
		private DymoAddIn PrinterSoftware;
		private DymoLabels LabelEditor;

		private int side; //Used to decide on the side to print on in a Twin Turbo if alternating

		public LabelWriter()
		{
			PrinterSoftware = new DymoAddIn();
			LabelEditor = new DymoLabels();
			side = 1;
		}

		public void ListPrinters()
		{
			//Console.WriteLine(PrinterSoftware.GetDymoPrinters());
			Debug.WriteLine(PrinterSoftware.GetDymoPrinters());
		}

		public string[] GetPrinters()
        {
			return PrinterSoftware.GetDymoPrinters().Split('|');
		}

		public bool PrinterExists()
		{
			return PrinterSoftware.GetDymoPrinters().Length > 0;
		}

		public bool PrinterExists(string printerName)
		{
			return GetPrinters().Contains(printerName);
		}



		/** <summary>Method to send label to print. Meant for use with any type of printer.</summary>
		 *	<remarks>If <c>"DEFAULT"</c> is passed in for labelPath, will use relative address to current_label.label</remarks>
		 *	<param name="printerName">A valid name of a Dymo Label Writer connected to the computer. <see cref="ListPrinters"/></param>
		 *	<param name="labelPath">A valid path to a .label, .lwl, or .lwt label file.</param>
		 *	<param name="copies">The number of copies of the label to be printed.</param>
		 */
		public void SendToPrinter(string printerName, string labelPath, int copies, bool alternateSides=true)
		{
			if (!openLabel(labelPath))
			{   //labelPath should be a valid path to a .label, .lwl, or .lwt file (Extension not necessary)
				throw new ArgumentException("Label Not Found at path: " + labelPath + "\n(Or label file is somehow damaged)");
			}
			if (!PrinterSoftware.IsPrinterOnline(printerName))
			{	
				Debug.WriteLine("Printer Offline");
			}
			if (!PrinterSoftware.SelectPrinter(printerName))
			{	// printerName should be a valid printer name. Names of connected dymo label writers can be printed with ListPrinters()
				throw new ArgumentException("Printer \"" + printerName + "\" not found");
			}
			bool printSuccess;
			if (PrinterSoftware.IsTwinTurboPrinter(printerName))
			{ //Twin Turbo Scenario. Not all the time for the sake of general use
                if (alternateSides) //Default option based on parameter
                {
					printSuccess = PrinterSoftware.Print2(copies, false, side); //Swaps side every time. May allow for faster printing
					side = (side == 0) ? 1 : 0;
				}
                else
                {
					printSuccess = PrinterSoftware.Print2(copies, false, 2); //Prints to one side until out of labels. Then automatically switches to other side
				}
			}
            else
            {
				printSuccess = PrinterSoftware.Print(copies, false);
            }
			if (!printSuccess) Debug.WriteLine("Print Failed");
		}

		// Simplified printing method only to be used within this class
		// Assumes label already loaded
		private void SendToPrinter(string printerName, int copies, bool alternateSides=true)
        {
			if (!PrinterSoftware.SelectPrinter(printerName))
			{   // printerName should be a valid printer name. Names of connected dymo label writers can be printed with ListPrinters()
				throw new ArgumentException("Printer \"" + printerName + "\" not found");
			}
			bool printSuccess;
			if (PrinterSoftware.IsTwinTurboPrinter(printerName))
            {
				if (alternateSides) //Default option
				{
					printSuccess = PrinterSoftware.Print2(copies, false, side); //Swaps side every time. May allow for faster printing
					side = (side == 0) ? 1 : 0;
				}
				else
				{
					printSuccess = PrinterSoftware.Print2(copies, false, 2); //Prints to one side until out of labels. Then automatically switches to other side
				}
			}
            else
            {
				printSuccess = PrinterSoftware.Print(copies, false);
            }
			if (!printSuccess) Debug.WriteLine("Print Failed");
        }

		public void ChangeLabelText(string newText, string labelPath, bool saveLabel)
		{
			if(!openLabel(labelPath))
            {
				//throw new ArgumentException("Label Not Found at path: " + labelPath + "\n(Or label file is somehow damaged)");
            }
			string a = LabelEditor.GetObjectNames(false);
			if(!LabelEditor.SetField("TEXT", newText))
            {
				Debug.WriteLine("Failed to set field");
            }
			if(saveLabel) PrinterSoftware.Save(); //When used for changing name and sending straight to printer, no need to Save()
		}

		public void PrintStudentNameTag(string printerName, string labelPath, string studentName)
        {
			//Change text without saving because assumed that label will be changed for other students anyway
			ChangeLabelText(studentName, labelPath, false); 
			SendToPrinter(printerName, 1);
        }

		private bool openLabel(string labelPath)
        {
			if(labelPath == "DEFAULT")
            {
				return PrinterSoftware.Open2("current_label.label");
            }
			return PrinterSoftware.Open2(labelPath);
        }
	}
}
