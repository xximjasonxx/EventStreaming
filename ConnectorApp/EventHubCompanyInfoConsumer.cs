using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventGrid;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;

namespace ConnectorApp.Functions
{
    public class EventHubCompanyInfoConsumer
    {
        [FunctionName("ConsumeCompanyInfoEvent")]
        public async Task Run(
            [EventHubTrigger("eh-company-info", Connection = "EventHubConnectionString")] EventData[] events,
            [EventGrid(TopicEndpointUri = "EventGridEndpoint", TopicKeySetting = "EventGridKey")] IAsyncCollector<EventGridEvent> eventCollector,
            ILogger log)
        {
            foreach (var anEvent in events)
            {
                var contents = Encoding.UTF8.GetString(anEvent.Body);
                var eventGridEvent = new EventGridEvent(
                    subject: "CompanyInfo",
                    eventType: "InfoEvent",
                    dataVersion: "1.0",
                    data: contents
                );

                log.LogInformation($"Sending event to Event Grid: {contents} - EventType: {eventGridEvent.EventType}");
                await eventCollector.AddAsync(eventGridEvent);
            }
        }
    }
}
