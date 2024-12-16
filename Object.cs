using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace Ceroes_
{
     class Object
     {
        public int x, y, color;
        public string name;
        

        public static int ReturnColor(int X, int Y,bool Battle=false)
        {
            if(Battle==false)
            {
                int thing = Map.mapa.plane[X][Y];
                if (thing == 1)
                {
                    for (int i = 0; i < Hero.list.Count; i++)
                    {
                        if (Hero.list[i].x == X && Hero.list[i].y == Y)
                        {
                            return Hero.list[i].color;
                        }
                    }
                }
                if (thing == 2 || thing == 3)
                {

                    for (int i = 0; i < Building.list.Count; i++)
                    {
                        if (Building.list[i].x == X && Building.list[i].y == Y)
                        {
                            return Building.list[i].color;
                        }
                    }
                }
                
            }
            else
            {
                int thing = Map.Battlefield.fightfield.plane[X][Y];
                if (thing>0&&thing<=5)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        for (int i = 0; i < Map.Battlefield.Armies[j].Count; i++)
                        {
                            if (Map.Battlefield.Armies[j][i].x==X&&Map.Battlefield.Armies[j][i].y==Y)
                            {
                                return Map.Battlefield.Armies[j][i].color;
                            }
                        }
                    }
                }
            }
            return 7;
        }
        public static void Initialization()
        {
            Object.Building.Placement();
            Object.PlaneInsertion();
        }
        public static void RemovalFromPlane()
        {
            for (int i = 0; i < Building.list.Count; i++)
            {
                //if (Building.list[i].name=="Castle")
                Map.mapa.plane[Building.list[i].x][Building.list[i].y] = 0;
            }
            for (int i = 0; i < Hero.list.Count; i++)
            {
                Map.mapa.plane[Hero.list[i].x][Hero.list[i].y] = 0;
            }
        }
        public static void PlaneInsertion()
        {

            for(int i=0; i<Building.list.Count;i++)
            {
                //if (Building.list[i].name=="Castle")
                Map.mapa.plane[Building.list[i].x][Building.list[i].y] = Building.list[i].id;
            }
            for(int i = 0; i < Hero.list.Count;i++)
            {
                Map.mapa.plane[Hero.list[i].x][Hero.list[i].y] = Hero.mapId;
            }
        }
        public class Pointer:Object
        {
            public int savedThing, colorSaved,radius,line,direction;
            public string type;//area arrow line
            public List<int> savedThings = new List<int>();
            public List<int> savedColors = new List<int>()
                ;
            public Pointer(string type,int x=0,int y=0,int savedThing=0, int colorSaved=0)
            {
                this.x = x;
                this.y = y;
                this.savedThing = savedThing;
                this.colorSaved = colorSaved;
                this.type = type;
            }
         
        }

        public class Hero : Object
        {
            public const int mapId = 1;
            public int id = 1;
            public int playerId;
            public List<Unit> Units = new List<Unit>() {Unit.Blank, Unit.Blank, Unit.Blank, Unit.Blank, Unit.Blank};
            public List<int> UnitsAmount =new List<int>() {0,0,0,0,0};

            public static List<Hero> list = new List<Hero> { new Hero("Player", 1, 1, 0, 0), new Hero("Oponent", 6, 6, 1, 1), new Hero("Oponent2", 4, 4, 1, 2) };
            public bool controlled = true;

            public Hero(string Name, int X, int Y, int player,int Id)
            {

                name = Name;
                x = X;
                y = Y;
                color = Visual.PlayerColour(player);
                playerId = player;
                id = Id;
            }
            public Hero() { }
        }
        public class Building:Object
        {
            public int id = 2;
      
            public static List<Building> list = new List<Building> {new Object.Building("Castle",5,10,4), new Object.Building("Castle", 10, 5, 3) };
            
            public Building(string Name, int X, int Y, int Color)
            {
                switch(Name)
                {
                    case "Castle": id = 2;break;
                    case "Banner": id = 3;break;
                }
                name = Name;
                x = X;
                y = Y;
                color = Color;
            }
            public Building() { }
            public static void Placement()
            {
                for(int i  = 0; i < Building.list.Count; i++) 
                {
                    int X= Building.list[i].x;
                    int Y = Building.list[i].y;

                    if (list[i].name == "Castle")
                    {
                        Map.mapa.background[X][Y] = 1;
                        Map.mapa.background[X+1][Y] = 1;
                        Map.mapa.background[X-1][Y] = 1;
                        Map.mapa.background[X - 1][Y-1]= 1;
                        Map.mapa.background[X + 1][Y-1] = 1;

                        Map.mapa.plane[X+1][Y] = 3;
                        Map.mapa.plane[X-1][Y] = 3;
                        Map.mapa.plane[X][Y-1] = 4;

                        list.Add(new Object.Building("Banner", X + 1, Y, list[i].color));  
                        list.Add(new Object.Building("Banner", X - 1, Y, list[i].color));
                    }
                }
            }
        }
   
    }
}
