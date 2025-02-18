using ZeidLab.ToolBox.Results; // Assumes Result, ResultError, Try, TryAsync, Bind & BindAsync are defined here.

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
		public static Result<int> ValidatePositive(int x)
			// implicit conversion from int to Result<int>
			=> x > 0 ? x : BindExampleErrors.NotPositive;

		// Validate that a number is even.
		public static Result<int> ValidateEven(int x) =>
			// implicit conversion from int to Result<int>
			x % 2 == 0 ? x : BindExampleErrors.NotEven;

		// Multiply by two asynchronously.
		public static async Task<Result<int>> MultiplyByTwoAsync(int x)
		{
			await Task.Delay(10); // Simulate async work.
			// implicit conversion from int to Result<int>
			return x * 2;
		}

		// Add three asynchronously.
		public static async Task<Result<int>> AddThreeAsync(int x)
		{
			await Task.Delay(10);
			// implicit conversion from int to Result<int>
			return x + 3;
		}

		// Try operation: Parse a string to an int.
		// implicit conversion and exception handling
		public static Try<int> ParseNumber(string input)
			=> () => int.Parse(input);

		// Process data asynchronously: return the length of the string.
		public static async Task<Result<int>> ProcessDataAsync(string data)
		{
			await Task.Delay(10); // Simulate processing.
			return data.Length;
		}

		// TryAsync operation: Fetch data asynchronously.
#pragma warning disable AMNF0002
		public static TryAsync<string> FetchDataAsync(string data)
#pragma warning restore AMNF0002
			=> async () =>
			{
				await Task.Delay(10); // Simulate async fetch.
				return Result.Success(data);
			};


		public static async Task RunAsync()
		{
			// --- Synchronous Bind Chaining ---
			Result<int> syncChain = Result.Success(4)
				.Bind(ValidatePositive)
				.Bind(ValidateEven);
			string syncOutput = syncChain.Match(
				success: (int x) => $"Sync Chain Success: {x}",
				failure: (ResultError error) => $"Sync Chain Failure: {error.Message}");
			Console.WriteLine(syncOutput); // Expected: "Sync Chain Success: 4"

			// --- Asynchronous BindAsync Chaining ---
			Result<int> asyncChain = await Result.Success(5)
				.BindAsync(MultiplyByTwoAsync)
				.BindAsync(AddThreeAsync);
			string asyncOutput = asyncChain.Match(
				success: (int x) => $"Async Chain Success: {x}",
				failure: (ResultError error) => $"Async Chain Failure: {error.Message}");
			Console.WriteLine(asyncOutput); // Expected: (5*2)+3 = 13

			// --- Asynchronous BindAsync with a Synchronous Function ---
			Result<int> asyncChainSync = Result.Success(10)
				.Bind(ValidatePositive)
				.Bind((int x) => Result.Success(x + 1));
			string asyncSyncOutput = asyncChainSync.Match(
				success: (int x) => $"Async Chain (Sync Func) Success: {x}",
				failure: (ResultError error) => $"Async Chain (Sync Func) Failure: {error.Message}");
			Console.WriteLine(asyncSyncOutput); // Expected: 11

			// --- Try Chaining ---
			Result<int> tryChain = ParseNumber("3")
				.Bind((int n) => n > 0 ? Result.Success(n) : BindExampleErrors.NotPositive);
			string tryOutput = tryChain.Match(
				success: (int n) => $"Try Chain Success: {n}",
				failure: (ResultError error) => $"Try Chain Failure: {error.Message}");
			Console.WriteLine(tryOutput); // Expected: "Try Chain Success: 42"

			// --- TryAsync Chaining ---
			// Wrap FetchDataAsync() as a TryAsync<string> delegate.
			Result<int> tryAsyncChain = await FetchDataAsync("Data Sample")
				.BindAsync(ProcessDataAsync);
			string tryAsyncOutput = tryAsyncChain.Match(
				success: (int n) => $"TryAsync Chain Success: {n}",
				failure: (ResultError error) => $"TryAsync Chain Failure: {error.Message}");
			Console.WriteLine(tryAsyncOutput); // Expected: length of "async data"
		}
	}
}