// See https://aka.ms/new-console-template for more information
using AiLab.Model;
using AiLab.Strategies;
using AiLab.Strategy;

var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(10));

var problem = new UriProblem()
{
	BaseUri = new("https://de.wikipedia.org/wiki/"),
	InitialState = new("https://de.wikipedia.org/wiki/Katzen"),
	FinalState = new("https://de.wikipedia.org/wiki/Dungeons_%26_Dragons")
};

var ids = new IterativeDeepeningSearch() { MaxDepth = 30, Interval = 3, MaxOrder = 100 };
var bis = new BidirectionalSearch(ids) { };

try
{
	var solution = await ids.SolveAsync(problem, timeout.Token);
	var stack = new Stack<ISearchNode<Uri, string>>([solution]);
	while (solution.Parent != null)
	{
		stack.Push(solution.Parent);
		solution = solution.Parent;
	}

	Console.WriteLine("Found a solution");
	while (stack.Count > 0)
	{
		var top = stack.Pop();
		if (!top.IsInitial)
			Console.WriteLine(top.Action);
	}
	
}
catch
{
	Console.WriteLine("No solution was found");
}



Console.ReadLine();