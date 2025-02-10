using FluentAssertions;
using ZeidLab.ToolBox.Results;
using ZeidLab.ToolBox.Common;

namespace ZeidLab.ToolBox.Test.Units.Results;

public class ResultExtensionsMatchAsyncVoidTests
{
    [Fact]
    public async Task MatchAsync_WhenResultWithAsyncActions_ShouldExecuteSuccessAction()
    {
        // Arrange
        var value = 42;
        var result = TestHelper.CreateSuccessResult(value);
        var successCalled = false;
        var failureCalled = false;
        
        // Act
        await result.MatchAsync(
            success: async v => 
            {
                await Task.Yield();
                successCalled = true;
                v.Should().Be(value);
            },
            failure: async e => 
            {
                await Task.Yield();
                failureCalled = true;
            });
        
        // Assert
        successCalled.Should().BeTrue();
        failureCalled.Should().BeFalse();
    }

    [Fact]
    public async Task MatchAsync_WhenResultWithAsyncActions_ShouldExecuteFailureAction()
    {
        // Arrange
        var result = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
        var successCalled = false;
        var failureCalled = false;
        
        // Act
        await result.MatchAsync(
            success: async v => 
            {
                await Task.Yield();
                successCalled = true;
            },
            failure: async e => 
            {
                await Task.Yield();
                failureCalled = true;
                e.Should().Be(TestHelper.DefaultResultError);
            });
        
        // Assert
        successCalled.Should().BeFalse();
        failureCalled.Should().BeTrue();
    }

    [Fact]
    public async Task MatchAsync_WhenTaskResultWithAsyncActions_ShouldExecuteSuccessAction()
    {
        // Arrange
        var value = 42;
        var taskResult = TestHelper.CreateSuccessResult(value).AsTaskAsync();
        var successCalled = false;
        var failureCalled = false;
        
        // Act
        await taskResult.MatchAsync(
            success: async v => 
            {
                await Task.Yield();
                successCalled = true;
                v.Should().Be(value);
            },
            failure: async e => 
            {
                await Task.Yield();
                failureCalled = true;
            });
        
        // Assert
        successCalled.Should().BeTrue();
        failureCalled.Should().BeFalse();
    }

    [Fact]
    public async Task MatchAsync_WhenTaskResultWithAsyncActions_ShouldExecuteFailureAction()
    {
        // Arrange
        var taskResult = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError).AsTaskAsync();
        var successCalled = false;
        var failureCalled = false;
        
        // Act
        await taskResult.MatchAsync(
            success: async v => 
            {
                await Task.Yield();
                successCalled = true;
            },
            failure: async e => 
            {
                await Task.Yield();
                failureCalled = true;
                e.Should().Be(TestHelper.DefaultResultError);
            });
        
        // Assert
        successCalled.Should().BeFalse();
        failureCalled.Should().BeTrue();
    }

    [Fact]
    public async Task MatchAsync_WhenTryWithAsyncActions_ShouldExecuteSuccessAction()
    {
        // Arrange
        var value = 42;
        var tryResult = TestHelper.CreateTryFuncWithSuccess(value);
        var successCalled = false;
        var failureCalled = false;
        
        // Act
        await tryResult.MatchAsync(
            success: async v => 
            {
                await Task.Yield();
                successCalled = true;
                v.Should().Be(value);
            },
            failure: async e => 
            {
                await Task.Yield();
                failureCalled = true;
            });
        
        // Assert
        successCalled.Should().BeTrue();
        failureCalled.Should().BeFalse();
    }

    [Fact]
    public async Task MatchAsync_WhenTryWithAsyncActions_ShouldExecuteFailureAction()
    {
        // Arrange
        var tryResult = TestHelper.CreateTryFuncWithFailure<int>(TestHelper.DefaultResultError);
        var successCalled = false;
        var failureCalled = false;
        
        // Act
        await tryResult.MatchAsync(
            success: async v => 
            {
                await Task.Yield();
                successCalled = true;
            },
            failure: async e => 
            {
                await Task.Yield();
                failureCalled = true;
                e.Should().Be(TestHelper.DefaultResultError);
            });
        
        // Assert
        successCalled.Should().BeFalse();
        failureCalled.Should().BeTrue();
    }

    [Fact]
    public async Task MatchAsync_WhenTryAsyncWithAsyncActions_ShouldExecuteSuccessAction()
    {
        // Arrange
        var value = 42;
        var tryAsync = TestHelper.CreateTryAsyncFuncWithSuccess(value);
        var successCalled = false;
        var failureCalled = false;
        
        // Act
        await tryAsync.MatchAsync(
            success: async v => 
            {
                await Task.Yield();
                successCalled = true;
                v.Should().Be(value);
            },
            failure: async e => 
            {
                await Task.Yield();
                failureCalled = true;
            });
        
        // Assert
        successCalled.Should().BeTrue();
        failureCalled.Should().BeFalse();
    }

    [Fact]
    public async Task MatchAsync_WhenTryAsyncWithAsyncActions_ShouldExecuteFailureAction()
    {
        // Arrange
        var tryAsync = TestHelper.CreateTryAsyncFuncWithFailure<int>(TestHelper.DefaultResultError);
        var successCalled = false;
        var failureCalled = false;
        
        // Act
        await tryAsync.MatchAsync(
            success: async v => 
            {
                await Task.Yield();
                successCalled = true;
            },
            failure: async e => 
            {
                await Task.Yield();
                failureCalled = true;
                e.Should().Be(TestHelper.DefaultResultError);
            });
        
        // Assert
        successCalled.Should().BeFalse();
        failureCalled.Should().BeTrue();
    }

    [Fact]
    public async Task MatchAsync_WhenAsyncSuccessFunctionThrows_ShouldPropagateException()
    {
        // Arrange
        var value = 42;
        var result = TestHelper.CreateSuccessResult(value);
        var expectedException = new InvalidOperationException();
        
        // Act
        var act = () => result.MatchAsync(
            success: async _ => throw expectedException,
            failure: async e => { });
            
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .Where(ex => ex == expectedException);
    }
    
    [Fact]
    public async Task MatchAsync_WhenAsyncFailureFunctionThrows_ShouldPropagateException()
    {
        // Arrange
        var result = TestHelper.CreateFailureResult<int>(TestHelper.DefaultResultError);
        var expectedException = new InvalidOperationException();
        
        // Act
        var act = () => result.MatchAsync(
            success: async v => { },
            failure: async _ => throw expectedException);
            
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .Where(ex => ex == expectedException);
    }

    [Fact]
    public async Task MatchAsync_WhenTaskIsFaulted_ShouldPropagateException()
    {
        // Arrange
        var expectedException = new InvalidOperationException();
        Task<Result<int>> taskResult = Task.FromException<Result<int>>(expectedException);
        
        // Act
        var act = () => taskResult.MatchAsync(
            success: async v => { },
            failure: async e => { });
            
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .Where(ex => ex == expectedException);
    }
}