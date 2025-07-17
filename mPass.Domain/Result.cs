namespace mPass.Domain;

public class Result<T>
{
    public bool IsSuccess { get; init; }
    public T Value { get; init; }
    public string[] Errors { get; init; }

    private Result(bool success, T value, string[] errors)
    {
        IsSuccess = success;
        Value = value;
        Errors = errors;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, []);
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>(false, default!, [error]);
    }
    
    public static Result<T> Failure(string[] errors)
    {
        return new Result<T>(false, default!, errors);
    }
}
