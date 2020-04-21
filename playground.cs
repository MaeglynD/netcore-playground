using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace test
{
    class testing
    {
        public static void Main(string[] args)
        {
            int[] ArrayOfMultiples(int num, int length)
            {
                return new int[length].Select((x, i) => i +1 * num).ToArray();
            }

            // Console.WriteLine("[{0}]", string.Join(", ", ArrayOfMultiples(6, 9)));

            bool AnagramStrStr(string needle, string haystack)
            {
                List<string> available = Enumerable.Range(0, haystack.Length - needle.Length + 1).Select((x, i) => haystack.Substring(i, needle.Length)).ToList();
                string sortedNeedle = String.Concat(needle.OrderBy(c => c));
                foreach (var x in available) if(String.Concat(x.OrderBy(c => c)) == sortedNeedle) return true;
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
                    if(isPalin(str)) return true;
                    List<int> testarr = new List<char>(str).Select(x => x - '0').ToList();
                    if (testarr.Count > 2 && recur(string.Join("", testarr.Where((x, i) => i % 2 != 0).Select((x, i) => x + testarr[i * 2])))) return true;
                    return false;
                }
                return recur(num.ToString());
            }

            // Console.WriteLine(PalindromeDescendant(11211230));

            string Simplify(string fraction)
            {
                int[] numbers = Array.ConvertAll(fraction.Split("/"), int.Parse);
                string recur(int[] arr){
                    if(arr[0] % arr[1] == 0) return $"{arr[0] / arr[1]}";
                    for (var i = 9; i > 1; i--){
                        if (arr[0] % i == 0 && arr[1] % i == 0){
                            return recur(new int[] {arr[0] / i, arr[1] / i});
                        }
                    }
                    return $"{arr[0]}/{arr[1]}";
                }
                return recur(numbers);
            }

            // Console.WriteLine(Simplify("8/4"));

            string LandscapeType(int[] arr)
            {
                int num = 0;

                Func<int, bool, string> possible = (y, isMax) => {
                    num = Array.IndexOf(arr, y);
                    if(arr.Where(x => x == y).ToArray().Length == 1 && num != 0 && num != arr.Length - 1){
                        if (!arr.Take(num).Select((x, i) => i == 0 ? true : (isMax ? x >= arr[i - 1] : x <= arr[i - 1])).Contains(false)){
                            if (!arr.Skip(num).Select((x, i) => i + 1 + num == arr.Length ? true : (isMax ? x >= arr[i + num + 1] : x <= arr[i + num + 1])).Contains(false)){
                                return isMax ? "mountain" : "valley";
                            }
                        }
                    }
                    return "";
                };

                string mountain = possible(arr.Max(), true);
                if (mountain.Length > 0) return mountain;
                string valley = possible(arr.Min(), false);
                if (valley.Length > 0) return valley;
                return "neither";
            }

            // Console.WriteLine(LandscapeType(new int[] { 1, 2, 3, 4 }));

            bool SameLetterPattern(string f, string s)
            {
                char[] fa = f.ToCharArray();
                char[] sa = s.ToCharArray();
                return Enumerable.SequenceEqual(
                    fa.Select((x, i) => i == 0 ? 0 : (int)x % 32 - (int)fa[i - 1] % 32).ToArray(),
                    sa.Select((x, i) => i == 0 ? 0 : (int)x % 32 - (int)sa[i - 1] % 32).ToArray());
            }

            // Console.WriteLine(SameLetterPattern("ABCBA", "BCDCB"));

			string TranslateWord(string word)
			{
				string vowels = "aeiouAEIOU";
				if (!word.Any(vowels.Contains)) return word;
				if(vowels.Contains(word[0])) return $"{word}yay";

				int i = word.IndexOfAny(vowels.ToCharArray());
				string new_word = $"{word.Substring(i)}{word.Substring(0, i)}ay".ToLower();
				return Char.IsUpper(word[0]) ? $"{Char.ToUpper(new_word[0])}{new_word.Substring(1)}" : new_word;
			}

			string TranslateSentence(string sentence)
			{
				return String.Join("", Regex.Split(sentence, "([^a-zA-Z])").ToList().Select(x => TranslateWord(x)));
			}

			// Console.WriteLine(TranslateSentence("I like to eat honey waffles."));
			// Console.WriteLine(TranslateWord("Flag"));

			string decompress(string compressed){
				Func<string, int, int> get_length = (str, ind) =>
				{
					int open_count = 0;
					for(int i = ind; i < str.Length; i++)
					{
						if(str[i] == ']')
						{
							if(open_count > 0)
								open_count--;
							else
								return i - ind;
						};
						if(str[i] == '[') open_count++;
					};
					return 0;
				};

				string split_up(string str)
				{
					string main_string = "";
					for(int i = 0; i < str.Length; i++)
					{
						Match reg_match = Regex.Match(str.Substring(i), @"^(\d+[[])");
						if(Char.IsLetter(str[i]))
						{
							main_string += str[i].ToString();
						}
						else if(reg_match.Success)
						{
							int multiplier = Int32.Parse(reg_match.Value.Remove(reg_match.Value.Length - 1));
							string contents = str.Substring(i + reg_match.Value.Length, get_length(str, i + reg_match.Value.Length));
							i += contents.Length + reg_match.Value.Length;
							main_string += string.Concat(Enumerable.Repeat(contents.Contains('[') ? split_up(contents) : contents, multiplier));
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

				while(iteration <= n)
				{
					int i = 1;
					while(a % iteration != 0)
					{
						if (a * i % iteration == 0) a *= i;
						i++;
					}
					iteration++;
				}
				return a;
			}
			
			// Console.WriteLine(smallest_number(10));
		}
    }
}
