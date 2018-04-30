using System;
using System.Collections.Generic;
namespace SqlIntro
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();
        Product GetProduct(int id);
        bool DeleteProduct(int id);
        bool UpdateProduct(Product prod);
        bool InsertProduct(Product prod);
        IEnumerable<Product>GetProductsWithReview();
        IEnumerable<Product>GetProductsAndReviews();

    }
}