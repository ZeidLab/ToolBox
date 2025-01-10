# ToolBox Project

The **ToolBox** project is a utility library designed to simplify error handling and functional programming in C#. It
provides types and methods for working with railway-oriented programming (ROP), making it easier to write robust,
predictable, and maintainable code.

## Key Features

- **Result Type**: Represents the outcome of an operation that can either succeed or fail.
- **Error Type**: Encapsulates error details, including a code, name, message, and optional exception.
- **Unit Type**: Represents a void-like value for operations that don't return meaningful results.
- **Try and TryAsync Delegates**: Simplify exception handling for synchronous and asynchronous operations.
~~~~
## Installation

Add the ToolBox library to your project via NuGet:

```bash
  dotnet add package ZeidLab.ToolBox
```

### Examples

1. Using Result<T> for Success and Failure

```csharp
Result<int> Divide(int a, int b)
{
    if (b == 0)
        return Result<int>.Failure(Error.New("Division by zero is not allowed."));

    return Result<int>.Success(a / b);
}

var result = Divide(10, 0);

if (result.IsSuccess)
    Console.WriteLine($"Result: {result.Value}");
else
    Console.WriteLine($"Error: {result.Error.Message}");
```

2. Using Unit for Void-like Operations

 ```csharp
   Result<Unit> LogMessage(string message)
   {
   Console.WriteLine(message);
   return Result<Unit>.Success(Unit.Default);
   }

var result = LogMessage("Hello, World!");

if (result.IsSuccess)
Console.WriteLine("Message logged successfully.");
else
Console.WriteLine($"Error: {result.Error.Message}");
```

3. Using Try and TryAsync for Error Handling

```csharp
   Try<int> riskyOperation = () => throw new InvalidOperationException("Something went wrong.");
   Result<int> result = riskyOperation.Try();

if (result.IsFailure)
Console.WriteLine($"Error: {result.Error.Message}");

TryAsync<int> asyncOperation = async () =>
{
await Task.Delay(100);
throw new InvalidOperationException("Async failure.");
};

var asyncResult = await asyncOperation.Try();

if (asyncResult.IsFailure)
Console.WriteLine($"Async Error: {asyncResult.Error.Message}");
```

4. Chaining Operations with Railway-Oriented Programming

```csharp
   Result<int> operation1 = Result<int>.Success(10);
   Result<int> operation2 = operation1.Bind(value => Result<int>.Success(value * 2));

if (operation2.IsSuccess)
Console.WriteLine($"Chained Result: {operation2.Value}");
```

## Contributing

Contributions are welcome! Please open an issue or submit a pull request on the GitHub repository.

## License

This project is licensed under the MIT License. See the LICENSE file for details.