using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.DAL.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static CarRescueSystem.DAL.Model.Transaction;

namespace CarRescueSystem.BLL.Background
{
    public class OrderStatusBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public OrderStatusBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    // Lấy danh sách đơn hàng chưa thanh toán quá 2 phút
                    var expiredOrders = await unitOfWork.TransactionRepo.GetUnpaidOrdersAsync(TimeSpan.FromMinutes(15));
                    Console.WriteLine($"Found {expiredOrders.Count} expired orders");

                    foreach (var order in expiredOrders)
                    {
                        
                        order.status = TransactionStatus.FAILED; // Cập nhật trạng thái
                        await unitOfWork.TransactionRepo.UpdateAsync(order);
                    }

                    await unitOfWork.SaveChangeAsync();
                }

                // Chờ 1 phút rồi kiểm tra tiếp
                await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
            }
        }
    }
}
