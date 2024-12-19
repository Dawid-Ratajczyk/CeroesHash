using System;
using System.Collections.Generic;
using System.Text;

namespace Ceroes_
{
    public class Player
    {
        public static Player player1 = new Player(3,"Main");
        public static Player player2 = new Player(4,"Opponent");
        public static Player player3 = new Player(8, "Opponent2");
        public static List<Player> list =new List<Player> (){player1,player2,player3};
        public int color;
        public string name;
        public List<int> Resources=new List<int>() ;

        public Player(int Color,string Name) 
        {
            this.name = Name;
            this.color = Color;
            this.Resources = new List<int>{300,10,10,5};
        }
       
        public Player(){}


    }
}
