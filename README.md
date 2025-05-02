<!--
---
name: Azure Functions C# Timer Trigger using Azure Developer CLI
description: This repository contains an Azure Functions timer trigger quickstart written in C# and deployed to Azure Functions Flex Consumption using the Azure Developer CLI (azd). The sample uses managed identity and a virtual network to make sure deployment is secure by default.
page_type: sample
products:
- azure-functions
- azure
- entra-id
urlFragment: starter-http-trigger-csharp
languages:
- csharp
- bicep
- azdeveloper
---
-->

# Azure Functions C# Timer Trigger using Azure Developer CLI

This template repository contains an timer trigger reference sample for functions written in C# (isolated process mode) and deployed to Azure using the Azure Developer CLI (`azd`). The sample uses managed identity and a virtual network to make sure deployment is secure by default. You can opt out of a VNet being used in the sample by setting SKIP_VNET to true in the parameters.

This source code builds on top of the article [Quickstart: Create and deploy functions to Azure Functions using the Azure Developer CLI](timers://learn.microsoft.com/azure/azure-functions/create-first-function-azure-developer-cli?pivots=programming-language-dotnet) to showcase the use of a timer trigger.

This project is designed to run on your local computer. You can also use GitHub Codespaces:

[![Open in GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://github.com/codespaces/new?hide_repo_select=true&ref=main&repo=836901178)

This codespace is already configured with the required tools to complete this tutorial using either `azd` or Visual Studio Code. If you're working a codespace, skip down to [Prepare your local environment](#prepare-your-local-environment).

## Prerequisites

+ [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
+ [Azure Functions Core Tools](https://learn.microsoft.com/azure/azure-functions/functions-run-local?pivots=programming-language-csharp#install-the-azure-functions-core-tools)
+ To use Visual Studio to run and debug locally:
  + [Visual Studio 2022](https://visualstudio.microsoft.com/vs/).
  + Make sure to select the **Azure development** workload during installation.
+ To use Visual Studio Code to run and debug locally:
  + [Visual Studio Code](https://code.visualstudio.com/)
  + [Azure Functions extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions)

## Initialize the local project

You can initialize a project from this `azd` template in one of these ways:

+ Use this `azd init` command from an empty local (root) folder:

    ```shell
    azd init --template functions-quickstart-dotnet-azd-timer
    ```

    Supply an environment name, such as `flexquickstart` when prompted. In `azd`, the environment is used to maintain a unique deployment context for your app.

+ Clone the GitHub template repository locally using the `git clone` command:

    ```shell
    git clone https://github.com/Azure-Samples/functions-quickstart-dotnet-azd-timer.git
    cd functions-quickstart-dotnet-azd-timer
    ```

    You can also clone the repository from your own fork in GitHub.

## Prepare your local environment

Navigate to the `timer` app folder and create a file in that folder named _local.settings.json_ that contains this JSON data:

```json
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
        "TIMER_SCHEDULE": "30 * * * * *"
    }
}
```

## Run your app from the terminal

1. From the `timer` folder, run this command to start the Functions host locally:

    ```shell
    func start
    ```

1. Wait for the timer schedule to execute the timer trigger.

1. When you're done, press Ctrl+C in the terminal window to stop the `func.exe` host process.

## Run your app using Visual Studio Code

1. Open the `timer` app folder in a new terminal.
1. Run the `code .` code command to open the project in Visual Studio Code.
1. In the command palette (F1), type `Azurite: Start`, which enables debugging without warnings.
1. Press **Run/Debug (F5)** to run in the debugger. Select **Debug anyway** if prompted about local emulator not running.
1. Wait for the timer schedule to trigger your timer function.

## Source Code

The function code for the timer function is defined in [`timerFunction.cs`](./timer/timerFunction.cs).

This code shows the timer function logic:  

```csharp
[Function("timerFunction")]
public void Run([TimerTrigger("%TIMER_SCHEDULE%")] TimerInfo myTimer, FunctionContext context)
{
    _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

    if (myTimer.IsPastDue)
    {
        _logger.LogWarning("The timer is running late!");
    }
}
```

## Deploy to Azure

Run this command to provision the function app, with any required Azure resources, and deploy your code:

```shell
azd up
```

Alternatively, you can opt-out of a VNet being used in the sample. To do so, use `azd env` to configure `SKIP_VNET` to `true` before running `azd up`:

```bash
azd env set SKIP_VNET true
azd up
```

You're prompted to supply these required deployment parameters:

| Parameter | Description |
| ---- | ---- |
| _Environment name_ | An environment that's used to maintain a unique deployment context for your app. You won't be prompted if you created the local project using `azd init`.|
| _Azure subscription_ | Subscription in which your resources are created.|
| _Azure location_ | Azure region in which to create the resource group that contains the new Azure resources. Only regions that currently support the Flex Consumption plan are shown.|

After publish completes successfully, `azd` provides you with the URL endpoints of your new functions, but without the function key values required to access the endpoints. To learn how to obtain these same endpoints along with the required function keys, see [Invoke the function on Azure](https://learn.microsoft.com/azure/azure-functions/create-first-function-azure-developer-cli?pivots=programming-language-dotnet#invoke-the-function-on-azure) in the companion article [Quickstart: Create and deploy functions to Azure Functions using the Azure Developer CLI](https://learn.microsoft.com/azure/azure-functions/create-first-function-azure-developer-cli?pivots=programming-language-dotnet).

## Redeploy your code

You can run the `azd up` command as many times as you need to both provision your Azure resources and deploy code updates to your function app.

> [!NOTE]
> Deployed code files are always overwritten by the latest deployment package.

## Clean up resources

When you're done working with your function app and related resources, you can use this command to delete the function app and its related resources from Azure and avoid incurring any further costs:

```shell
azd down
```
