using Newtonsoft.Json;

namespace Tornado.Shared.ViewModels
{
    public class BasePageQueryViewModel
    {
        public BasePageQueryViewModel()
        {
            PageIndex = 1;
            PageTotal = 50;
        }

        [JsonProperty("pageIndex")]
        public int? PageIndex { get; set; }

        [JsonProperty("pageTotal")]
        public int? PageTotal { get; set; }
    }

    public class BaseSearchViewModel : BasePageQueryViewModel
    {
        [JsonProperty("keyword")]
        public string Keyword { get; set; }
    }
}