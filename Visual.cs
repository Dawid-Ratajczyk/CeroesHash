using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Ceroes_
{
    internal class Visual
    {
        static public string[] mapSymbols = {" ", "ȸ", "▄", "░"," ","G","W","S","C", "←", "→", "↑", "↓" };
        static public string[] mapNames = { "Air", "Hero" ,"Castle","Banner","Barrier","Gold","Wood","Stone","Crystal","Left","Right","Up","Down"};
        static public string[] unitSymbols = { " " };
        
        public static int PlayerColour(int Id)
        {

            switch(Id) 
            {
                case 0: return 3;
                case 1: return 4;
            }
            return 0;

        }
        public static void hSpacer(int space = 35)
        {
            for (int i = 0; i < space; i++)
            {
                Console.Write(" ");
            }
        }
        public static void CenterText(string text,int space)
        {
            int length = space-text.Length;
            if(text.Length%2==1) { text += " "; }
            for(int i = 0; i < length/2; i++) 
            {
             text = text+" ";
             text = " "+text;
            }
            Console.Write(text);
        }
        public static void SideBoxLine(int space,bool endLine=false)
        {
            Console.Write("┼");
            for (int i=0; i<space+1; i++)
            {
                Console.Write("─");
            }
            Console.Write("┼");
            if(endLine) Console.WriteLine();
        }
        public static void SideBoxStick()
        {
            Console.Write("│");
        }
        public static void ColoredString(string text,int objectColor,bool nL=false,int backColor=0)
        {
           SetObjectColour(objectColor);
           SetBackgroundColour(backColor);
           Console.Write(text);
            if(nL) Console.WriteLine();
           ResetColour();

        }
        public static void SetBackgroundColour(int id)
        {
            switch (id)
            {
                case 0: Console.BackgroundColor = ConsoleColor.Black; break;
                case 1: Console.BackgroundColor = ConsoleColor.White; break;
                case 2: Console.BackgroundColor = ConsoleColor.Green; break;
                case 3: Console.BackgroundColor = ConsoleColor.Blue; break;
                case 4: Console.BackgroundColor = ConsoleColor.Red; break;
                case 5: Console.BackgroundColor = ConsoleColor.Yellow; break;
                case 6: Console.BackgroundColor = ConsoleColor.DarkRed; break;
                case 7: Console.BackgroundColor = ConsoleColor.DarkGray; break;
                case 8: Console.BackgroundColor = ConsoleColor.Magenta; break;
                case 9: Console.BackgroundColor = ConsoleColor.DarkYellow;break;
                case 10: Console.BackgroundColor = ConsoleColor.Yellow; break;
            }
        }
        public static void SetObjectColour(int id)
        {
            switch (id)
            {
                case 0: Console.ForegroundColor = ConsoleColor.Black; break;
                case 1: Console.ForegroundColor = ConsoleColor.White; break;
                case 2: Console.ForegroundColor = ConsoleColor.Green; break;
                case 3: Console.ForegroundColor = ConsoleColor.Blue; break;
                case 4: Console.ForegroundColor = ConsoleColor.Red; break;
                case 5: Console.ForegroundColor = ConsoleColor.Yellow; break;
                case 6: Console.ForegroundColor = ConsoleColor.DarkRed; break;
                case 7: Console.ForegroundColor = ConsoleColor.DarkGray; break;
                case 8: Console.ForegroundColor = ConsoleColor.Magenta; break;
            }
        }
        public static void ResetColour(int back = 0, int obj = 1)
        {
            SetBackgroundColour(back);
            SetObjectColour(obj);
        }
    }
}
