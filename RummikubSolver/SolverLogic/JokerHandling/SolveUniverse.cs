using SolverLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.JokerHandling
{
    public class SolveUniverse
    {
        public SolveUniverse(CurrentBoard board, InitialHand hand, JokerAwareSolveResult result)
        {
            Board = board;
            Hand = hand;
            Result = result;
        }
        public SolveUniverse(CurrentBoard board, InitialHand hand)
        {
            Board = board;
            Hand = hand;
            Result = new JokerAwareSolveResult()
            {
                JokerClearingMoves = new List<JokerClearMove>(),
                SolveResult = new SolveResult(
                    board.Groups.Select(l => l.ToList()).ToList(),
                    board.Runs.Select(l => l.ToList()).ToList(),
                    hand.ToList()
                )
            };
        }
        public CurrentBoard Board { get; }
        public InitialHand Hand { get; }
        public JokerAwareSolveResult Result { get; }
        public SolveUniverse Branch()
        {
            return new SolveUniverse(new CurrentBoard(Board), new InitialHand(Hand), new JokerAwareSolveResult(Result));
        }
    }
}
