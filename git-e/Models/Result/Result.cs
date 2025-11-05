using OneOf;

namespace GitE.Models.Result;

[GenerateOneOf]
public sealed partial class Result<T> : OneOfBase<SuccessResult<T>, ErrorResult>
{
    public static implicit operator Result<T>(T success) => new SuccessResult<T>(success);
}
