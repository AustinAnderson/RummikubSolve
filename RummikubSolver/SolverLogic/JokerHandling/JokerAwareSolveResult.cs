﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public class JokerAwareSolveResult
    {
        public JokerAwareSolveResult() { }
        public JokerAwareSolveResult(JokerAwareSolveResult other) 
        {
            JokerClearingMoves = new List<JokerClearMove>();
            foreach(var move in other.JokerClearingMoves)
            {
                JokerClearingMoves.Add(new JokerClearMove(move));
            }
            SolveResult = null;
        }
        public List<JokerClearMove> JokerClearingMoves { get; set; }=new List<JokerClearMove>();
        public SolveResult? SolveResult { get; set; }
    }
}
