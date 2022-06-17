using SolverLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.JokerHandling
{
    internal class SolveUniverse
    {
        public SolveUniverse(CurrentBoard board, InitialHand hand)
        {
            Board = board;
            Hand = hand;
        }
        public CurrentBoard Board { get; }
        public InitialHand Hand { get; }
        public SolveUniverse Branch()
        {
            return new SolveUniverse(new CurrentBoard(Board), new InitialHand(Hand));
        }
    }
}
