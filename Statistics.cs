using System.IO;
using System;

namespace Statistics
{	
	// Клас для финансовых вычислений
	public class FinancialArray
	{	

		// Массив для вычисления данных для таблицы
		public int[] data;

		// Переменные Максимума, Минимума и Среднего. Данные для таблицы.
		public int max;
		public int min;
		public int avg;


		public FinancialArray(int days_count)
		{
			data = new int[days_count];
		}

		// Метод вычисления данных
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

	// Тип выравнивания
	enum AlignType { 
		Left, 
		Right, 
		Center 
	}

	struct TableCell
	{
		public string data;
		public AlignType alignType;
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
		private StreamWriter outFile;

		public void WriteToFile(string filename)
		{
			//Открываем файл
			outFile = File.CreateText(filename);

			//Указываем кол-во колонок в строке
			int columns_count = rows[0].cells.Length;   //Почему 0  ?

			//Создаем массив "Ширина колонок"
			int[] columns_w = new int[columns_count];

			//Заполняем массив Ширины - нулями, перед вычислением максимальной длины ячейки
			for(int c=0; c<columns_count; c++)
			{
				columns_w[c] = 0;
			}

			//Вычисляем максимальую ширину ячейки
			for(int r = 0; r < rows.Length; r++)
			{
				for(int c = 0; c < columns_count; c++)
				{
					int w = rows[r].cells[c].data.Length;
					if(columns_w[c] < w)
					{
						columns_w[c] = w;
					}
				}
			}

			// Чертим таблицу
			for(int r = 0; r < rows.Length; r++)
			{	
				// Чертим таблицу. Верхнюю и среднюю часть (без строк с данными)
				for(int c = 0; c < columns_count; c++)
				{
					if(r==0)
					{
						outFile.Write(c==0 ? "╔" : "╦");
					}
					else
					{
						outFile.Write(c==0 ? "╠" : "╬");
					}
					WriteDup(columns_w[c]+2, "═");
					if(c == columns_count - 1)
					{
						outFile.Write(r==0 ? "╗" : "╣");
						outFile.Write("\n");
					}
				}

				// Чертим таблицу. Ячейки и данные
				for(int c = 0; c < columns_count; c++)
				{
					//Присваиваем S - данные для ячейки
					string s = rows[r].cells[c].data;
					int left_spaces = 0, right_spaces = 0;

					//Выравниивание внутри ячейки. Опираясь на Enum - AlignType
					switch(rows[r].cells[c].alignType)
					{
					case AlignType.Left:
						left_spaces = 0;
						right_spaces = columns_w[c] - s.Length;
						break;
					case AlignType.Right:
						left_spaces = columns_w[c] - s.Length;
						right_spaces = 0;
						break;
					case AlignType.Center:
						left_spaces = (columns_w[c] - s.Length) / 2;
						right_spaces = columns_w[c] - s.Length - left_spaces;
						break;
					}
					outFile.Write("║ "); 											// Левая часть ячейки
					WriteDup(left_spaces, " "); 									// Отступы слева
					outFile.Write(s); 												// Запись данных в ячейку
					WriteDup(right_spaces, " "); 									// Отступы справа
					outFile.Write((c == (columns_count - 1)) ? " ║\n" : " "); 		// Правая часть ячейки
				}

				// Чертим таблицу. Нижнуюю часть.
				if(r == rows.Length -1)
				{
					for(int c = 0; c < columns_count; c++)
					{
						outFile.Write (c==0 ? "╚" : "╩");
						WriteDup(columns_w[c]+2, "═");
						if(c == columns_count - 1)
						{
							outFile.Write("╝\n");
						}
					}
				}
			}

			// Закрываем файл
			outFile.Close();
			// Готовим объект к удалению
			outFile = null;	
		}	   

		//Рисуем одинаковые символы. Кол-во и Символ
		public void WriteDup(int count, string s)
		{
			for(int i = 0; i < count; i++) 
			{
				outFile.Write(s);
			}
		}
	}

	class MainClass
	{
		public static void Main (string[] args)
		{
			string myData = System.IO.File.OpenText ("raw_data.txt").ReadToEnd ().Trim ();

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



			tab2.WriteToFile("report.txt");
		}
	}
}