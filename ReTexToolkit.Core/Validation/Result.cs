namespace ReTexToolkit.Core.Validation;

/// <summary>
///     A class to hold the result of an action.
/// </summary>
public class Result
{
    public List<ResultMessage> Errors { get; init; } = [];

    public bool HasError() => Errors.Count > 0;

    public bool HasError<TException>() where TException : Exception
    {
        var result = Errors.Any(error => error.IsExceptionOfType<TException>());
        return result;
    }

    public Result Merge(Result result)
    {
        Errors.AddRange(result.Errors);
        return this;
    }

    public Result AddError(Exception ex)
    {
        Errors.Add(new ResultMessage(ex));
        return this;
    }

    public Result Try(Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            AddError(ex);
        }

        return this;
    }

    public async Task<Result> TryAsync(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            AddError(ex);
        }

        return this;
    }
}

public class Result<T> : Result
{
    public Result()
    {
        Value = default;
    }

    public Result(T value)
    {
        Value = value;
    }

    public T? Value { get; set; }

    public new Result<T> Merge(Result result)
    {
        base.Merge(result);
        return this;
    }

    public new Result<T> AddError(Exception ex)
    {
        base.AddError(ex);
        return this;
    }

    public Result<T> Try(Func<T> action, Func<Exception>? onException = null)
    {
        try
        {
            Value = action();
        }
        catch (Exception ex)
        {
            AddError(onException != null ? onException() : ex);
        }

        return this;
    }

    public async Task<Result<T>> TryAsync(Func<Task<T>> action, Func<Exception>? onException = null)
    {
        try
        {
            Value = await action();
        }
        catch (Exception ex)
        {
            AddError(onException != null ? onException() : ex);
        }

        return this;
    }
}