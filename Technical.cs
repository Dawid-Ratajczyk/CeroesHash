using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Transactions;

namespace Ceroes_
{
    public class Technical
    {
        public static int uiSpacer = Visual.uiSpacer;
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
        public static string Input(string text)
        {
            Console.Clear();
            Map.mapa.vSpacer();
            Map.mapa.vSpacer();
            Map.mapa.hSpacer();
            Map.mapa.hSpacer();
            Console.WriteLine(text);
            Map.mapa.hSpacer();
            Map.mapa.hSpacer();
            string ret = Console.ReadLine();
            return ret;
        }
        public static int BuyAmountSelectOld(int unitId,int playerId)
        {
            Unit current =Unit.All[unitId];
            string symbol = current.BfSymbol;
            int cPrice = current.cprice, gPrice = current.gprice,index = 0; 
            if (cPrice==0) {}
            int maxGold = Player.list[playerId].Resources[0] / gPrice;
            int maxCrystal = maxGold;
            if(cPrice!=0) { maxCrystal = Player.list[playerId].Resources[3] / cPrice; }
            int maxAmount=maxGold; if (maxGold > maxCrystal) maxAmount = maxCrystal;//max amount of units to buy

            while (true)
            {
                Console.Clear();
                Map.mapa.vSpacer();
                Map.mapa.vSpacer();
                string bar = "<";
                for(int i = 0; i <= maxAmount; i++) 
                {
                    if (index == i) { bar += "Θ"; }
                    else bar += "-";
                }
                bar+= ">";
                List<string> unitImage = new List<string>() { "╬═══╬" , "║ " + symbol + " ║" , "╬═══╬" };
                List<string> unitStats = new List<string>() { "Health: "+current.healthMax, "Damage: " + current.damage, "Type: " + current.type};   
                 Visual.hSpacer(uiSpacer); Visual.ColoredString("╬═══╬", Player.list[playerId].color,true ,0);
                Visual.hSpacer(uiSpacer); Visual.ColoredString("║ "+symbol+" ║", Player.list[playerId].color, true,0); 
                Visual.hSpacer(uiSpacer); Visual.ColoredString("╬═══╬", Player.list[playerId].color,true, 0); 
               // Visual.DrawMultipleBoxes(new List<List<string>>(){ unitImage,unitStats});

                Console.WriteLine();
                Visual.hSpacer(uiSpacer); Visual.ColoredString(current.name, 1, true, 0);
                Visual.hSpacer(uiSpacer); Console.WriteLine(" buy");
                Visual.hSpacer(uiSpacer-10); Visual.CenterText(bar,26); Console.WriteLine();
                Visual.hSpacer(uiSpacer+2); Console.WriteLine(index);
                Console.WriteLine();
                Visual.hSpacer(uiSpacer-10); Visual.CenterText("Gold: " + Player.list[playerId].Resources[0],12); Visual.CenterText("Cost: " +index*gPrice,10);
                Console.WriteLine();
                Visual.hSpacer(uiSpacer-10); Visual.CenterText("Crystal: " + Player.list[playerId].Resources[3],12) ; Visual.CenterText("Cost: " + index * cPrice, 10);
                Console.WriteLine();               
                string key = Technical.KeyPress();
                switch (key)
                {
                    case "A": index = Increment(index, -1, 0); break;
                    case "D": index = Increment(index, 1, maxAmount ); break;
                    case "X": return index;
                }
                //Thread.Sleep(200);
                Console.Clear();    
            }
            return 0;
        }
        public static int BuyAmountSelect(int unitId, int playerId)
        {
            Unit current = Unit.All[unitId];
            string symbol = current.BfSymbol;
            int cPrice = current.cprice, gPrice = current.gprice, index = 0;
            if (cPrice == 0) { }
            int maxGold = Player.list[playerId].Resources[0] / gPrice;
            int maxCrystal = maxGold;
            if (cPrice != 0) { maxCrystal = Player.list[playerId].Resources[3] / cPrice; }
            int maxAmount = maxGold; if (maxGold > maxCrystal) maxAmount = maxCrystal;//max amount of units to buy
            uiSpacer = 55;
            while (true)
            {
                Console.Clear();
                Map.mapa.vSpacer();
                Map.mapa.vSpacer();
                string bar = "<";
                for (int i = 0; i <= maxAmount; i++)
                {
                    if (index == i) { bar += "Θ"; }
                    else bar += "-";
                }
                bar += ">";
                //bar = "x";
                List<string> unitImage = new List<string>() { "╬═══╬", "║ " + symbol + " ║", "╬═══╬" };
                List<string> unitStats = new List<string>() { " Health: " + current.healthMax, " Damage: " + current.damage, " Moves: " + current.move };
                Visual.DrawMultipleBoxes(new List<List<string>>(){ unitImage,unitStats});
                Console.WriteLine();
                List<string> buyamount = new List<string>() { "Name:" + current.name,"Type: ",current.type ,"Buy: "+ Convert.ToString(index) };
                List<string> costs = new List<string>() { "Gold: " + Player.list[playerId].Resources[0], "Cost: " + index * gPrice, "Crystal: " + Player.list[playerId].Resources[3], "Cost: " + index * cPrice };
                Visual.DrawMultipleBoxes(new List<List<string>>() { buyamount, costs });
                Console.WriteLine();
                Visual.hSpacer(uiSpacer+2); Console.WriteLine(index+" / "+maxAmount);
                Visual.hSpacer(uiSpacer/2+5); Visual.CenterText(bar, uiSpacer); Console.WriteLine();

                string key = Technical.KeyPress();
                switch (key)
                {
                    case "A": index = Increment(index, -1, 0); break;
                    case "D": index = Increment(index, 1, maxAmount); break;
                    case "X": return index;
                }
                Console.Clear();
            }
            return 0;
        }
        public class Select
        {
            int index;
            public int count;
            List<string> ToDraw;
            
            public Select(List<string> Lines)
            {
                index = 0;
                count = Lines.Count;
                ToDraw = Lines;
                
            }
            public int Choice(bool exit=true)
            {
                Console.Clear();
                int longestString = ToDraw.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length;

                while (true)
                {
                    Map.mapa.vSpacer();
                    Map.mapa.vSpacer();
                    
                    for (int i = 0; i < count; i++) 
                    {
                        if(i==count-1&&exit) { Console.WriteLine(); }
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
                    //Thread.Sleep(200);
                    Console.Clear();
                }
                
            }


        }
        
    }
}
