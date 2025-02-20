using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.ExampleProject.ResultExamples;

internal sealed class MatchExample
{
	// Example: Asynchronously parsing a string into an integer and executing actions based on the result
	// If there is an invalid format exception it will be captured
	private static Try<int> ParseNumber(string input)
		=> () => int.Parse(input);

#pragma warning disable AMNF0002
	private static TryAsync<int> ParseNumberAsync(string input)
#pragma warning restore AMNF0002
		=> async () =>
		{
			await Task.Delay(10);
			return int.Parse(input);
		};

	public static async Task RunAsync()
	{
		// Use the Try delegate and match its outcome asynchronously:
		await ParseNumber("42")
			.MatchAsync(
				success: async x =>
				{
					await Task.Delay(50);
					Console.WriteLine($"Parsed successfully: {x}");
				},
				failure: async error =>
				{
					await Task.Delay(50);
					Console.WriteLine("Failed to parse number");
				}
			);
		// When input is "42", prints: "Parsed successfully: 42"

		await ParseNumber("InvalidString")
			.MatchAsync(
				success: async x =>
				{
					await Task.Delay(50);
					Console.WriteLine($"Parsed successfully: {x}");
				},
				// there is an invalid format exception
				failure: async error =>
				{
					await Task.Delay(50);
					Console.WriteLine("Failed to parse number");
				}
			);
		// When input is "InvalidString", prints: "Failed to parse number"

		// Use the TryAsync delegate and match its outcome asynchronously:
		await ParseNumberAsync("42")
			.MatchAsync(
				success: async x =>
				{
					await Task.Delay(50);
					Console.WriteLine($"Parsed successfully: {x}");
				},
				failure: async error =>
				{
					await Task.Delay(50);
					Console.WriteLine("Failed to parse number");
				}
			);
		// When input is "42", prints: "Parsed successfully: 42"

		await ParseNumberAsync("InvalidString")
			.MatchAsync(
				success: async x =>
				{
					await Task.Delay(50);
					Console.WriteLine($"Parsed successfully: {x}");
				},
				// there is an invalid format exception
				failure: async error =>
				{
					await Task.Delay(50);
					Console.WriteLine("Failed to parse number");
				}
			);
		// When input is "InvalidString", prints: "Failed to parse number"
	}
}