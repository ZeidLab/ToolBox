# What is ZeidLab.ToolBox library?

**ZeidLab.ToolBox** is a versatile and robust utility library designed to simplify common programming tasks, enhance
error handling, and promote functional programming paradigms in C#. It provides a collection of tools and extensions
that streamline operations such as null checks, error handling, task management, and railway-oriented programming (ROP).

## Features

* > **Unit Type:** A type representing the absence of a meaningful value, useful in functional programming.

* > **Maybe Type:** A monadic type for handling optional values, similar to Option in functional languages.

* > **Result Type:** A robust error-handling type for railway-oriented programming, allowing chaining of operations with explicit success and failure states.

* > **Error Handling:** Structured error handling with ResultError,supporting error codes, messages, and exceptions.

* > **Asynchronous Support:** Comprehensive support for asynchronous operations with Result types.

I have learned a lot from [LanguageExt](https://github.com/louthy/language-ext) library and I suggest you to check it out. However, this library is more compact and easy to use version of that library with a lot of examples and how to use tutorials.

## Table Of Contents

[TableOfContents]:#table-of-contents

- [How to install?](#installation)
- [What is `Maybe<T>`?](#what-is-maybe-type)
  - [Nomadic Option or `Maybe<T>`](#1---nomadic-option-or-option-type--or-maybet)
- [Contribution](#contribution)
- [License](#license)



## Installation
To use **ZeidLab.ToolBox** in your project, you can install it via NuGet:

```bash
dotnet add package ZeidLab.ToolBox
```

[^ Back To Top][TableOfContents]

## What is Maybe Type?

The concepts such as - the "Nomadic Option"  and using `Option<T>` or `Maybe<T>` instead of primitive types like `int?` - are rooted in functional programming paradigms and best practices for writing robust, maintainable code. Following sections are few explanations of these concepts based on established programming principles and best practices in C#. In this library the optional nomad type is `Maybe<T>`
### 1 - Nomadic Option (or Option Type , or `Maybe<T>`)

The "Nomadic Option" is not a standard term in C# or programming literature, but it likely refers to the Option type (or Maybe type in functional programming). The Option type is a functional programming construct used to represent the presence or absence of a value, without resorting to null. In this library it is `Maybe<T>`.

#### Why is it important?

* > It forces developers to explicitly handle the absence of a value, reducing the risk of NullReferenceException.
* > It makes the code more expressive and self-documenting by clearly indicating that a value might be missing.
* > It aligns with functional programming principles, promoting immutability and reducing side effects.

#### Why Returning null is Not a Good Idea?
Returning `null` in C# (or any language) can lead to several issues:
* > **Null Reference Exceptions:** If the caller forgets to check for `null`, it can result in runtime crashes.

* > **Ambiguity:** `null` can mean different things—missing data, an error, or an uninitialized value. This ambiguity makes code harder to understand and maintain.

* > **Poor Readability:** Code littered with `null` checks `(if (x != null))` becomes cluttered and harder to read.

* > **Violates Functional Principles:** Functional programming encourages avoiding `null` in favor of explicit types like `Maybe<T>`.

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