using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UruItTest.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public int Score { get; set; }

        public Player() {
        }
        public Player(string nickname, int score)
        {
            Nickname = nickname;
            Score = score;
        }
    }
}
