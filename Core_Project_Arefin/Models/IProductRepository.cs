using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Project_Arefin.Models
{
    public interface IProductRepository
    {
        Product GetProduct(int id);

        IEnumerable<Product> GetAll();

        Product Add(Product product);

        Product Update(Product product);

        Product Delete(int id);

    }
}
