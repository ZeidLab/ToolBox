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