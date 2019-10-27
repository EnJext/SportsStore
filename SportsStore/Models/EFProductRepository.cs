using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Models
{
    public class EFProductRepository : IProductRepository
    {
        private ApplicationDbContext context;

        public EFProductRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Product> Products => context.Products;

        public Product DeleteProduct(int ProductID)
        {
            Product productToDelete = context.Products.FirstOrDefault(p => p.ProductID == ProductID);

            if(productToDelete != null)
            {
                // удаления объекта из бд
                context.Products.Remove(productToDelete);
                context.SaveChanges();
            }
            return productToDelete;
        }

        // EF отслеживает только объекты которые сам создал 
        // так как объект создан системой привязки модели, EF не видит его похожая ситуация была с сохранением order
        // для order сипользовали - context.Attach() или AttachRange(); ( указывает EF отслежывать переданый обьект)

        // более понятный способ сохранения изменений объекат в бд - изменить отслеживаемый объект с тем же ID
        public void SaveProduct(Product product)
        {
            if(product.ProductID == 0)
            {
                context.Add(product);
            }
            else
            {
                Product ChangeProduct = Products.FirstOrDefault(p => p.ProductID == product.ProductID);
                if (ChangeProduct != null)
                {
                    ChangeProduct.Name = product.Name;
                    ChangeProduct.Price = product.Price;
                    ChangeProduct.Description = product.Description;
                    ChangeProduct.Category = product.Category;
                }
            }
            context.SaveChanges();
        }
    }
}
