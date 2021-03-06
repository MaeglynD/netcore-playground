using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Numerics;

namespace test
{
	class testing
	{
		public static void Main(string[] args)
		{
			int[] ArrayOfMultiples(int num, int length)
			{
				return new int[length].Select((x, i) => i + 1 * num).ToArray();
			}

			// Console.WriteLine("[{0}]", string.Join(", ", ArrayOfMultiples(6, 9)));

			bool AnagramStrStr(string needle, string haystack)
			{
				List<string> available = Enumerable.Range(0, haystack.Length - needle.Length + 1)
					.Select((x, i) => haystack.Substring(i, needle.Length))
					.ToList();

				string sortedNeedle = String.Concat(needle.OrderBy(c => c));

				foreach (var x in available)
					if (String.Concat(x.OrderBy(c => c)) == sortedNeedle) return true;

				return false;
			}

			// Console.WriteLine(AnagramStrStr("bag", "grab"));

			bool PalindromeDescendant(int num)
			{
				Func<string, bool> isPalin = str =>
				{
					char[] reversedStr = str.Substring(str.Length / 2).ToCharArray();

					Array.Reverse(reversedStr);

					return str.Substring(0, str.Length / 2) == new string(reversedStr);
				};

				bool recur(string str)
				{
					if (isPalin(str)) return true;

					List<int> testarr = new List<char>(str)
						.Select(x => x - '0').ToList();

					if (testarr.Count > 2 && recur(
						string.Join("", testarr.Where((x, i) => i % 2 != 0)
							.Select((x, i) => x + testarr[i * 2])))
					) return true;

					return false;
				}

				return recur(num.ToString());
			}

			// Console.WriteLine(PalindromeDescendant(11211230));

			string Simplify(string fraction)
			{
				int[] numbers = Array.ConvertAll(fraction.Split("/"), int.Parse);

				string recur(int[] arr)
				{
					if (arr[0] % arr[1] == 0) return $"{arr[0] / arr[1]}";

					for (var i = 9; i > 1; i--)
					{
						if (arr[0] % i == 0 && arr[1] % i == 0)
						{
							return recur(new int[] { arr[0] / i, arr[1] / i });
						}
					}

					return $"{arr[0]}/{arr[1]}";
				}

				return recur(numbers);
			}

			// Console.WriteLine(Simplify("8/4"));

			bool SameLetterPattern(string f, string s)
			{
				char[] fa = f.ToCharArray(), sa = s.ToCharArray();

				return Enumerable.SequenceEqual(
					fa.Select((x, i) => i == 0 ? 0 : (int)x % 32 - (int)fa[i - 1] % 32).ToArray(),
					sa.Select((x, i) => i == 0 ? 0 : (int)x % 32 - (int)sa[i - 1] % 32).ToArray());
			}

			// Console.WriteLine(SameLetterPattern("ABCBA", "BCDCB"));

			string TranslateWord(string word)
			{
				string vowels = "aeiouAEIOU";

				if (!word.Any(vowels.Contains)) return word;

				if (vowels.Contains(word[0])) return $"{word}yay";

				int i = word.IndexOfAny(vowels.ToCharArray());

				string new_word = $"{word.Substring(i)}{word.Substring(0, i)}ay".ToLower();

				return Char.IsUpper(word[0]) ? $"{Char.ToUpper(new_word[0])}{new_word.Substring(1)}" : new_word;
			}

			// Console.WriteLine(TranslateWord("Flag"));

			string TranslateSentence(string sentence)
			{
				return String.Join("", Regex.Split(sentence, "([^a-zA-Z])")
							.ToList()
							.Select(x => TranslateWord(x)));
			}

			// Console.WriteLine(TranslateSentence("I like to eat honey waffles."));


			string decompress(string compressed)
			{
				Func<string, int, int> get_length = (str, ind) =>
				{
					int open_count = 0;

					for (int i = ind; i < str.Length; i++)
					{
						if (str[i] == ']')
						{
							if (open_count > 0)
							{
								open_count--;
							}
							else
							{
								return i - ind;
							}
						};

						if (str[i] == '[') open_count++;

					};

					return 0;
				};

				string split_up(string str)
				{
					string main_string = "";

					for (int i = 0; i < str.Length; i++)
					{
						Match reg_match = Regex.Match(str.Substring(i), @"^(\d+[[])");

						if (Char.IsLetter(str[i]))
						{
							main_string += str[i].ToString();
						}
						else if (reg_match.Success)
						{
							int multiplier = Int32.Parse(reg_match.Value.Remove(reg_match.Value.Length - 1));

							string contents = str.Substring(
								i + reg_match.Value.Length,
								get_length(str, i + reg_match.Value.Length)
							);

							i += contents.Length + reg_match.Value.Length;

							main_string += string.Concat(
								Enumerable.Repeat(contents.Contains('[') ? split_up(contents) : contents, multiplier)
							);
						}
					}

					return main_string;
				}

				return split_up(compressed);
			}

			// Console.WriteLine(decompress("2[a5[c]4[b2[u]]]3[x]"));

			long smallest_number(int n)
			{
				long iteration = 1, a = 1;

				while (iteration <= n)
				{
					int i = 1;

					while (a % iteration != 0)
					{
						if (a * i % iteration == 0) a *= i;

						i++;
					}

					iteration++;
				}

				return a;
			}

			// Console.WriteLine(smallest_number(10));

			bool can_traverse(int[][] playing_field)
			{
				int col_length = playing_field.Length;

				int[] columns = new int[playing_field[0].Length]
					.Select((x, i) => new int[col_length]
						.Select((y, c) => playing_field[c][i])
						.ToList()
						.FindIndex(y => y == 1))
					.Select(x => x == -1 ? col_length : x)
					.ToArray();

				for (int i = 0; i < columns.Length - 1; i++)
				{
					int x = columns[i];

					if (!Array.Exists(new int[] { x, x + 1, x - 1 }, e => e == columns[i + 1]))
						return false;
				}

				return true;
			}

			// Console.WriteLine(can_traverse(
			// 	new int[][]
			// 	{
			// 		new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0 },
			// 		new int[] {0, 0, 0, 1, 0, 0, 0, 0, 1 },
			// 		new int[] {0, 0, 1, 1, 1, 0, 1, 0, 1 },
			// 		new int[] {0, 1, 1, 1, 1, 1, 1, 1, 1 }
			// 	}
			// ));

		}
	}
}
