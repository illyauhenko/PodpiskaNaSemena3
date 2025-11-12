namespace PodpiskaNaSemena.Application.Models.Base
{
    /// <summary>
    /// Базовая модель для запросов с пагинацией
    /// </summary>
    public abstract record PagedRequest(int Page = 1, int PageSize = 10)
    {
        public int Skip => (Page - 1) * PageSize;
        public int Take => PageSize;
    }
}