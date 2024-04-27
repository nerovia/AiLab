namespace AiLab.Model
{
	
	interface IProblem<TState, TAction>
	{
		TState InitialState { get; }
		TState FinalState { get; }
		public Task<IEnumerable<Transition<TState, TAction>>> TransitionAsync(TState state, CancellationToken token);
	}

	record struct Transition<TState, TAction>(TState State, TAction Action);
}

