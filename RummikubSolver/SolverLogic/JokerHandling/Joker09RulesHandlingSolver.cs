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
            JokerAwareSolveResult res;
            if (hand.All(t => !t.IsJoker))
            {
                res=SolveIfNoJokerInHand(new SolveUniverse(board, hand));
            }
            else
            {
                res = null;
            }
            return res;
        }
        public JokerAwareSolveResult SolveIfNoJokerInHand(SolveUniverse currentSolve)
        {
            if(!currentSolve.Board.Runs.Any(x=>x.JokerIndexes.Count>0) && !currentSolve.Board.Groups.Any(x => x.JokerIndexes.Count > 0))
            {
                return new JokerAwareSolveResult
                {
                    SolveResult = solver.Solve(
                        new TileSetForCurrentHand(currentSolve.Board.Flattened,currentSolve.Hand.ToList())
                    )
                };
            }
            //var currentSolve = new SolveUniverse(board, hand, new JokerAwareSolveResult());
            //updateJokerValues to have the joker tile contain what it currently represents
            foreach(var group in currentSolve.Board.Groups)
            {
                group.UpdateJokerValues();
            }
            foreach(var run in currentSolve.Board.Runs)
            {
                run.UpdateJokerValues();
            }
            //if jokers that can be fulfilled with a tile in hand,
            //swap joker and tile in hand, then foreach immediate play using only the joker and the tiles in hand,
            //solve with the remaining tiles.
            //otherwise solve for each way you can minimize the tiles tied up with the joker with the tiles not tied up with it
            for(int i=currentSolve.Board.Groups.Count-1;i>=0;i--)
            {
                //two jokers has at least a run of one tile two jokers
                if (currentSolve.Board.Groups[i].JokerIndexes.Count == 2 || IsMoveAvailableWithOneJoker(currentSolve.Hand))
                {
                    foreach(var jokerIndex in currentSolve.Board.Groups[i].JokerIndexes)
                    {
                        var swappableIndex = currentSolve.Hand.IndexWhere(x => x.SameValue(currentSolve.Board.Groups[i][jokerIndex]));
                        if(swappableIndex != -1)
                        {
                            //makes readonly copy
                            var clearMove = new JokerClearMove(currentSolve.Board.Groups[i]);
                            SwapAtIndexes(currentSolve.Board.Groups[i], jokerIndex, currentSolve.Hand, swappableIndex);
                            //copy for solver to not mess it up
                            clearMove.AfterClearing = currentSolve.Board.Groups[i].Copy();
                            currentSolve.Result.JokerClearingMoves.Add(clearMove);
                        }
                    }
                }
            }
            for(int i=currentSolve.Board.Runs.Count-1; i>=0; i--)
            {
                //two jokers has at least a run of one tile two jokers
                if (currentSolve.Board.Groups[i].JokerIndexes.Count == 2 || IsMoveAvailableWithOneJoker(currentSolve.Hand))
                {
                    foreach(var jokerIndex in currentSolve.Board.Groups[i].JokerIndexes)
                    {
                        var swappableIndex = currentSolve.Hand.IndexWhere(x => x.SameValue(currentSolve.Board.Groups[i][jokerIndex]));
                        if(swappableIndex != -1)
                        {
                            //makes readonly copy
                            var clearMove = new JokerClearMove(currentSolve.Board.Groups[i]);
                            SwapAtIndexes(currentSolve.Board.Groups[i], jokerIndex, currentSolve.Hand, swappableIndex);
                            //copy for solver to not mess it up
                            clearMove.AfterClearing = currentSolve.Board.Groups[i].Copy();
                            currentSolve.Result.JokerClearingMoves.Add(clearMove);
                        }
                    }
                }
            }
            //TODO: handle if no joker buy available
            //now need to immediately play a move from hand, play all possible branching the universe each time and solve the list, then pick the best result
            List<SolveUniverse> possibilities=GetPossibleJokerMoves(currentSolve.Branch());
            Parallel.For(0, possibilities.Count, new ParallelOptions { MaxDegreeOfParallelism = 20 }, i =>
            {
                possibilities[i].Result.SolveResult = solver.Solve(
                    new TileSetForCurrentHand(possibilities[i].Board.Flattened, possibilities[i].Hand.ToList())
                );
            });
            JokerAwareSolveResult result = currentSolve.Result;
            foreach(var solved in possibilities)
            {
                if(solved.Result.SolveResult.Hand.Count < result.SolveResult.Hand.Count)
                {
                    result = solved.Result;
                }
            }
            return result;
        }

        private List<SolveUniverse> GetPossibleJokerMoves(SolveUniverse originalSet)
        {
            var possibilities = new List<SolveUniverse>();
            var jokers = new List<Tile>();
            for(int i=originalSet.Hand.Count-1; i>=0; i--)
            {
                if (originalSet.Hand[i].IsJoker)
                {
                    jokers.Add(originalSet.Hand[i]);
                    originalSet.Hand.RemoveAt(i);
                }
            }
            if (jokers.Count == 1)
            {
                FillPossibleJokerMovesOneJokerInHand(originalSet, jokers[0], possibilities);
            }
            else if(jokers.Count == 2)
            {
                FillPossibleJokerMovesTwoJokersInHand(originalSet, jokers, possibilities);
            }
            return possibilities;
        }
        private void FillPossibleJokerMovesOneJokerInHand(SolveUniverse orignalSet, Tile joker, List<SolveUniverse> toFill)
        {
            
        }
        private void FillPossibleJokerMovesTwoJokersInHand(SolveUniverse orignalSet, List<Tile> joker, List<SolveUniverse> toFill)
        {

        }

        private bool IsMoveAvailableWithOneJoker(InitialHand hand)
        {
            if(hand.Count==0) return false;
            int minGroupTiles = 2;
            var potentialGroups=hand.GroupBy(x => x.Number)
                .Select(x=>x.ToList()).ToList();
            var potentialRuns = hand.GroupBy(x => x.Color)
                .Select(grouping => grouping.OrderBy(t=>t.Number).ToList()).ToList();
            //if 2 tiles of same number, 3rd can be joker
            if (potentialGroups.Any(x => x.Count >= minGroupTiles))
            {
                return true;
            }
            else
            {
                foreach(var potentialRun in potentialRuns)
                {
                    int contiguousCount = 1;
                    for(int i = 0; i < potentialRun.Count-1; i++)
                    {
                        var nextNumber = potentialRun[i + 1].Number;
                        var thisNumber = potentialRun[i].Number;
                        if(nextNumber == thisNumber+1)
                        {
                            contiguousCount++;
                            if(contiguousCount == 3)
                            {
                                return true;
                            }
                        }
                        else if(nextNumber == thisNumber + 2)
                        {
                            //current + (joker=current+1) + next=current+2
                            return true;
                        }
                        else
                        {
                            contiguousCount=1;
                        }
                    }

                }
            }
            return false;
        }

        private void SwapAtIndexes(InitialList list1, int index1, InitialList list2, int index2)
        {
            var temp = list1[index1];
            list1[index1] = list2[index2];
            list2[index2] = temp;
        }
    }
}
