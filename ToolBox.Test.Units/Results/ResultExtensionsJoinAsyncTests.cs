using System.Runtime.Serialization;
using FluentAssertions;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results;

public class ResultExtensionsJoinAsyncTests
{
    public static IEnumerable<object[]> AsyncDataProvider(int count = 10)
    {
        // Generate a list of results with at least one failure
        for (int i = 0; i < count; i++)
        {
            var results = TestHelper.CreateAsyncResults(count, 10, i).ToList();
            yield return [results];
        }
    }

    #region FailTests

    [Theory]
    [MemberData(nameof(AsyncDataProvider), 2)]
    public async Task JoinAsync_WithAnyResultFailureForJoinWith02Params_ShouldReturnFailureResult(List<Task<Result<int>>> results)
    {
        // Arrange

        Func<int, int, Result<int>> func =
            (x1, x2) =>
                TestHelper.CreateSuccessResult(x1 + x2);

        // Act
        var result = await results[0].JoinAsync(
            results[1],
            func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.ResultError.Message.Should().Be(TestHelper.DefaultResultError.Message);
    }

    [Theory]
    [MemberData(nameof(AsyncDataProvider), 3)]
    public async Task JoinAsync_WithAnyResultFailureForJoinWith03Params_ShouldReturnFailureResult(List<Task<Result<int>>> results)
    {
        // Arrange

        Func<int, int, int, Result<int>> func =
            (x1, x2, x3) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3);

        // Act
        var result = await results[0].JoinAsync(
            results[1],
            results[2],
            func);


        // Assert
        result.IsFailure.Should().BeTrue();
        result.ResultError.Message.Should().Be(TestHelper.DefaultResultError.Message);
    }

    [Theory]
    [MemberData(nameof(AsyncDataProvider), 4)]
    public async Task JoinAsync_WithAnyResultFailureForJoinWith04Params_ShouldReturnFailureResult(List<Task<Result<int>>> results)
    {
        // Arrange

        Func<int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4);

        // Act
        var result = await results[0].JoinAsync(
            results[1],
            results[2],
            results[3],
            func);


        // Assert
        result.IsFailure.Should().BeTrue();
        result.ResultError.Message.Should().Be(TestHelper.DefaultResultError.Message);
    }

    [Theory]
    [MemberData(nameof(AsyncDataProvider), 5)]
    public async Task JoinAsync_WithAnyResultFailureForJoinWith05Params_ShouldReturnFailureResult(List<Task<Result<int>>> results)
    {
        // Arrange

        Func<int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5);

        // Act
        var result = await results[0].JoinAsync(
            results[1],
            results[2],
            results[3],
            results[4],
            func);


        // Assert
        result.IsFailure.Should().BeTrue();
        result.ResultError.Message.Should().Be(TestHelper.DefaultResultError.Message);
    }

    [Theory]
    [MemberData(nameof(AsyncDataProvider), 6)]
    public async Task JoinAsync_WithAnyResultFailureForJoinWith06Params_ShouldReturnFailureResult(List<Task<Result<int>>> results)
    {
        // Arrange

        Func<int, int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5, x6) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5 + x6);

        // Act
        var result = await results[0].JoinAsync(
            results[1],
            results[2],
            results[3],
            results[4],
            results[5],
            func);


        // Assert
        result.IsFailure.Should().BeTrue();
        result.ResultError.Message.Should().Be(TestHelper.DefaultResultError.Message);
    }

    [Theory]
    [MemberData(nameof(AsyncDataProvider), 7)]
    public async Task JoinAsync_WithAnyResultFailureForJoinWith07Params_ShouldReturnFailureResult(List<Task<Result<int>>> results)
    {
        // Arrange

        Func<int, int, int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5, x6, x7) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5 + x6 + x7);

        // Act
        var result = await results[0].JoinAsync(
            results[1],
            results[2],
            results[3],
            results[4],
            results[5],
            results[6],
            func);


        // Assert
        result.IsFailure.Should().BeTrue();
        result.ResultError.Message.Should().Be(TestHelper.DefaultResultError.Message);
    }


    [Theory]
    [MemberData(nameof(AsyncDataProvider), 8)]
    public async Task JoinAsync_WithAnyResultFailureForJoinWith08Params_ShouldReturnFailureResult(List<Task<Result<int>>> results)
    {
        // Arrange

        Func<int, int, int, int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5, x6, x7, x8) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8);

        // Act
        var result = await results[0].JoinAsync(
            results[1],
            results[2],
            results[3],
            results[4],
            results[5],
            results[6],
            results[7],
            func);


        // Assert
        result.IsFailure.Should().BeTrue();
        result.ResultError.Message.Should().Be(TestHelper.DefaultResultError.Message);
    }

    [Theory]
    [MemberData(nameof(AsyncDataProvider), 9)]
    public async Task JoinAsync_WithAnyResultFailureForJoinWith09Params_ShouldReturnFailureResult(List<Task<Result<int>>> results)
    {
        // Arrange

        Func<int, int, int, int, int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5, x6, x7, x8, x9) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9);

        // Act
        var result = await results[0].JoinAsync(
            results[1],
            results[2],
            results[3],
            results[4],
            results[5],
            results[6],
            results[7],
            results[8],
            func);


        // Assert
        result.IsFailure.Should().BeTrue();
        result.ResultError.Message.Should().Be(TestHelper.DefaultResultError.Message);
    }

    [Theory]
    [MemberData(nameof(AsyncDataProvider), 10)]
    public async Task JoinAsync_WithAnyResultFailureForJoinWith10Params_ShouldReturnFailureResult(List<Task<Result<int>>> results)
    {
        // Arrange

        Func<int, int, int, int, int, int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9 + x10);

        // Act
        var result = await results[0].JoinAsync(
            results[1],
            results[2],
            results[3],
            results[4],
            results[5],
            results[6],
            results[7],
            results[8],
            results[9],
            func);


        // Assert
        result.IsFailure.Should().BeTrue();
        result.ResultError.Message.Should().Be(TestHelper.DefaultResultError.Message);
    }

    #endregion

    #region SuccessTests
    
    [Fact]
    public async Task JoinAsync_With02SuccessResults_ShouldReturnSuccessResult()
    {
        // Arrange
        var results = TestHelper.CreateAsyncResults(2, 10).ToList();

        Func<int, int, Result<int>> func =
            (x1, x2) => TestHelper.CreateSuccessResult(x1 + x2);

        // Act
        var result = await results[0].JoinAsync(results[1], func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(20);
    }

    [Fact]
    public async Task JoinAsync_With03SuccessResults_ShouldReturnSuccessResult()
    {
        // Arrange
        var results = TestHelper.CreateAsyncResults(3, 10).ToList();

        Func<int, int, int, Result<int>> func =
            (x1, x2, x3) => TestHelper.CreateSuccessResult(x1 + x2 + x3);

        // Act
        var result = await results[0].JoinAsync(results[1], results[2], func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(results.Count * 10);
    }

    [Fact]
    public async Task JoinAsync_With04SuccessResults_ShouldReturnSuccessResult()
    {
        // Arrange
        var results = TestHelper.CreateAsyncResults(4, 10).ToList();

        Func<int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4);

        // Act
        var result = await results[0].JoinAsync(
            results[1],
            results[2],
            results[3],
            func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(results.Count * 10);
    }

    [Fact]
    public async Task JoinAsync_With05SuccessResults_ShouldReturnSuccessResult()
    {
        // Arrange
        var results = TestHelper.CreateAsyncResults(5, 10).ToList();

        Func<int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5);

        // Act
        var result = await results[0].JoinAsync(
            results[1],
            results[2],
            results[3],
            results[4],
            func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(results.Count * 10);
    }

    [Fact]
    public async Task JoinAsync_With06SuccessResults_ShouldReturnSuccessResult()
    {
        // Arrange
        var results = TestHelper.CreateAsyncResults(6, 10).ToList();

        Func<int, int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5, x6) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5 + x6);

        // Act
        var result = await results[0].JoinAsync(
            results[1],
            results[2],
            results[3],
            results[4],
            results[5],
            func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(results.Count * 10);
    }

    [Fact]
    public async Task JoinAsync_With07SuccessResults_ShouldReturnSuccessResult()
    {
        // Arrange
        var results = TestHelper.CreateAsyncResults(7, 10).ToList();

        Func<int, int, int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5, x6, x7) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5 + x6 + x7);

        // Act
        var result = await results[0].JoinAsync(
            results[1],
            results[2],
            results[3],
            results[4],
            results[5],
            results[6],
            func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(results.Count * 10);
    }

    [Fact]
    public async Task JoinAsync_With08SuccessResults_ShouldReturnSuccessResult()
    {
        // Arrange
        var results = TestHelper.CreateAsyncResults(8, 10).ToList();

        Func<int, int, int, int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5, x6, x7, x8) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8);

        // Act
        var result = await results[0].JoinAsync(
            results[1],
            results[2],
            results[3],
            results[4],
            results[5],
            results[6],
            results[7],
            func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(results.Count * 10);
    }

    [Fact]
    public async Task JoinAsync_With09SuccessResults_ShouldReturnSuccessResult()
    {
        // Arrange
        var results = TestHelper.CreateAsyncResults(9, 10).ToList();

        Func<int, int, int, int, int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5, x6, x7, x8, x9) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9);

        // Act
        var result = await results[0].JoinAsync(
            results[1],
            results[2],
            results[3],
            results[4],
            results[5],
            results[6],
            results[7],
            results[8],
            func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(results.Count * 10);
    }


    [Fact]
    public async Task JoinAsync_With10SuccessResults_ShouldReturnSuccessResult()
    {
        // Arrange
        var results = TestHelper.CreateAsyncResults(10, 10).ToList();

        Func<int, int, int, int, int, int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9 + x10);

        // Act
        var result = await results[0].JoinAsync(
            results[1],
            results[2],
            results[3],
            results[4],
            results[5],
            results[6],
            results[7],
            results[8],
            results[9],
            func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(results.Count * 10);
    }

    #endregion
}