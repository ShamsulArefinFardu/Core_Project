using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core_Project_Arefin.Data;


namespace Core_Project_Arefin.Models
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext db;

        public ProductRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public Product Add(Product product)
        {
            db.Products.Add(product);
            db.SaveChanges();
            return product;
        }

        public Product Delete(int id)
        {
            Product product = db.Products.Find(id);
            if(product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
            }
            return product;

        }

        public IEnumerable<Product> GetAll()
        {
            return db.Products;
        }

        public Product GetProduct(int id)
        {
            return db.Products.Where(x => x.ProductID == id).SingleOrDefault();

        }

        public Product Update(Product product)
        {
            db.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            return product;
        }
    }
}
