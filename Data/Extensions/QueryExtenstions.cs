using ScanerApi.Data.Queries;

namespace Data.Extensions
{
    public static class QueryExtenstions
    {
        public static string Page(this ListQuery query)
        {
            return query?.Limit == 0 ? null : "OFFSET (@offset) ROWS FETCH NEXT @limit ROWS ONLY";
        }
    }
}
