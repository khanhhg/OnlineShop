using WebBanHangOnline.Data.Models.EF;

namespace WebBanHangOnline.Services.IRepository
{
    public interface IOrderRepository
    {
        Task<IList<Order>> GetAll();
        Task<Order> Get(int Id);
        Task Add(Order order);
        Task<Order> Update(Order order);
        Task Delete(Order order);
        Task<IList<OrderDetail>> GetOrderDetail(int OrderID);
        Task ChangeStatus(Order order, int status);
        Task<IList<Order>> GetOderByUser(string UserID);

    }
}
