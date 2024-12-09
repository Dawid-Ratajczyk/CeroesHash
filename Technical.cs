using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Ceroes_
{
    public class Technical
    {
        static public void CleanBuffer() { while (Console.KeyAvailable) { Console.ReadKey(true); } }
        static public string KeyPress()
        {
            ConsoleKeyInfo press;
            press = Console.ReadKey();
            string key = press.Key.ToString();
            return key;

        }
        static public int Flip(int i)
        {
            if (i == 0) return 1;
            else return 0;
        }
        static public int Increment(int value,int change,int boundary)
        {
            if(change<0&&change+value>=boundary)
            {
                return change + value;
            }
            else if(change > 0 && change + value <= boundary)
            {
                return change + value;
            }
            return value;
        }
        public static int SetValue(string text)
        {
            Console.Clear();
            Map.mapa.vSpacer();
            Map.mapa.vSpacer();
            Map.mapa.hSpacer();
            Map.mapa.hSpacer();
            Console.WriteLine(text);
            Map.mapa.hSpacer();
            Map.mapa.hSpacer();
            int ret = Convert.ToInt32(Console.ReadLine());
            return ret;
        }
        public class Select
        {
            int index;
            int count;
            List<string> ToDraw;
            
            public Select(List<string> Lines)
            {
                index = 0;
                count = Lines.Count;
                ToDraw = Lines;
                
            }
            public int Choice()
            {
                Console.Clear();
                int longestString = ToDraw.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length;

                while (true)
                {
                    Map.mapa.vSpacer();
                    Map.mapa.vSpacer();
                    
                    for (int i = 0; i < count; i++) 
                    {
                        Map.mapa.hSpacer();
                        Map.mapa.hSpacer();
                        string drawing = "";
                        if(i == index) drawing+="-> ";
                        drawing+=ToDraw[i];
                        if (i == index)drawing+=" <-";
                        Visual.CenterText(drawing, longestString+6);
                        Console.WriteLine();
                    }
                    string key = Technical.KeyPress();
                    switch (key)
                    {
                        case "W": index = Increment(index,-1,0); break;   
                        case "S": index = Increment(index, 1, count - 1); break;
                        case "X": return index;
                    }
                    Console.Clear();
                }
            }


        }
        
    }
}
