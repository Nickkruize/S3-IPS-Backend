using DAL;
using GenericBusinessLogic;
using S3_webshop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3_webshop.Repositories
{
    public class ProductRepo : GenericRepository<Product>, IProductRepo
    {
        private readonly WebshopContext _context;

        public ProductRepo(WebshopContext db) : base(db)
        {
            _context = db;
        }

        public IEnumerable<Product> GetAll()
        {
            List<DAL.ContextModels.Product> products = _context.Products.ToList();
            
            return ModelConverter.ProductsContextModelsToProductViewModels(products);
        }
    }
}
