using System.Collections.Generic;
using System.Linq;
using Tornado.Shared.Enums;

namespace Tornado.Shared.ViewModels
{
    public class ApiResponse
    {
        public ApiResponseCodes Code { get; set; }
        public string Description { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public bool HasErrors => Errors.Any();

    }

    public class PayloadMetaData
    {
        public PayloadMetaData(int pageIndex, int pageSize, int totalCount, int totalPageCount)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPageCount = totalPageCount;
        }

        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPageCount { get; private set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Payload { get; set; } = default;
        public int TotalCount { get; set; }
        public string ResponseCode { get; set; }

        public PayloadMetaData PayloadMetaData { get; set; } = null;

        public ApiResponse(T data = default, string message = "",
            ApiResponseCodes codes = ApiResponseCodes.OK, int? totalCount = 0, params string[] errors)
        {
            Payload = data;
            Errors = errors.ToList();
            Code = !errors.Any() ? codes : codes == ApiResponseCodes.OK ? ApiResponseCodes.ERROR : codes;
            Description = message;
            TotalCount = totalCount ?? 0;
        }
    }

}
