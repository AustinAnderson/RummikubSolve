using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverLogic.Models
{
    public class JokerClearMove
    {
        public JokerClearMove(JokerClearMove other)
        {
            AfterClearing = other.AfterClearing.Copy();
            InitialRunOrGroupWithJoker = other.InitialRunOrGroupWithJoker.Copy();
        }
        public JokerClearMove(InitialList initialRunOrGroup)
        {
            InitialRunOrGroupWithJoker = new InitialRun();
            if(initialRunOrGroup is InitialGroup)
            {
                InitialRunOrGroupWithJoker = new InitialGroup();
            }
            foreach(var item in initialRunOrGroup)
            {
                InitialRunOrGroupWithJoker.Add(new Tile(item));
            }
        }
        public InitialList InitialRunOrGroupWithJoker { get; }
        public InitialList AfterClearing { get; set; }
        /// <summary>
        /// only applicable in 09 rules
        /// </summary>
        public InitialList PlayedFromHandWhenCleared { get; set; }
    }
}
