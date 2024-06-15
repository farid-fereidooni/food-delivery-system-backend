namespace Identity.Core.Models;

public abstract record EmptyResult : IRichEnum
{
    private readonly List<string> _messages = [];
    public IReadOnlyCollection<string> Messages => _messages;

    public void AddMessage(string message)
    {
        _messages.Add(message);
    }

    protected void AddMessages(IEnumerable<string> messages)
    {
        foreach (var message in messages)
        {
            AddMessage(message);
        }
    }

    public record Ok: EmptyResult;

    public record Error : EmptyResult
    {
        public Error(string message) => AddMessage(message);

        public Error(IEnumerable<string> messages) => AddMessages(messages);
    }
}

public abstract record EmptyResult<TError> : EmptyResult
{
    public new record Ok : EmptyResult<TError>;
    public new record Error : EmptyResult<TError>
    {
        public TError Value { get; }

        public Error(string message, TError value)
        {
            AddMessage(message);
            Value = value;
        }

        public Error(IEnumerable<string> messages, TError value)
        {
            AddMessages(messages);
            Value = value;
        }
    }
}

public abstract record Result<TOk> : EmptyResult
{
    public abstract TOk Unwrap();
    public abstract TOk Expect(string message);

    public new record Ok(TOk Value) : Result<TOk>
    {
        public override TOk Unwrap() => Value;

        public override TOk Expect(string message) => Value;
    }

    public new record Error : Result<TOk>
    {
        public Error(string message) => AddMessage(message);
        public Error(IEnumerable<string> messages) => AddMessages(messages);

        public override TOk Unwrap() => Expect($"Value is non-existent for {nameof(Error)} result type");
        public override TOk Expect(string message) => throw new NullReferenceException(message);
    }
}

public abstract record Result<TOk, TError> : Result<TOk>
    where TError : IRichEnum
{
    public new record Ok(TOk Value) : Result<TOk, TError>
    {
        public override TOk Unwrap() => Value;

        public override TOk Expect(string message) => Value;
    }

    public new record Error : Result<TOk, TError>
    {
        public TError Value { get; }

        public Error(string message, TError value)
        {
            AddMessage(message);
            Value = value;
        }

        public Error(IEnumerable<string> messages, TError value)
        {
            AddMessages(messages);
            Value = value;
        }

        public override TOk Unwrap() => Expect($"Value is non-existent for {nameof(Error)} result type");
        public override TOk Expect(string message) => throw new NullReferenceException(message);
    }
}
