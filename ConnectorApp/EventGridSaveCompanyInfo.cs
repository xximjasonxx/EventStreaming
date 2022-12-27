
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using ConnectorApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace ConnectorApp
{
    public class EventGridSaveCompanyInfo
    {
        [FunctionName("EventGridSaveCompanyInfo")]
        public void Run(
            [EventGridTrigger]JObject eventGridEvent,
            [CosmosDB(
                databaseName: "StockData",
                collectionName: "InfoItems",
                PartitionKey = "company",
                ConnectionStringSetting = "CosmosDBConnection")]JArray companies,
            [CosmosDB(
                databaseName: "StockData",
                collectionName: "InfoItems",
                ConnectionStringSetting = "CosmosDBConnection")] ICollector<Company> collector,
            ILogger log)
        {
            log.LogInformation(eventGridEvent.ToString());
            
            var company = companies?.FirstOrDefault(c => c["id"].ToString() == eventGridEvent["data"]["Symbol"].ToString());
            if (company == null)
            {
                collector.Add(JsonConvert.DeserializeObject<Company>(eventGridEvent["data"].ToString()));
                log.LogInformation("Company Info saved to CosmosDB");
            }

            log.LogInformation("Save Company Info to CosmosDB");
        }
    }
}
