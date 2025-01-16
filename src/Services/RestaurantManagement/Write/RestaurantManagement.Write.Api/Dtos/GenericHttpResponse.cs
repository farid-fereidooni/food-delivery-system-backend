using RestaurantManagement.Write.Domain.Dtos;

namespace RestaurantManagement.Write.Api.Dtos;

public class GenericHttpResponse
{
    public GenericHttpResponse()
    {
        IsSuccess = true;
    }

    public GenericHttpResponse(IError error)
    {
        IsSuccess = false;
        ErrorMessages = error.Messages;
    }

    public bool IsSuccess { get; }
    public IReadOnlyCollection<Message> ErrorMessages { get; } = [];
}

public class GenericHttpResponse<TData> : GenericHttpResponse
{
    public GenericHttpResponse(TData data)
    {
        Data = data;
    }

    public GenericHttpResponse(IError error) : base(error)
    {
        Data = default;
    }

    public TData? Data { get; }
}
