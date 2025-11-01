using OneOf;

namespace GitE.Models.Result;

[GenerateOneOf]
public sealed partial class Result<T> : OneOfBase<SuccessResult<T>, ErrorResult>;
