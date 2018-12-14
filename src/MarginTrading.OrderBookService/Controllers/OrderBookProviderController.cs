using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lykke.Common.Api.Contract.Responses;
using Lykke.MarginTrading.OrderBookService.Contracts;
using Lykke.MarginTrading.OrderBookService.Contracts.Models;
using MarginTrading.OrderBookService.Core.Domain;
using MarginTrading.OrderBookService.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MarginTrading.OrderBookService.Controllers
{
    /// <summary>
    /// Controller to retrieve current order books
    /// </summary>
    [Route("api/[controller]")]
    public class OrderBookProviderController : Controller, IOrderBookProviderApi
    {
        private readonly IOrderBooksProviderService _orderBooksProviderService;
        private readonly IExecutionOrderBooksProviderService _executionOrderBooksProviderService;

        public OrderBookProviderController(
            IOrderBooksProviderService orderBooksProviderService,
            IExecutionOrderBooksProviderService executionOrderBooksProviderService)
        {
            _orderBooksProviderService = orderBooksProviderService;
            _executionOrderBooksProviderService = executionOrderBooksProviderService;
        }

        /// <summary>
        /// Get current order book for <paramref name="exchange"/> and <paramref name="assetPair"/>.
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="assetPair"></param>
        /// <returns></returns>
        [HttpGet("GetOrderBook")]
        public async Task<ExternalOrderBookContract> GetOrderBook(string exchange, string assetPair)
        {
            var orderBook = await _orderBooksProviderService.GetCurrentOrderBookAsync(exchange, assetPair);
            
            return orderBook.ToContract();
        }

        /// <summary>
        /// Get all current order books.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetOrderBooks")]
        public async Task<List<ExternalOrderBookContract>> GetOrderBooks()
        {
            var orderBook = await _orderBooksProviderService.GetCurrentOrderBooksAsync();
            
            return orderBook.Select(x => x.ToContract()).ToList();
        }

        /// <summary>
        /// Get trade execution order book for <paramref name="orderId"/>.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("GetExecutionOrderBook")]
        public async Task<OrderExecutionOrderBookContract> GetExecutionOrderBook(string orderId)
        {
            var orderBook = await _executionOrderBooksProviderService.GetAsync(orderId);
            
            return orderBook.ToContract();
        }
    }
}