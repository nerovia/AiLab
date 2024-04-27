using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace AiLab.Model
{
	partial class UriProblem : IProblem<Uri, string>
	{
		public required Uri InitialState { get; init; }
		public required Uri FinalState { get; init; }
		public required Uri BaseUri { get; init; }

		readonly HttpClient httpClient = new();

		public async Task<IEnumerable<Transition<Uri, string>>> TransitionAsync(Uri relativeUri, CancellationToken token)
		{
			var res = await httpClient.GetAsync(relativeUri, token);
			if (!res.IsSuccessStatusCode)
				return Enumerable.Empty<Transition<Uri, string>>();

			var str = await res.Content.ReadAsStringAsync();
			var matches = HrefRegex().Matches(str);

			var test = 
				(from match in matches 
				 let uri = new Uri(match.Groups[1].Value, UriKind.RelativeOrAbsolute)
				 where BaseUri.IsBaseOf(uri)
				 let rel = new Uri(BaseUri, uri)
				 where rel.Segments.Length > BaseUri.Segments.Length
				 where rel.AbsolutePath != relativeUri.AbsolutePath
				 where !rel.IsFile
				 select new Transition<Uri, string>(rel, $"{relativeUri.AbsoluteUri} --> {rel.AbsoluteUri}")).ToArray();

			return test;

			//return (from match in matches
			//		let href = match.Groups[1].Value
			//		let uri = CreateUri(href)
			//		where uri.valid
			//		where uri.value != relativeUri
			//		select (uri.value, $"{relativeUri} -> {uri}")).ToArray();
		}
		
		[GeneratedRegex(@"href=""([^:]+?)""")]
		private static partial Regex HrefRegex();

		private (bool valid, Uri? value) CreateUri(string href)
		{
			if (Uri.TryCreate(BaseUri, href, out var uri))
			{ 
				bool valid = !Path.HasExtension(uri.AbsolutePath) 
					&& BaseUri.IsBaseOf(uri);
				var rel = new Uri(uri.AbsolutePath, UriKind.Relative);
				Debug.WriteLine(uri);
				return (valid, rel);
			}
			return (false, null);
		}
	}
}
