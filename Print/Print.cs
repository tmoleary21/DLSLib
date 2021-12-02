using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLSLib;

namespace Print
{
	class Print
	{
		static string logfile = @"C:\LabelPrint\logs\" + DateTime.Today.Year.ToString() + "_" + DateTime.Today.Month.ToString() + "_" + DateTime.Today.Day.ToString() + "_" + DateTime.Today.Hour + "_" + DateTime.Today.Minute + "_" + DateTime.Today.Second +  ".txt";
		static void Main(string[] args)
		{
			if (args.Length == 2)
			{
				run(args[0] + " " + args[1]); //args[1]: first name. args[2]: last name.
			}
			else
			{
				Console.WriteLine("Command should be in the form:");
				Console.WriteLine("Print.exe First Last");
				using (StreamWriter swlog = File.CreateText(logfile))
				{
					swlog.WriteLine("Command should be in the form:");
					swlog.WriteLine("Print.exe First Last");
				}
			}
		}

		static void run(string studentName)
		{
			LabelWriter printer = new LabelWriter();
			using (StreamWriter swlog = File.CreateText(logfile))
			{
				swlog.WriteLine("Available Label Writer Printers: " + string.Join(",",printer.GetPrinters()));
				swlog.WriteLine("Printing label with text: " + studentName);
				swlog.WriteLine("With Label Template at: C:\\Temp\\current_label.label");
				swlog.WriteLine("To Printer: " + printer.GetPrinters()[0]);
			}
			printer.PrintStudentNameTag(printer.GetPrinters()[0], "C:\\Temp\\current_label.label", studentName);
		}
	}
}
