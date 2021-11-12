namespace PlaylistMaker.Commons
{
    public static class Result
    {
        public static IResult Ok() => new ResultIntern { Success = true };
        public static IResult Ok(string message) => new ResultIntern { Message = message, Success = true };
        public static IResult Error() => new ResultIntern { Success = false };
        public static IResult Error(string message) => new ResultIntern { Success = false, Message = message };
        public static IResult<T> Ok<T>(T arg) => new ResultIntern<T> { Value = arg, Success = true };
        public static IResult<T> Ok<T>(T arg, string message) => new ResultIntern<T> { Value = arg, Success = true, Message = message };
        public static IResult<T> Error<T>(string message) => new ResultIntern<T> { Message = message, Success = false };
        

        class ResultIntern : IResult
        {
            public string Message { get;  set; } = "";
            public bool Success { get; set; } = false;
        }

        class ResultIntern<T> : ResultIntern, IResult<T>
        {
            public T Value;
            T IResult<T>.Value => Value;
        }
    }

}
