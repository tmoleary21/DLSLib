using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLSLib;

namespace DLSLibTester
{
	class Tester
	{
		static void Main(string[] args)
		{
			if(args.Length > 1)
            {
				run(args[0] + " " + args[1]); //args[1]: first name. args[2]: last name.
            }
            else
            {	//testing code
				const string LabelPath = "C:\\Users\\tmoleary\\source\\repos\\DLSLib\\NameBadge-RamWelcome.label";
				LabelWriter Printer = new LabelWriter();
				string PrinterName = Printer.GetPrinters()[0];
				Printer.ListPrinters();
				Console.WriteLine(Printer.PrinterExists("DYMO LabelWriter 450 Twin Turbo"));
				Printer.SendToPrinter(PrinterName, LabelPath, 1);
				Printer.PrintStudentNameTag(PrinterName, LabelPath, "Tyson O'Leary");
            }
		}

		static void run(string studentName)
        {
            LabelWriter printer = new LabelWriter();
			printer.PrintStudentNameTag(printer.GetPrinters()[0], "C:\\Temp\\current_label.label", studentName);
        }
	}
}
