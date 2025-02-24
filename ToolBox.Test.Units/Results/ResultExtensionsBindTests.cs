using FluentAssertions;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results
{
    public class ResultExtensionsBindTests
    {
        #region Bind<TIn, TOut>(this Result<TIn> self, Func<TIn, Result<TOut>> func)

        [Fact]
        public void Bind_SuccessfulResult_ShouldReturnTransformedResult()
        {
            // Arrange
            Result<int> result = TestHelper.CreateSuccessResult(10);
            Func<int, Result<string>> func = x => Result.Success((x * 2).ToString());

            // Act
            var transformedResult = result.Bind(func);

            // Assert
            transformedResult.IsSuccess.Should().BeTrue();
            transformedResult.Value.Should().Be("20");
        }

        [Fact]
        public void Bind_FailedResult_ShouldPropagateError()
        {
            // Arrange
            Result<int> result = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
            Func<int, Result<string>> func = x => Result.Success((x * 2).ToString());

            // Act
            var transformedResult = result.Bind(func);

            // Assert
            transformedResult.IsFailure.Should().BeTrue();
            transformedResult.Error.Message.Should().Be(TestHelper.DefaultResultError.Message);
        }

        #endregion

        #region Bind<TIn, TOut>(this Result<TIn> self, Func<TIn, Try<TOut>> func)

        [Fact]
        public void Bind_TrySuccessfulResult_ShouldReturnTransformedResult()
        {
            // Arrange
            Result<int> result = TestHelper.CreateSuccessResult(10);
            Func<int, Try<string>> func = x => () => Result.Success((x * 2).ToString());

            // Act
            var transformedResult = result.Bind(func);

            // Assert
            transformedResult.IsSuccess.Should().BeTrue();
            transformedResult.Value.Should().Be("20");
        }

        [Fact]
        public void Bind_TryFailedResult_ShouldPropagateError()
        {
            // Arrange
            Result<int> result = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
            Func<int, Try<string>> func = x => () => Result.Success((x * 2).ToString());

            // Act
            var transformedResult = result.Bind(func);

            // Assert
            transformedResult.IsFailure.Should().BeTrue();
            transformedResult.Error.Message.Should().Be(TestHelper.DefaultResultError.Message);
        }

        #endregion

        #region BindAsync<TIn, TOut>(this Result<TIn> self, Func<TIn, Task<Result<TOut>>> func)

        [Fact]
        public async Task BindAsync_SuccessfulResult_ShouldReturnTransformedResult()
        {
            // Arrange
            Result<int> result = TestHelper.CreateSuccessResult(10);
            Func<int, Task<Result<string>>> func = async x =>
            {
                await Task.Delay(10);
                return Result.Success((x * 2).ToString());
            };

            // Act
            var transformedResult = await result.BindAsync(func);

            // Assert
            transformedResult.IsSuccess.Should().BeTrue();
            transformedResult.Value.Should().Be("20");
        }

        [Fact]
        public async Task BindAsync_FailedResult_ShouldPropagateError()
        {
            // Arrange
            Result<int> result = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
            Func<int, Task<Result<string>>> func = async x =>
            {
                await Task.Delay(10);
                return Result.Success((x * 2).ToString());
            };

            // Act
            var transformedResult = await result.BindAsync(func);

            // Assert
            transformedResult.IsFailure.Should().BeTrue();
            transformedResult.Error.Message.Should().Be(TestHelper.DefaultResultError.Message);
        }

        #endregion

        #region BindAsync<TIn, TOut>(this Result<TIn> self, Func<TIn, TryAsync<TOut>> func)

        [Fact]
        public async Task BindAsync_TryAsyncSuccessfulResult_ShouldReturnTransformedResult()
        {
            // Arrange
            Result<int> result = TestHelper.CreateSuccessResult(10);
            Func<int, TryAsync<string>> func = x => async () =>
            {
                await Task.Delay(10);
                return Result.Success((x * 2).ToString());
            };

            // Act
            var transformedResult = await result.BindAsync(func);

            // Assert
            transformedResult.IsSuccess.Should().BeTrue();
            transformedResult.Value.Should().Be("20");
        }

        [Fact]
        public async Task BindAsync_TryAsyncFailedResult_ShouldPropagateError()
        {
            // Arrange
            Result<int> result = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
            Func<int, TryAsync<string>> func = x => async () =>
            {
                await Task.Delay(10);
                return Result.Success((x * 2).ToString());
            };

            // Act
            var transformedResult = await result.BindAsync(func);

            // Assert
            transformedResult.IsFailure.Should().BeTrue();
            transformedResult.Error.Message.Should().Be(TestHelper.DefaultResultError.Message);
        }

        #endregion

        #region Bind<TIn, TOut>(this Try<TIn> self, Func<TIn, Result<TOut>> func)

        [Fact]
        public void Bind_Try_SuccessfulResult_ShouldReturnTransformedResult()
        {
            // Arrange
            Try<int> tryResult = TestHelper.CreateTryFuncWithSuccess(10);
            Func<int, Result<string>> func = x => Result.Success((x * 2).ToString());

            // Act
            var transformedResult = tryResult.Bind(func);

            // Assert
            transformedResult.IsSuccess.Should().BeTrue();
            transformedResult.Value.Should().Be("20");
        }

        [Fact]
        public void Bind_Try_FailedResult_ShouldPropagateError()
        {
            // Arrange
            Try<int> tryResult = TestHelper.CreateTryFuncWithFailure<int>(TestHelper.DefaultResultError);
            Func<int, Result<string>> func = x => Result.Success((x * 2).ToString());

            // Act
            var transformedResult = tryResult.Bind(func);

            // Assert
            transformedResult.IsFailure.Should().BeTrue();
            transformedResult.Error.Message.Should().Be(TestHelper.DefaultResultError.Message);
        }

        #endregion

        #region Bind<TIn, TOut>(this Try<TIn> self, Func<TIn, Try<TOut>> func)

        [Fact]
        public void Bind_Try_TrySuccessfulResult_ShouldReturnTransformedResult()
        {
            // Arrange
            Try<int> tryResult = TestHelper.CreateTryFuncWithSuccess(10);
            Func<int, Try<string>> func = x => () => Result.Success((x * 2).ToString());

            // Act
            var transformedResult = tryResult.Bind(func);

            // Assert
            transformedResult.IsSuccess.Should().BeTrue();
            transformedResult.Value.Should().Be("20");
        }

        [Fact]
        public void Bind_Try_TryFailedResult_ShouldPropagateError()
        {
            // Arrange
            Try<int> tryResult = TestHelper.CreateTryFuncWithFailure<int>(TestHelper.DefaultResultError);
            Func<int, Try<string>> func = x => () => Result.Success((x * 2).ToString());

            // Act
            var transformedResult = tryResult.Bind(func);

            // Assert
            transformedResult.IsFailure.Should().BeTrue();
            transformedResult.Error.Message.Should().Be(TestHelper.DefaultResultError.Message);
        }

        #endregion

        #region BindAsync<TIn, TOut>(this Try<TIn> self, Func<TIn, Task<Result<TOut>>> func)

        [Fact]
        public async Task BindAsync_Try_SuccessfulResult_ShouldReturnTransformedResult()
        {
            // Arrange
            Try<int> tryResult = TestHelper.CreateTryFuncWithSuccess(10);
            Func<int, Task<Result<string>>> func = async x =>
            {
                await Task.Delay(10);
                return Result.Success((x * 2).ToString());
            };

            // Act
            var transformedResult = await tryResult.BindAsync(func);

            // Assert
            transformedResult.IsSuccess.Should().BeTrue();
            transformedResult.Value.Should().Be("20");
        }

        [Fact]
        public async Task BindAsync_Try_FailedResult_ShouldPropagateError()
        {
            // Arrange
            Try<int> tryResult = TestHelper.CreateTryFuncWithFailure<int>(TestHelper.DefaultResultError);
            Func<int, Task<Result<string>>> func = async x =>
            {
                await Task.Delay(10);
                return Result.Success((x * 2).ToString());
            };

            // Act
            var transformedResult = await tryResult.BindAsync(func);

            // Assert
            transformedResult.IsFailure.Should().BeTrue();
            transformedResult.Error.Message.Should().Be(TestHelper.DefaultResultError.Message);
        }

        #endregion

        #region BindAsync<TIn, TOut>(this Try<TIn> self, Func<TIn, TryAsync<TOut>> func)

        [Fact]
        public async Task BindAsync_Try_TryAsyncSuccessfulResult_ShouldReturnTransformedResult()
        {
            // Arrange
            Try<int> tryResult = TestHelper.CreateTryFuncWithSuccess(10);
            Func<int, TryAsync<string>> func = x => async () =>
            {
                await Task.Delay(10);
                return Result.Success((x * 2).ToString());
            };

            // Act
            var transformedResult = await tryResult.BindAsync(func);

            // Assert
            transformedResult.IsSuccess.Should().BeTrue();
            transformedResult.Value.Should().Be("20");
        }

        [Fact]
        public async Task BindAsync_Try_TryAsyncFailedResult_ShouldPropagateError()
        {
            // Arrange
            Try<int> tryResult = TestHelper.CreateTryFuncWithFailure<int>(TestHelper.DefaultResultError);
            Func<int, TryAsync<string>> func = x => async () =>
            {
                await Task.Delay(10);
                return Result.Success((x * 2).ToString());
            };

            // Act
            var transformedResult = await tryResult.BindAsync(func);

            // Assert
            transformedResult.IsFailure.Should().BeTrue();
            transformedResult.Error.Message.Should().Be(TestHelper.DefaultResultError.Message);
        }

        #endregion

        #region BindAsync<TIn, TOut>(this Task<Result<TIn>> self, Func<TIn, Result<TOut>> func)

        [Fact]
        public async Task BindAsync_TaskSuccessfulResult_ShouldReturnTransformedResult()
        {
            // Arrange
            Task<Result<int>> taskResult = TestHelper.CreateSuccessResultTaskAsync(10);
            Func<int, Result<string>> func = x => Result.Success((x * 2).ToString());

            // Act
            var transformedResult = await taskResult.BindAsync(func);

            // Assert
            transformedResult.IsSuccess.Should().BeTrue();
            transformedResult.Value.Should().Be("20");
        }

        [Fact]
        public async Task BindAsync_TaskFailedResult_ShouldPropagateError()
        {
            // Arrange
            Task<Result<int>> taskResult = TestHelper.CreateFailureResultTaskAsync<int>(TestHelper.DefaultResultError);
            Func<int, Result<string>> func = x => Result.Success((x * 2).ToString());

            // Act
            var transformedResult = await taskResult.BindAsync(func);

            // Assert
            transformedResult.IsFailure.Should().BeTrue();
            transformedResult.Error.Message.Should().Be(TestHelper.DefaultResultError.Message);
        }

        #endregion

        #region BindAsync<TIn, TOut>(this Task<Result<TIn>> self, Func<TIn, Try<TOut>> func)

        [Fact]
        public async Task BindAsync_Task_TrySuccessfulResult_ShouldReturnTransformedResult()
        {
            // Arrange
            Task<Result<int>> taskResult = TestHelper.CreateSuccessResultTaskAsync(10);
            Func<int, Try<string>> func = x => () => Result.Success((x * 2).ToString());

            // Act
            var transformedResult = await taskResult.BindAsync(func);

            // Assert
            transformedResult.IsSuccess.Should().BeTrue();
            transformedResult.Value.Should().Be("20");
        }

        [Fact]
        public async Task BindAsync_Task_TryFailedResult_ShouldPropagateError()
        {
            // Arrange
            Task<Result<int>> taskResult = TestHelper.CreateFailureResultTaskAsync<int>(TestHelper.DefaultResultError);
            Func<int, Try<string>> func = x => () => Result.Success((x * 2).ToString());

            // Act
            var transformedResult = await taskResult.BindAsync(func);

            // Assert
            transformedResult.IsFailure.Should().BeTrue();
            transformedResult.Error.Message.Should().Be(TestHelper.DefaultResultError.Message);
        }

        #endregion

        #region BindAsync<TIn, TOut>(this Task<Result<TIn>> self, Func<TIn, Task<Result<TOut>>> func)

        [Fact]
        public async Task BindAsync_Task_TaskSuccessfulResult_ShouldReturnTransformedResult()
        {
            // Arrange
            Task<Result<int>> taskResult = TestHelper.CreateSuccessResultTaskAsync(10);
            Func<int, Task<Result<string>>> func = async x =>
            {
                await Task.Delay(10);
                return Result.Success((x * 2).ToString());
            };

            // Act
            var transformedResult = await taskResult.BindAsync(func);

            // Assert
            transformedResult.IsSuccess.Should().BeTrue();
            transformedResult.Value.Should().Be("20");
        }

        [Fact]
        public async Task BindAsync_Task_TaskFailedResult_ShouldPropagateError()
        {
            // Arrange
            Task<Result<int>> taskResult = TestHelper.CreateFailureResultTaskAsync<int>(TestHelper.DefaultResultError);
            Func<int, Task<Result<string>>> func = async x =>
            {
                await Task.Delay(10);
                return Result.Success((x * 2).ToString());
            };

            // Act
            var transformedResult = await taskResult.BindAsync(func);

            // Assert
            transformedResult.IsFailure.Should().BeTrue();
            transformedResult.Error.Message.Should().Be(TestHelper.DefaultResultError.Message);
        }

        #endregion

        #region BindAsync<TIn, TOut>(this Task<Result<TIn>> self, Func<TIn, TryAsync<TOut>> func)

        [Fact]
        public async Task BindAsync_Task_TryAsyncSuccessfulResult_ShouldReturnTransformedResult()
        {
            // Arrange
            Task<Result<int>> taskResult = TestHelper.CreateSuccessResultTaskAsync(10);
            Func<int, TryAsync<string>> func = x => async () =>
            {
                await Task.Delay(10);
                return Result.Success((x * 2).ToString());
            };

            // Act
            var transformedResult = await taskResult.BindAsync(func);

            // Assert
            transformedResult.IsSuccess.Should().BeTrue();
            transformedResult.Value.Should().Be("20");
        }

        [Fact]
        public async Task BindAsync_Task_TryAsyncFailedResult_ShouldPropagateError()
        {
            // Arrange
            Task<Result<int>> taskResult = TestHelper.CreateFailureResultTaskAsync<int>(TestHelper.DefaultResultError);
            Func<int, TryAsync<string>> func = x => async () =>
            {
                await Task.Delay(10);
                return Result.Success((x * 2).ToString());
            };

            // Act
            var transformedResult = await taskResult.BindAsync(func);

            // Assert
            transformedResult.IsFailure.Should().BeTrue();
            transformedResult.Error.Message.Should().Be(TestHelper.DefaultResultError.Message);
        }

        #endregion
        #region BindAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, Result<TOut>> func)

        [Fact]
        public async Task BindAsync_TryAsync_SuccessfulResult_ShouldReturnTransformedResult()
        {
            // Arrange
            TryAsync<int> tryAsyncResult = TestHelper.CreateTryAsyncFuncWithSuccess(10);
            Func<int, Result<string>> func = x => Result.Success((x * 2).ToString());

            // Act
            var transformedResult = await tryAsyncResult.BindAsync(func);

            // Assert
            transformedResult.IsSuccess.Should().BeTrue();
            transformedResult.Value.Should().Be("20");
        }

        [Fact]
        public async Task BindAsync_TryAsync_FailedResult_ShouldPropagateError()
        {
            // Arrange
            TryAsync<int> tryAsyncResult = TestHelper.CreateTryAsyncFuncWithFailure<int>(TestHelper.DefaultResultError);
            Func<int, Result<string>> func = x => Result.Success((x * 2).ToString());

            // Act
            var transformedResult = await tryAsyncResult.BindAsync(func);

            // Assert
            transformedResult.IsFailure.Should().BeTrue();
            transformedResult.Error.Message.Should().Be(TestHelper.DefaultResultError.Message);
        }

        #endregion

        #region BindAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, Try<TOut>> func)

        [Fact]
        public async Task BindAsync_TryAsync_TrySuccessfulResult_ShouldReturnTransformedResult()
        {
            // Arrange
            TryAsync<int> tryAsyncResult = TestHelper.CreateTryAsyncFuncWithSuccess(10);
            Func<int, Try<string>> func = x => () => Result.Success((x * 2).ToString());

            // Act
            var transformedResult = await tryAsyncResult.BindAsync(func);

            // Assert
            transformedResult.IsSuccess.Should().BeTrue();
            transformedResult.Value.Should().Be("20");
        }

        [Fact]
        public async Task BindAsync_TryAsync_TryFailedResult_ShouldPropagateError()
        {
            // Arrange
            TryAsync<int> tryAsyncResult = TestHelper.CreateTryAsyncFuncWithFailure<int>(TestHelper.DefaultResultError);
            Func<int, Try<string>> func = x => () => Result.Success((x * 2).ToString());

            // Act
            var transformedResult = await tryAsyncResult.BindAsync(func);

            // Assert
            transformedResult.IsFailure.Should().BeTrue();
            transformedResult.Error.Message.Should().Be(TestHelper.DefaultResultError.Message);
        }

        #endregion

        #region BindAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, Task<Result<TOut>>> func)

        [Fact]
        public async Task BindAsync_TryAsync_TaskSuccessfulResult_ShouldReturnTransformedResult()
        {
            // Arrange
            TryAsync<int> tryAsyncResult = TestHelper.CreateTryAsyncFuncWithSuccess(10);
            Func<int, Task<Result<string>>> func = async x =>
            {
                await Task.Delay(10);
                return Result.Success((x * 2).ToString());
            };

            // Act
            var transformedResult = await tryAsyncResult.BindAsync(func);

            // Assert
            transformedResult.IsSuccess.Should().BeTrue();
            transformedResult.Value.Should().Be("20");
        }

        [Fact]
        public async Task BindAsync_TryAsync_TaskFailedResult_ShouldPropagateError()
        {
            // Arrange
            TryAsync<int> tryAsyncResult = TestHelper.CreateTryAsyncFuncWithFailure<int>(TestHelper.DefaultResultError);
            Func<int, Task<Result<string>>> func = async x =>
            {
                await Task.Delay(10);
                return Result.Success((x * 2).ToString());
            };

            // Act
            var transformedResult = await tryAsyncResult.BindAsync(func);

            // Assert
            transformedResult.IsFailure.Should().BeTrue();
            transformedResult.Error.Message.Should().Be(TestHelper.DefaultResultError.Message);
        }

        #endregion

        #region BindAsync<TIn, TOut>(this TryAsync<TIn> self, Func<TIn, TryAsync<TOut>> func)

        [Fact]
        public async Task BindAsync_TryAsync_TryAsyncSuccessfulResult_ShouldReturnTransformedResult()
        {
            // Arrange
            TryAsync<int> tryAsyncResult = TestHelper.CreateTryAsyncFuncWithSuccess(10);
            Func<int, TryAsync<string>> func = x => async () =>
            {
                await Task.Delay(10);
                return Result.Success((x * 2).ToString());
            };

            // Act
            var transformedResult = await tryAsyncResult.BindAsync(func);

            // Assert
            transformedResult.IsSuccess.Should().BeTrue();
            transformedResult.Value.Should().Be("20");
        }

        [Fact]
        public async Task BindAsync_TryAsync_TryAsyncFailedResult_ShouldPropagateError()
        {
            // Arrange
            TryAsync<int> tryAsyncResult = TestHelper.CreateTryAsyncFuncWithFailure<int>(TestHelper.DefaultResultError);
            Func<int, TryAsync<string>> func = x => async () =>
            {
                await Task.Delay(10);
                return Result.Success((x * 2).ToString());
            };

            // Act
            var transformedResult = await tryAsyncResult.BindAsync(func);

            // Assert
            transformedResult.IsFailure.Should().BeTrue();
            transformedResult.Error.Message.Should().Be(TestHelper.DefaultResultError.Message);
        }

        #endregion
    }
}