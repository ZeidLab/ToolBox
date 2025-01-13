using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace ZeidLab.ToolBox.Results;

/// <summary>
/// Represents a list of results.
/// </summary>
/// <typeparam name="TIn">The type of the input.</typeparam>
[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates")]
public readonly record struct ResultList<TIn> : IEnumerable<Result<TIn>>
{
    internal readonly ResultError InternalResultError;
    internal readonly Result<TIn>[] ResultsList;
    internal readonly bool IsSuccess;

    internal Result<TIn> this[int index] => ResultsList[index];
    /// <summary>
    /// Number of results in the list.
    /// </summary>
    public int Count => ResultsList.Length;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultList{TIn}"/> struct with the specified results.
    /// </summary>
    /// <param name="results">An enumerable collection of <see cref="Result{TIn}"/> to initialize the list with.</param>
    internal ResultList(IEnumerable<Result<TIn>> results)
    {
        ResultsList = results.ToArray();
        InternalResultError = default;
        IsSuccess = ResultsList.All(x => x.IsSuccess);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultList{TIn}"/> struct with the specified error.
    /// </summary>
    /// <param name="internalResultError">The error to initialize the list with.</param>
    internal ResultList(ResultError internalResultError)
    {
        InternalResultError = internalResultError;
        ResultsList = [];
        IsSuccess = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultList{TIn}"/> struct with an error derived from the specified exception.
    /// </summary>
    /// <param name="exception">The exception from which to create the error.</param>
    internal ResultList(Exception exception)
    {
        InternalResultError = ResultError.New(exception);
        ResultsList = [];
        IsSuccess = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultList{TIn}"/> struct with an empty collection of results.
    /// </summary>
#pragma warning disable S1133 // Do not forget to remove this deprecated code someday
    [Obsolete("Use extension methods like Expand and ExpandAsync instead.",true)]
    public ResultList()
        => throw new InvalidOperationException("This line of code should not be reached by developers. Use extension methods like Expand and ExpandAsync instead.");
#pragma warning restore S1133 // Do not forget to remove this deprecated code someday


    /// <summary>
    /// conveniently converts an instance of Error into <see cref="ResultList{TIn}"/>
    /// </summary>
    /// <param name="resultError">an error that may happen in the process</param>
    /// <returns>instance of <see cref="ResultList{TIn}"/> </returns>
    public static implicit operator ResultList<TIn>(ResultError resultError) => new(resultError);
    /// <summary>
    /// conveniently converts an instance of Exception into <see cref="ResultList{TIn}"/>
    /// </summary>
    /// <param name="exception">an exception that may happen in the process</param>
    /// <returns>instance of <see cref="ResultList{TIn}"/> </returns>
    public static implicit operator ResultList<TIn>(Exception exception) => new(exception);

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    public IEnumerator<Result<TIn>> GetEnumerator()
    {
        return new ResultListEnumerator(this);
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Custom enumerator for the <see cref="ResultList{TIn}"/> struct.
    /// </summary>
    private sealed class ResultListEnumerator : IEnumerator<Result<TIn>>
    {
        private readonly Result<TIn>[] _results;
        private int _index;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultListEnumerator"/> class.
        /// </summary>
        /// <param name="resultList">The result list to enumerate.</param>
        public ResultListEnumerator(ResultList<TIn> resultList)
        {
            _results = resultList.ResultsList;
            _index = -1;
        }

        /// <summary>
        /// Gets the current element in the collection.
        /// </summary>
        public Result<TIn> Current => _results[_index];

        /// <summary>
        /// Gets the current element in the collection.
        /// </summary>
        object IEnumerator.Current => Current;

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
        public bool MoveNext()
        {
            _index++;
            return _index < _results.Length;
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        public void Reset()
        {
            _index = -1;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // No resources to dispose
        }
    }
}