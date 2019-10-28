using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SportsStore.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private ApplicationDbContext context;

        public EFOrderRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        // Подгружаем все связаные данные когда считываем объекты Order из бд
        public IQueryable<Order> Orders => context.Orders.Include(o => o.Lines).ThenInclude(l => l.Product);

        public void SaveOrder(Order order)
        {
            // указываем EF отслежывать обьекты Product полученые из сессии
            // (так как они уже существуют их не нужно добавлять в таблицу Products)
            context.AttachRange(order.Lines.Select(l => l.Product));
            if(order.OrderID == 0)
            {
                context.Orders.Add(order);
            }
            context.SaveChanges();
        }

    }
}
