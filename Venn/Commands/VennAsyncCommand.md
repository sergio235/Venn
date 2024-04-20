# VennAsyncCommand\<TResult>

The `VennAsyncCommand<TResult>` class is a generic asynchronous command implementation in the Venn.Commands namespace. This command allows you to execute asynchronous operations while providing support for cancellation and notification of task completion.

## Overview

The `VennAsyncCommand<TResult>` class implements the ICommand interface and is designed to work with asynchronous operations that return a result of type `TResult`. It provides properties and methods to manage the execution of the asynchronous command, including cancellation support and notification of task completion.

## Features

- **Asynchronous Execution**: Executes asynchronous operations using the provided `Func<CancellationToken, Task<TResult>>` delegate.
- **Cancellation Support**: Includes a nested `CancelAsyncCommand` class to support cancellation of the asynchronous operation.
- **Task Completion Notification**: Utilizes the `NotifyTaskCompletion<TResult>` class to notify when the asynchronous task completes.

## Usage

To use the `VennAsyncCommand<TResult>` class, follow these steps:

1. **Instantiate the Command**: Create an instance of `VennAsyncCommand<TResult>` by providing the asynchronous operation as a `Func<CancellationToken, Task<TResult>>` delegate.

2. **Execute the Command**: Call the `ExecuteAsync` method to execute the asynchronous operation.

3. **Manage Task Completion**: Access the `Execution` property to monitor the status of the asynchronous task. This property returns a `NotifyTaskCompletion<TResult>` object, which provides information about the task's progress and completion.

4. **Cancel the Operation**: Use the `CancelCommand` property to cancel the asynchronous operation if needed.

## Example

```csharp
// Create an asynchronous command
var asyncCommand = new VennAsyncCommand<int>(async token =>
{
    await Task.Delay(1000, token); // Simulate asynchronous operation
    return 42; // Return a result
});

// Execute the command
await asyncCommand.ExecuteAsync(null);

// Access the result when the task completes
var result = asyncCommand.Execution.Result;
