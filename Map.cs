using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;

namespace Ceroes_
{
    internal class Map
    {
        public static Material mapm;
        public static Map mapa = new Map(40,30);
        private static List<string> toDraw = new List<string> { };
        public int size,x,y;
        public int sideS;
        public  List <List<int>> plane,background;
      
        private List<List<int>>EmptyPlane(int fillWith=0)
        {
            List<List<int>>fill=new List<List<int>>();
            for (int i = 0; i < x; i++)
            {
                fill.Add(new List<int>(0));
                for (int j = 0; j < y; j++)
                {
                    fill[i].Add(fillWith);
                }
            }
            return fill;
        }
        

        public Map(int width,int height) 
        {
            this.y= height; this.x = width;
            this.size = x * y;
            this.plane =EmptyPlane(0);
            this.background=EmptyPlane(2);
            this.sideS = x / 2;
            this.plane[2][2] = 8;
        }
        public void SaveCurrentMap()
        {
            string jsonString = x+" "+y+System.Environment.NewLine;
            for (int i=0; i < y; i++)
            {
                for(int j = 0; j < x; j++)
                {
                    jsonString += Convert.ToString(plane[j][i]);   
                }
               jsonString+=System.Environment.NewLine;
            }
            jsonString += System.Environment.NewLine;
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    jsonString += Convert.ToString(background[j][i]);
                }
                jsonString += System.Environment.NewLine;
            }
            string fileName = "1.json";
            jsonString += Object.Building.list.Count;
            jsonString += System.Environment.NewLine;
            for (int i = 0; i < Object.Building.list.Count; i++)
            {
                jsonString += JsonSerializer.Serialize(Object.Building.list[i].name+" "+ Object.Building.list[i].x+ " " + Object.Building.list[i].y+ " " + Object.Building.list[i].id+ " " + Object.Building.list[i].color);
                jsonString += System.Environment.NewLine;
            }  
            File.WriteAllText(fileName, jsonString);
        }
        
        //color
        public void SetBackgroundColour(int id)
        {
            switch (id)
            {
                case 0: Console.BackgroundColor = ConsoleColor.Black;    break;
                case 1: Console.BackgroundColor = ConsoleColor.White;    break;
                case 2: Console.BackgroundColor = ConsoleColor.Green;    break;
                case 3: Console.BackgroundColor = ConsoleColor.Blue;     break;
                case 4: Console.BackgroundColor = ConsoleColor.Red;      break;
                case 5: Console.BackgroundColor = ConsoleColor.Yellow;   break;
                case 6: Console.BackgroundColor = ConsoleColor.DarkRed;  break;
                case 7: Console.BackgroundColor = ConsoleColor.DarkGray; break;
                case 8: Console.BackgroundColor = ConsoleColor.Magenta;  break;
            }
        }
        public void SetObjectColour(int id)
        {
            switch (id)
            {
                case 0: Console.ForegroundColor = ConsoleColor.Black;    break;
                case 1: Console.ForegroundColor = ConsoleColor.White;    break;
                case 2: Console.ForegroundColor = ConsoleColor.Green;    break;
                case 3: Console.ForegroundColor = ConsoleColor.Blue;     break;
                case 4: Console.ForegroundColor = ConsoleColor.Red;      break;
                case 5: Console.ForegroundColor = ConsoleColor.Yellow;   break;
                case 6: Console.ForegroundColor = ConsoleColor.DarkRed;  break;
                case 7: Console.ForegroundColor = ConsoleColor.DarkGray; break;
                case 8: Console.ForegroundColor = ConsoleColor.Magenta;  break;
            }
        }
        public void ResetColour(int back=0,int obj=1)
        {
            SetBackgroundColour(back);
            SetObjectColour(obj);
        }
        //visual basics
        public void BreakLine() { Console.WriteLine(); } 
        public void vSpacer()
        { 
        for(int i= 0; i < 5; i++)
            {
                Console.WriteLine();
            }
        }
        public void hSpacer(int space=35)
        {
            for (int i = 0; i < space; i++)
            {
                Console.Write(" ");
            }
        }
        public void cSpacer()
        {
            vSpacer();
            hSpacer();
        }
        public void hLine()
        {
            SetBackgroundColour(7);
            Console.Write("║");
            ResetColour();
        }
        //hud elements
        private void HoriznotalLine(bool corners = true, bool newLine = true)
        {
            hSpacer();
            SetBackgroundColour(7);
            Console.Write("╬");
            for (int border = 0; border < mapa.x; border++) { Console.Write("═"); }
            Console.Write("╬");
            if (newLine) Console.WriteLine();
            ResetColour();
        }
        public void DrawBox(List<string> Lines)
        {
            Lines = new List<string> {"X: ", Convert.ToString(Object.Hero.list[0].x), Convert.ToString(Object.Hero.list[0].y) };
            
            
            BreakLine();
            HoriznotalLine();
            hSpacer();
            
            for(int i=0; i<Lines.Count; i++)
            {
                hLine();
                Visual.CenterText(Lines[i], x);
                hLine();
                BreakLine();
                if(i!=Lines.Count-1)hSpacer();
            }

            /*
            bool answear = IsInteractingWithBuilding(Object.Hero.list[Program.player].x, Object.Hero.list[Program.player].y);
            Visual.CenterText(Convert.ToString(answear), x);*/       
            HoriznotalLine();
        }
        public void DrawRightMapBox(int line)
        {
            
           
            hSpacer(5);

            if (line == 0) { Visual.SideBoxLine(sideS-1,true); }
            else if (line <= toDraw.Count) { Visual.SideBoxStick(); }

            if (line > 0 && line <= toDraw.Count)
            { Visual.CenterText(Resources.names[line-1]+": " +toDraw[line - 1], (x/2)); Visual.SideBoxStick(); }
            
            if (line == toDraw.Count + 1)
            {  Visual.SideBoxLine(sideS-1); }    
            if(line>toDraw.Count+1)hSpacer(sideS+3);
        }

        private void ObjectFromString(Player player1, Player player2, string v)
        {
            throw new NotImplementedException();
        }

        public void PrintPlane()
        {
            toDraw = new List<string> { };
            //toDraw.Add("Player: " + Program.player);
            for (int i = 0; i < Resources.typesAmout; i++) { toDraw.Add(Convert.ToString(Player.list[Program.player].Resources[i])); }

            vSpacer();
            HoriznotalLine(true,false);
            DrawRightMapBox(0);
            for (int i = 0; i < y; i++)
            {
                hSpacer();
              
                for (int j = 0; j < x; j++)
                {
                    
                    ResetColour();
                    if (j == 0) { hLine(); }


                    SetBackgroundColour(background[j][i]);
                    int print = plane[j][i];
                    if (print >0&& print<5) { SetObjectColour(Object.ReturnColor(j, i)); }
                    else if (print > 4 && print < 9) { SetObjectColour(0); SetBackgroundColour(Resources.Color(Visual.mapSymbols[print])); }
                    Console.Write(Visual.mapSymbols[plane[j][i]]);


                    ResetColour();
                    if (j == x - 1) { hLine(); }
                }
                DrawRightMapBox(i+1);
                SetBackgroundColour(0);
                Console.WriteLine();
            }
            HoriznotalLine();
        }
        //checks
        public bool IsInteractingWithBuilding(int X,int Y,int heroId=0,int buildingId=2)
        { 
            for(int i = 0; i < Object.Building.list.Count;i++)
            {
                if (Object.Building.list[i].id == buildingId)
                if (Object.Building.list[i].x == X && Object.Building.list[i].y== Y-1)
                if (Object.Hero.list[Program.heroId].color==Object.ReturnColor(X,Y-1))
                return true;
            }
            return false;
        }
        public bool IsInside(int X,int Y)
        {
            bool answear=true;
            if(X < 0 || Y < 0 || X>x-1 || Y>y-1)answear=false;
            return answear;
            
        }
        public bool SpotEmpty(int X,int Y, bool checkForThing=true)
        {
            if ((Map.mapa.plane[X][Y] == 0 && Map.mapa.background[X][Y] == 2)&&checkForThing==true) return true;
            if (Map.mapa.background[X][Y] == 2&&checkForThing==false)return true; 
            return false;
        }
        public int  Thing(int X,int Y)
        {
            if (X >= 0 && Y >= 0&&Y<=mapa.y-1 && X <= mapa.x-1) return mapa.plane[X][Y];
            else return 0;
        }
        public static bool IsResource(int thingId)
        {
            if(thingId >4&&thingId<9) return true;
            else return false;
        }
        //plane manipulation
        public void Teleport(int X, int Y,int fromX,int fromY)
        {
            plane[X][Y] = plane[fromX][fromY]; 
        }
        public void Move(int fromX,int fromY,int dirX=0,int dirY = 0)
        {
            plane[fromX + dirX][fromY + dirY] = plane[fromX][fromY];
            plane[fromX][fromY] = 0;
        }
        public struct Resources
        {
            public static List<String> names = new List<string> { "Gold", "Wood", "Stone", "Crystal" };
            public static List<int> colors = new List<int> { 5, 6, 7, 8 };
            public static int typesAmout = 4;

            
            public static void Place()
            {
               //  Map.mapa.plane[2][2] = 6;
               // Map.mapa.plane[12][12] = 6;
            }
            public static int Color(string symbol)
            {
                switch (symbol)
                {
                    case "G": return 5;
                    case "W": return 6;
                    case "S": return 7;
                    case "C": return 8;
                }
                return 0;
            }
        }
    }  
}
