using SharedModels;
using SolverLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.JokerHandling
{
    public class Joker09RulesHandlingSolver:IJokerHandlingSolver
    {
        private readonly Solver solver;

        public Joker09RulesHandlingSolver(IRunResultRainbowTable table)
        {
            solver = new Solver(table);
        }
        //if joker on the board and not in hand, 09 rules are restrictive enough to 
        //be able to just solve once with each possible joker value,
        //putting the joker set in and ignoring the rest
        public JokerAwareSolveResult Solve(CurrentBoard board, InitialHand hand)
        {
            return new JokerAwareSolveResult
            {
                SolveResult = solver.Solve(new TileSetForCurrentHand(board.Flattened, hand.ToList()))
            };
            /*
            JokerAwareSolveResult res;
            if (hand.All(t => !t.IsJoker))
            {
                res=SolveIfNoJokerInHand(board, hand);
            }
            else
            {
                res = null;
            }
            return res;
            */
        }
        public JokerAwareSolveResult SolveIfNoJokerInHand(CurrentBoard board, InitialHand hand)
        {
            var res = new JokerAwareSolveResult();
            //updateJokerValues to have the joker tile contain what it currently represents
            foreach(var group in board.Groups)
            {
                group.UpdateJokerValues();
            }
            foreach(var run in board.Runs)
            {
                run.UpdateJokerValues();
            }
            //if jokers that can be fulfilled with a tile in hand,
            //swap joker and tile in hand, then foreach immediate play using only the joker and the tiles in hand,
            //solve with the remaining tiles.
            //otherwise solve for each way you can minimize the tiles tied up with the joker with the tiles not tied up with it
            for(int i=board.Groups.Count-1;i>=0;i--)
            {
                if (board.Groups[i].JokerIndexes.Count == 1 && MoveAvailableWithJoker(hand))
                {
                        int jokerIndex = board.Groups[i].JokerIndexes[0];
                        var swappableIndex = hand.IndexWhere(x => x.SameValue(board.Groups[i][jokerIndex]));
                        if (swappableIndex != -1)
                        {
                            var clearMove = new JokerClearMove(board.Groups[i]);
                            SwapAtIndexes(board.Groups[i], jokerIndex, hand, swappableIndex);
                            clearMove.AfterClearing = new List<InitialList> { board.Groups[i] };
                            res.JokerClearingMoves.Add(clearMove);
                        }
                    //must be tentative move, that can be backed out if can't play with joker from hand, or check first
                }
                else if(board.Groups[i].JokerIndexes.Count == 2)
                {
                    int jokerIndex = board.Groups[i].JokerIndexes[0];
                }
            }
            return null;
        }

        //TODO: change to actually find the move
        private bool MoveAvailableWithJoker(InitialHand hand)
        {
            return true;
            //uh oh, not garunteed for the play that uses the most tiles to be the best,
            //need to check each univers of possibilities

        }

        private void SwapAtIndexes(InitialList list1, int index1, InitialList list2, int index2)
        {
            var temp = list1[index1];
            list1[index1] = list2[index2];
            list2[index2] = temp;
        }
    }
}
