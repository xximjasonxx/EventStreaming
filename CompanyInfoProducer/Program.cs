
using System.Text;
using System.Text.Json;
using Azure.Data.SchemaRegistry;
using Azure.Identity;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using CompanyInfoProducer;

Console.Write("Enter a Company Name: ");
var companyName = Console.ReadLine();
if (string.IsNullOrWhiteSpace(companyName))
{
    Console.WriteLine("Invalid Company Name");
    return;
}

Console.Write("Enter a Company Symbol: ");
var companySymbol = Console.ReadLine();
if (companySymbol?.Length < 2)
{
    Console.WriteLine("Invalid Company Symbol");
    return;
}

Console.Write("Enter a Company Size: ");
var companySize = Console.ReadLine()?.AsInt() ?? 0;
if (companySize <= 0)
{
    Console.WriteLine("Invalid Company Size");
    return;
}

// send the event
var producerClient = new EventHubProducerClient(
    connectionString: "Endpoint=sb://ehns-main-jx01.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=9yD2KmC4jueVMtJNwgDSQcnrEsegpLSKNKGA5yxvADw=",
    eventHubName: "eh-company-info");
var schemaRegistryEndpoint = "ehns-main-jx01.servicebus.windows.net>";
var schemaRegistryClient = new SchemaRegistryClient(schemaRegistryEndpoint, credential: new DefaultAzureCredential());

using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();
var companyInfo = new CompanyInfo { Name = companyName, Symbol = companySymbol, Size = companySize };
var companyInfoJson = JsonSerializer.Serialize(companyInfo);
eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(companyInfoJson)));

try
{
    await producerClient.SendAsync(eventBatch);
    Console.WriteLine("Successfuly sent event");
}
finally
{
    await producerClient.DisposeAsync();
    
}