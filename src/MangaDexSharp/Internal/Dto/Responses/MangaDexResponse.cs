#nullable disable
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

using MangaDexSharp.Exceptions;
using MangaDexSharp.Internal.Dto;

namespace MangaDexSharp.Internal
{
    internal class MangaDexResponse
    {
        public const string ResultError = "error";
        public const string ResultOk = "ok";

        public string Result { get; set; }
        public IEnumerable<ErrorDto> Errors { get; set; }
        public string Response { get; set; }

        [JsonIgnore]
        public bool IsError => Result == ResultError;

        [JsonIgnore]
        public bool IsOk => Result == ResultOk;

        public void EnsureResponseIsValid()
        {
            if(Result == ResultOk || !IsError)
            {
                return;
            }

            ErrorDto error = Errors.First();

            switch (error.Status)
            {
                case 400:
                    throw new BadRequestException(new Error(error));

                case 401:
                    throw new UnauthorizedException();

                case 403:
                    throw new ForbiddenException(new Error(error));

                case 404:
                    throw new NotFoundException(new Error(error));

                default:
                    throw new MangaDexException(error.Status, new Error(error));
            }
        }
    }
}
