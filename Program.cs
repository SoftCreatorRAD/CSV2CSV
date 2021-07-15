using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;
using System.Text;

namespace csv2csv
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("CSV2CSV: Transforms CSV to CSV with field, wrapped in double-quotes.");
			Console.WriteLine("--------------------------------------------------------------------");


			if (args.Length < 1)
			{
				Console.WriteLine("Insufficient number of arguments.");
				Console.WriteLine("Expected at least full path to source CSV.");
				Console.WriteLine("Ex.: csv2csv.exe  c:\\temp\\temp.csv");
				Console.WriteLine("Or full path to source CSV and target CSV");
				Console.WriteLine("Ex.: csv2csv.exe  c:\\temp\\temp.csv c:\\temp\\temp2.csv");
				return;
			}

			var csvSrc = args[0];
			var csvTrg = (args.Length >= 2) ? args[1] : string.Empty;
			var trgRow = new StringBuilder();
			var trgRows = new StringBuilder();

			if (!File.Exists(csvSrc))
			{
				Console.WriteLine("Source CSV does not exist.");
				Console.WriteLine(csvSrc);
				return;
			}

			var srcFileName = Path.GetFileNameWithoutExtension(csvSrc);
			var trgFileName = Path.GetFileNameWithoutExtension(csvTrg);
			if (csvTrg == string.Empty)
			{
				trgFileName = srcFileName + "_result";
				csvTrg = Path.Combine(Path.GetDirectoryName(csvSrc), trgFileName + ".csv");
			}


			Console.WriteLine("Source: " + csvSrc);
			Console.WriteLine("Target: " + csvTrg);

			//Console.WriteLine(srcFileName);
			//Console.WriteLine(trgFileName);

			using (TextFieldParser parser = new TextFieldParser(csvSrc))
			{
				int recCounter = 0;
				parser.TextFieldType = FieldType.Delimited;
				parser.SetDelimiters(",");
				while (!parser.EndOfData)
				{
					//Processing row
					trgRow.Clear();
					string[] fields = parser.ReadFields();
					foreach (string field in fields)
					{
						trgRow.Append(((trgRow.Length > 0) ? "," : "") + "\"" + field.Replace("\"", "") + "\"");
					}
					trgRows.AppendLine(trgRow.ToString());

					recCounter++;
					if (recCounter > 100000) // Buffer size
					{
						File.AppendAllText(csvTrg, trgRows.ToString());
						trgRows.Clear();
						recCounter = 0;
					}
				}
			}
			File.AppendAllText(csvTrg, trgRows.ToString());
		}

	}
}
