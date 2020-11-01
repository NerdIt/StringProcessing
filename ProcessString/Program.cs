using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using System.Threading;

namespace ProcessString
{
	class Program
	{
		static List<StringVariable> vars = new List<StringVariable>() {new StringVariable("loud", "!"), 
																		new StringVariable("question", "?"), 
																		new StringVariable("stop", ".") };

		static void Main(string[] args)
		{
			string inputTest = "\"hello \" + \"world\" + loud + question + stop + loud";

			inputTest = ProcessString(inputTest);
			
			Console.WriteLine(inputTest);
			Console.ReadKey();
		}

		static String ProcessString(string og)
		{
			bool atEnd = false;

			
			int currentIndex = 0;
			
			List<IndexPair> pairs = GeneratePairs(og);

			bool inString = false;
			

			string newOg = "";
			for(int i = 0; i < og.Length; i++)
			{
				if(!InsidePair(i, pairs))
				{
					if(og[i] != ' ')
					{
						newOg += og[i];
					}
				}
				else
				{
					newOg += og[i];
				}
			}
			og = newOg;
			pairs = GeneratePairs(og);
			string undifinedValue = "";
			//bool definingName = false;

			for(int i = 0; i < og.Length; i++)
			{
				//Console.WriteLine(InsidePair(i, pairs));
				//Console.Write(og[i] + ": " + InsidePair(i, pairs));
				
				if ((!InsidePair(i, pairs) && og[i] != '+') && (i < og.Length - 1))
				{
					
					undifinedValue += og[i];
				}
				else if(undifinedValue != "")
				{

					if (i >= og.Length - 1)
					{
						undifinedValue += og[i];
					}

					og = ReplaceFirstOccurrence(og, undifinedValue, "\"" + GetVariableValue(undifinedValue) + "\"");


					undifinedValue = "";
					i = -1;
					pairs = GeneratePairs(og);
				}





			}


			inString = false;
			string finalReturn = "";
			for(int i = 0; i < og.Length; i++)
            {
				if(og[i] == '"')
                {
					inString = !inString;
                }
				if(inString && og[i] != '"')
                {
					finalReturn += og[i];
                }
            }

			return finalReturn;
		}



		static string ReplaceFirstOccurrence(string Source, string Find, string Replace)
		{
			int Place = Source.IndexOf(Find);
			string result = "";
			if(Place > 0)
            {
				result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
			}
			return result;
		}

		

		static string GetVariableValue(string name)
		{

			for(int i = 0; i < vars.Count; i++)
			{
				if(vars[i].name == name)
				{
					return vars[i].value;
				}
			}
			return "";

			
		}

		static List<IndexPair> GeneratePairs(string body)
		{
			List<IndexPair> pairs = new List<IndexPair>(); 
			List<int> indexOfQuote = AllIndexesOf(body, "\"");
			if (indexOfQuote.Count % 2 == 0)
			{
				for (int i = 0; i < indexOfQuote.Count; i += 2)
				{
					pairs.Add(new IndexPair(indexOfQuote[i], indexOfQuote[i + 1]));
				}


				//for (int i = 0; i < indexOfQuote.Count; i += 2)
				//{
				//	inString = !inString;

				//	Console.WriteLine(Gather(indexOfQuote[i] + 1, indexOfQuote[i + 1] - 1, og));
				//}
			}
			else
			{
				Console.WriteLine("String Error");
			}

			return pairs;
		}

		static bool InsidePair(int index, List<IndexPair> pairs)
		{
			for(int i = 0; i < pairs.Count; i++)
			{
				if(index >= pairs[i].start && index <= pairs[i].end)
				{
					return true;
				}
			}

			return false;
		}

		static List<int> AllIndexesOf(string str, string value)
		{
			if (String.IsNullOrEmpty(value))
				throw new ArgumentException("the string to find may not be empty", "value");
			List<int> indexes = new List<int>();
			for (int index = 0; ; index += value.Length)
			{
				index = str.IndexOf(value, index);
				if (index == -1)
					return indexes;
				indexes.Add(index);
			}

			
		}
		static string Gather(int index1, int index2, string body)
		{
			string finalReturn = "";
			for (int i = 0; i < body.Length; i++)
			{
				if (i >= index1 && i <= index2)
				{
					finalReturn += body[i];
				}
			}

			return finalReturn;
		}
	}


	
}
