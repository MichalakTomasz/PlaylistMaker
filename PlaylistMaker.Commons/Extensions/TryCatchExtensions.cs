using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistMaker.Commons
{
    public static class TryCatchExtensions
    {
        public static IResult TryCatch(Action action, string message = "")
        {
            try
            {
                action();
                return Result.Ok();
            }
            catch (Exception e)
            {
                return Result.Error(!string.IsNullOrEmpty(message) ? message : e.Message);
            }
        }

        public static IResult<T> TryCatch<T>(Func<T> func, string message = "")
        {
            try
            {
                return Result.Ok(func());
            }
            catch (Exception e)
            {
                return Result.Error<T>(!string.IsNullOrEmpty(message) ? message : e.Message);
            }
        }
        public static IResult TryCatch<T>(this T value, Action<T> action, string message = "")
        {
            try
            {
                action(value);
                return Result.Ok();
            }
            catch (Exception e)
            {
                return Result.Error(!string.IsNullOrEmpty(message) ? message : e.Message);
            }
        }

        public static IResult<TResult> TryCatch<T, TResult>(this T value, Func<T, TResult> func)
        {
            try
            {
                return Result.Ok(func(value));
            }
            catch (Exception e) 
            {
                return Result.Error<TResult>(e.Message);
            }
        }

        public static IResult<TResult> TryCatch<T, TResult>(this IResult<T> value, Func<T, IResult<TResult>> func)
        {
            try
            {
                var result = func(value.Value);
                return result.Success ? result : throw GetExceprion(result.Message);
            }
            catch (Exception e)
            {
                return Result.Error<TResult>(e.Message);
            }
        }

        public static IResult<TResult> TryCatch<T, TResult>(this IResult<T> value, Func<IResult<T>, IResult<TResult>> func)
        {
            try
            {
                var result = func(value);
                return result.Success ? result : throw GetExceprion(result.Message);
            }
            catch (Exception e)
            {
                return Result.Error<TResult>(e.Message);
            }
        }

        static Exception GetExceprion(string message) => 
            new Exception(message != string.Empty ? message : Literals.resultError);
    }
}
