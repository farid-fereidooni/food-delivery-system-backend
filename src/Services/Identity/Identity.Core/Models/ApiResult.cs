using System.Diagnostics.CodeAnalysis;

namespace Identity.Core.Models;

public class ApiResult<TOk> : EmptyApiResult
{
    public TOk? Data { get; set; }

    [MemberNotNullWhen(true, nameof(Data))]
    public new bool IsSuccessful { get; set; }
}

public class EmptyApiResult
{
    public IEnumerable<string> Messages { get; set; } = [];

    public bool IsSuccessful { get; set; }
}
