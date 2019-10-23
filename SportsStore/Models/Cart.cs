using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class Cart
    {
        private IList<CartLine> CartLineCollection = new List<CartLine>();

        public virtual void AddItem(Product product, int quantity)
        {
            CartLine line = CartLineCollection.Where(l => l.Product.ProductID == product.ProductID).FirstOrDefault();

            if(line == null)
            {
                CartLineCollection.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }

        }
        public virtual bool RemoveLine(Product product) => 
            CartLineCollection.Remove(CartLineCollection.Where(p=>p.Product.ProductID ==product.ProductID).FirstOrDefault());
        public virtual void Clear() => CartLineCollection.Clear();
        public virtual IEnumerable<CartLine> Lines => CartLineCollection;

        public virtual decimal ComputeTotalValue() => CartLineCollection.Sum(line => line.Quantity * line.Product.Price);
    }

    public class CartLine
    {
        public int CardLineID { get; set; } 
        public Product Product { get; set; }
        public int Quantity { get; set; }

        
    }
}
