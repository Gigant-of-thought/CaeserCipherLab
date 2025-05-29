using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    public class CaeserCipher
    {
        public static char[] RussianAlphabet = ['а', 'б', 'в', 'г', 'д', 'е', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я'];
        public static char[] EnglishAlphabet = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'];
        public static Dictionary<char, int> RussianDict;
        public static Dictionary<char, int> EnglishDict;

        public static Dictionary<char, double> RussianFreq;
        public static Dictionary<char, double> EnglishFreq;

        static CaeserCipher()
        {
            RussianDict = new();
            EnglishDict = new();

            double[] freq = [0.062, 0.014, 0.038, ];

            for(int i = 0; i < RussianAlphabet.Length; i++)
                RussianDict[RussianAlphabet[i]] = i;
            for (int i = 0; i < EnglishAlphabet.Length; i++)
                EnglishDict[EnglishAlphabet[i]] = i;

            RussianFreq = new Dictionary<char, double>()
            {
                {'а', 0.062},  {'б', 0.014},  {'в', 0.038},
                {'г', 0.013},  {'д', 0.025},  {'е', 0.072},  
                {'ж', 0.007},  {'з', 0.016},
                {'и', 0.062},  {'й', 0.010},  {'к', 0.028},
                {'л', 0.035},  {'м', 0.026},  {'н', 0.053},
                {'о', 0.090},  {'п', 0.023},  {'р', 0.040},
                {'с', 0.045},  {'т', 0.053},  {'у', 0.021},
                {'ф', 0.002},  {'х', 0.009},  {'ц', 0.003},
                {'ч', 0.012},  {'ш', 0.006},  {'щ', 0.003},
                {'ъ', 0.014},  {'ы', 0.016},  {'ь', 0.014},
                {'э', 0.003},  {'ю', 0.006},  {'я', 0.018}
            };

            EnglishFreq = new Dictionary<char, double>()
            {
                {'e', 0.127}, {'t', 0.0906}, {'a', 0.0817},
                {'o', 0.0751}, {'i', 0.0697}, {'n', 0.0675},
                {'s', 0.0633}, {'h', 0.0609}, {'r', 0.0599},
                {'d', 0.0425}, {'l', 0.0403}, {'c', 0.0278},
                {'u', 0.0276}, {'m', 0.0241}, {'w', 0.0236},
                {'f', 0.0223}, {'g', 0.0202}, {'y', 0.0197},
                {'p', 0.0193}, {'b', 0.0149}, {'v', 0.0098},
                {'k', 0.0077}, {'x', 0.0015}, {'j', 0.0015},
                {'q', 0.0010}, {'z', 0.0005}
            };
        }

        public static string Cipher(StringToHandle source, int key)
        {
            var result = new StringBuilder();

            Dictionary<char, int> dict;
            char[] Alphabet;

            if(source.Language == Language.English)
            {
                dict = EnglishDict;
                Alphabet = EnglishAlphabet;
            }
            else
            {
                dict = RussianDict;
                Alphabet = RussianAlphabet;
            }

            key = key % Alphabet.Length;

            foreach (char c in source.StringOriginal)
            {
                var shift = (dict[c] + key) % Alphabet.Length;
                if (shift < 0)
                    shift = shift + Alphabet.Length;
                var tmp = Alphabet[shift];
                result.Append(tmp);
            }
            
            return result.ToString();
        }

        public static string Decipher(StringToHandle source, int key)
        {
            var result = new StringBuilder();

            Dictionary<char, int> dict;
            char[] Alphabet;

            if (source.Language == Language.English)
            {
                dict = EnglishDict;
                Alphabet = EnglishAlphabet;
            }
            else
            {
                dict = RussianDict;
                Alphabet = RussianAlphabet;
            }

            key = -key;

            key = key % Alphabet.Length;

            foreach (char c in source.StringOriginal)
            {
                var shift = (dict[c] + key) % Alphabet.Length;
                if (shift < 0)
                    shift = shift + Alphabet.Length;
                var tmp = Alphabet[shift];
                result.Append(tmp);
            }

            return result.ToString();
        }

        public static (int, string) BreakCipher(StringToHandle source)
        {
            var bestShift = 0;
            var minError = double.MaxValue;
            char[] alphabet;
            

            if (source.Language == Language.English)
                alphabet = EnglishAlphabet;
            else
                alphabet = RussianAlphabet;

            for (int shift = 0; shift < alphabet.Length; shift++)
            {
                string deciphered = Decipher(source, shift);
                double error = CalculateFrequencyError(deciphered, source.Language);

                if (error < minError)
                {
                    minError = error;
                    bestShift = shift;
                }
            }

            return (bestShift, Decipher(source, bestShift));
        }

        private static double CalculateFrequencyError(string text, Language lang)
        {
            int totalLetters = text.Length;
            if (totalLetters == 0) return double.MaxValue;

            char[] alphabet;
            Dictionary<char, double> freq;

            if (lang == Language.English)
            {
                alphabet = EnglishAlphabet;
                freq = EnglishFreq;
            }
            else
            {
                freq = RussianFreq;
                alphabet = RussianAlphabet;
            }

            // Считаем частоты букв в тексте
            var observedFrequencies = new Dictionary<char, double>();

            foreach(var c in alphabet)
            {
                observedFrequencies[c] = 0;
            }

            foreach (char c in text)
            {
                observedFrequencies[c]++;
            }

            // Нормируем частоты
            foreach(var e in observedFrequencies.Keys)
                observedFrequencies[e] /= totalLetters;

            // Считаем сумму квадратов отклонений
            double error = 0;
            foreach(var c in alphabet)
            {
                double diff = observedFrequencies[c] - freq[c];
                error += diff * diff;
            }

            return error;
        }
    }

    public enum Language
    {
        English,
        Russian,
        Invalid,
        Mixed
    }

    public class StringToHandle
    {
        public string StringOriginal {  get; private set; }
        public Language Language { get; private set; }

        public StringToHandle(string original, Language language)
        {
            StringOriginal = original;
            Language = language;
        }

        public static StringToHandle CleanString(string sourceString)
        {
            var result = new StringBuilder();
            var lang = Language.Invalid;


            foreach (char c in sourceString.ToLower())
            {
                if (char.IsLetter(c))
                {
                    var tmp = c;
                    if (c == 'ё')
                        tmp = 'е';

                    if (CaeserCipher.EnglishDict.ContainsKey(c))
                    {
                        if (lang == Language.Russian || lang == Language.Mixed)
                            lang = Language.Mixed;
                        if(lang == Language.Invalid)
                            lang = Language.English;
                        result.Append(tmp);
                    }
                    else if (CaeserCipher.RussianDict.ContainsKey(c))
                    {
                        if (lang == Language.English || lang == Language.Mixed)
                            lang = Language.Mixed;
                        if (lang == Language.Invalid)
                            lang = Language.Russian;
                        result.Append(tmp);
                    }
                }
            }

            return new StringToHandle(result.ToString(), lang);
        }
    }
}
