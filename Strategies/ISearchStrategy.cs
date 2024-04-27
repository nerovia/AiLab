using AiLab.Model;

namespace AiLab.Strategy
{
    interface ISearchStrategy
    {
        Task<ISearchNode<TState, TAction>> SolveAsync<TState, TAction>(IProblem<TState, TAction> problem, CancellationToken cancellationToken);
    }

    interface ISearchNode<out TState, out TAction>
    {
        bool IsInitial { get; }
        ISearchNode<TState, TAction>? Parent { get; }
        TState State { get; }
        TAction Action { get; }
        int Depth { get; }
        int Cost { get; }
    };

    class InitialNode<TState, TAction>(TState state) : ISearchNode<TState, TAction>
    {
        public bool IsInitial { get => true; }

        public ISearchNode<TState, TAction>? Parent { get => null; }

        public TState State { get => state; }

        public TAction Action => throw new Exception();

        public int Depth { get => 0; }

        public int Cost => throw new Exception();
    }

    class SearchNode<TState, TAction>(ISearchNode<TState, TAction>? parent, TState state, TAction action, int depth, int cost) : ISearchNode<TState, TAction>
    {
        public bool IsInitial { get => false; }

        public ISearchNode<TState, TAction>? Parent { get => parent; }

        public TState State { get => state; }

        public TAction Action { get => action; }

        public int Depth { get => depth; }

        public int Cost { get => cost; }
    }

}
