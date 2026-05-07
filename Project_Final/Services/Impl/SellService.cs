using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project_Final.Context;
using Project_Final.ExceptionHandle;
using Project_Final.Models;
using Project_Final.Models.Entity;
using Project_Final.Models.Request;
using Project_Final.Models.Response;
using Project_Final.Utils;
using System.Text.Json;

namespace Project_Final.Services.Impl
{
    public class SellService : ISellService
    {

        private DBContext dbContext;
        private readonly IMapper mapper;
        private readonly RedisCacheService redisCacheService;

        public SellService(DBContext dbContext, IMapper mapper, RedisCacheService redisCacheService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.redisCacheService = redisCacheService;
        }

        public Response<OrderResponse> Create(string id)
        {
            var cardOrder = redisCacheService.GetCacheAsync<CardOrder>(Constant.CARD_ORDER_CACHE_KEY_PREFIX + id).Result;
            if (cardOrder == null)
                return Response<OrderResponse>.OfFailed(new BussinessException(ErrorCode.NOT_FOUND_DATA, "Không tìm thấy thông tin giỏ hàng"));
            if (cardOrder.CardOrderDetails == null || cardOrder.CardOrderDetails.Count == 0)
                return Response<OrderResponse>.OfFailed(new BussinessException(ErrorCode.NOT_FOUND_DATA, "Không tìm sản phẩm trong giỏ hàng"));
            var order = mapper.Map<Order>(cardOrder);
            dbContext.Orders.Add(order);
            dbContext.SaveChanges();
            var orderResponse = mapper.Map<OrderResponse>(order);
            foreach (var item in cardOrder.CardOrderDetails)
            {
                var detail = mapper.Map<OrderDetail>(item);
                detail.OrderId = order.Id;
                var product = dbContext.Products.FirstOrDefault(p => p.Id == item.ProductId && p.Status == Status.ACTIVE);
                if (product == null)
                    return Response<OrderResponse>.OfFailed(new BussinessException(ErrorCode.NOT_FOUND_DATA, "Sản phẩm không hợp lệ"));
                if (detail.Quantity > product.Quantity || detail.Quantity == 0)
                {
                    return Response<OrderResponse>.OfFailed(
                        new BussinessException(ErrorCode.INVALID_ORDER_QUANTITY, "'" + item.ProductName + "' không đủ số lượng hoặc số lượng trong đơn hàng = 0"));
                }
                product.Quantity = product.Quantity - detail.Quantity;
                dbContext.Update(product);
                if (detail.Price != product.Price)
                {
                    detail.Price = product.Price;
                }
                dbContext.OrderDetails.Add(detail);
            }
            dbContext.SaveChanges();
            redisCacheService.DeleteByKey(Constant.CARD_ORDER_CACHE_KEY_PREFIX + id);
            return Response<OrderResponse>.OfSuccessed(orderResponse);
        }

        public Response<OrderResponse> Get(long id)
        {
            var order = dbContext.Orders.Find(id);
            if (order == null)
            {
                return Response<OrderResponse>.OfFailed(ErrorCode.NOT_FOUND_DATA, "Not found order by id: " + id);
            }
            var orderResponse = mapper.Map<OrderResponse>(order);
            decimal totalAmount = dbContext.OrderDetails
                    .Where(d => d.OrderId == order.Id)
                    .Sum(d => d.Quantity * d.Price);
            orderResponse.TotalAmount = totalAmount;
            return Response<OrderResponse>.OfSuccessed(orderResponse);
        }

        public Response<List<OrderResponse>> Gets(string? createdBy, OrderStatus status, DateTime from, DateTime to, int page, int size)
        {
            var query = dbContext.Orders.AsQueryable();
            if (!createdBy.IsNullOrEmpty())
            {
                query = query.Where(o => o.CreatedBy == createdBy);
            }
            if (status != OrderStatus.ALL)
            {
                query = query.Where(o => o.Status == status);
            }
            if (from < to)
            {
                query = query.Where(o => o.CreatedAt >= from && o.CreatedAt <= to);
            }
            query = query
                .OrderByDescending(o => o.CreatedAt);
            var totalElemant = query.Count();
            var ordersResponse = query
                .Skip((page - 1) * size)
                .Take(size)
                .Select(order => mapper.Map<OrderResponse>(order))
                .ToList();
            foreach (var item in ordersResponse)
            {
                decimal totalAmount = dbContext.OrderDetails
                    .Where(d => d.OrderId == item.Id)
                    .Sum(d => d.Quantity * d.Price);
                item.TotalAmount = totalAmount;
            }
            return Response<List<OrderResponse>>.OfSuccessed(ordersResponse, page, size, totalElemant);
        }

        public Response<List<OrderDetailResponse>> GetDetails(long orderId)
        {
            var detailResponses = dbContext.OrderDetails
                .Where(o => o.OrderId == orderId)
                .OrderBy(d => d.CreatedAt)
                .Select(detail => mapper.Map<OrderDetailResponse>(detail))
                .ToList();
            if (detailResponses.Count == 0)
                return Response<List<OrderDetailResponse>>.OfFailed(
                    new BussinessException(ErrorCode.NOT_FOUND_DATA, "Mã hóa đơn " + orderId + " không tồn tại"));
            return Response<List<OrderDetailResponse>>.OfSuccessed(detailResponses);
        }
    }
}
