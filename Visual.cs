using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Ceroes_
{
    internal class Visual
    {
        static public string[] mapSymbols = {" ", "ȸ", "▄", "░"," ","G","W","S","C" };
        static public string[] mapNames = { "Air", "Hero" ,"Castle","Banner","Barrier","Gold","Wood","Stone","Crystal"};
        
        
        public static int PlayerColour(int Id)
        {

            switch(Id) 
            {
                case 0: return 3;
                case 1: return 4;
            }
            return 0;

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

    }
}
