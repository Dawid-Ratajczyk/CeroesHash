using System;
using System.Collections.Generic;
using System.Text;

namespace Ceroes_
{
    internal class Unit
    {
        public static Unit Soldier = new Unit(10, 5, 1, "Soldier");
        public static Unit Knight = new Unit(20, 10, 2, "Knight");

        public static List<Unit> All = new List<Unit>() { Soldier, Knight };

        public int stack = 0;
        public int healthMax = 0;
        public int health, damage, tier;
        public string name;

        public Unit(int Health, int Damage, int Tier, string Name)
        {
            this.healthMax = Health;
            this.health = Health;
            this.damage = Damage;
            this.tier = Tier;
            this.name = Name;
        }
        
    }
}
