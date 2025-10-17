using Ecommerce.Shared.Contract.Abtractions.Enums;
using Ecommerce.Shared.Contract.Commons;
using Microsoft.AspNetCore.Mvc;
namespace Ecommerce.Shared.Contract.Extensions
{
    public static class ResultExtensions
    {
        public static T Match<T>(
        this Result result,
        Func<T> onSuccess,
        Func<Error, T> onFailure)
        {
            return result.IsSuccess ? onSuccess() : onFailure(result.Error!);
        }

        public static T Match<T, TValue>(
            this ResultT<TValue> result,
            Func<TValue, T> onSuccess,
            Func<Error, T> onFailure)
        {
            return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error!);
        }
    }
}
