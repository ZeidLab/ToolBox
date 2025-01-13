using FluentAssertions;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Test.Units.Common;

public class TaskExtensionsTests
{
    [Fact]
    public void IsCompletedSuccessfully_ShouldReturnTrue_WhenTaskIsCompletedSuccessfully()
    {
        // Arrange
        var task = Task.FromResult(42);

        // Act
        var result = task.IsCompletedSuccessfully();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsCompletedSuccessfully_ShouldReturnFalse_WhenTaskIsFaulted()
    {
        // Arrange
        var task = Task.FromException<int>(new Exception("Simulated fault"));

        // Act
        var result = task.IsCompletedSuccessfully();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsCompletedSuccessfully_ShouldReturnFalse_WhenTaskIsCanceled()
    {
        // Arrange
        var task = Task.FromCanceled<int>(new CancellationToken(true));

        // Act
        var result = task.IsCompletedSuccessfully();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void AsFailedTask_ShouldReturnFaultedTask_WithProvidedException()
    {
        // Arrange
        var exception = new Exception("Test exception");

        // Act
        var task = exception.AsFailedTaskAsync<int>();

        // Assert
        task.IsFaulted.Should().BeTrue();
        task.Exception.Should().NotBeNull();
        task.Exception?.InnerException.Should().Be(exception);
    }

    [Fact]
    public async Task AsTask_ShouldReturnCompletedTask_WithProvidedValue()
    {
        // Arrange
        var value = 42;

        // Act
        var task = value.AsTaskAsync();
        await Task.WhenAll(task);
        // Assert
        task.IsCompletedSuccessfully().Should().BeTrue();
        task.Result.Should().Be(value);
    }

    [Fact]
    public async Task Flatten_ShouldReturnFlattenedTask_WhenGivenNestedTask()
    {
        // Arrange
        var innerTask = Task.FromResult(42);
        var nestedTask = Task.FromResult(innerTask);

        // Act
        var result = await nestedTask.FlattenAsync();

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public async Task Flatten_ShouldReturnFlattenedTask_WhenGivenDoubleNestedTask()
    {
        // Arrange
        var innerTask = Task.FromResult(42);
        var nestedTask = Task.FromResult(Task.FromResult(innerTask));

        // Act
        var result = await nestedTask.FlattenAsync();

        // Assert
        result.Should().Be(42);
    }

    [Fact]
    public async Task Flatten_ShouldThrow_WhenNestedTaskIsFaulted()
    {
        // Arrange
        var exception = new Exception("Test exception");
        var faultedTask = Task.FromException<Task<int>>(exception);
        var nestedTask = Task.FromResult(faultedTask);

        // Act
        Func<Task> act = async () => await nestedTask.FlattenAsync();

        // Assert
        await act.Should().ThrowAsync<Exception>().Where(ex => ex == exception);
    }

    [Fact]
    public async Task Flatten_ShouldThrow_WhenDoubleNestedTaskIsFaulted()
    {
        // Arrange
        var exception = new Exception("Test exception");
        var faultedTask = Task.FromException<Task<Task<int>>>(exception);
        var nestedTask = Task.FromResult(faultedTask);

        // Act
        Func<Task> act = async () => await nestedTask.FlattenAsync();

        // Assert
        await act.Should().ThrowAsync<Exception>().Where(ex => ex == exception);
    }
    
}