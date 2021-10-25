using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistMaker.Commons
{
    public static class ResultExtensions
    {
        public static IResult Then(this IResult value)
            => value.Success ? Result.Ok() : Result.Error(value.Message);

        public static IResult<TResult> Then<T, TResult>(this IResult<T> value, Func<T, TResult> func)
            => value.Success ? Result.Ok(func(value.Value)) : Result.Error<TResult>(value.Message);

        public static IResult<TResult> Then<T, TResult>(this IResult<T> value, Func<IResult<T>, IResult<TResult>> func)
            => value.Success ? func(value) : Result.Error<TResult>(value.Message);

        public static IResult<TResult> Then<T, TResult>(this IResult<T> value, Func<T, IResult<TResult>> func)
            => value.Success ? func(value.Value) : Result.Error<TResult>(value.Message);

        public static IResult<TResult> Then<T, TResult>(this T value, Func<T, TResult> func)
            => TryCatchExtensions.TryCatch(value, func);
    }
}
