using System;

namespace PlaylistMaker.Commons
{
    public interface IResult
    {
        string Message { get; }
        bool Success { get; }
    }

    public interface IResult<T> : IResult
    {
        T Value { get; }
    }
}