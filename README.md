# What is ZeidLab.ToolBox library?

**ZeidLab.ToolBox** is a versatile and robust utility library designed to simplify common programming tasks, enhance
error handling, and promote functional programming paradigms in C#. It provides a collection of tools and extensions
that streamline operations such as null checks, error handling, task management, and railway-oriented programming (ROP) sometimes known as "Result Pattern".

## Features

* **Unit Type:** A type representing the absence of a meaningful value, useful in functional programming.

* **Maybe Type:** A monadic type for handling optional values, similar to Option in functional languages.

* **Result Type:** A robust error-handling type for railway-oriented programming, allowing chaining of operations with explicit success and failure states.

* **Error Handling:** Structured error handling with ResultError,supporting error codes, messages, and exceptions.

* **Asynchronous Support:** Comprehensive support for asynchronous operations with Result types.

Inspired by [LanguageExt](https://github.com/louthy/language-ext), this library offers a more compact and user-friendly alternative with extensive examples and tutorials.

## Table Of Contents

[TableOfContents]:#table-of-contents

- [How to install?](#installation)
- [Types included in this library](#types-included-in-this-library)
- [What is `Maybe<T>`?](#what-is-maybet-type)
  - [Why Use `Maybe<T>` Instead of Nullable?](#why-use-maybet-instead-of-nullable)
  - [Code examples of Maybe<T>](#code-examples-of-maybet)
  - [Extension Methods of `Maybe<T>` with Examples](#extension-methods-of-maybet-with-examples)
- [Contribution](#contribution)
- [License](#license)



## Installation
To use **ZeidLab.ToolBox** in your project, you can install it via NuGet:

```bash
dotnet add package ZeidLab.ToolBox
```

[^ Back To Top][TableOfContents]

## Types included in this library

* **Unit:** A type representing the absence of a meaningful value, useful in functional programming.
* **`Maybe<T>`:** A type representing the presence or absence of a value explicitly, avoiding the pitfalls of `null`.
* **`Result<T>`:** A type representing the success or failure of an operation, allowing for chaining of operations with explicit success and failure states.
* **`ResultError`:** A type representing a structured error, supporting error codes, messages, and exceptions.


## What is `Maybe<T>` Type?

The concepts such as - the "Optional Nomad"  and using `Option<T>` or `Maybe<T>` instead of primitive nullable types like `int?` - are rooted in functional programming paradigms and best practices for writing robust, maintainable code, and  It represents the presence or absence of a value explicitly, avoiding the pitfalls of `null`.

### Why Use `Maybe<T>` Instead of Nullable?

Here are some disadvantages of Using Nullable:
- **Null Reference Exceptions:** Forgetting to check for `null` can cause runtime crashes.
- **Ambiguity:** `null` can signify missing data, errors, or uninitialized values, making code harder to understand.
- **Poor Readability:** Excessive `null` checks clutter code and reduce readability.
- **Violates Functional Principles:** `null` contradicts functional programming's emphasis on explicitness and immutability.

Here are some advantages of Using `Maybe<T>`:
- **Explicit Handling of Missing Values:** Forces developers to handle the absence of a value explicitly.
- **Avoids Magic Numbers:** Eliminates the need for special values (e.g., `-1`) to represent missing data which is error-prone and unclear.
- **Type Safety:** Ensures compile-time handling of absent values, reducing runtime errors.
- **Functional Composition:** Supports operations like `Map` and `Bind`, enabling chaining without `null` concerns.

[^ Back To Top][TableOfContents]

### Code examples of Maybe<T>

This is a simple example of how to create the `Maybe<T>` type:

```csharp
 // Creating a Maybe with a value
 Maybe<int> someValue = Maybe.Some(10);
 Console.WriteLine(someValue.IsSome); // Output: true
 Console.WriteLine(someValue.IsNone); // Output: false

 // Creating a Maybe with a value implicitly
 Maybe<int> someValue2 = 10;
 Console.WriteLine(someValue2.IsSome); // Output: true
 Console.WriteLine(someValue2.IsNone); // Output: false

 // Creating a Maybe in the 'None' state
 Maybe<string> noneValue = Maybe.None<string>();
 Console.WriteLine(noneValue.IsSome); // Output: false
 Console.WriteLine(noneValue.IsNone); // Output: true
```

[^ Back To Top][TableOfContents]

## Extension Methods of `Maybe<T>` with Examples
The value of `Maybe<T>` instance is not accessible directly, therefore extension methods are provided to help interacting with the `Maybe<T>` type. This section provides a brief overview of each extension method available in the `ZeidLab.ToolBox.Options` namespace.

### `ToSome<TIn>()`

Converts a non-null object to a `Maybe<TIn>` instance in the 'Some' state.

```csharp
var maybeInt = 10.ToSome(); // maybeInt is now a Maybe<int> in the 'Some' state.
```

### `ToNone<TIn>()`

Creates a `Maybe<TIn>` instance in the None state, regardless of the input value.

```csharp
var maybeInt = ((int?)null).ToNone(); // maybeInt is now a Maybe<int> in the 'None' state.
```

### `Bind<TIn, TOut>(this Maybe<TIn> self, Func<TIn, Maybe<TOut>> map)`

Maps the content of a `Maybe<TIn>` instance to a new `Maybe<TOut>` instance using the provided mapping function. This method is for chaining operations.

A simple example:
```csharp
Maybe<int> maybeInt = Maybe.Some(5);
Maybe<string> maybeString = maybeInt.Bind(x => x.ToString().ToSome()); // maybeString is now a Maybe<string> containing "5".
```
An example with method chaining and implicit conversion:
```csharp
// Initial Maybe value
Maybe<int> maybeNumber = Maybe.Some(10);

// Function that multiplies the number by 2 and
// returns an implicitly converted instance of Maybe<int> with the state of Some
Func<int, Maybe<int>> multiplyByTwo = x => x * 2;

// Function that converts the number to a string if it's even
// returns an implicitly converted instance of Maybe<string> with the state of Some
Func<int, Maybe<string>> convertToString = x => x.ToString();

// Chaining operations using Bind
Maybe<string> result = maybeNumber
	.Bind(multiplyByTwo)
	.Bind(convertToString);

// Output result
result.Do(
	some: val => Console.WriteLine($"Result: {val}"),
	none: () => Console.WriteLine("No result")
	);
```



### `Map<TIn, TOut>(this Maybe<TIn> self, Func<TIn, TOut> some, Func<TOut> none)`

Maps the content of a `Maybe<TIn>` instance to a new value using two functions: one for handling the Some case and another for handling the None case.

```csharp
var maybeAge = Maybe.Some(25);
var description = maybeAge.Map(
    some: age => $"Age is {age}",
    none: () => "Age unknown"
); // Returns "Age is 25"
```

### `If<TIn>(this Maybe<TIn> self, Func<TIn, bool> predicate)`

Ensures that the content of a `Maybe{TIn}` instance satisfies a predicate.

```csharp
Maybe<int> maybeInt = Maybe.Some(10);
bool result = maybeInt.If(x => x > 5); // result is true.
```

### `Where<TIn>(this IEnumerable<Maybe<TIn>> self, Func<TIn, bool> predicate)`

Filters a sequence of Maybe instances based on a predicate applied to their content.

```csharp
var maybeList = new List<Maybe<int>> { Maybe.Some(5), Maybe.None<int>(), Maybe.Some(10) };
var filtered = maybeList.Where(x => x > 5); // filtered contains only Maybe.Some(10).
```

### `Filter<TIn>(this Maybe<TIn> self, Func<TIn, bool> predicate)`

Filters the Maybe instance based on a predicate applied to its content.

```csharp
var maybe = Maybe.Some(42);
var filtered = maybe.Filter(value => value > 0); // Returns Maybe.Some(42)
```

### `Flatten<TIn>(this IEnumerable<Maybe<TIn>> self)`

Converts a sequence of Maybe instances to an IEnumerable of their content, excluding None instances.

```csharp
var maybeList = new List<Maybe<int>> { Maybe.Some(1), Maybe.<int>None(), Maybe.Some(3) };
var result = maybeList.Flatten(); // Output: [1, 3]
```

### `Flatten<TIn>(this IEnumerable<Maybe<TIn>> self, TIn substitute)`

Converts a sequence of `Maybe{TIn}` instances to an `IEnumerable{TIn}` of their content, replacing `Maybe{TIn}.IsNone` instances with a specified substitute value.

```csharp
var maybeList = new List<Maybe<int>> { Maybe.Some(1), Maybe.None<int>(), Maybe.Some(3) };
var result = maybeList.Flatten(0); // Output: [1, 0, 3]
```

### `Flatten<TIn>(this IEnumerable<Maybe<TIn>> self, Func<TIn> substitute)`

Converts a sequence of `Maybe{TIn}` instances to an `IEnumerable{TIn}` of their content, using a substitute function for `Maybe{TIn}.IsNone` instances.

```csharp
var maybeList = new List<Maybe<int>> { Maybe.Some(1), Maybe.None<int>(), Maybe.Some(3) };
var result = maybeList.Flatten(() => 0); // Output: [1, 0, 3]
```

### `Reduce<TIn>(this Maybe<TIn> self, TIn substitute)`

Reduces the content of a `Maybe{TIn}` instance to a single value using the provided default value.

```csharp
Maybe<int> maybeInt = Maybe.Some(10);
int result = maybeInt.Reduce(0); // result is 10.
```

### `Reduce<TIn>(this Maybe<TIn> self, Func<TIn> substitute)`

Reduces the content of a `Maybe{TIn}` instance to a single value using the provided function.

```csharp
Maybe<int> maybeInt = Maybe.None<int>();
int result = maybeInt.Reduce(() => 0); // result is 0.
```

### `DoIfSome<TIn>(this Maybe<TIn> self, Action<TIn> action)`

Applies the provided action to the content of the `Maybe{TIn}` instance if it is `Maybe{TIn}.IsSome`.

```csharp
Maybe<int> maybeInt = Maybe.Some(10);
maybeInt.DoIfSome(x => Console.WriteLine(x)); // Prints 10 to the console.
```

### `DoIfNone<TIn>(this Maybe<TIn> self, Action action)`

Executes the provided action if the `Maybe{TIn}` instance is `Maybe{TIn}.IsNone`.

```csharp
Maybe<int> maybeInt = Maybe.None<int>();
maybeInt.DoIfNone(() => Console.WriteLine("No value.")); // Prints "No value." to the console.
```

### `ValueOrThrow<TIn>(this Maybe<TIn> self)`

Returns the value of the `Maybe{TIn}` instance if it is `Maybe{TIn}.IsSome`, otherwise throws an `InvalidOperationException`.

```csharp
Maybe<int> maybeInt = Maybe.Some(10);
int value = maybeInt.ValueOrThrow(); // value is 10.
```
## Contribution
Contributions are welcome! Please feel free to submit issues, feature requests, or pull requests.

1. [x] Fork the repository.
2. [x] Create a new branch for your feature or bugfix.
3. [x] Commit your changes.
4. [x] Push your branch and submit a pull request.

[^ Back To Top][TableOfContents]

## License
This project is licensed under the MIT License. See the [LICENSE](./LICENSE.txt) file for details.

[^ Back To Top][TableOfContents]