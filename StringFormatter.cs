using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grape_Fruit_Bar
{

        //Right aligned words get cut from the beginning at this time.
        internal class StringFormatter
        {
            //Centrates a a text token within a chosen row length as well as possible around chosen filler chars.
            public static string CentrateText(string text, int rowLength, char filler)
            {
                int space = rowLength - text.Length;
                int halfSpace = space / 2;
                string newText = "".PadRight(halfSpace, filler) + text + "".PadRight(halfSpace, filler);
                if (newText.Length == rowLength)
                {
                    return newText;
                }
                else
                {
                    return newText + filler;
                }
            }

            //Only used for visual debugging.
            //Shows the limits of the columns more clearly.
            private static string GetColumnWidthsAsString(int words, int rowLength, int[] columnWidths)
            {
                string columnRuler = "";
                for (int i = 0; i < words; i++)
                {
                    for (int j = 0; j < columnWidths[i]; j++)
                    {
                        if (j == 0)
                        {
                            columnRuler += "+";
                        }
                        else if (j == columnWidths[i] - 1)
                        {
                            columnRuler += "+";
                        }
                        else
                        {
                            columnRuler += "-";
                        }
                    }
                }
                return columnRuler;
            }

        //Overloading method. Allows us to use both string and string[] as option parameters.
        public static string AlignText(string[] words, int rowLength, char filler, string option, int[] columnWidths)
            {
                //This list decides if the column should be right or left aligned.
                //You can also mix the list as ["LEFT","RIGHT","LEFT",etc...], to get mixed alignments.
                string[] optionList = new string[words.Length];
                switch (option)
                {
                    case "RULER":
                        return AlignText(words, rowLength, filler, new string[] { "RULER" }, columnWidths);

                    case "LEFT":
                        optionList = Enumerable.Repeat("LEFT", words.Length).ToArray();
                        return AlignText(words, rowLength, filler, optionList, columnWidths);

                    case "RIGHT":
                        optionList = Enumerable.Repeat("RIGHT", words.Length).ToArray();
                        return AlignText(words, rowLength, filler, optionList, columnWidths);

                    //This option gives:
                    //Left half of the tokens become left aligned while right half becomes right aligned.
                    case "LEFTRIGHT":
                        for (int i = 0; i < words.Length / 2; i++)
                        {
                            optionList[i] = "LEFT";
                        }
                        for (int i = words.Length / 2; i < words.Length; i++)
                        {
                            optionList[i] = "RIGHT";
                        }
                        return AlignText(words, rowLength, filler, optionList, columnWidths);

                }
                return "Invalid OPTION!";

            }

            private static int[] GetColumnWidths(int rowLength, string[] words)
            {
                //Creating an integer array with column widths that sum up to the rowLength.
                //Some distribution has to be done to make the columns as even as possible.
                //For example: a rowlength of 40 into 6 columns will result in columns with widths: [7 7 7 7 6 6]
                int columnWidth = rowLength / words.Length;
                int spareChars = rowLength - columnWidth * words.Length;
                int[] columnWidths = new int[words.Length];
                for (int i = 0; i < words.Length; i++)
                {
                    if (i < spareChars)
                    {
                        columnWidths[i] = columnWidth + 1;
                    }
                    else
                    {
                        columnWidths[i] = columnWidth;
                    }
                }
                return columnWidths;
            }

            //Gets an array of chosen filler character for padding.
            private static char[] GetFillers(int columnWidth, char[] wordChars, char filler)
            {
                int fillerCount = columnWidth - wordChars.Length;
                char[] fillers = Array.Empty<char>();
                //No fillers are returned in the string if the word is too big or just fitting the column width.
                if (fillerCount > 0)
                {
                    fillers = Enumerable.Repeat(filler, fillerCount).ToArray();
                }
                return fillers;
            }

            private static char[] GetAlignedToken(int columnWidth, char[] wordChars, char filler, string alignment)
            {
                char[] front = GetFillers(columnWidth, wordChars, filler);
                char[] back = wordChars.Take(columnWidth).ToArray();
                if (alignment == "RIGHT")
                {
                    return front.Concat(back).ToArray();
                }
                else if (alignment == "LEFT")
                {
                    //For left alignment we flip back and front around.s
                    return back.Concat(front).ToArray();
                }
                else
                {
                    Console.WriteLine("Bad formatting option! Returning empty array.");
                    return new char[] { };
                }
            }
            private static string GetColumn(string word, int columnWidth, char filler, string alignment)
            {
                //Word token that we are going to try to fit into the column space.
                char[] wordChars = word.ToCharArray();
                //The column space where we store the token and eventual filler symbols.
                char[] columnText = GetAlignedToken(columnWidth, wordChars, filler, alignment);

                return string.Join("", columnText);
            }

            //Aligns text tokens stored in an array into neatly spaced columns.
            //optionList[] can contain strings with words: 'LEFT' or 'RIGHT'.
            //It can also contain word 'RULER' for printing a rulerlike object.
            //Send custom column widths as an int[] array or use null to let the method decide the column widths.
            public static string AlignText(string[] words, int rowLength, char filler, string[] optionList, int[] columnWidths)
            {
                //Returns an int[] with the column widths.
                if(columnWidths == null)
                {
                    columnWidths = GetColumnWidths(rowLength, words);
                }
                

                string rowText = "";
                //Special option that returns a rulerlike string showing the columnwidths.
                if (optionList[0] == "RULER")
                {
                    return GetColumnWidthsAsString(words.Length, rowLength, columnWidths);
                }
                else
                {
                    for (int i = 0; i < words.Length; i++)
                    {
                        rowText += GetColumn(words[i], columnWidths[i], filler, optionList[i]);
                    }
                }
                return rowText;
            
            }
    }
}
