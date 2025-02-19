using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.ExampleProject.ResultExamples
{
	public static class BindExampleErrors
	{
		public static readonly ResultError NotPositive = ResultError.New("Not positive");
		public static readonly ResultError NotEven = ResultError.New("Not even");
	}
	public static class BindExamples
	{
		// Validate that a number is positive.
		private static Result<int> ValidatePositive(int x)
			// implicit conversion from int to Result<int>
			=> x > 0 ? x : BindExampleErrors.NotPositive;

		// Validate that a number is even.
		private static Result<int> ValidateEven(int x) =>
			// implicit conversion from int to Result<int>
			x % 2 == 0 ? x : BindExampleErrors.NotEven;

		// Multiply by two asynchronously.
		private static async Task<Result<int>> MultiplyByTwoAsync(int x)
		{
			await Task.Delay(10); // Simulate async work.
			// implicit conversion from int to Result<int>
			return x * 2;
		}

		// Try operation: Parse a string to an int.
		// implicit conversion and exception handling
		private static Try<int> ParseNumber(string input)
			=> () => int.Parse(input);

		// TryAsync operation: Fetch data asynchronously.
#pragma warning disable AMNF0002
		private static TryAsync<string> FetchDataAsync(string data)
#pragma warning restore AMNF0002
			=> async () =>
			{
				await Task.Delay(10); // Simulate async fetch.
				return Result.Success(data);
			};

		public static async Task RunAsync()
		{
			await FetchDataAsync("258")
				.BindAsync(ParseNumber)
				.BindAsync(ValidatePositive)
				.BindAsync(ValidateEven)
				.BindAsync(MultiplyByTwoAsync)
				// Result<int>.Value or Result<int>.Error is not accessible publicly
				// to get the result value, you need to use the Match or MatchAsync method
				.MatchAsync(
					success: (int x) => Console.WriteLine($"Async Success: {x}"),
					failure: (ResultError error) => Console.WriteLine($"Async Failure: {error.Message}")
				);

			ParseNumber("InvalidString")
				.Bind(ValidatePositive)
				.Bind(x => x % 2 == 0 ? Result.Success(x + 3) : BindExampleErrors.NotEven)
				// Result<int>.Value or Result<int>.Error is not accessible publicly
				// to get the result value, you need to use the Match or MatchAsync method
				.Match(
					success: (int x) => Console.WriteLine($"Async Success: {x}"),
					failure: (ResultError error) => Console.WriteLine($"Async Failure: {error.Message}")
				);
		}
	}
}