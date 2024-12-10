using System;
using System.Collections.Generic;
using System.Text;

namespace Ceroes_
{
    internal class Unit
    {
        public static Unit Soldier = new Unit(10, 5, 1, "Soldier", "α",15);
        public static Unit Knight = new Unit(20, 10, 2, "Knight", "β",50,1);
        
        public static List<Unit> All = new List<Unit>() { Soldier, Knight };

        public int stack = 0;
        public int healthMax = 0;
        public int health, damage, tier;
        public string name;
        public string BfSymbol;// battelfield symbol
        public int gprice = 0;//gold price
        public int cprice = 0;//crystal price

        public Unit(int Health, int Damage, int Tier, string Name,string bfSymbol,int gPrice=0,int cPrice=0)
        {
            this.healthMax = Health;
            this.health = Health;
            this.damage = Damage;
            this.tier = Tier;
            this.name = Name;
            this.BfSymbol = bfSymbol;
            this.gprice = gPrice;
            this.cprice = cPrice;
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

                Object.Hero.list[heroId].Units[unitId].stack += amount;
            }
        }
    }
}
