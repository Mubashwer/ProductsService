using System;

namespace Products.Domain.Exceptions
{
    public class ProductsDomainException : Exception
    {
        public ProductsDomainException()
        {
        }

        public ProductsDomainException(string message) : base(message)
        {
        }

        public ProductsDomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}