using System.IO;
using System;

namespace Statistics
{
	public class FinancialArray
	{
		public int[] data;

		public int max;
		public int min;
		public int avg;

		public FinancialArray(int days_count)
		{
			data = new int[days_count];
		}

		public void CalcStat()
		{
			max = data[0];
			min = data[0];
			int sum = data[0];
			for (int i = 1; i < data.Length; i++)
			{
				if(min > data[i])
				{
					min = data[i];
				}
				if (max < data[i])
				{
					max = data[i];
				}
				sum += data[i];
			}
			avg = sum / data.Length;
		}
	}

	enum AlignType { Left, Right, center }

	struct TableCell
	{
		string data;
		AlignType alignType;
	}

	struct TableRaw
	{
		TableCell[] cells;
	}

	class TableWriter 
	{
		TableRaw[] rows;

		public void WriteToFile(string filename)
		{
			StreamWriter outFile = File.CreateText(filename);

			// ... implement here

			outFile.Close();
		}        
	}

	class MainClass
	{
		public static void Main (string[] args)
		{
			string myData = System.IO.File.OpenText ("raw_data.txt").ReadToEnd ().Trim ();

			string numbers_1 = myData.Replace("\r\n", ";");
			string[] numbers = myData.Replace("\r\n", ";").Split(' ', ';', '\n');

			int days_count = numbers.Length / 2;
			FinancialArray debit = new FinancialArray(days_count);
			FinancialArray credit = new FinancialArray(days_count);

			for (int i = 0; i < days_count; i++)
			{
				debit.data[i] = Convert.ToInt32(numbers[i * 2]);
				credit.data[i] = Convert.ToInt32(numbers[i * 2 + 1]);
				Console.WriteLine("{0}\t{1}", debit.data[i], credit.data[i]);
			}

			debit.CalcStat();
			credit.CalcStat();

			TableWriter tab1 = new TableWriter();

			// ... implement here

			tab1.WriteToFile("stat.txt");


			TableWriter tab2 = new TableWriter();

			// ... implement here

			tab2.WriteToFile("report.txt");
		}
	}
}
