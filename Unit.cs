﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Ceroes_
{
    internal class Unit 
    {
        public static Unit Blank =new Unit(0,0,0,0,"","","");
        public static Unit Soldier = new Unit(10, 5, 1, 6,"Soldier", "α", "Melee", 15);
        public static Unit Knight = new Unit(20, 10, 2,4,"Knight", "β", "Melee Sweep", 50,1);
        public static Unit Archer = new Unit(10, 5, 3, 3,"Archer", "Δ", "Ranged", 30, 2);
        public static Unit Charger = new Unit(30, 15, 4, 4, "Charger", "Ψ", "Charge", 60, 2);
        public static List<Unit> All = new List<Unit>() { Soldier, Knight, Archer,Charger};

        public int x, y,color;
        public string type;
        public int stack = 0;
        public int healthMax = 0;
        public int health, damage, tier;
        public string name;
        public string BfSymbol;// battelfield symbol
        public int gprice = 0;//gold price
        public int cprice = 0;//crystal price
        public int move = 3;

        public Unit(int Health, int Damage, int Tier,int Move, string Name,string bfSymbol,string Type, int gPrice=0,int cPrice=0)
        {
            this.healthMax = Health;
            this.health = Health;
            this.damage = Damage;
            this.tier = Tier;
            this.name = Name;
            this.BfSymbol = bfSymbol;
            this.gprice = gPrice;
            this.cprice = cPrice;
            this.type = Type;
            this.move= Move;
        }
        public Unit() { }
        public static void Purchase(int unitId, int heroId, int amount=0)
        {
            int playerId = Object.Hero.list[heroId].playerId;

            int crystalCost = amount * All[unitId].cprice;
            int goldCost = amount * All[unitId].gprice;
            //if enough resources
            if (crystalCost <= Player.list[playerId].Resources[3] && goldCost <= Player.list[playerId].Resources[0]) 
            {
                Player.list[playerId].Resources[0] -= goldCost;
                Player.list[playerId].Resources[3] -= crystalCost;
                if (Object.Hero.list[heroId].Units[unitId].name =="")
                {
                    Object.Hero.list[heroId].Units[unitId]=(Unit.All[unitId]);
                }
                
                Object.Hero.list[heroId].UnitsAmount[unitId] += amount;
            }
        }
        public void SetPlace(int X,int Y)
        {
            this.x = X;
            this.y = Y;
        }
        public static Unit Copy(int index)
        {
            Unit unit = new Unit();
            unit.health = Unit.All[index].health;
            unit.BfSymbol = Unit.All[index].BfSymbol;
            unit.color = Unit.All[index].color;
            unit.damage = Unit.All[index].damage;
            unit.healthMax = Unit.All[index].healthMax;
            unit.name = Unit.All[index].name;
            return unit;
        }
    }
}
