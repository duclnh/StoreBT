using StoreBT.Models;
using StoreBT.Repositories.Interfaces;
using StoreBT.Services.Interfaces;

namespace StoreBT.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IProductRepository _productRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository, IProductRepository productRepository)
        {
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId)
        {
            var items = await _orderItemRepository.GetAllAsync(orderId);
            foreach (var item in items)
            {
                item.Product = await _productRepository.FindByIdAsync(item.ProductId);
            }
            return items;
        }

        public async Task AddRangeAsync(List<OrderItem> items)
        {
            if (items == null || !items.Any())
                return;

            var orderId = items.First().OrderId;

            var existingItems = await _orderItemRepository
                .FindAllAsync(x => !x.IsDeleted && x.OrderId == orderId);

            var productIds = items.Select(x => x.ProductId)
                                  .Concat(existingItems.Select(x => x.ProductId))
                                  .Distinct()
                                  .ToList();

            var products = await _productRepository.FindAllAsync(x => productIds.Contains(x.Id));

            foreach (var item in items)
            {
                var existingItem = existingItems.FirstOrDefault(x => x.Id == item.Id);
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);

                if (existingItem != null)
                {
                    if (product != null)
                    {
                        var diff = item.Quantity - existingItem.Quantity;
                        product.Stock -= diff;
                        product.UpdatedAt = DateTime.Now;
                        _productRepository.Update(product);
                    }
                    existingItem.Quantity = item.Quantity;
                    existingItem.UpdatedAt = DateTime.Now;
                    _orderItemRepository.Update(existingItem);
                }
                else
                {

                    await _orderItemRepository.AddAsync(item);


                    if (product != null)
                    {
                        product.Stock -= item.Quantity;
                        product.UpdatedAt = DateTime.Now;
                        _productRepository.Update(product);
                    }
                }
            }

            var newItemIds = items.Select(x => x.Id).ToList();
            var itemsToDelete = existingItems
                .Where(x => !newItemIds.Contains(x.Id))
                .ToList();

            foreach (var item in itemsToDelete)
            {
                var existingItem = existingItems.FirstOrDefault(x => x.Id == item.Id);
                if (existingItem is not null)
                {
                    existingItem.IsDeleted = true;
                    existingItem.UpdatedAt = DateTime.Now;
                    _orderItemRepository.Update(item);
                }
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null)
                {
                    product.Stock += item.Quantity;
                    product.UpdatedAt = DateTime.Now;
                    _productRepository.Update(product);
                }
            }


            await _orderItemRepository.SaveChangeAsync();
        }

        public Task<bool> AnyAsync(Guid productId)
        {
            return _orderItemRepository.AnyAsync(x => !x.IsDeleted && x.ProductId == productId);
        }
    }
}
