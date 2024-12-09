using System;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Ceroes_
{
    internal class Program
    {
        public static int player = 0;
        public static int heroId = 0;

        public static int stateId = 0; //0-map 1-town 2-combat
        public static bool gameLoopRunning = true;
       
        public static Technical.Select TownMenu = new Technical.Select(new List<string>() { "Buy Units", "Build Building", "Exit" });
        public static Technical.Select BuyUnitMenu = new Technical.Select(new List<string>() { "Buy " + Unit.All[0].name, "Buy " + Unit.All[1].name, "Exit" });

        //public static Map mapa;
        static void TechnicalSetup()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }
        static void GameSetup()
        {
            Object.Initialization();
            Object.Hero.list[0].Units.Add(Unit.Soldier);
            Object.Hero.list[0].Units[0].stack=10;
            Object.Hero.list[0].Units.Add(Unit.Knight);
            Object.Hero.list[0].Units[1].stack = 5;
        }
        void HeroPick(int id)
        {
            heroId=id;
        }
       

        static void PlayerAction()
        {            
            int moveX = 0, moveY = 0;
            string key = Technical.KeyPress();

            switch(key) 
            {
                //movement
                case "W": moveY=-1; break;
                case "D": moveX= 1; break;
                case "S": moveY= 1; break;
                case "A": moveX=-1; break;
                //action
                case "X": Interact();break;
                case "L": Map.mapa.LoadMap(Technical.Input("Load Map"));break;
                case "P": Map.mapa.SaveCurrentMapJSon(Technical.Input("Save Map"));break;
                case "U": Technical.BuyAmountSelect(0,player);break;
                case "J": player=Technical.Flip(player); heroId = Technical.Flip(heroId); break;
            }
            int nextSpotX = moveX + Object.Hero.list[heroId].x, nextSpotY = Object.Hero.list[heroId].y + moveY;
            int thingSpot = Map.mapa.Thing(nextSpotX, nextSpotY);
            //move to empty spot
            if (Map.mapa.IsInside(nextSpotX, nextSpotY) && Map.mapa.SpotEmpty(nextSpotX, nextSpotY))
            {

                Map.mapa.Move(Object.Hero.list[heroId].x, Object.Hero.list[heroId].y, moveX, moveY);
                Object.Hero.list[heroId].x += moveX;
                Object.Hero.list[heroId].y += moveY;
            }
            //pickup resource
            else if (Map.mapa.IsInside(nextSpotX, nextSpotY) && Map.IsResource(thingSpot)&&Map.mapa.SpotEmpty(nextSpotX,nextSpotY,false))
            {
               switch(thingSpot)
                {
                    case 5: Player.list[player].Resources[0] += Material.Resources[0].pickup; break;
                    case 6: Player.list[player].Resources[1] += Material.Resources[1].pickup; break;
                    case 7: Player.list[player].Resources[2] += Material.Resources[2].pickup; break;
                    case 8: Player.list[player].Resources[3] += Material.Resources[3].pickup; break;
                }
                Map.mapa.Move(Object.Hero.list[heroId].x, Object.Hero.list[heroId].y, moveX, moveY);
                Object.Hero.list[heroId].x += moveX;
                Object.Hero.list[heroId].y += moveY;
            }
        }
        static void Interact()
        {
            if(Map.mapa.IsInteractingWithBuilding(Object.Hero.list[Program.player].x, Object.Hero.list[Program.player].y))
            {
                switch(TownMenu.Choice())
                {
                    case 0: BuyUnitsMenu(); break;
                }

            }
        }
        static void BuyUnitsMenu() 
        {
            int amount = 0;
            int choice = BuyUnitMenu.Choice();
            if(choice!=BuyUnitMenu.count-1)
            {
                amount = Technical.BuyAmountSelect(choice, player);
                Unit.Purchase(choice,heroId,amount);
            }       
        }
        
        static void GameLoop()
        {
            while(gameLoopRunning)
            {

                //drawing image
                Map.mapa.PrintPlane();
                Map.mapa.DrawBox(new List<string> {"a","b","c"});
              
                //game logic
                PlayerAction();
                
                //refresh
                Technical.CleanBuffer();
                Thread.Sleep(200);
                Console.Clear();
            }

        }
        
        static void Main(string[] args)
        {
          
            TechnicalSetup();
            GameSetup();
            GameLoop();
      
        }
    }
}
