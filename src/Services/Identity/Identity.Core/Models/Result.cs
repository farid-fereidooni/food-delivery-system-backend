using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Identity.Core.Models;

public interface IResult;

public interface IResult<out TError>: IResult where TError : IError
{
    bool IsSuccess { get; }
    bool IsFailure { get; }
    TError UnwrapError();
    TResponse Match<TResponse>(Func<TResponse> onSuccess, Func<TError, TResponse> onFailure);
    ValueTask<TResponse> MatchAsync<TResponse>(
        Func<Task<TResponse>> onSuccess, Func<TError, Task<TResponse>> onFailure);
}
public struct Result : IResult<Error>
{
    private Error? _error;

    [MemberNotNullWhen(false, nameof(_error))]
    public bool IsSuccess => _error is null;
    public bool IsFailure => !IsSuccess;

    public TResponse Match<TResponse>(Func<TResponse> onSuccess, Func<Error, TResponse> onFailure)
    {
        return IsSuccess ? onSuccess() : onFailure(_error.Value);
    }

    public async ValueTask<TResponse> MatchAsync<TResponse>(
        Func<Task<TResponse>> onSuccess, Func<Error, Task<TResponse>> onFailure)
    {
        return IsSuccess ? await onSuccess() : await onFailure(_error.Value);
    }

    public Error UnwrapError()
    {
        if (IsSuccess)
            throw new InvalidOperationException("Result is success");

        return _error.Value;
    }


    public static Result Success() => new Result();
    public static Result Error(Error error) => new Result { _error = error };

    public static implicit operator Result(Error error) => new Result { _error = error };

    public static explicit operator Error(Result result)
    {
        return new Error();
    }

}

public interface IResult<TValue, out TError> : IResult<TError>
    where TError : IError
{
    TResponse Match<TResponse>(Func<TValue, TResponse> onSuccess, Func<TError, TResponse> onFailure);
    ValueTask<TResponse> MatchAsync<TResponse>(
        Func<TValue, Task<TResponse>> onSuccess, Func<TError, Task<TResponse>> onFailure);

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
    private TValue? _value;

    [MemberNotNullWhen(false, nameof(_error))]
    [MemberNotNullWhen(true, nameof(_value))]
    public bool IsSuccess { get; private init; }
    public bool IsFailure => !IsSuccess;

    public TResponse Match<TResponse>(Func<TResponse> onSuccess, Func<TError, TResponse> onFailure)
    {
        return IsSuccess ? onSuccess() : onFailure(_error);
    }

    public TResponse Match<TResponse>(Func<TValue, TResponse> onSuccess, Func<TError, TResponse> onFailure)
    {
        return IsSuccess ? onSuccess(_value) : onFailure(_error);
    }

    public async ValueTask<TResponse> MatchAsync<TResponse>(
        Func<Task<TResponse>> onSuccess, Func<TError, Task<TResponse>> onFailure)
    {
        return IsSuccess ? await onSuccess() : await onFailure(_error);
    }

    public async ValueTask<TResponse> MatchAsync<TResponse>(
        Func<TValue, Task<TResponse>> onSuccess, Func<TError, Task<TResponse>> onFailure)
    {
        return IsSuccess ? await onSuccess(_value) : await onFailure(_error);
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
    public static implicit operator Result<TValue, TError>(TValue value) => new() { _value = value, IsSuccess = true };
}

public struct Result<T> : IResult<T, Error>
{
    private Result<T, Error> _result;

    public bool IsSuccess => _result.IsSuccess;
    public bool IsFailure => _result.IsFailure;

    public TResponse Match<TResponse>(Func<TResponse> onSuccess, Func<Error, TResponse> onFailure)
    {
        return _result.Match(onSuccess, onFailure);
    }
    public TResponse Match<TResponse>(Func<T, TResponse> onSuccess, Func<Error, TResponse> onFailure)
    {
        return _result.Match(onSuccess, onFailure);
    }

    public async ValueTask<TResponse> MatchAsync<TResponse>(Func<Task<TResponse>> onSuccess, Func<Error, Task<TResponse>> onFailure)
    {
        return await _result.Match(onSuccess, onFailure);
    }

    public async ValueTask<TResponse> MatchAsync<TResponse>(
        Func<T, Task<TResponse>> onSuccess, Func<Error, Task<TResponse>> onFailure)
    {
        return await _result.Match(onSuccess, onFailure);
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

    public Error UnwrapError()
    {
        return _result.UnwrapError();
    }

    public object Case()
    {
        return _result.Case();
    }

    public static implicit operator Result<T>(Result<T, Error> result) => new() { _result = result };
    public static implicit operator Result<T>(Result<T, IError> result)
    {
        return result.Match(
            value => new Result<T> { _result = value },
            error => new Result<T> { _result = new Error(error.Messages) });
    }

    public static implicit operator Result<T, Error>(Result<T> result)
    {
        return result.Match<Result<T, Error>>(
            value => value,
            error => error);
    }
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
    public Error()
    {
    }

    public Error(ErrorReason reason)
    {
        Reason = reason;
    }

    public Error(string message, [CallerArgumentExpression("message")] string code = "")
    {
        AddMessage(message, code);
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

    public Error AddMessage(string message, [CallerArgumentExpression("message")] string code = "")
    {
        if (string.IsNullOrEmpty(code))
            throw new ArgumentException("Code not specified for error");

        _messages.Add(new Message(message, code));
        return this;
    }

    public Error AddMessages(IEnumerable<Message> messages)
    {
        _messages.AddRange(messages);
        return this;
    }

    public Error CombineError(IError error)
    {
        _messages.AddRange(error.Messages);
        return this;
    }

    public Error CombineError<TError>(IResult<TError> result) where TError : IError
    {
        _messages.AddRange(result.UnwrapError().Messages);
        return this;
    }
}

public readonly struct Message(string description, string code)
{
    public string Description => description;
    public string Code => code;
}
