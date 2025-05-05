using DTOs.Statistic;
using Responses;

namespace Business.Interface
{
    public interface IStatisticService
    {
        public Task<ResponseObject<StatisticGetCounts>> StatisticGetCounts();
    }
}
