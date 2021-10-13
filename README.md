# Order Processing Example
This is a simple producer/consumer example of processing events from Azure Event Hubs with Azure Functions.

## Setup
### Local
Ensure you include a `local.settings.json` file with the following properties:
```json
    {
        "IsEncrypted": false,
        "Values": {
            "AzureWebJobsStorage": "[your_azure_storage_connection_string]",
            "FUNCTIONS_WORKER_RUNTIME": "dotnet",
            "OrderConsumer": "[your_event_hubs_consumer_connection_string]",
            "OrderProducer": "[your_event_hubs_producer_connection_string]"
        }
    }
```
### Deployed
Ensure you include app settings that correspond to the above values section of the local.settings.json file.