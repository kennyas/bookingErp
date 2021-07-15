namespace Tornado.Shared.ViewModels
{
    public class BasePaginatedViewModel
    {
        public BasePaginatedViewModel()
        {
            PageIndex = 1;
            PageTotal = 50;
        }
        public int? PageIndex { get; set; }
        public int? PageTotal { get; set; }
    }
}
