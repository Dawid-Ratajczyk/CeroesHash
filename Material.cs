using System;
using System.Collections.Generic;
using System.Text;

namespace Ceroes_
{
    public class Material
    {
        public static Material Gold = new Material(5, "Gold", 5,100);
        public static Material Wood = new Material(6, "Wood", 6,10);
        public static Material Stone = new Material(7, "Stone", 7,10);
        public static Material Crystal = new Material(8, "Crystal", 8,5);
        public static List<Material> Resources = new List<Material>() { Gold,Wood,Stone,Crystal};

        public int id;
        public string name;
        public int color;
        public int pickup;
        public Material(int id, string name, int color,int pickup)
        {

            this.id = id;
            this.name = name;
            this.color = color;
            this.pickup = pickup;
        }
        public static void Place()
        {
            Map.mapa.plane[10][10] = 5;
        }
    }
}
