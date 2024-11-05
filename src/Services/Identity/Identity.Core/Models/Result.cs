using System.Diagnostics.CodeAnalysis;

namespace Identity.Core.Models;

public interface IResult
{
    bool IsSuccess { get; }
    bool IsFailure { get; }
    IError UnwrapError();
}
public struct Result : IResult
{
    private Error? _error;

    [MemberNotNullWhen(false, nameof(_error))]
    public bool IsSuccess => _error is null;
    public bool IsFailure => !IsSuccess;

    public TResponse Match<TResponse>(Func<TResponse> onSuccess, Func<Error, TResponse> onFailure)
    {
        return IsSuccess ? onSuccess() : onFailure(_error.Value);
    }

    IError IResult.UnwrapError()
    {
        return UnwrapError();
    }

    public Error UnwrapError()
    {
        if (IsSuccess)
            throw new InvalidOperationException("Result is success");

        return _error.Value;
    }

    public Result<TValue> And<TValue>(TValue value)
    {
        return IsSuccess ? value : _error.Value;
    }

    public Result<TValue> And<TValue>(Error error)
    {
        return IsSuccess ? error : _error.Value;
    }

    public Result<TValue> And<TValue>(Result<TValue> result)
    {
        return IsSuccess ? result : _error.Value;
    }

    public Result<TValue> AndThen<TValue>(Func<TValue> value)
    {
        return IsSuccess ? value() : _error.Value;
    }

    public Result<TValue> AndThen<TValue>(Func<Error> value)
    {
        return IsSuccess ? value() : _error.Value;
    }

    public Result<TValue> AndThen<TValue>(Func<Result<TValue>> value)
    {
        return IsSuccess ? value() : _error.Value;
    }

    public static Result Success() => new Result();
    public static Result Error(Error error) => new Result { _error = error };

    public static implicit operator Result(Error error) => new Result { _error = error };

    public static explicit operator Error(Result result)
    {
        return new Error();
    }

}

public interface IResult<TValue, out TError> : IResult
    where TError : IError
{
    TResponse Match<TResponse>(Func<TValue, TResponse> onSuccess, Func<TError, TResponse> onFailure);

    TValue Unwrap();
    TValue UnwrapOr(TValue value);
    TValue Expect(string exceptionMessage);
    TValue Expect(Exception exception);

    object Case();
}

public struct Result<TValue, TError> : IResult<TValue, TError>
    where TError : IError
{
    private TError _error;
    private TValue _value;

    [MemberNotNullWhen(false, nameof(_error))]
    [MemberNotNullWhen(true, nameof(_value))]
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public TResponse Match<TResponse>(Func<TValue, TResponse> onSuccess, Func<TError, TResponse> onFailure)
    {
        return IsSuccess ? onSuccess(_value) : onFailure(_error);
    }

    public TValue Unwrap()
    {
        return Expect("Result is failure");
    }

    public TValue UnwrapOr(TValue value)
    {
        return IsSuccess
            ? _value
            : value;
    }

    public TValue Expect(string exceptionMessage)
    {
        return Expect(new InvalidOperationException(exceptionMessage));
    }

    public TValue Expect(Exception exception)
    {
        return IsSuccess
            ? _value
            : throw exception;
    }

    IError IResult.UnwrapError()
    {
        return UnwrapError();
    }

    public TError UnwrapError()
    {
        if (IsSuccess)
            throw new InvalidOperationException("Result is success");

        return _error;
    }

    public object Case()
    {
        return IsSuccess
            ? _value
            : _error;
    }

    public static implicit operator Result<TValue, TError>(TError error) => new() { _error = error };
    public static implicit operator Result<TValue, TError>(TValue value) => new() { _value = value };

}

public struct Result<T> : IResult<T, Error>
{
    private Result<T, Error> _result;

    public bool IsSuccess => _result.IsSuccess;
    public bool IsFailure => _result.IsFailure;

    public TResponse Match<TResponse>(Func<T, TResponse> onSuccess, Func<Error, TResponse> onFailure)
    {
        return _result.Match(onSuccess, onFailure);
    }

    public T Unwrap()
    {
        return _result.Unwrap();
    }

    public T UnwrapOr(T value)
    {
        return _result.UnwrapOr(value);
    }

    public T Expect(string exceptionMessage)
    {
        return _result.Expect(exceptionMessage);
    }

    public T Expect(Exception exception)
    {
        return _result.Expect(exception);
    }

    IError IResult.UnwrapError()
    {
        return UnwrapError();
    }

    public Error UnwrapError()
    {
        return _result.UnwrapError();
    }

    public object Case()
    {
        return _result.Case();
    }

    public static implicit operator Result<T>(Result<T, IError> result)
    {
        return result.Match(
            value => new Result<T> { _result = value },
            error => new Result<T> { _result = new Error(error.Messages) });
    }

    public static implicit operator Result<T>(Result<T, Error> result) => new() { _result = result };
    public static implicit operator Result<T>(Error error) => new() { _result = error };
    public static implicit operator Result<T>(T value) => new() { _result = value };
}

public enum ErrorReason
{
    General = 1,
    NotFound,
    NotAuthenticated,
    NotAllowed,
}
public interface IError
{
    ErrorReason Reason { get; }
    IReadOnlyCollection<Message> Messages { get; }
}

public struct Error : IError
{
    public Error(ErrorReason reason)
    {
        Reason = reason;
    }

    public Error(string code, string message)
    {
        _messages.Add(new Message(code, message));
    }

    public Error(IEnumerable<Message> messages)
    {
        _messages.AddRange(messages);
    }

    public bool IsEmpty => _messages.Count == 0;

    public ErrorReason Reason { get; private set; } = ErrorReason.General;

    private readonly List<Message> _messages = new(1);
    public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();

    public Error WithReason(ErrorReason reason)
    {
        Reason = reason;
        return this;
    }

    public Error AddMessage(string code, string message)
    {
        _messages.Add(new Message(code, message));
        return this;
    }

    public Error AddMessages(IEnumerable<Message> messages)
    {
        _messages.AddRange(messages);
        return this;
    }
}

public readonly struct Message(string code, string description)
{
    public string Code => code;
    public string Description => description;
}
