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
        static public string[] battleSymbols = {" ","α","β","Δ","x","x"," "," "," ","←", "→", "↑", "↓" };
        public static int uiSpacer = 65;
        public static void DrawBoxByLine(int line, List<string> Lines,int width=5,bool sides=true)
        {
            width = width * 2;
            if (line >= 0 && line <= Lines.Count+1)
            {
                if (line == 0&&sides) SideBoxLine(width-1, false);else if(line==0) hSpacer(width);
                if (line >0 && line <= Lines.Count)
                {  
                    if (sides)  { SideBoxStick(); } 
                    Visual.CenterText(Lines[line - 1], width); 
                    if (sides) { SideBoxStick(); } 
                } 
                if ((line == Lines.Count+1 )&&sides) SideBoxLine(width-1, false); else if (line==Lines.Count + 1) hSpacer(width);
            }
        }
        public static void DrawMultipleBoxes(List<List<string>>lists)
        {
            int amount = lists.Count,longestString=0,longestList=0;

            for(int i=0;i<amount;i++)//per list
            {
                if (lists[i].Count > longestList-2) longestList = lists[i].Count+2;
                for (int j = 0; j < lists[i].Count; j++)//trough list
                {
                    if (lists[i][j].Length>longestString)
                    {
                        longestString = lists[i][j].Length;
                    }
                }
            }
            for(int i=0;i<longestList;i++)//per line
            {
                hSpacer((uiSpacer-(longestString*amount)));
                for(int j=0;j<amount;j++)//per list
                {
                    DrawBoxByLine(i,lists[j],(longestString/2)+1,true);hSpacer(5);
                }
                Console.WriteLine();
            }
        }
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
