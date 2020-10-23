using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using X.PagedList;

namespace Products.API.Application.Dtos
{
    public class PagedListDto<T>
    {
        public PagedListDto(IPagedList<T> pagedList)
        {
            Items = pagedList.AsEnumerable();
            Metadata = pagedList.GetMetaData();
        }

        [Required]
        [JsonPropertyName("items")]
        public IEnumerable<T> Items { get; }

        [JsonPropertyName("metadata")]
        public PagedListMetaData Metadata { get; }
    }
}
