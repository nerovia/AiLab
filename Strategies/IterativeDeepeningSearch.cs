using AiLab.Strategy;
using AiLab.Model;
using System.Transactions;
using System.Runtime.Serialization;

namespace AiLab.Strategies
{
    class IterativeDeepeningSearch : ISearchStrategy
    {
        public int MaxDepth { get; set; } = 30;
        public int MaxOrder { get; set; } = 100;
        public int Interval { get; set; } = 1;

        public async Task<ISearchNode<TState, TAction>> SolveAsync<TState, TAction>(IProblem<TState, TAction> problem, CancellationToken cancellationToken)
        {
            HashSet<TState> visited = new();
            Queue<ISearchNode<TState, TAction>> queue = new();
            Stack<ISearchNode<TState, TAction>> stack = new();

            queue.Enqueue(new InitialNode<TState, TAction>(problem.InitialState));

            for (int d = 1; d <= MaxDepth; d += Interval)
            {
                //visited.Clear();
                stack.Clear();
                while (queue.Count > 0)
                    stack.Push(queue.Dequeue());

                while (stack.Count > 0)
                {
                    var current = stack.Pop();

                    Console.WriteLine($"{new string(' ', current.Depth)}{current.State}");

                    // return current node if it is a final state
                    if (Equals(problem.FinalState, current.State))
                        return current;

                    // stop iteration if max depth is reached
                    if (current.Depth < d)
                    {
                        // enqueue successors to queue
                        var successors = await problem.TransitionAsync(current.State, cancellationToken);
                        foreach (var successor in successors.Take(MaxOrder))
                        {
                            if (visited.Add(successor.State))
                                stack.Push(new SearchNode<TState, TAction>(current, successor.State, successor.Action, current.Depth + 1, 0));
                        }
                    }
                    else
                    {
                        queue.Enqueue(current);
                    }
                }

            }

            throw new Exception("No solution was found");
        }
    }

	
}
