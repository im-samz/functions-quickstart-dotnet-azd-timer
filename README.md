<!--
---
name: Azure Functions C# Timer Trigger using Azure Developer CLI
description: This repository contains an Azure Functions timer trigger quickstart written in C# and deployed to Azure Functions Flex Consumption using the Azure Developer CLI (azd). The sample uses managed identity and a virtual network to make sure deployment is secure by default.
page_type: sample
products:
- azure-functions
- azure
- entra-id
urlFragment: starter-timer-trigger-csharp
languages:
- csharp
- bicep
- azdeveloper
---
-->

# Azure Functions C# Timer Trigger using Azure Developer CLI

This template repository contains an timer trigger reference sample for functions written in C# (isolated process mode) and deployed to Azure using the Azure Developer CLI (`azd`). The sample uses managed identity and a virtual network to make sure deployment is secure by default. You can opt out of a VNet being used in the sample by setting VNET_ENABLED to false in the parameters.

This project is designed to run on your local computer. You can also use GitHub Codespaces:

[![Open in GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://github.com/codespaces/new?hide_repo_select=true&ref=main&repo=836901178)

This codespace is already configured with the required tools to complete this tutorial using either `azd` or Visual Studio Code. If you're working a codespace, skip down to [Run your app section](#run-your-app-from-the-terminal).

## Common Use Cases for Timer Triggers

- **Regular data processing**: Schedule batch processing jobs to run at specific intervals
- **Maintenance tasks**: Perform periodic cleanup or maintenance operations on your data
- **Scheduled notifications**: Send automated reports or alerts on a fixed schedule
- **Integration polling**: Regularly check for updates in external systems that don't support push notifications

## Prerequisites

- [Azure Storage Emulator (Azurite)](https://learn.microsoft.com/azure/storage/common/storage-use-azurite) - Required for local development with Azure Functions
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Azure Functions Core Tools](https://learn.microsoft.com/azure/azure-functions/functions-run-local?pivots=programming-language-csharp#install-the-azure-functions-core-tools)
- To use Visual Studio to run and debug locally:
  - [Visual Studio 2022](https://visualstudio.microsoft.com/vs/).
  - Make sure to select the **Azure development** workload during installation.
- To use Visual Studio Code to run and debug locally:
  - [Visual Studio Code](https://code.visualstudio.com/)
  - [Azure Functions extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions)

## Run your app from the terminal

1. Start Azurite storage emulator in a separate terminal window:

   ```shell
   docker run -p 10000:10000 -p 10001:10001 -p 10002:10002 mcr.microsoft.com/azure-storage/azurite
   ```

2. From the `timer` folder, run this command to start the Functions host locally:

    ```shell
    func start
    ```

3. Wait for the timer schedule to execute the timer trigger.

4. When you're done, press Ctrl+C in the terminal window to stop the `func.exe` host process.

## Run your app using Visual Studio Code

1. Open the `timer` app folder in a new terminal.
2. Run the `code .` code command to open the project in Visual Studio Code.
3. In the command palette (F1), type `Azurite: Start`, which enables debugging without warnings.
4. Press **Run/Debug (F5)** to run in the debugger. Select **Debug anyway** if prompted about local emulator not running.
5. Wait for the timer schedule to trigger your timer function.

## Deploy to Azure

Run this command to provision the function app, with any required Azure resources, and deploy your code:

```shell
azd up
```

Alternatively, you can opt-out of a VNet being used in the sample. To do so, use `azd env` to configure `VNET_ENABLED` to `false` before running `azd up`:

```bash
azd env set VNET_ENABLED false
azd up
```

## Redeploy your code

You can run the `azd up` command as many times as you need to both provision your Azure resources and deploy code updates to your function app.

> [!NOTE]
> Deployed code files are always overwritten by the latest deployment package.

## Clean up resources

When you're done working with your function app and related resources, you can use this command to delete the function app and its related resources from Azure and avoid incurring any further costs:

```shell
azd down
```

## Source Code

The function code for the timer trigger is defined in [`timerFunction.cs`](./timer/timerFunction.cs).

This code shows the timer function implementation:  

```csharp
/// <summary>
/// Timer-triggered function that executes on a schedule defined by TIMER_SCHEDULE app setting.
/// </summary>
/// <param name="myTimer">Timer information including schedule status</param>
/// <param name="context">Function execution context</param>
/// <remarks>
/// The RunOnStartup=true parameter is useful for development and testing as it triggers
/// the function immediately when the host starts, but should typically be set to false
/// in production to avoid unexpected executions during deployments or restarts.
/// </remarks>
[Function("timerFunction")]
public void Run(
    [TimerTrigger("%TIMER_SCHEDULE%", RunOnStartup = true)] TimerInfo myTimer,
    FunctionContext context
)
{
    _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

    if (myTimer.IsPastDue)
    {
        _logger.LogWarning("The timer is running late!");
    }
}
```

The isolated worker process C# library uses the [TimerTriggerAttribute](https://github.com/Azure/azure-functions-dotnet-worker/blob/main/extensions/Worker.Extensions.Timer/src/TimerTriggerAttribute.cs) from [Microsoft.Azure.Functions.Worker.Extensions.Timer](https://www.nuget.org/packages/Microsoft.Azure.Functions.Worker.Extensions.Timer) to define the function

### Key Features

1. **Parameterized Schedule**: The function uses the `%TIMER_SCHEDULE%` environment variable to determine the execution schedule, making it configurable without code changes.

2. **RunOnStartup Parameter**: Setting `RunOnStartup = true` makes the function execute immediately when the app starts, in addition to running on the defined schedule. This is useful for testing but can be disabled in production.

3. **Past Due Detection**: The function checks if the timer is past due using the `myTimer.IsPastDue` property, allowing for appropriate handling of delayed executions.

4. **Dependency Injection**: The function uses dependency injection to get a properly configured logger, following best practices for Azure Functions.

5. **Isolated Process Mode**: This function runs in isolated process mode, which provides better isolation and more flexibility compared to in-process execution.

### Configuration

The timer schedule is configured through the `TIMER_SCHEDULE` application setting, which follows the NCRONTAB expression format. For example:

- `0 */5 * * * *` - Run once every 5 minutes
- `0 0 */1 * * *` - Run once every hour
- `0 0 0 * * *` - Run once every day at midnight

For more information on NCRONTAB expressions, see [Timer trigger for Azure Functions](https://learn.microsoft.com/azure/azure-functions/functions-bindings-timer).
