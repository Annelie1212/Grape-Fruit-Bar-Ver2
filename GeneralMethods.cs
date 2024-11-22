using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grape_Fruit_Bar
{
    internal class GeneralMethods
    {
        public static string RepeatString(string text, int repeatCount)
        {

            string repeatedString = "";
            for (int i = 0; i < repeatCount; i++)
            {
                repeatedString += text;
            }
            return repeatedString;
        }
        public static string TitleDesign(string text,string rowSymbol, string columnSymbol)
        {
            string framedString = "";
            string[] textArr = text.Split("\n");
            int max = 0;
            foreach (string str in textArr)
            {
                if (str.Length > max)
                {
                    max = str.Length;
                }
                
            }
            foreach (string str in textArr)
            {
                framedString += columnSymbol+" "+ str + RepeatString(" ", max - str.Length+0) + " "+columnSymbol+"\n";
            }


            string row1 = GeneralMethods.RepeatString(rowSymbol, max + 2+2);
            string lastRow = GeneralMethods.RepeatString(rowSymbol, max + 2+2);

            framedString = row1+"\n"+framedString+lastRow;

            return framedString;

        }
        //Puts stars around a text.
        public static string TitleDesign(string title)
        {
            string row1 = GeneralMethods.RepeatString("*", title.Length + 2);
            string row2 = "*" + title + "*";
            string row3 = row1;

            return row1 + "\n" + row2 + "\n" + row3;
        }
        //a mod b
        public static int Mod(int a,int b)
        {
            return (a % b + b) % b;
        }
    }
}
