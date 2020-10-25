using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using X.PagedList;

namespace Products.API.Application.Dtos
{
    public class PagedListDto<T>
    {
        [Required]
        [JsonPropertyName("items")]
        public IEnumerable<T> Items { get; set; } = default!;

        [JsonPropertyName("metadata")]
        public PagedListMetaData Metadata { get; set; } = default!;
    }
}
