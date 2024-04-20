# ExtendedBindableObject

The `ExtendedBindableObject` class provides extended functionality for bindable objects in .NET MAUI applications. This class extends the functionality of `BindableObject` and provides additional capabilities for pausing and resuming property change notifications, as well as an enhanced property method for change notification.

## Features

- **Pause and Resume Notifications**: The `ExtendedBindableObject` class includes methods for pausing and resuming property change notifications. This can be useful in situations where you want to temporarily halt UI updates, such as during intensive processing operations.

- **Enhanced Property Change Notification**: The `ExtendedBindableObject` class provides a `SetProperty` method that simplifies property change notification and ensures notifications are properly sent even when notifications are suspended.

## Basic Usage

```csharp
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Venn.Base.Interfaces;

namespace Venn.Base.Bindables
{
    public class ExtendedBindableObject : BindableObject, ISuspendableNotifyPropertyChanged
    {
        // Class implementation
    }
}
