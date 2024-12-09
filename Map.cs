﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Xml.Linq;

namespace Ceroes_
{
    internal class Map
    {
        public static Material mapm;
        public static Map mapa = new Map(30,16);
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
            //this.plane[2][2] = 8;
        }

        public void SaveGame(string filename="0")
        {
            SaveCurrentMapJSon(filename);
            SaveState(filename+"st");
        }
        public void LoadGame(string filename="0") 
        {
            LoadMap(filename);
            LoadState(filename + "st");
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
                jsonString += System.Text.Json.JsonSerializer.Serialize(Object.Building.list[i].name+" "+ Object.Building.list[i].x+ " " + Object.Building.list[i].y+ " " + Object.Building.list[i].id+ " " + Object.Building.list[i].color);
                jsonString += System.Environment.NewLine;
            }  
            File.WriteAllText(fileName, jsonString);
        }
        public void SaveCurrentMapJSon(string fileName)
        {
            File.WriteAllText(fileName, Newtonsoft.Json.JsonConvert.SerializeObject(Map.mapa));
        }
        public void LoadMap(string mapChoice)
        {
            using (StreamReader file = File.OpenText(mapChoice))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                mapa=Newtonsoft.Json.JsonConvert.DeserializeObject<Map>(File.ReadAllText(mapChoice));
            }
        }
        public static void SaveState(string file="state")
        {
            List<object> State = new List<object>() {Player.list,Object.Hero.list, Object.Building.list};
            File.WriteAllText(file, Newtonsoft.Json.JsonConvert.SerializeObject(State));
        }
        public static void LoadState(string fileName = "state") 
        {
            List<object> Heroes = new List<object>(); List<object> Players = new List<object>(); List<object> Buildings = new List<object>();
            dynamic State; ;
            using (StreamReader file = File.OpenText(fileName))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                State = Newtonsoft.Json.JsonConvert.DeserializeObject<List<List<object>>>(File.ReadAllText(fileName));
            }
            Object.RemovalFromPlane();
            Player.list.Clear();
            Object.Hero.list.Clear();
            Object.Building.list.Clear();

            for(int i = 0; i < State[0].Count; i++)
            {
                Player Load = new Player();
                Load.color = State[0][i].color;
                for(int j=0; j < State[0][i].Resources.Count;j++)
                {
                     
                    dynamic u = State[0][i].Resources[j];
                    int add = u;
                    Load.Resources.Add(add);
                }
                Player.list.Add(Load);
            }
            for (int i = 0; i < State[1].Count; i++)
            {
                Object.Hero Load = new Object.Hero();
                Load.x = State[1][i].x;
                Load.y = State[1][i].y;
                Load.color = State[1][i].color;
                Load.id = State[1][i].id;
                Load.name = State[1][i].name;
                Load.playerId = State[1][i].playerId;
                for(int j = 0; j< State[1][i].Units.Count;j++)
                {
                    dynamic u = State[1][i].Units[j];
                    Unit New = new Unit();
                    New.stack = u.stack;
                    New.health = u.health;
                    New.health = u.damage;
                    New.tier = u.tier;
                    New.name = u.name;
                    New.BfSymbol = u.BfSymbol;
                    New.healthMax = u.healthMax;
                    Load.Units.Add(New);
                }
                Object.Hero.list.Add(Load);
            }
            for(int i = 0; i < State[2].Count;i++)
            {
                dynamic u = State[2][i];
                Object.Building Load = new Object.Building();
                Load.x = u.x;
                Load.y = u.y;
                Load.name= u.name;
                Load.id = u.id;
                Load.color= u.color;
                Object.Building.list.Add(Load);
            }
            Object.Initialization();
            Thread.Sleep(1000);
            
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
        public void hLine(int color=1)
        {
            Visual.SetBackgroundColour(7);
            Visual.ColoredString("║", Player.list[Program.player].color,false,1);
            Visual.ResetColour();
        }
        //hud elements
        private void HoriznotalLine(bool corners = true, bool newLine = true,int color=1)
        {
            hSpacer();
            string bar = "╬";
           // Visual.SetBackgroundColour(7);
           // Console.Write("╬");
            for (int border = 0; border < mapa.x; border++) { bar+="═"; }
            //Console.Write("╬");
            bar += "╬";
            Visual.ColoredString(bar, Player.list[Program.player].color, false, 1);
            if (newLine) Console.WriteLine();
            Visual.ResetColour();
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

            if (line == 0) { Visual.SideBoxLine(sideS,true); }
            if (line ==3||line==8 ) { Visual.SideBoxLine(sideS, false); }

            if (line > 0 && line <= toDraw.Count&& line!=3&&line!=8)
            { Visual.SideBoxStick(); Visual.CenterText(toDraw[line-1],(x/2)+1); Visual.SideBoxStick(); }
            
            if (line == toDraw.Count + 1){Visual.SideBoxLine(sideS); }    
            if(line>toDraw.Count+1)hSpacer(sideS+3);
        }
        public List<string> RightMapBoxValues()
        {
            int unitsStacks = Object.Hero.list[Program.heroId].Units.Count;
            List<string> toReturn = new List<string> { };
            int spacer = 3;
            toReturn.Add("Player " + Player.list[Program.player].name);
            toReturn.Add("Hero " + Program.heroId);
            toReturn.Add(" ");
            for (int i = spacer; i < Resources.typesAmout+spacer; i++)
            {
                toReturn.Add(Convert.ToString( Material.Resources[i-spacer].name +": "+Player.list[Program.player].Resources[i-spacer])); 
            }
            toReturn.Add(" ");
            toReturn.Add("Units");
            for(int i = 0; i < unitsStacks; i++)
            {
                toReturn.Add(Object.Hero.list[Program.heroId].Units[i].BfSymbol +" "+Object.Hero.list[Program.heroId].Units[i].name + ": " + Object.Hero.list[Program.heroId].Units[i].stack);
            }
            return toReturn;
        }

        public void PrintPlane()
        {
            toDraw = RightMapBoxValues();
            vSpacer();
            HoriznotalLine(true,false);
            DrawRightMapBox(0);
            for (int i = 0; i < y; i++)
            {
                hSpacer();         
                for (int j = 0; j < x; j++)
                {
                    Visual.ResetColour();
                    if (j == 0) { hLine(); }

                    Visual.SetBackgroundColour(background[j][i]);
                    int print = plane[j][i];
                    if (print >0&& print<5) { Visual.SetObjectColour(Object.ReturnColor(j, i)); }
                    else if (print > 4 && print < 9) { Visual.SetObjectColour(0); Visual.SetBackgroundColour(Resources.Color(Visual.mapSymbols[print])); }
                    Console.Write(Visual.mapSymbols[plane[j][i]]);

                    Visual.ResetColour();
                    if (j == x - 1) { hLine(); }
                }
                DrawRightMapBox(i+1);
                Visual.SetBackgroundColour(0);
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
