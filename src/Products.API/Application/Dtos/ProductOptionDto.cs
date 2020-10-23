using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Products.API.Application.Dtos
{
    public class ProductOptionDto
    {
        [Required]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [Required]
        [JsonPropertyName("productId")]
        public Guid ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [MaxLength(1000)]
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
