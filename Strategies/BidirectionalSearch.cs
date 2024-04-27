using AiLab.Strategy;
using AiLab.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiLab.Strategies
{
	internal class BidirectionalSearch(ISearchStrategy internalStrategy) : ISearchStrategy
	{
		public Task<ISearchNode<TState, TAction>> SolveAsync<TState, TAction>(IProblem<TState, TAction> problem, CancellationToken cancellationToken)
		{
			var stack = new Stack<ISearchNode<TState, TAction>>();
			throw new Exception();

		}

		
	}

	
}
