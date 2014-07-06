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
            int sum = data[0];
			max = data[0];
			min = data[0];
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

	enum AlignType { 
        Left, 
        Right, 
        Center 
    }

    struct TableCell
    {
        string data;
        AlignType alignType;
        public TableCell(string theData, AlignType theAlignType)
        {
            data = theData;
            alignType = theAlignType;
        }
    }

	struct TableRaw
	{
		public TableCell[] cells;
	}

	class TableWriter 
	{
		public TableRaw[] rows;

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

			tab1.rows = new TableRaw[4];
            tab1.rows[0].cells = new TableCell[]{
	            new TableCell("", AlignType.Center),
	            new TableCell("debit", AlignType.Center),
	            new TableCell("credit", AlignType.Center)
            };

            tab1.rows[1].cells = new TableCell[]{
	            new TableCell("min", AlignType.Left),
	            new TableCell(debit.min.ToString(), AlignType.Right),
	            new TableCell(credit.min.ToString(), AlignType.Right)
            };

            tab1.rows[2].cells = new TableCell[]{
	            new TableCell("max", AlignType.Left),
	            new TableCell(debit.max.ToString(), AlignType.Right),
	            new TableCell(credit.max.ToString(), AlignType.Right)
            };

            tab1.rows[3].cells = new TableCell[]{
	            new TableCell("avg", AlignType.Left),
	            new TableCell(debit.avg.ToString(), AlignType.Right),
	            new TableCell(credit.avg.ToString(), AlignType.Right)
            };


			tab1.WriteToFile("stat.txt");




			TableWriter tab2 = new TableWriter();

            tab2.rows = new TableRaw[days_count + 2];
			tab2.rows[0].cells = new TableCell[4]{
				new TableCell("day", AlignType.Center),
				new TableCell("debit", AlignType.Center),
				new TableCell("credit", AlignType.Center),
				new TableCell("cur balance", AlignType.Center)
			};

            int cur_debit = 0;
            int cur_credit = 0;
			for (int i = 0; i < days_count; i++) 
            {
                cur_debit += debit.data[i];
                cur_credit += credit.data[i];
                int cur_balance = cur_debit - cur_credit;
                int i1 = i + 1;
				tab2.rows [i+1].cells = new TableCell[4] {
					new TableCell (i1.ToString(), AlignType.Right),
					new TableCell (debit.data[i].ToString(), AlignType.Right),
					new TableCell (credit.data[i].ToString(), AlignType.Right),
					new TableCell (cur_balance.ToString(), AlignType.Right)
				};
			}

            tab2.rows[days_count+1].cells = new TableCell[4]{
				new TableCell("", AlignType.Center),
				new TableCell(cur_debit.ToString(), AlignType.Right),
				new TableCell(cur_credit.ToString(), AlignType.Right),
				new TableCell((cur_debit - cur_credit).ToString(), AlignType.Right)
			};

			//Console.Write(tab2.rows[1].ToString());

			tab2.WriteToFile("report.txt");
		}
	}
}
