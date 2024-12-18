﻿using System;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Numerics;

namespace Ceroes_
{
    internal class Program
    {
        public static int player = 0;
        public static int heroId = 0;

        public static int stateId = 0; //0-map 1-town 2-combat
        public static bool gameLoopRunning = true;
        public static bool fight = false;
       
        public static Technical.Select TownMenu = new Technical.Select(new List<string>() { "Buy Units", "Build Building", "Exit" });
        public static Technical.Select BuyUnitMenu = new Technical.Select(new List<string>() { "Buy " + Unit.All[0].name, "Buy " + Unit.All[1].name,"Buy "+ Unit.All[2].name, "Buy " + Unit.All[3].name, "Exit" });

        //public static Map mapa;
        static void TechnicalSetup()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
        }
        static void GameSetup()
        {
            Object.Initialization();

            Map.mapa.LitterWithResources(4);
            Map.Battlefield.fightfield.FightFieldSetup(30,10);
        }
        static  void HeroPick(int id)
        {
            heroId=id;
            player = Object.Hero.list[id].playerId;
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
                //debug
                case "L": Map.mapa.LoadMap(Technical.Input("Load Map"));break;
                case "P": Map.mapa.SaveCurrentMapJSon(Technical.Input("Save Map"));break;
                case "U": Technical.BuyAmountSelect(0,player);break;
                case "J": player=Technical.Flip(player); heroId = Technical.Flip(heroId); break;
                case "O": HeroPick(Technical.Increment(heroId,1,Object.Hero.list.Count-1));break;
                case "I": HeroPick(Technical.Increment(heroId, -1, 0)); break;
                case "K": Map.mapa.SaveGame(); break;
                case "M": Map.mapa.LoadGame(); break;
                case "B": Map.mapa.SelectAreaAround(Object.Hero.list[heroId].x, Object.Hero.list[heroId].y,3); break;
                case "Q": gameLoopRunning=false; break;
                case "T": Map.Battlefield.fightfield.DrawField();break;
                case "R": Map.Battlefield.fightfield.Fight(0,1); break;
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
           if(Map.mapa.IsInteractingWithBuilding(Object.Hero.list[Program.heroId].x, Object.Hero.list[Program.heroId].y))
            {
                switch(TownMenu.Choice())
                {
                    case 0: BuyUnitsMenu(); break;
                }

            }
        }
        static void BuyUnitsMenu() 
        {
            bool buying = true;
            while(buying)
            {
                int amount = 0;
                int choice = BuyUnitMenu.Choice();
                if (choice != BuyUnitMenu.count - 1)
                {
                    amount = Technical.BuyAmountSelect(choice, player);
                    Unit.Purchase(choice, heroId, amount);
                }
                if (choice == BuyUnitMenu.count - 1) buying = false;
            }

        }
        
        static void GameLoop()
        {
            while(gameLoopRunning)
            {

                //drawing image
                Map.mapa.Select(Object.Hero.list[heroId].x, Object.Hero.list[heroId].y);
                Map.mapa.PrintSelectedPlane(Object.Hero.list[heroId].x, Object.Hero.list[heroId].y,13);
                //Map.mapa.DrawBox(new List<string> {"a","b","c"});
              
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
