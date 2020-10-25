using System.Linq;
using Products.API.Application.Dtos;
using X.PagedList;

namespace Products.API.Extensions
{
    public static class PagedListExtensions
    {
        public static PagedListDto<T> ToPagedListDto<T>(this IPagedList<T> pagedList)
        {
            return new PagedListDto<T>
            {
                Items = pagedList.AsEnumerable(),
                Metadata = pagedList.GetMetaData()
            };
        }
    }
}
