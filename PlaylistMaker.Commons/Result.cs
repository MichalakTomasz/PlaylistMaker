using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistMaker.Commons
{
    public static class Result
    {
        public static IResult Ok() => new ResultIntern { Success = true };
        public static IResult Ok(string message) => new ResultIntern { Message = message, Success = true };
        public static IResult Error() => new ResultIntern { Success = false };
        public static IResult Error(string message) => new ResultIntern { Success = false, Message = message };
        public static IResult<T> Ok<T>(T arg) => new ResultIntern<T> { Value = arg, Success = true};
        public static IResult<T> Ok<T>(T arg, string message) => new ResultIntern<T> { Value = arg, Success = true, Message = message };
        public static IResult<T> Error<T>(string message) => new ResultIntern<T> { Message = message, Success = false };
        

        class ResultIntern : IResult
        {
            public string Message { get;  set; } = "";

            public bool Success { get; set; } = false;
            public IResult Ok()
            {
                Success = true;
                return this;
            }
            public IResult Ok(string message)
            {
                Success = true;
                Message = message;
                return this;
            }

            public IResult Error(string message)
            {
                Success = false;
                Message = message;
                return this;
            }
        }

        class ResultIntern<T> : ResultIntern, IResult<T>
        {
            public T Value = default;

            T IResult<T>.Value => default;

            public IResult<T> Ok(T value)
            {
                Success = true;
                Value = value;
                return this;
            }

            public IResult<T> Error(string message, T value)
            {
                Success = false;
                Message = message;
                Value = value;
                return this;
            }
        }
    }
}
