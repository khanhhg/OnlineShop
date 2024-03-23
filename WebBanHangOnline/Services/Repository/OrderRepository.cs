using Microsoft.EntityFrameworkCore;
using WebBanHangOnline.Data;
using WebBanHangOnline.Data.Models.EF;
using WebBanHangOnline.Services.IRepository;

namespace WebBanHangOnline.Services.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Order order)
        {
            _context.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Order order)
        {
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Order>> GetAll()
        {
            return await _context.Order.OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        public async Task<Order> Get(int Id)
        {
            return await _context.Order.FirstOrDefaultAsync(x => x.OrderId == Id);
        }

        public async Task<Order> Update(Order OrderChanges)
        {
            var objOder = _context.Order.Attach(OrderChanges);
            objOder.State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return OrderChanges;
        }

        public async Task ChangeStatus(Order order, int status)
        {
            _context.Order.Attach(order);
            order.TypePayment = status;
            _context.Entry(order).Property(x => x.TypePayment).IsModified = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IList<OrderDetail>> GetOrderDetail(int orderID)
        {
            var items = await _context.OrderDetail.Where(x => x.OrderId == orderID).ToListAsync();
            var objProduct = await _context.Product.Include(x => x.Orders.Where(x => x.OrderId == orderID)).ToListAsync();          
            return items;
        }
        public async Task<IList<Order>> GetOderByUser(string userID)
        {
            return await _context.Order.Where(x => x.UserID == userID).OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

    }
}
