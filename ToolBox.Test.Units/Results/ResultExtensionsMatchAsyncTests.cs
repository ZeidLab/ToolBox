using FluentAssertions;
using ZeidLab.ToolBox.Results;
using static ZeidLab.ToolBox.Test.Units.Results.TestHelper;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace ZeidLab.ToolBox.Test.Units.Results
{
    public class ResultExtensionsMatchAsyncTests
    {
        #region MatchAsync(Task<Result<TIn>>, Func<TIn, TOut>, Func<ResultError, TOut>)

        [Fact]
        public async Task MatchAsync_SuccessResult_ExecutesSuccessHandler()
        {
            // Arrange
            var successValue = 42;
            var task = Task.FromResult(Result.Success(successValue));

            // Act
            var result = await task.MatchAsync(
                success: value => value.ToString(),
                failure: _ => "error"
            );

            // Assert
            result.Should().Be("42");
        }

        [Fact]
        public async Task MatchAsync_FailureResult_ExecutesFailureHandler()
        {
            // Arrange
            var error = DefaultResultError;
            var task = Task.FromResult(Result.Failure<int>(error));

            // Act
            var result = await task.MatchAsync(
                success: _ => "success",
                failure: err => err.Message
            );

            // Assert
            result.Should().Be(DefaultResultError.Message);
        }

        #endregion

        #region MatchAsync(Task<Result<TIn>>, Func<TIn, Result<TOut>>, Func<ResultError, Result<TOut>>)

        [Fact]
        public async Task MatchAsync_SuccessResult_ReturnsTransformedResult()
        {
            // Arrange
            var successValue = 42;
            var task = Task.FromResult(Result.Success(successValue));

            // Act
            var result = await task.MatchAsync(
                success: value => Result.Success(value.ToString()),
                failure: _ => Result.Failure<string>(DefaultResultError)
            );

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be("42");
        }

        [Fact]
        public async Task MatchAsync_FailureResult_ReturnsTransformedError()
        {
            // Arrange
            var error = DefaultResultError;
            var task = Task.FromResult(Result.Failure<int>(error));

            // Act
            var result = await task.MatchAsync(
                success: _ => Result.Success("success"),
                failure: err => Result.Failure<string>(err)
            );

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(error);
        }

        #endregion

        #region MatchAsync(Task<Result<TIn>>, Func<TIn, Task<Result<TOut>>>, Func<ResultError, Task<Result<TOut>>>)

        [Fact]
        public async Task MatchAsync_SuccessResult_ReturnsTransformedResultAsync()
        {
            // Arrange
            var successValue = 42;
            var task = Task.FromResult(Result.Success(successValue));

            // Act
            var result = await task.MatchAsync(
                success: async value => {
                    await Task.Delay(10);
                    return Result.Success(value.ToString());
                },
                failure: async _ => {
                    await Task.Delay(10);
                    return Result.Failure<string>(DefaultResultError);
                }
            );

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be("42");
        }

        [Fact]
        public async Task MatchAsync_FailureResult_ReturnsTransformedErrorAsync()
        {
            // Arrange
            var error = DefaultResultError;
            var task = Task.FromResult(Result.Failure<int>(error));

            // Act
            var result = await task.MatchAsync(
                success: async _ => {
                    await Task.Delay(10);
                    return Result.Success("success");
                },
                failure: async err => {
                    await Task.Delay(10);
                    return Result.Failure<string>(err);
                }
            );

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(error);
        }

        #endregion

        #region MatchAsync(TryAsync<TIn>, Func<TIn, TOut>, Func<ResultError, TOut>)

        [Fact]
        public async Task MatchAsync_TryAsyncSuccess_ExecutesSuccessHandler()
        {
            // Arrange
            var successValue = 42;
            var tryAsync = new TryAsync<int>(() => Task.FromResult(Result.Success(successValue)));

            // Act
            var result = await tryAsync.MatchAsync(
                success: value => value.ToString(),
                failure: _ => "error"
            );

            // Assert
            result.Should().Be("42");
        }

        [Fact]
        public async Task MatchAsync_TryAsyncFailure_ExecutesFailureHandler()
        {
            // Arrange
            var error = DefaultResultError;
            var tryAsync = new TryAsync<int>(() => Task.FromResult(Result.Failure<int>(error)));

            // Act
            var result = await tryAsync.MatchAsync(
                success: _ => "success",
                failure: err => err.Message
            );

            // Assert
            result.Should().Be(DefaultResultError.Message);
        }

        #endregion

        #region MatchAsync(TryAsync<TIn>, Func<TIn, Result<TOut>>, Func<ResultError, Result<TOut>>)

        [Fact]
        public async Task MatchAsync_TryAsyncSuccess_ReturnsTransformedResult()
        {
            // Arrange
            var successValue = 42;
            var tryAsync = new TryAsync<int>(() => Task.FromResult(Result.Success(successValue)));

            // Act
            var result = await tryAsync.MatchAsync(
                success: value => Result.Success(value.ToString()),
                failure: _ => Result.Failure<string>(DefaultResultError)
            );

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be("42");
        }

        [Fact]
        public async Task MatchAsync_TryAsyncFailure_ReturnsTransformedError()
        {
            // Arrange
            var error = DefaultResultError;
            var tryAsync = new TryAsync<int>(() => Task.FromResult(Result.Failure<int>(error)));

            // Act
            var result = await tryAsync.MatchAsync(
                success: _ => Result.Success("success"),
                failure: err => Result.Failure<string>(err)
            );

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(error);
        }

        #endregion

        #region MatchAsync(TryAsync<TIn>, Func<TIn, Task<TOut>>, Func<ResultError, Task<TOut>>)

        [Fact]
        public async Task MatchAsync_TryAsyncSuccess_ReturnsTransformedValueAsync()
        {
            // Arrange
            var successValue = 42;
            var tryAsync = new TryAsync<int>(() => Task.FromResult(Result.Success(successValue)));

            // Act
            var result = await tryAsync.MatchAsync(
                success: async value => {
                    await Task.Delay(10);
                    return value.ToString();
                },
                failure: async _ => {
                    await Task.Delay(10);
                    return "error";
                }
            );

            // Assert
            result.Should().Be("42");
        }

        [Fact]
        public async Task MatchAsync_TryAsyncFailure_ReturnsTransformedErrorAsync()
        {
            // Arrange
            var error = DefaultResultError;
            var tryAsync = new TryAsync<int>(() => Task.FromResult(Result.Failure<int>(error)));

            // Act
            var result = await tryAsync.MatchAsync(
                success: async _ => {
                    await Task.Delay(10);
                    return "success";
                },
                failure: async err => {
                    await Task.Delay(10);
                    return err.Message;
                }
            );

            // Assert
            result.Should().Be(DefaultResultError.Message);
        }

        #endregion

        #region MatchAsync(TryAsync<TIn>, Func<TIn, Task<Result<TOut>>>, Func<ResultError, Task<Result<TOut>>>)

        [Fact]
        public async Task MatchAsync_TryAsyncSuccessWithTaskResult_ReturnsTransformedResultAsync()
        {
            // Arrange
            var successValue = 42;
            var tryAsync = new TryAsync<int>(() => Task.FromResult(Result.Success(successValue)));

            // Act
            var result = await tryAsync.MatchAsync(
                success: async value => {
                    await Task.Delay(10);
                    return Result.Success(value.ToString());
                },
                failure: async _ => {
                    await Task.Delay(10);
                    return Result.Failure<string>(DefaultResultError);
                }
            );

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be("42");
        }

        [Fact]
        public async Task MatchAsync_TryAsyncFailureWithTaskResult_ReturnsTransformedErrorAsync()
        {
            // Arrange
            var error = DefaultResultError;
            var tryAsync = new TryAsync<int>(() => Task.FromResult(Result.Failure<int>(error)));

            // Act
            var result = await tryAsync.MatchAsync(
                success: async _ => {
                    await Task.Delay(10);
                    return Result.Success("success");
                },
                failure: async err => {
                    await Task.Delay(10);
                    return Result.Failure<string>(err);
                }
            );

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(error);
        }

        #endregion

        #region MatchAsync(Task<Result<TIn>>, Action<TIn>, Action<ResultError>)

        [Fact]
        public async Task MatchAsync_SuccessResult_ExecutesSuccessAction()
        {
            // Arrange
            var successValue = 42;
            var task = Task.FromResult(Result.Success(successValue));
            var executed = false;

            // Act
            await task.MatchAsync(
                success: value => {
                    executed = true;
                    value.Should().Be(42);
                },
                failure: _ => throw new Exception("Should not execute failure action")
            );

            // Assert
            executed.Should().BeTrue();
        }

        [Fact]
        public async Task MatchAsync_FailureResult_ExecutesFailureAction()
        {
            // Arrange
            var error = DefaultResultError;
            var task = Task.FromResult(Result.Failure<int>(error));
            var executed = false;

            // Act
            await task.MatchAsync(
                success: _ => throw new Exception("Should not execute success action"),
                failure: err => {
                    executed = true;
                    err.Should().Be(error);
                }
            );

            // Assert
            executed.Should().BeTrue();
        }

        #endregion

        #region MatchAsync(TryAsync<TIn>, Action<TIn>, Action<ResultError>)

        [Fact]
        public async Task MatchAsync_TryAsyncSuccess_ExecutesSuccessAction()
        {
            // Arrange
            var successValue = 42;
            var tryAsync = new TryAsync<int>(() => Task.FromResult(Result.Success(successValue)));
            var executed = false;

            // Act
            await tryAsync.MatchAsync(
                success: value => {
                    executed = true;
                    value.Should().Be(42);
                },
                failure: _ => throw new Exception("Should not execute failure action")
            );

            // Assert
            executed.Should().BeTrue();
        }

        [Fact]
        public async Task MatchAsync_TryAsyncFailure_ExecutesFailureAction()
        {
            // Arrange
            var error = DefaultResultError;
            var tryAsync = new TryAsync<int>(() => Task.FromResult(Result.Failure<int>(error)));
            var executed = false;

            // Act
            await tryAsync.MatchAsync(
                success: _ => throw new Exception("Should not execute success action"),
                failure: err => {
                    executed = true;
                    err.Should().Be(error);
                }
            );

            // Assert
            executed.Should().BeTrue();
        }

        #endregion

        #region MatchAsync(Result<TIn>, Func<TIn, Task>, Func<ResultError, Task>)

        [Fact]
        public async Task MatchAsync_SuccessResult_ExecutesSuccessTask()
        {
            // Arrange
            var successValue = 42;
            var result = Result.Success(successValue);
            var executed = false;

            // Act
            await result.MatchAsync(
                success: async value => {
                    await Task.Delay(10);
                    executed = true;
                    value.Should().Be(42);
                },
                failure: async _ => throw new Exception("Should not execute failure task")
            );

            // Assert
            executed.Should().BeTrue();
        }

        [Fact]
        public async Task MatchAsync_FailureResult_ExecutesFailureTask()
        {
            // Arrange
            var error = DefaultResultError;
            var result = Result.Failure<int>(error);
            var executed = false;

            // Act
            await result.MatchAsync(
                success: async _ => throw new Exception("Should not execute success task"),
                failure: async err => {
                    await Task.Delay(10);
                    executed = true;
                    err.Should().Be(error);
                }
            );

            // Assert
            executed.Should().BeTrue();
        }

        #endregion

        #region MatchAsync(Try<TIn>, Func<TIn, Task>, Func<ResultError, Task>)

        [Fact]
        public async Task MatchAsync_TrySuccess_ExecutesSuccessTask()
        {
            // Arrange
            var successValue = 42;
            var tryOp = new Try<int>(() => Result.Success(successValue));
            var executed = false;

            // Act
            await tryOp.MatchAsync(
                success: async value => {
                    await Task.Delay(10);
                    executed = true;
                    value.Should().Be(42);
                },
                failure: async _ => throw new Exception("Should not execute failure task")
            );

            // Assert
            executed.Should().BeTrue();
        }

        [Fact]
        public async Task MatchAsync_TryFailure_ExecutesFailureTask()
        {
            // Arrange
            var error = DefaultResultError;
            var tryOp = new Try<int>(() => Result.Failure<int>(error));
            var executed = false;

            // Act
            await tryOp.MatchAsync(
                success: async _ => throw new Exception("Should not execute success task"),
                failure: async err => {
                    await Task.Delay(10);
                    executed = true;
                    err.Should().Be(error);
                }
            );

            // Assert
            executed.Should().BeTrue();
        }

        #endregion

        #region MatchAsync(Task<Result<TIn>>, Func<TIn, Task>, Func<ResultError, Task>)

        [Fact]
        public async Task MatchAsync_TaskSuccess_ExecutesSuccessTask()
        {
            // Arrange
            var successValue = 42;
            var task = Task.FromResult(Result.Success(successValue));
            var executed = false;

            // Act
            await task.MatchAsync(
                success: async value => {
                    await Task.Delay(10);
                    executed = true;
                    value.Should().Be(42);
                },
                failure: async _ => throw new Exception("Should not execute failure task")
            );

            // Assert
            executed.Should().BeTrue();
        }

        [Fact]
        public async Task MatchAsync_TaskFailure_ExecutesFailureTask()
        {
            // Arrange
            var error = DefaultResultError;
            var task = Task.FromResult(Result.Failure<int>(error));
            var executed = false;

            // Act
            await task.MatchAsync(
                success: async _ => throw new Exception("Should not execute success task"),
                failure: async err => {
                    await Task.Delay(10);
                    executed = true;
                    err.Should().Be(error);
                }
            );

            // Assert
            executed.Should().BeTrue();
        }

        #endregion

        #region MatchAsync(TryAsync<TIn>, Func<TIn, Task>, Func<ResultError, Task>)

        [Fact]
        public async Task MatchAsync_TryAsyncSuccess_ExecutesSuccessTask()
        {
            // Arrange
            var successValue = 42;
            var tryAsync = new TryAsync<int>(() => Task.FromResult(Result.Success(successValue)));
            var executed = false;

            // Act
            await tryAsync.MatchAsync(
                success: async value => {
                    await Task.Delay(10);
                    executed = true;
                    value.Should().Be(42);
                },
                failure: async _ => throw new Exception("Should not execute failure task")
            );

            // Assert
            executed.Should().BeTrue();
        }

        [Fact]
        public async Task MatchAsync_TryAsyncFailure_ExecutesFailureTask()
        {
            // Arrange
            var error = DefaultResultError;
            var tryAsync = new TryAsync<int>(() => Task.FromResult(Result.Failure<int>(error)));
            var executed = false;

            // Act
            await tryAsync.MatchAsync(
                success: async _ => throw new Exception("Should not execute success task"),
                failure: async err => {
                    await Task.Delay(10);
                    executed = true;
                    err.Should().Be(error);
                }
            );

            // Assert
            executed.Should().BeTrue();
        }

        #endregion

        #region MatchAsync(TryAsync<TIn>, Func<TIn, Task<Result<TOut>>>, Func<ResultError, Task<Result<TOut>>>)

        [Fact]
        public async Task MatchAsync_TryAsyncSuccessWithTaskResultHandler_ReturnsTransformedResultAsync()
        {
            // Arrange
            var successValue = 42;
            var tryAsync = new TryAsync<int>(() => Task.FromResult(Result.Success(successValue)));

            // Act
            var result = await tryAsync.MatchAsync(
                success: async value => {
                    await Task.Delay(10);
                    return Result.Success(value.ToString());
                },
                failure: async _ => {
                    await Task.Delay(10);
                    return Result.Failure<string>(DefaultResultError);
                }
            );

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be("42");
        }

        [Fact]
        public async Task MatchAsync_TryAsyncFailureWithTaskResultHandler_ReturnsTransformedErrorAsync()
        {
            // Arrange
            var error = DefaultResultError;
            var tryAsync = new TryAsync<int>(() => Task.FromResult(Result.Failure<int>(error)));

            // Act
            var result = await tryAsync.MatchAsync(
                success: async _ => {
                    await Task.Delay(10);
                    return Result.Success("success");
                },
                failure: async err => {
                    await Task.Delay(10);
                    return Result.Failure<string>(err);
                }
            );

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(error);
        }

        #endregion
    }
}