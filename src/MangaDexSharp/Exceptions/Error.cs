using System;

using MangaDexSharp.Internal.Dto;

namespace MangaDexSharp.Exceptions
{
    /// <summary>
    /// Error details for failed request
    /// </summary>
    public sealed class Error
    {
        public string? Context { get; internal set; }
        public string Detail { get; }
        public Guid Id { get; }
        public int Status { get; }
        public string Title { get; }

        internal Error(ErrorDto dto)
           : this(new Guid(dto.Id), dto.Status, dto.Title, dto.Detail)
        {
            if(dto.Context != null)
            {
                Context = dto.Context;
            }
        }

        internal Error(Guid id, int status, string title, string detail)
        {
            Id = id;
            Status = status;
            Title = title;
            Detail = detail;
        }
    }
}
