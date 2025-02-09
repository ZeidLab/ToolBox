using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Provides extension methods for asynchronous pattern matching operations on <see cref="Result{T}"/> types.
/// These methods enable functional-style error handling and transformation of asynchronous operations.
/// </summary>
/// <remarks>
/// This class extends the Result type system with async/await support for pattern matching operations.
/// It allows for elegant handling of asynchronous computations while maintaining the Result monad's error handling capabilities.
/// </remarks>
/// <example>
/// Basic usage with async/await:
/// <code>
/// async Task&lt;string&gt; ProcessUserAsync(int userId)
/// {
///     return await GetUserAsync(userId)
///         .MatchAsync(
///             success: user => $"User {user.Name} found",
///             failure: error => $"Error: {error.Message}"
///         );
/// }
/// </code>
///
/// Using with async transformations:
/// <code>
/// async Task&lt;Result&lt;UserProfile&gt;&gt; UpdateProfileAsync(int userId)
/// {
///     return await GetUserAsync(userId)
///         .MatchAsync(
///             success: async user => await UpdateUserProfileAsync(user),
///             failure: async error => await LogErrorAsync(error)
///         );
/// }
/// </code>
/// </example>
[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
public static class ResultExtensionsMatchAsync
{
	/// <summary>
	/// Asynchronously pattern matches a <see cref="Task{T}"/> of <see cref="Result{T}"/> and transforms its content
	/// based on whether it represents a success or failure state.
	/// </summary>
	/// <typeparam name="TIn">The type of value in the successful result case.</typeparam>
	/// <typeparam name="TOut">The type of the transformed result.</typeparam>
	/// <param name="self">The task containing a result to match against.</param>
	/// <param name="success">
	/// A function that transforms the successful value of type <typeparamref name="TIn"/> into a value of type <typeparamref name="TOut"/>.
	/// Only executed if the result represents success.
	/// </param>
	/// <param name="failure">
	/// A function that transforms a <see cref="ResultError"/> into a value of type <typeparamref name="TOut"/>.
	/// Only executed if the result represents failure.
	/// </param>
	/// <returns>
	/// A task that represents the asynchronous operation, containing the transformed value of type <typeparamref name="TOut"/>
	/// produced by either the success or failure transformation.
	/// </returns>
	/// <example>
	/// Here's how to use this method to handle different result states:
	/// <code>
	/// async Task&lt;int&gt; ProcessUserDataAsync(string userId)
	/// {
	///     return await GetUserAsync(userId)
	///         .MatchAsync(
	///             success: user => user.DataCount,        // Transform success to count
	///             failure: error => 0                     // Return 0 on failure
	///         );
	/// }
	/// </code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<TOut> MatchAsync<TIn, TOut>(this Task<Result<TIn>> self, Func<TIn, TOut> success,
		Func<ResultError, TOut> failure)
	{
		var result = await self.ConfigureAwait(false);
		return result.Match(success, failure);
	}

	/// <summary>
	/// Asynchronously pattern matches a <see cref="Task{T}"/> of <see cref="Result{T}"/> and transforms its content
	/// into a new Result instance, enabling monadic composition of Result operations.
	/// </summary>
	/// <typeparam name="TIn">The type of value in the successful result case.</typeparam>
	/// <typeparam name="TOut">The type of value in the transformed result.</typeparam>
	/// <param name="self">The task containing a result to match against.</param>
	/// <param name="success">
	/// A function that transforms the successful value of type <typeparamref name="TIn"/> into a new <see cref="Result{T}"/>
	/// of type <typeparamref name="TOut"/>. This function is called only if the result represents success.
	/// </param>
	/// <param name="failure">
	/// A function that transforms a <see cref="ResultError"/> into a new <see cref="Result{T}"/>
	/// of type <typeparamref name="TOut"/>. This function is called only if the result represents failure.
	/// </param>
	/// <returns>
	/// A task that represents the asynchronous operation, containing a new <see cref="Result{T}"/>
	/// produced by either the success or failure transformation.
	/// </returns>
	/// <example>
	/// Here's how to use this method for Result chaining:
	/// <code>
	/// async Task&lt;Result&lt;UserDetails&gt;&gt; GetUserDetailsAsync(string userId)
	/// {
	///     return await GetUserAsync(userId)
	///         .MatchAsync(
	///             success: user => ValidateAndEnrichUser(user),    // Returns Result&lt;UserDetails&gt;
	///             failure: error => Result.Failure&lt;UserDetails&gt;(
	///                 new ResultError("User details unavailable", error))
	///         );
	/// }
	/// </code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<Result<TOut>> MatchAsync<TIn, TOut>(this Task<Result<TIn>> self,
		Func<TIn, Result<TOut>> success,
		Func<ResultError, Result<TOut>> failure)
	{
		var result = await self.ConfigureAwait(false);
		return result.Match(success, failure);
	}

	/// <summary>
	/// Asynchronously pattern matches a <see cref="Task{T}"/> of <see cref="Result{T}"/> and transforms its content
	/// using asynchronous transformation functions, enabling complex async Result operations.
	/// </summary>
	/// <typeparam name="TIn">The type of value in the successful result case.</typeparam>
	/// <typeparam name="TOut">The type of value in the transformed result.</typeparam>
	/// <param name="self">The task containing a result to match against.</param>
	/// <param name="success">
	/// An asynchronous function that transforms the successful value of type <typeparamref name="TIn"/>
	/// into a Task of <see cref="Result{T}"/> of type <typeparamref name="TOut"/>.
	/// This function is called only if the result represents success.
	/// </param>
	/// <param name="failure">
	/// An asynchronous function that transforms a <see cref="ResultError"/> into a Task of <see cref="Result{T}"/>
	/// of type <typeparamref name="TOut"/>. This function is called only if the result represents failure.
	/// </param>
	/// <returns>
	/// A task that represents the asynchronous operation, containing a new <see cref="Result{T}"/>
	/// produced by awaiting either the success or failure transformation.
	/// </returns>
	/// <example>
	/// Here's how to use this method with async transformations:
	/// <code>
	/// async Task&lt;Result&lt;UserProfile&gt;&gt; UpdateUserProfileAsync(string userId)
	/// {
	///     return await GetUserAsync(userId)
	///         .MatchAsync(
	///             success: async user => await UpdateProfileInDatabaseAsync(user),  // Async DB operation
	///             failure: async error => {
	///                 await LogErrorAsync(error);                                   // Async logging
	///                 return Result.Failure&lt;UserProfile&gt;(error);
	///             }
	///         );
	/// }
	/// </code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<Result<TOut>> MatchAsync<TIn, TOut>(this Task<Result<TIn>> self,
		Func<TIn, Task<Result<TOut>>> success,
		Func<ResultError, Task<Result<TOut>>> failure)
	{
		var result = await self.ConfigureAwait(false);
		return await result.Match(success, failure).ConfigureAwait(false);
	}

	/// <summary>
	/// Asynchronously pattern matches a <see cref="TryAsync{TIn}"/> and transforms its content
	/// based on whether it represents a success or failure state.
	/// </summary>
	/// <typeparam name="TIn">The type of value in the successful result case.</typeparam>
	/// <typeparam name="TOut">The type of the transformed result.</typeparam>
	/// <param name="self">The asynchronous operation to match against.</param>
	/// <param name="success">
	/// A function that transforms the successful value of type <typeparamref name="TIn"/>
	/// into a value of type <typeparamref name="TOut"/>.
	/// Only executed if the result represents success.
	/// </param>
	/// <param name="failure">
	/// A function that transforms a <see cref="ResultError"/> into a value of type <typeparamref name="TOut"/>.
	/// Only executed if the result represents failure.
	/// </param>
	/// <returns>
	/// A task that represents the asynchronous operation, containing the transformed value of type <typeparamref name="TOut"/>
	/// produced by either the success or failure transformation.
	/// </returns>
	/// <example>
	/// Here's how to use this method to handle different result states:
	/// <code>
	/// public class UserData
	/// {
	///     public int Id { get; set; }
	///     public string Name { get; set; }
	/// }
	///
	/// // Define an async operation that might fail
	/// TryAsync&lt;UserData&gt; GetUserAsync(int userId) =>
	///     async () => {
	///         if (userId &lt; 0)
	///             return Result.Failure&lt;UserData&gt;(new ResultError("Invalid user ID"));
	///         return Result.Success(new UserData { Id = userId, Name = "John Doe" });
	///     };
	///
	/// // Using MatchAsync to transform the result
	/// async Task&lt;string&gt; GetUserDisplayAsync(int userId)
	/// {
	///     return await GetUserAsync(userId)
	///         .MatchAsync(
	///             success: user => $"User found: {user.Name}",      // Transform success to string
	///             failure: error => $"Error: {error.Message}"       // Transform error to string
	///         );
	/// }
	///
	/// // Usage example:
	/// var validResult = await GetUserDisplayAsync(1);    // Returns: "User found: John Doe"
	/// var errorResult = await GetUserDisplayAsync(-1);   // Returns: "Error: Invalid user ID"
	/// </code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<TOut> MatchAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, TOut> success,
		Func<ResultError, TOut> failure)
	{
		var result = await self.TryAsync().ConfigureAwait(false);
		return result.Match(success, failure);
	}

	/// <summary>
	/// Asynchronously pattern matches a <see cref="TryAsync{TIn}"/> and transforms its content
	/// into a new Result instance, enabling monadic composition of Result operations.
	/// </summary>
	/// <typeparam name="TIn">The type of value in the successful result case.</typeparam>
	/// <typeparam name="TOut">The type of value in the transformed result.</typeparam>
	/// <param name="self">The asynchronous operation to match against.</param>
	/// <param name="success">
	/// A function that transforms the successful value of type <typeparamref name="TIn"/> into a new
	/// <see cref="Result{TOut}"/>. This function is called only if the result represents success.
	/// </param>
	/// <param name="failure">
	/// A function that transforms a <see cref="ResultError"/> into a new <see cref="Result{TOut}"/>.
	/// This function is called only if the result represents failure.
	/// </param>
	/// <returns>
	/// A task that represents the asynchronous operation, containing a new <see cref="Result{TOut}"/>
	/// produced by either the success or failure transformation.
	/// </returns>
	/// <example>
	/// Here's how to use this method for Result chaining:
	/// <code>
	/// public class UserProfile
	/// {
	///     public int UserId { get; set; }
	///     public string Email { get; set; }
	///     public bool IsVerified { get; set; }
	/// }
	///
	/// // Define async operations that might fail
	/// TryAsync&lt;UserProfile&gt; GetUserProfileAsync(int userId) =>
	///     async () => {
	///         if (userId &lt; 0)
	///             return Result.Failure&lt;UserProfile&gt;(new ResultError("Invalid user ID"));
	///         return Result.Success(new UserProfile {
	///             UserId = userId,
	///             Email = "user@example.com"
	///         });
	///     };
	///
	/// TryAsync&lt;UserProfile&gt; ValidateProfileAsync(UserProfile profile) =>
	///     async () => {
	///         if (string.IsNullOrEmpty(profile.Email))
	///             return Result.Failure&lt;UserProfile&gt;(new ResultError("Email is required"));
	///         profile.IsVerified = true;
	///         return Result.Success(profile);
	///     };
	///
	/// // Using MatchAsync to chain validations
	/// async Task&lt;Result&lt;UserProfile&gt;&gt; CreateVerifiedProfileAsync(int userId)
	/// {
	///     return await GetUserProfileAsync(userId)
	///         .MatchAsync(
	///             success: profile => ValidateProfileAsync(profile).TryAsync(),
	///             failure: error => Result.Failure&lt;UserProfile&gt;(
	///                 new ResultError("Profile creation failed", error))
	///         );
	/// }
	///
	/// // Usage:
	/// var successResult = await CreateVerifiedProfileAsync(1);  // Returns Success with verified profile
	/// var failureResult = await CreateVerifiedProfileAsync(-1); // Returns Failure with error
	/// </code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<Result<TOut>> MatchAsync<TIn, TOut>(this TryAsync<TIn> self,
		Func<TIn, Result<TOut>> success,
		Func<ResultError, Result<TOut>> failure)
	{
		var result = await self.TryAsync().ConfigureAwait(false);
		return result.Match(success, failure);
	}

	/// <summary>
	/// Asynchronously pattern matches a <see cref="TryAsync{TIn}"/> and transforms its content using asynchronous functions,
	/// allowing for complex transformations that require async operations.
	/// </summary>
	/// <typeparam name="TIn">The type of value in the successful result case.</typeparam>
	/// <typeparam name="TOut">The type of the transformed result.</typeparam>
	/// <param name="self">The asynchronous operation to match against.</param>
	/// <param name="success">
	/// An asynchronous function that transforms the successful value of type <typeparamref name="TIn"/>
	/// into a Task of <typeparamref name="TOut"/>. This function is called only if the result represents success.
	/// </param>
	/// <param name="failure">
	/// An asynchronous function that transforms a <see cref="ResultError"/> into a Task of <typeparamref name="TOut"/>.
	/// This function is called only if the result represents failure.
	/// </param>
	/// <returns>
	/// A task that represents the asynchronous operation, containing the transformed value of type <typeparamref name="TOut"/>
	/// produced by awaiting either the success or failure transformation.
	/// </returns>
	/// <example>
	/// Here's how to use this method with async transformations:
	/// <code>
	/// public class UserMetrics
	/// {
	///     public int UserId { get; set; }
	///     public int PostCount { get; set; }
	///     public int FollowerCount { get; set; }
	/// }
	///
	/// // Define async operations
	/// TryAsync&lt;int&gt; GetUserIdAsync(string username) =>
	///     async () => {
	///         if (string.IsNullOrEmpty(username))
	///             return Result.Failure&lt;int&gt;(new ResultError("Username is required"));
	///         // Simulate database lookup
	///         return Result.Success(123);
	///     };
	///
	/// // Async functions for metrics calculation
	/// async Task&lt;UserMetrics&gt; CalculateMetricsAsync(int userId)
	/// {
	///     // Simulate parallel API calls
	///     var postCountTask = Task.FromResult(42);      // Posts count
	///     var followerCountTask = Task.FromResult(1000); // Followers count
	///
	///     await Task.WhenAll(postCountTask, followerCountTask);
	///
	///     return new UserMetrics {
	///         UserId = userId,
	///         PostCount = await postCountTask,
	///         FollowerCount = await followerCountTask
	///     };
	/// }
	///
	/// // Using MatchAsync with async transformations
	/// async Task&lt;UserMetrics&gt; GetUserMetricsAsync(string username)
	/// {
	///     return await GetUserIdAsync(username)
	///         .MatchAsync(
	///             success: async userId => await CalculateMetricsAsync(userId),
	///             failure: async error => {
	///                 await LogErrorAsync(error);  // Assume logging is defined
	///                 return new UserMetrics();    // Return empty metrics on failure
	///             }
	///         );
	/// }
	///
	/// // Usage:
	/// var metrics = await GetUserMetricsAsync("johndoe");  // Returns populated metrics
	/// var empty = await GetUserMetricsAsync("");          // Returns empty metrics
	/// </code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<TOut> MatchAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, Task<TOut>> success,
		Func<ResultError, Task<TOut>> failure)
	{
		var result = await self.TryAsync().ConfigureAwait(false);
		return result.IsSuccess
			? await success(result.Value).ConfigureAwait(false)
			: await failure(result.Error).ConfigureAwait(false);
	}

	/// <summary>
	/// Asynchronously pattern matches a <see cref="TryAsync{TIn}"/> and transforms its content using asynchronous functions,
	/// enabling monadic composition of Result operations with full async support. This method is particularly useful for
	/// complex transformations that require multiple async operations and error handling.
	/// </summary>
	/// <typeparam name="TIn">The type of value in the successful result case.</typeparam>
	/// <typeparam name="TOut">The type of value in the transformed result.</typeparam>
	/// <param name="self">The asynchronous operation to match against.</param>
	/// <param name="success">
	/// An asynchronous function that transforms the successful value of type <typeparamref name="TIn"/>
	/// into a Task of <see cref="Result{TOut}"/>. This function is called only if the result represents success.
	/// </param>
	/// <param name="failure">
	/// An asynchronous function that transforms a <see cref="ResultError"/> into a Task of <see cref="Result{TOut}"/>.
	/// This function is called only if the result represents failure.
	/// </param>
	/// <returns>
	/// A task that represents the asynchronous operation, containing a new <see cref="Result{TOut}"/>
	/// produced by awaiting either the success or failure transformation.
	/// </returns>
	/// <example>
	/// Here's how to use this method for complex async operations with error handling:
	/// <code>
	/// public class OrderDetails
	/// {
	///     public int OrderId { get; set; }
	///     public string CustomerEmail { get; set; }
	///     public decimal TotalAmount { get; set; }
	///     public bool IsProcessed { get; set; }
	/// }
	///
	/// // Define async operations that might fail
	/// TryAsync&lt;OrderDetails&gt; CreateOrderAsync(int orderId) =>
	///     async () => {
	///         if (orderId &lt; 0)
	///             return Result.Failure&lt;OrderDetails&gt;(new ResultError("Invalid order ID"));
	///         return Result.Success(new OrderDetails {
	///             OrderId = orderId,
	///             CustomerEmail = "customer@example.com",
	///             TotalAmount = 99.99m
	///         });
	///     };
	///
	/// async Task&lt;Result&lt;OrderDetails&gt;&gt; ProcessPaymentAsync(OrderDetails order)
	/// {
	///     try {
	///         await Task.Delay(100); // Simulate payment processing
	///         if (order.TotalAmount &lt;= 0)
	///             return Result.Failure&lt;OrderDetails&gt;(new ResultError("Invalid amount"));
	///
	///         order.IsProcessed = true;
	///         return Result.Success(order);
	///     }
	///     catch (Exception ex) {
	///         return Result.Failure&lt;OrderDetails&gt;(
	///             new ResultError("Payment processing failed", ex));
	///     }
	/// }
	///
	/// async Task&lt;Result&lt;OrderDetails&gt;&gt; SendConfirmationEmailAsync(OrderDetails order)
	/// {
	///     try {
	///         if (string.IsNullOrEmpty(order.CustomerEmail))
	///             return Result.Failure&lt;OrderDetails&gt;(
	///                 new ResultError("Customer email is required"));
	///
	///         await Task.Delay(100); // Simulate email sending
	///         return Result.Success(order);
	///     }
	///     catch (Exception ex) {
	///         return Result.Failure&lt;OrderDetails&gt;(
	///             new ResultError("Failed to send confirmation email", ex));
	///     }
	/// }
	///
	/// // Using MatchAsync to compose a complex workflow
	/// async Task&lt;Result&lt;OrderDetails&gt;&gt; ProcessOrderAsync(int orderId)
	/// {
	///     return await CreateOrderAsync(orderId)
	///         .MatchAsync(
	///             success: async order => {
	///                 // Process payment first
	///                 var paymentResult = await ProcessPaymentAsync(order);
	///                 if (!paymentResult.IsSuccess)
	///                     return paymentResult;
	///
	///                 // Then send confirmation email
	///                 return await SendConfirmationEmailAsync(order);
	///             },
	///             failure: async error => {
	///                 await LogErrorAsync(error);  // Assume logging is defined
	///                 return Result.Failure&lt;OrderDetails&gt;(
	///                     new ResultError("Order processing failed", error));
	///             }
	///         );
	/// }
	///
	/// // Usage:
	/// var success = await ProcessOrderAsync(1);    // Returns Success with processed order
	/// var failure = await ProcessOrderAsync(-1);   // Returns Failure with error
	/// </code>
	/// </example>
	[Pure]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<Result<TOut>> MatchAsync<TIn, TOut>(this TryAsync<TIn> self,
		Func<TIn, Task<Result<TOut>>> success,
		Func<ResultError, Task<Result<TOut>>> failure)
		=> self.TryAsync().MatchAsync(success, failure);

	/// <summary>
	/// Asynchronously pattern matches a task containing a Result instance and executes the appropriate action
	/// based on whether the result represents a success or failure state. This method is useful for
	/// handling different outcomes of asynchronous operations without returning a value.
	/// </summary>
	/// <typeparam name="TIn">The type of value in the successful result case.</typeparam>
	/// <param name="self">The task containing a result to match against.</param>
	/// <param name="success">
	/// An action that processes the successful value of type <typeparamref name="TIn"/>.
	/// This action is executed only if the result represents success.
	/// </param>
	/// <param name="failure">
	/// An action that processes a <see cref="ResultError"/>.
	/// This action is executed only if the result represents failure.
	/// </param>
	/// <returns>
	/// A task representing the asynchronous operation. The task completes when the appropriate
	/// action has been executed.
	/// </returns>
	/// <example>
	/// Here's how to use this method for logging or side effects:
	/// <code>
	/// async Task ProcessOrderResultAsync(int orderId)
	/// {
	///     await GetOrderAsync(orderId)
	///         .MatchAsync(
	///             success: order => Console.WriteLine($"Order {order.Id} processed"),
	///             failure: error => Console.WriteLine($"Error: {error.Message}")
	///         );
	/// }
	/// </code>
	/// </example>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task MatchAsync<TIn>(this Task<Result<TIn>> self, Action<TIn> success,
		Action<ResultError> failure)
	{
		var result = await self.ConfigureAwait(false);
		if (result.IsSuccess)
			success(result.Value);
		else
			failure(result.Error);
	}

	/// <summary>
	/// Asynchronously pattern matches a <see cref="TryAsync{TIn}"/> instance and executes the appropriate action
	/// based on whether the result represents a success or failure state. This method is useful for
	/// handling different outcomes of asynchronous operations without returning a value.
	/// </summary>
	/// <typeparam name="TIn">The type of value in the successful result case.</typeparam>
	/// <param name="self">The asynchronous operation to match against.</param>
	/// <param name="success">
	/// An action that processes the successful value of type <typeparamref name="TIn"/>.
	/// This action is executed only if the result represents success.
	/// </param>
	/// <param name="failure">
	/// An action that processes a <see cref="ResultError"/>.
	/// This action is executed only if the result represents failure.
	/// </param>
	/// <returns>
	/// A task representing the asynchronous operation. The task completes when the appropriate
	/// action has been executed.
	/// </returns>
	/// <example>
	/// Here's how to use this method for logging or notifications:
	/// <code>
	/// async Task ProcessUserResultAsync(int userId)
	/// {
	///     await GetUserAsync(userId)
	///         .MatchAsync(
	///             success: user => Console.WriteLine($"User {user.Name} found"),
	///             failure: error => Console.WriteLine($"Error: {error.Message}")
	///         );
	/// }
	///
	/// // Example with database operations:
	/// async Task UpdateUserStatusAsync(int userId)
	/// {
	///     await GetUserAsync(userId)
	///         .MatchAsync(
	///             success: async user => {
	///                 user.IsActive = true;
	///                 await SaveUserAsync(user);
	///             },
	///             failure: error => LogErrorAsync(error)
	///         );
	/// }
	/// </code>
	/// </example>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task MatchAsync<TIn>(this TryAsync<TIn> self, Action<TIn> success, Action<ResultError> failure)
	{
		var result = await self.TryAsync().ConfigureAwait(false);
		if (result.IsSuccess)
			success(result.Value);
		else
			failure(result.Error);
	}

	/// <summary>
	/// Asynchronously pattern matches a <see cref="Result{TIn}"/> instance and executes the appropriate asynchronous action
	/// based on whether the result represents a success or failure state. This method is useful for
	/// handling different outcomes of operations with asynchronous side effects.
	/// </summary>
	/// <typeparam name="TIn">The type of value in the successful result case.</typeparam>
	/// <param name="self">The result to match against.</param>
	/// <param name="success">
	/// An asynchronous function that processes the successful value of type <typeparamref name="TIn"/>.
	/// This function is executed only if the result represents success.
	/// </param>
	/// <param name="failure">
	/// An asynchronous function that processes a <see cref="ResultError"/>.
	/// This function is executed only if the result represents failure.
	/// </param>
	/// <returns>
	/// A task representing the asynchronous operation. The task completes when the appropriate
	/// asynchronous action has completed.
	/// </returns>
	/// <example>
	/// Here's how to use this method for async operations:
	/// <code>
	/// async Task ProcessUserResultAsync(Result&lt;User&gt; userResult)
	/// {
	///     await userResult.MatchAsync(
	///         success: async user => {
	///             await SendWelcomeEmailAsync(user);
	///             await UpdateUserStatusAsync(user, UserStatus.Active);
	///         },
	///         failure: async error => {
	///             await LogErrorAsync(error);
	///             await NotifyAdminAsync(error);
	///         }
	///     );
	/// }
	///
	/// // Example with database operations:
	/// async Task UpdateUserProfileAsync(Result&lt;User&gt; userResult)
	/// {
	///     await userResult.MatchAsync(
	///         success: async user => {
	///             user.LastLogin = DateTime.UtcNow;
	///             await SaveUserAsync(user);
	///         },
	///         failure: async error => {
	///             await LogErrorAsync(error);
	///             await CreateAuditEntryAsync(error);
	///         }
	///     );
	/// }
	/// </code>
	/// </example>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task MatchAsync<TIn>(this Result<TIn> self, Func<TIn, Task> success,
		Func<ResultError, Task> failure)
	{
		if (self.IsSuccess)
			await success(self.Value).ConfigureAwait(false);
		else
			await failure(self.Error).ConfigureAwait(false);
	}

	/// <summary>
	/// Asynchronously pattern matches a <see cref="Try{TIn}"/> instance and executes the appropriate asynchronous action
	/// based on whether the operation represents a success or failure state. This method is useful for
	/// handling different outcomes of potentially failing operations with asynchronous side effects.
	/// </summary>
	/// <typeparam name="TIn">The type of value in the successful result case.</typeparam>
	/// <param name="self">The potentially failing operation to match against.</param>
	/// <param name="success">
	/// An asynchronous function that processes the successful value of type <typeparamref name="TIn"/>.
	/// This function is executed only if the operation succeeds.
	/// </param>
	/// <param name="failure">
	/// An asynchronous function that processes a <see cref="ResultError"/>.
	/// This function is executed only if the operation fails.
	/// </param>
	/// <returns>
	/// A task representing the asynchronous operation. The task completes when the appropriate
	/// asynchronous action has completed.
	/// </returns>
	/// <example>
	/// Here's how to use this method with Try operations:
	/// <code>
	/// async Task ProcessUserDataAsync(Try&lt;User&gt; userTry)
	/// {
	///     await userTry.MatchAsync(
	///         success: async user => {
	///             await UpdateUserLastLoginAsync(user);
	///             await SendWelcomeMessageAsync(user);
	///         },
	///         failure: async error => {
	///             await LogErrorAsync(error);
	///             await NotifySupportTeamAsync(error);
	///         }
	///     );
	/// }
	///
	/// // Example with database operations:
	/// async Task UpdateUserProfileAsync(Try&lt;User&gt; userTry)
	/// {
	///     await userTry.MatchAsync(
	///         success: async user => {
	///             user.ProfileUpdatedAt = DateTime.UtcNow;
	///             await SaveUserAsync(user);
	///         },
	///         failure: async error => {
	///             await LogErrorAsync(error);
	///             await CreateAuditEntryAsync(error);
	///         }
	///     );
	/// }
	/// </code>
	/// </example>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task MatchAsync<TIn>(this Try<TIn> self, Func<TIn, Task> success,
		Func<ResultError, Task> failure)
	{
		var result = self.Try();
		if (result.IsSuccess)
			await success(result.Value).ConfigureAwait(false);
		else
			await failure(result.Error).ConfigureAwait(false);
	}

	/// <summary>
	/// Asynchronously pattern matches a <see cref="Task{T}"/> of <see cref="Result{TIn}"/> instance and executes the appropriate asynchronous action
	/// based on whether the result represents a success or failure state. This method is useful for
	/// handling different outcomes of asynchronous operations with side effects.
	/// </summary>
	/// <typeparam name="TIn">The type of value in the successful result case.</typeparam>
	/// <param name="self">The task containing the result to match against.</param>
	/// <param name="success">
	/// An asynchronous function that processes the successful value of type <typeparamref name="TIn"/>.
	/// This function is executed only if the result represents success.
	/// </param>
	/// <param name="failure">
	/// An asynchronous function that processes a <see cref="ResultError"/>.
	/// This function is executed only if the result represents failure.
	/// </param>
	/// <returns>
	/// A task representing the asynchronous operation. The task completes when the appropriate
	/// asynchronous action has completed.
	/// </returns>
	/// <example>
	/// Here's how to use this method with async operations:
	/// <code>
	/// async Task ProcessUserDataAsync(Task&lt;Result&lt;User&gt;&gt; userTask)
	/// {
	///     await userTask.MatchAsync(
	///         success: async user => {
	///             await UpdateUserLastLoginAsync(user);
	///             await SendWelcomeMessageAsync(user);
	///         },
	///         failure: async error => {
	///             await LogErrorAsync(error);
	///             await NotifySupportTeamAsync(error);
	///         }
	///     );
	/// }
	///
	/// // Example with database operations:
	/// async Task UpdateUserProfileAsync(Task&lt;Result&lt;User&gt;&gt; userTask)
	/// {
	///     await userTask.MatchAsync(
	///         success: async user => {
	///             user.ProfileUpdatedAt = DateTime.UtcNow;
	///             await SaveUserAsync(user);
	///         },
	///         failure: async error => {
	///             await LogErrorAsync(error);
	///             await CreateAuditEntryAsync(error);
	///         }
	///     );
	/// }
	/// </code>
	/// </example>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task MatchAsync<TIn>(this Task<Result<TIn>> self, Func<TIn, Task> success,
		Func<ResultError, Task> failure)
	{
		var result = await self.ConfigureAwait(false);
		if (result.IsSuccess)
			await success(result.Value).ConfigureAwait(false);
		else
			await failure(result.Error).ConfigureAwait(false);
	}

	/// <summary>
	/// Asynchronously pattern matches a <see cref="TryAsync{TIn}"/> instance and executes the appropriate asynchronous action
	/// based on whether the operation represents a success or failure state. This method is useful for
	/// handling different outcomes of potentially failing operations with asynchronous side effects.
	/// </summary>
	/// <typeparam name="TIn">The type of value in the successful result case.</typeparam>
	/// <param name="self">The potentially failing asynchronous operation to match against.</param>
	/// <param name="success">
	/// An asynchronous function that processes the successful value of type <typeparamref name="TIn"/>.
	/// This function is executed only if the operation succeeds.
	/// </param>
	/// <param name="failure">
	/// An asynchronous function that processes a <see cref="ResultError"/>.
	/// This function is executed only if the operation fails.
	/// </param>
	/// <returns>
	/// A task representing the asynchronous operation. The task completes when the appropriate
	/// asynchronous action has completed.
	/// </returns>
	/// <example>
	/// Here's how to use this method with TryAsync operations:
	/// <code>
	/// async Task ProcessUserDataAsync(TryAsync&lt;User&gt; userTry)
	/// {
	///     await userTry.MatchAsync(
	///         success: async user => {
	///             await UpdateUserLastLoginAsync(user);
	///             await SendWelcomeMessageAsync(user);
	///         },
	///         failure: async error => {
	///             await LogErrorAsync(error);
	///             await NotifySupportTeamAsync(error);
	///         }
	///     );
	/// }
	///
	/// // Example with database operations:
	/// async Task UpdateUserProfileAsync(TryAsync&lt;User&gt; userTry)
	/// {
	///     await userTry.MatchAsync(
	///         success: async user => {
	///             user.ProfileUpdatedAt = DateTime.UtcNow;
	///             await SaveUserAsync(user);
	///         },
	///         failure: async error => {
	///             await LogErrorAsync(error);
	///             await CreateAuditEntryAsync(error);
	///         }
	///     );
	/// }
	/// </code>
	/// </example>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task MatchAsync<TIn>(this TryAsync<TIn> self, Func<TIn, Task> success,
		Func<ResultError, Task> failure)
	{
		var result = await self.TryAsync().ConfigureAwait(false);
		if (result.IsSuccess)
			await success(result.Value).ConfigureAwait(false);
		else
			await failure(result.Error).ConfigureAwait(false);
	}
}