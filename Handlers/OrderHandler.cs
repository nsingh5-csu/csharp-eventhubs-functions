using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Function.Models;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Company.Function.Handlers
{
    public static class OrderHandler
    {
        [FunctionName("ProcessOrders")]
        public static async Task ProcessOrders(
            [EventHubTrigger("orders", Connection = "OrderConsumer")] EventData[] events,
            ILogger log)
        {
            var exceptions = new List<Exception>();

            foreach (EventData eventData in events)
            {
                try
                {
                    string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                    log.LogInformation(messageBody);
                }
                catch (Exception e)
                {
                    // We need to keep processing the rest of the batch - capture this exception and continue.
                    // Also, consider capturing details of the message that failed processing so it can be processed again later.
                    exceptions.Add(e);
                }
            }

            // Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }

        // Add up to 100 orders every 5 seconds to the event hub.
        [FunctionName("CreateOrders")]
        public static async Task CreateOrders(
            [TimerTrigger("*/5 * * * * *")] TimerInfo timer,
            [EventHub("orders", Connection = "OrderProducer")] IAsyncCollector<Order> orders,
            ILogger log)
        {
            var rnd = new Random();
            var count = rnd.Next(1, 100);
            log.LogInformation($"Creating {count} orders");

            for (var i = count; i > 0; i--)
            {
                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    Timestamp = DateTime.UtcNow,
                    Cost = rnd.Next(1, 100)
                };

                await orders.AddAsync(order);
            }
        }
    }
}
