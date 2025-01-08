using System.Runtime.Serialization;
using FluentAssertions;
using ZeidLab.ToolBox.Results;

namespace ZeidLab.ToolBox.Test.Units.Results;

public class ResultExtensionsJoinTests
{
    public static IEnumerable<object[]> DataProvider(int count = 10)
    {
        // Generate a list of results with at least one failure
        for (int i = 0; i < count; i++)
        {
            var results = TestHelper.CreateResults(count, 10, i).ToList();
            yield return [results];
        }
    }

    #region FailTests

    [Theory]
    [MemberData(nameof(DataProvider), 2)]
    public void Join_WithAnyResultFailureForJoinWith02Params_ShouldReturnFailureResult(List<Result<int>> results)
    {
        // Arrange

        Func<int, int, Result<int>> func =
            (x1, x2) =>
                TestHelper.CreateSuccessResult(x1 + x2);

        // Act
        var result = results[0].Join(
            results[1],
            func);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.GetValueOrDefault().Message.Should().Be(TestHelper.DefaultError.Message);
    }

    [Theory]
    [MemberData(nameof(DataProvider), 3)]
    public void Join_WithAnyResultFailureForJoinWith03Params_ShouldReturnFailureResult(List<Result<int>> results)
    {
        // Arrange

        Func<int, int, int, Result<int>> func =
            (x1, x2, x3) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3);

        // Act
        var result = results[0].Join(
            results[1],
            results[2],
            func);


        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.GetValueOrDefault().Message.Should().Be(TestHelper.DefaultError.Message);
    }

    [Theory]
    [MemberData(nameof(DataProvider), 4)]
    public void Join_WithAnyResultFailureForJoinWith04Params_ShouldReturnFailureResult(List<Result<int>> results)
    {
        // Arrange

        Func<int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4);

        // Act
        var result = results[0].Join(
            results[1],
            results[2],
            results[3],
            func);


        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.GetValueOrDefault().Message.Should().Be(TestHelper.DefaultError.Message);
    }

    [Theory]
    [MemberData(nameof(DataProvider), 5)]
    public void Join_WithAnyResultFailureForJoinWith05Params_ShouldReturnFailureResult(List<Result<int>> results)
    {
        // Arrange

        Func<int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5);

        // Act
        var result = results[0].Join(
            results[1],
            results[2],
            results[3],
            results[4],
            func);


        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.GetValueOrDefault().Message.Should().Be(TestHelper.DefaultError.Message);
    }

    [Theory]
    [MemberData(nameof(DataProvider), 6)]
    public void Join_WithAnyResultFailureForJoinWith06Params_ShouldReturnFailureResult(List<Result<int>> results)
    {
        // Arrange

        Func<int, int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5, x6) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5 + x6);

        // Act
        var result = results[0].Join(
            results[1],
            results[2],
            results[3],
            results[4],
            results[5],
            func);


        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.GetValueOrDefault().Message.Should().Be(TestHelper.DefaultError.Message);
    }

    [Theory]
    [MemberData(nameof(DataProvider), 7)]
    public void Join_WithAnyResultFailureForJoinWith07Params_ShouldReturnFailureResult(List<Result<int>> results)
    {
        // Arrange

        Func<int, int, int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5, x6, x7) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5 + x6 + x7);

        // Act
        var result = results[0].Join(
            results[1],
            results[2],
            results[3],
            results[4],
            results[5],
            results[6],
            func);


        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.GetValueOrDefault().Message.Should().Be(TestHelper.DefaultError.Message);
    }


    [Theory]
    [MemberData(nameof(DataProvider), 8)]
    public void Join_WithAnyResultFailureForJoinWith08Params_ShouldReturnFailureResult(List<Result<int>> results)
    {
        // Arrange

        Func<int, int, int, int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5, x6, x7, x8) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8);

        // Act
        var result = results[0].Join(
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
        result.Error.GetValueOrDefault().Message.Should().Be(TestHelper.DefaultError.Message);
    }

    [Theory]
    [MemberData(nameof(DataProvider), 9)]
    public void Join_WithAnyResultFailureForJoinWith09Params_ShouldReturnFailureResult(List<Result<int>> results)
    {
        // Arrange

        Func<int, int, int, int, int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5, x6, x7, x8, x9) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9);

        // Act
        var result = results[0].Join(
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
        result.Error.GetValueOrDefault().Message.Should().Be(TestHelper.DefaultError.Message);
    }

    [Theory]
    [MemberData(nameof(DataProvider), 10)]
    public void Join_WithAnyResultFailureForJoinWith10Params_ShouldReturnFailureResult(List<Result<int>> results)
    {
        // Arrange

        Func<int, int, int, int, int, int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9 + x10);

        // Act
        var result = results[0].Join(
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
        result.Error.GetValueOrDefault().Message.Should().Be(TestHelper.DefaultError.Message);
    }

    #endregion

    #region SuccessTests

    [Fact]
    public void Join_WithNineSuccessResults_ShouldReturnSuccessResult()
    {
        // Arrange
        var results = TestHelper.CreateResults(9, 10).ToList();

        Func<int, int, int, int, int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5, x6, x7, x8, x9) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9);

        // Act
        var result = results[0].Join(
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
        result.Value.Should().Be(results.Sum(x => x.Value));
    }

    [Fact]
    public void Join_With02SuccessResults_ShouldReturnSuccessResult()
    {
        // Arrange
        var results = TestHelper.CreateResults(2, 10).ToList();

        Func<int, int, Result<int>> func =
            (x1, x2) => TestHelper.CreateSuccessResult(x1 + x2);

        // Act
        var result = results[0].Join(results[1], func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(results.Sum(x => x.Value));
    }

    [Fact]
    public void Join_With03SuccessResults_ShouldReturnSuccessResult()
    {
        // Arrange
        var results = TestHelper.CreateResults(3, 10).ToList();

        Func<int, int, int, Result<int>> func =
            (x1, x2, x3) => TestHelper.CreateSuccessResult(x1 + x2 + x3);

        // Act
        var result = results[0].Join(results[1], results[2], func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(results.Sum(x => x.Value));
    }

    [Fact]
    public void Join_With04SuccessResults_ShouldReturnSuccessResult()
    {
        // Arrange
        var results = TestHelper.CreateResults(4, 10).ToList();

        Func<int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4);

        // Act
        var result = results[0].Join(
            results[1],
            results[2],
            results[3],
            func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(results.Sum(x => x.Value));
    }

    [Fact]
    public void Join_With05SuccessResults_ShouldReturnSuccessResult()
    {
        // Arrange
        var results = TestHelper.CreateResults(5, 10).ToList();

        Func<int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5);

        // Act
        var result = results[0].Join(
            results[1],
            results[2],
            results[3],
            results[4],
            func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(results.Sum(x => x.Value));
    }

    [Fact]
    public void Join_With06SuccessResults_ShouldReturnSuccessResult()
    {
        // Arrange
        var results = TestHelper.CreateResults(6, 10).ToList();

        Func<int, int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5, x6) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5 + x6);

        // Act
        var result = results[0].Join(
            results[1],
            results[2],
            results[3],
            results[4],
            results[5],
            func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(results.Sum(x => x.Value));
    }

    [Fact]
    public void Join_With07SuccessResults_ShouldReturnSuccessResult()
    {
        // Arrange
        var results = TestHelper.CreateResults(7, 10).ToList();

        Func<int, int, int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5, x6, x7) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5 + x6 + x7);

        // Act
        var result = results[0].Join(
            results[1],
            results[2],
            results[3],
            results[4],
            results[5],
            results[6],
            func);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(results.Sum(x => x.Value));
    }

    [Fact]
    public void Join_With08SuccessResults_ShouldReturnSuccessResult()
    {
        // Arrange
        var results = TestHelper.CreateResults(8, 10).ToList();

        Func<int, int, int, int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5, x6, x7, x8) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8);

        // Act
        var result = results[0].Join(
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
        result.Value.Should().Be(results.Sum(x => x.Value));
    }

    [Fact]
    public void Join_With09SuccessResults_ShouldReturnSuccessResult()
    {
        // Arrange
        var results = TestHelper.CreateResults(9, 10).ToList();

        Func<int, int, int, int, int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5, x6, x7, x8, x9) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9);

        // Act
        var result = results[0].Join(
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
        result.Value.Should().Be(results.Sum(x => x.Value));
    }


    [Fact]
    public void Join_With10SuccessResults_ShouldReturnSuccessResult()
    {
        // Arrange
        var results = TestHelper.CreateResults(10, 10).ToList();

        Func<int, int, int, int, int, int, int, int, int, int, Result<int>> func =
            (x1, x2, x3, x4, x5, x6, x7, x8, x9, x10) =>
                TestHelper.CreateSuccessResult(x1 + x2 + x3 + x4 + x5 + x6 + x7 + x8 + x9 + x10);

        // Act
        var result = results[0].Join(
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
        result.Value.Should().Be(results.Sum(x => x.Value));
    }

    #endregion
}