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
using System.Security;
using static System.Net.WebRequestMethods;
using static Ceroes_.Object;

namespace Ceroes_
{
    internal class Map
    {
        public static Material mapm;
        public static Map mapa = new Map(30,16); //map object
        public static List<int> pointer = new List<int>() { -1, -1, 0,0};//x y thing saved,arrow or color

        public static Object.Pointer arrowPointer  = new Object.Pointer("arrow" ,-1 ,-1);
        public static Object.Pointer areaPointer   = new Object.Pointer("area"  ,-1 ,-1);
        public static Object.Pointer squarePointer = new Object.Pointer("square",-1 ,-1);
        public static Object.Pointer linePointer   = new Object.Pointer("line"  ,-1 ,-1);

        public List<int> walkableThings=new List<int>(){9,5,6,7,8,10,11,12,0};
        public List<int> walkableBack = new List<int>(){2,9,10};
        //{" ","α","β","Δ","x","x"," "," "," ","←", "→", "↑", "↓" };
        public List<int> walkableThingsFight = new List<int>() {0,9,10,11,12};


        public static List<string> arrows = new List<string>() { "←", "→", "↑", "↓" };
        private static List<string> toDraw = new List<string> { };//right box to draw
        public static List<string> lowBoxDraw = new List<string> {};//low box to draw
        public int size,x,y;//map settings
        public int sideS;

        public  List <List<int>> plane,background;// map objects and background values
      
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
            System.IO.File.WriteAllText(fileName, jsonString);
        }//legacy
       
        //map loading and saving
        public void SaveCurrentMapJSon(string fileName)
        {
            System.IO.File.WriteAllText(fileName, Newtonsoft.Json.JsonConvert.SerializeObject(Map.mapa));
        }
        public void LoadMap(string mapChoice)
        {
            using (StreamReader file = System.IO.File.OpenText(mapChoice))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                mapa=Newtonsoft.Json.JsonConvert.DeserializeObject<Map>(System.IO.File.ReadAllText(mapChoice));
            }
        }
        public static void SaveState(string file="state")
        {
            List<object> State = new List<object>() {Player.list,Object.Hero.list, Object.Building.list};
            System.IO.File.WriteAllText(file, Newtonsoft.Json.JsonConvert.SerializeObject(State));
        }
        public static void LoadState(string fileName = "state") 
        {
            List<object> Heroes = new List<object>(); List<object> Players = new List<object>(); List<object> Buildings = new List<object>();
            dynamic State; ;
            using (StreamReader file = System.IO.File.OpenText(fileName))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                State = Newtonsoft.Json.JsonConvert.DeserializeObject<List<List<object>>>(System.IO.File.ReadAllText(fileName));
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
            for (int border = 0; border < this.x; border++) { bar+="═"; }
            //Console.Write("╬");
            bar += "╬";
            Visual.ColoredString(bar, Player.list[Program.player].color, false, 1);
            if (newLine) Console.WriteLine();
            Visual.ResetColour();
        }
        public void DrawBox(List<string> Lines)
        {
            if (lowBoxDraw.Count == 0) { lowBoxDraw.Add(" "); }
            Lines = lowBoxDraw;
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
            int unitsStacks = 5;
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
                if (Object.Hero.list[Program.heroId].Units[i].name != "")
                    toReturn.Add(Object.Hero.list[Program.heroId].Units[i].BfSymbol + " " + Object.Hero.list[Program.heroId].Units[i].name + ": " + Object.Hero.list[Program.heroId].UnitsAmount[i]);
                else toReturn.Add("");            
            }
            return toReturn;
        }
        //main plane generation
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

                    if (print >0&& print<5) { Visual.SetObjectColour(Object.ReturnColor(j, i)); }//if building or player
                    else if (print > 4 && print < 9) { Visual.SetObjectColour(0); Visual.SetBackgroundColour(Resources.Color(Visual.mapSymbols[print])); }//if resource
                    Console.Write(Visual.mapSymbols[plane[j][i]]);

                    Visual.ResetColour();
                    if (j == x - 1) { hLine(); }

                }
                DrawRightMapBox(i+1);
                Visual.SetBackgroundColour(0);
                Console.WriteLine();
            }
            HoriznotalLine();
            lowBoxDraw = new List<string> {};//reset lower box values
    }
        public void Select(int X,int Y)
        {
            List<int> spots =FreeSpotsAround(X,Y);

           if ((arrowPointer.x < 0 || arrowPointer.y<0)==false)//if pointer exists erase earlier
           {
                if (arrowPointer.savedThing == 0 && IsArrow(arrowPointer.x, arrowPointer.y))//if object
                {
                    this.plane[arrowPointer.x][arrowPointer.y] = 0;
                }
                else if(arrowPointer.savedThing != 0) // if background
                {
                    this.background[arrowPointer.x][arrowPointer.y] = arrowPointer.savedThing;
                }
            }
            if (spots[2] != 0)//if place is not in object
            {
                arrowPointer.savedThing = this.plane[spots[0]][spots[1]];
                this.plane[spots[0]][spots[1]] = spots[2];
                arrowPointer.colorSaved = 0;
            }
            else//if place is object
            {
                arrowPointer.colorSaved = this.background[spots[0]][spots[1]];
                this.background[spots[0]][spots[1]] = 9; 
            }
            arrowPointer.x = spots[0];
            arrowPointer.y = spots[1];
        }
        public void SelectSquare(int X, int Y,int size)
        {
            if (squarePointer.y > 0 && squarePointer.x > 0)
            {
                for (int i = areaPointer.radius; i >= 0; i--)
                {
                    for (int j = (i * 2) + 1; j > 0; j--)
                    {

                        SetBack(areaPointer.x + i - j + 1, areaPointer.y - areaPointer.radius + i, 2);
                        SetBack(areaPointer.x + i - j + 1, areaPointer.y + areaPointer.radius - i, 2);
                    }
                }
            }
            for (int i=0; i < size;i++)
            {
                for( int j=0; j < size;j++)
                {
                    areaPointer.savedColors.Add(mapa.background[X + i][Y + j]);
                    Map.mapa.background[X+i][Y+j] = 9;
                }
            }
        }
        public void SelectAreaAround(int X, int Y, int Radius)
        {
            if (areaPointer.y >= 0 && areaPointer.x >= 0)
            {
                for (int i = areaPointer.radius; i >= 0; i--)
                {
                    for (int j = (i * 2) + 1; j > 0; j--)
                    {

                        if (Back(areaPointer.x + i - j + 1, areaPointer.y - areaPointer.radius + i) == 9) SetBack(areaPointer.x + i - j + 1, areaPointer.y - areaPointer.radius + i, 2);
                        if (Back(areaPointer.x + i - j + 1, areaPointer.y + areaPointer.radius - i) == 9) SetBack(areaPointer.x + i - j + 1, areaPointer.y + areaPointer.radius - i, 2);
                    }
                }
            }
            areaPointer.radius = Radius;
            areaPointer.x = X;
            areaPointer.y = Y;
            
            
            for (int i = Radius; i >= 0; i--)
            {
                for (int j = (i*2)+1; j >0 ; j--)
                {
                    
                    if(Back(X+i-j+1,Y-Radius+i)==2)SetBack(X + i - j + 1, Y-Radius+i, 9);
                    if(Back(X+i-j+1,Y+Radius-i)==2)SetBack(X + i-j+1,Y+Radius-i,9);
                }
            }                    
        }
       
        //check
        public bool IsArrow(int x,int y)
        {
            if (this.plane[x][y] >= 9 && this.plane[x][y] <= 12) return true;
            else return false;
        }
        public List<int> FreeSpotsAround(int x, int y)
        {
            if(x< 0 || y < 0) return new List<int>() {-1,-1};
            if (SpotEmpty(x, y + 1, true)) { return new List<int>() { x, y + 1, 11 }; }
            if (SpotEmpty(x, y - 1, true)) { return new List<int>() { x, y - 1, 12 }; }
            if (SpotEmpty(x+1,y,true)){ return new List<int>() { x + 1, y,9};}
            if (SpotEmpty(x -1, y, true)) { return new List<int>() { x - 1, y,10 }; }
            
         
            else { return new List<int>() {x, y,0};}
        }
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
        public bool SpotEmpty(int X, int Y, bool checkForThing = true, bool ignoreArrows = true)
        {
            if (X>=this.x || Y>=this.y|| X <0 || Y <0d) return false;
            if ( (Thing(X,Y)==0||IsArrow(X,Y)) && walkableBack.Contains(this.background[X][Y])) return true;
            if (this.background[X][Y] == 2&&checkForThing==false)return true; 
            return false;
        }
        public bool IsWalkable(int X, int Y, bool checkThing=true,bool checkBack=true)
        {
            if ((walkableBack.Contains(Back(X, Y))&&checkBack)&&(walkableThings.Contains(Thing(X, Y))&&checkThing))
            {
                return true; 
            }
            return false;
                
        }
        public int  Back(int X, int Y)
        {
            if (X >= 0 && Y >= 0 && Y <= this.y - 1 && X <= this.x - 1) return this.background[X][Y];
            else return 0;
        }
        public int  Thing(int X,int Y)
        {
            if (X >= 0 && Y >= 0&&Y<=this.y-1 && X <= this.x-1) return this.plane[X][Y];
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
            this.plane[fromX + dirX][fromY + dirY] = this.plane[fromX][fromY];
            this.plane[fromX][fromY] = 0;
        }
        public void SetBack(int X,int Y,int color)
        {
            if (X >= 0&&Y>= 0&& X < this.x && Y < this.y) this.background[X][Y] = color;
        }
        public struct Resources
        {
            public static List<String> names = new List<string> { "Gold", "Wood", "Stone", "Crystal" };
            public static List<int> colors = new List<int> { 5, 6, 7, 8 };
            public static int typesAmout = 4;
            
            public static void Place()
            {

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

        public class Battlefield : Map
        {
            static List<Unit> LUnits = new List<Unit>();
            static List<Unit> RUnits = new List<Unit>();
           public static List<int> Heroes = new List<int>() { 0, 1};
           public static  List<List<Unit>> Armies = new List<List<Unit>>() { LUnits, RUnits };
            public static int currentPlayer=0;
            public static Battlefield fightfield = new Battlefield();           
            public Battlefield() :base(20, 20)
            {
                
            }
           
            public void FightFieldSetup(int X,int Y)
            {
                fightfield.x=X; fightfield.y=Y;
                fightfield.plane = EmptyPlane(0);
                fightfield.background= EmptyPlane(2);
                fightfield.sideS = x / 2;
                LitterWithRocks(15);
            }
            public void Fight(int lHeroId,int rHeroId)
            { 
                FightPlaceUnits(lHeroId,rHeroId);
                Program.fight = true;
                while(Program.fight)
                { 
                    UnitAction();
                    //refresh
                    Technical.CleanBuffer();
                    Thread.Sleep(200);
                    Console.Clear();

                }
                
            }

            public void FightPlaceUnits(int lHeroId, int rHeroId)
            {
                Heroes = new List<int>() { lHeroId, rHeroId };
                for (int H=0; H<Heroes.Count; H++)
                {
                    for (int i = 0; i < Object.Hero.list[H].Units.Count; i++)
                    {
                        for (int j = 0; j < Object.Hero.list[H].UnitsAmount[i]; j++)
                        {
                            if (Object.Hero.list[H].name != "" && Object.Hero.list[H].UnitsAmount[i] > 0)
                            {
                                Unit unit = Unit.Copy(i);
                                unit.color = Object.Hero.list[H].color;
                                unit.x = i + (H * ((H * this.x - 1) - (2 * (H * i))));
                                unit.y = j;
                                Armies[H].Add(unit);
                                this.plane[unit.x][unit.y] = i+1;
                            }
                        }
                    }
                }
               
               
                

            }
            public void UnitAction()
            {   
                bool unitAction = true; 
                while (unitAction)
                {
                    for(int  i = 0;i <2;i++)
                    {
                        for (int a = 0; a < Armies[i].Count; a++)
                        {
                            if (this.plane[Armies[i][a].x][Armies[i][a].y] != 0)
                            {
                                lowBoxDraw.Clear();
                                lowBoxDraw.Add(Armies[i][a].name);
                                lowBoxDraw.Add("Health: " + Armies[i][a].health + "/" + Armies[i][a].healthMax);

                                for (int m = 0; m < Armies[i][a].move; m++)
                                {
                                    int uX = Armies[i][a].x;
                                    int uY = Armies[i][a].y;

                                    this.Select(uX, uY);
                                    this.SelectAreaAround(uX, uY, Armies[i][a].move - m);

                                    DrawField();
                                    DrawBox(lowBoxDraw);
                                    Console.WriteLine("aX" + arrowPointer.x + "aY" + arrowPointer.y);
                                    int moveX = 0, moveY = 0;

                                    Console.WriteLine("x: " + uX + " y: " + uY);
                                    string key = Technical.KeyPress();
                                    switch (key)
                                    {
                                        //movement
                                        case "W": moveY = -1; break;
                                        case "D": moveX = 1; break;
                                        case "S": moveY = 1; break;
                                        case "A": moveX = -1; break;
                                        //action
                                        case "X": { Attack(Armies[i][a]); break; }
                                    }
                                    int nextSpotX = moveX + uX, nextSpotY = uY + moveY;
                                    int thingSpot = Map.Battlefield.fightfield.Thing(nextSpotX, nextSpotY);
                                    //move to empty spot
                                    if (Map.Battlefield.fightfield.IsInside(nextSpotX, nextSpotY) && Map.Battlefield.fightfield.SpotEmpty(nextSpotX, nextSpotY))
                                    {
                                        Console.WriteLine("moe");
                                        Map.Battlefield.fightfield.Move(uX, uY, moveX, moveY);
                                        Armies[i][a].x += moveX;
                                        Armies[i][a].y += moveY;
                                    }
                                }
                            }
                            else Armies[i].RemoveAt(a);
                            lowBoxDraw = new List<string>{};
                        }
                    }
       
                }
           }
            public void Attack(Unit attacker)
            {
                int dmgDelay = 400;
                int X = attacker.x;
                int Y = attacker.y;
                int dmg = attacker.damage;
                List<int>direction = new List<int> {0,0};
                string key = Technical.KeyPress();
                switch (key)
                {
                    //movement
                    case "W": direction[1]=-1; break;
                    case "D": direction[0] = 1; break;
                    case "S": direction[1] = 1; break;
                    case "A": direction[0] = -1; break;
                }
         
                switch (attacker.name)
                {
                    case ("Soldier"):
                        this.SetBack(X + direction[0], Y + direction[1], 6);
                        GetUnit(X + direction[0], Y + direction[1]).health-=dmg;
                        DrawField();
                        Thread.Sleep(dmgDelay);
                        if(GetUnit(X + direction[0], Y + direction[1]).health <= 0) { this.plane[X + direction[0]][Y + direction[1]]=0; }
                        this.SetBack(X + direction[0], Y + direction[1], 2);
                        break;
                    case ("Knight"):
                        List<List<int>> offets=new List<List<int>>() { new List<int>() { 0,0,0}, new List<int>() { 0, 0, 0 } };
                        if (direction[0] == 0) {offets = new List<List<int>>() { new List<int>() { -1, 0, 1 }, new List<int>() { 0, 0, 0 } }; }
                        if (direction[0] != 0) {offets = new List<List<int>>() { new List<int>() { 0,0,0 }, new List<int>() { 1, 0, -1 } }; }
                        for(int i = 0;i<3;i++)
                        {
                            this.SetBack(X + offets[0][i] + direction[0], Y + offets[1][i] + direction[1], 6);
                            GetUnit(X+offets[0][i] + direction[0], Y+ offets[1][i] + direction[1]).health -= dmg;    
                        }
                        DrawField();
                        Thread.Sleep(dmgDelay);
                        for (int i = 0; i < 3; i++)
                        {
                            if (GetUnit(X + offets[0][i] + direction[0], Y + offets[1][i] + direction[1]).health <= 0) { this.plane[X + direction[0]][Y + direction[1]] = 0; }
                            this.SetBack(X + offets[0][i] + direction[0], Y + offets[1][i] + direction[1], 2);
                        }
                        break;

                    case ("Archer"):
                        int line = 7;
                        if (direction[0] == 0) line ++;
                        
                    
                         List<int> point=ProjectileScan(X, Y,direction);
                        for (int l = 1; l < this.x; l++)
                        {
                            if (X + (l * direction[0]) == point[0] && Y + (l * direction[1]) == point[1])
                            {
                                break;
                            }
                            else plane[X + (l * direction[0])][Y + (l * direction[1])] = line;
                        }

                        this.SetBack(point[0], point[1], 6);
                        GetUnit(point[0], point[1]).health -= dmg;

                        DrawField();
                        Thread.Sleep(dmgDelay);

                        for (int l = 1; l < this.x; l++)
                        {
                            if (X + (l * direction[0]) == point[0] && Y + (l * direction[1]) == point[1])
                            {
                                break;
                            }
                            else plane[X + (l * direction[0])][Y + (l * direction[1])] = 0;
                        }
                        if (GetUnit(point[0], point[1]).health <= 0) { this.plane[point[0]][point[1]] = 0; }
                        this.SetBack(point[0], point[1], 2);

                     break;
                }
            }
            public Unit GetUnit(int X,int Y)
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int a = 0; a < Armies[i].Count; a++)
                    {
                        if (Armies[i][a].x == X && Armies[i][a].y == Y)
                        {
                            return Armies[i][a];
                        }
                    }
                }
                        return Unit.Blank;
            }
            public List<int> ProjectileScan(int X,int Y,List<int>dir)
            {
                List<int> pointReturn = new List<int>() { 0,0};
                for(int i=1; i < this.x; i++)
                {
                    if (walkableBack.Contains(Back(X+(i * dir[0]), Y + (i * dir[1])))==false|| walkableThingsFight.Contains(Thing(X + (i * dir[0]), Y + (i * dir[1])))==false)
                    {

                        if (Back(X + (i * dir[0]), Y + (i * dir[1])) == 1) i--;

                        pointReturn = new List<int>() { X + (i * dir[0]), Y + (i * dir[1]) };
                        if (pointReturn[0] < 0) pointReturn[0] = 0;
                        else if (pointReturn[0] >= this.x) pointReturn[0] = this.x - 1;

                        if (pointReturn[1] < 0) pointReturn[1] = 0;
                        else if (pointReturn[1] >= this.y) pointReturn[1] = this.y - 1;
                        return pointReturn;
                    }
                }
                return pointReturn;
            }
            public void DrawField()
            {
                Console.Clear();
                vSpacer();
                HoriznotalLine(true, false);
                Console.WriteLine();
                for (int i = 0; i < y; i++)
                {
                    hSpacer();
                    for (int j = 0; j < x; j++)
                    {
                        Visual.ResetColour();
                        if (j == 0) { hLine(); }

                        Visual.SetBackgroundColour(background[j][i]);
                        int print = plane[j][i];

                        if (print > 0 && print < 4) { Visual.SetObjectColour(Object.ReturnColor(j, i,true)); }//if units
   
                        Console.Write(Visual.battleSymbols[plane[j][i]]);

                        Visual.ResetColour();
                        if (j == x - 1) { hLine(); }

                    }   
                    Visual.SetBackgroundColour(0);
                    Console.WriteLine();
                }
                HoriznotalLine();
            }
            public void LitterWithRocks(int percentage)
            {
                Random chance = new Random();
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {   if(i>2&&i<x-3)
                        if(chance.Next(1, 100)<=percentage)fightfield.background[i][j] = 1;
                    }
                }
            }
        }
    
    }
 

}
