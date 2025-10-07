using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChessLogic
{
    public class Result(Player winner, EndReason reason)
    {
        public Player Winner { get; } = winner;
        public EndReason EndReason { get; } = reason;
    
        public static Result WinPlayer(Player winner)
        {
            return new Result(winner, EndReason.Checkmate);
        }
    
        public static Result Draw(EndReason reason)
        {
            return new Result(Player.None, reason);
        }
    
    
    
    }
}
