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

        static CaeserCipher()
        {
            RussianDict = new();
            EnglishDict = new();

            for(int i = 0; i < RussianAlphabet.Length; i++)
                RussianDict[RussianAlphabet[i]] = i;
            for (int i = 0; i < EnglishAlphabet.Length; i++)
                EnglishDict[EnglishAlphabet[i]] = i;
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
