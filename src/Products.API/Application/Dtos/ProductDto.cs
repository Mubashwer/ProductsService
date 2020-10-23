using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Products.API.Application.Dtos
{
    public class ProductDto
    {
        [Required]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [MaxLength(1000)]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [Required]
        [Range(0, (double)decimal.MaxValue)]
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, (double)decimal.MaxValue)]
        [JsonPropertyName("deliveryPrice")]
        public decimal DeliveryPrice { get; set; }
    }
}
