using System;
using System.Collections.Generic;
using System.Text;

namespace Ceroes_
{
    public class Player
    {
        public static Player player1 = new Player(3);
        public static Player player2 = new Player(4);
        public static List<Player> list =new List<Player> (){player1,player2 };



        public int color;
        public List<int> Resources=new List<int>() ;

        public Player(int Color) 
        {
            this.color = Color;
            this.Resources = new List<int>{0,0,0,0};
        }


    }
}
