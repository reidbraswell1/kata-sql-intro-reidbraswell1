using System;
using System.Collections.Generic;
namespace SqlIntro
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts(int id);
        Product GetProduct(int id);
        bool DeleteProduct(int id);
        bool UpdateProduct(Product prod);
        bool InsertProduct(Product prod);
        IEnumerable<ProductsAndReviews>GetProductsWithReview();
        IEnumerable<ProductsAndReviews>GetProductsAndReviews();

    }
}