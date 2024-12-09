using System;
using System.Collections.Generic;
using System.Text;

namespace Ceroes_
{
    public class Player
    {
        public static Player player1 = new Player(3,"Main");
        public static Player player2 = new Player(4,"Opponent");
        public static List<Player> list =new List<Player> (){player1,player2};
        public int color;
        public string name;
        public List<int> Resources=new List<int>() ;

        public Player(int Color,string Name) 
        {
            this.name = Name;
            this.color = Color;
            this.Resources = new List<int>{100,10,10,5};
        }


    }
}
