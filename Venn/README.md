# Venn

<!-- [![version](https://img.shields.io/badge/version-1.0.1-yellow.svg)](https://semver.org) -->

The Venn project is a set of tools for working with WPF applications. It was initially born as a collection of extension methods, but is evolving into something more complex.

## NotifyPropertyChangedWrapper
The `NotifyPropertyChangedWrapper<T>` class is designed to wrap an object that does not implement the INotifyPropertyChanged interface. It facilitates the notification of property changes for the wrapped object by exposing an observable stream that emits events whenever a property changes.

### Class Overview:
- **Generic Type `T`:** Represents the type of the object that the class wraps.
- **Constructor:** Initializes a new instance of the class with an initial value for the wrapped object.
- **Observable Property:** Provides an observable stream (`IObservable<T>`) that emits events whenever a property of the wrapped object changes.
- **Value Property:** Gets or sets the value of the wrapped object. If the value changes, the class triggers the PropertyChanged event and notifies observers through the observable stream.
- **PropertyChanged Event:** Event that is triggered when a property of the wrapped object changes.

### Usage:
1. **Initialization:** Create an instance of `NotifyPropertyChangedWrapper<T>` by providing an initial value for the wrapped object.
   ```csharp
   var wrapper = new NotifyPropertyChangedWrapper<int>(initialValue);

## WhenAny methods
Venn provides you WhenAny methods that allow you to observe INotifyPropertyChanged objects. WhenAny methods return an IObservable to which you can subscribe and perform necessary actions.

The `NotifyPropertyChangedExtensions` class provides extension methods for enabling observable property change notifications on objects that implement the `INotifyPropertyChanged` interface or use the `NotifyPropertyChangedWrapper<T>` class.

### Method Overview:

1. **`WhenAny<T, TProperty>` Method for Objects Implementing `INotifyPropertyChanged`:**
   - **Parameters:**
     - `source`: The object implementing `INotifyPropertyChanged` on which to observe property changes.
     - `propertySelector`: An expression specifying the property to observe changes for.
   - **Returns:**
     - An observable stream (`IObservable<TProperty>`) that emits events whenever the specified property changes.
   - **Usage Example:**
     ```csharp
     var disposables = new CompositeDisposable();
     
     var observable = myObject.WhenAny(obj => obj.MyProperty);
     observable.Subscribe(newValue => {
         // Handle property change
     })
     .DisposeWith(disposables); //add an `IDisposable` object to a `CompositeDisposable`
     ```

2. **`WhenAny<T, TProperty>` Method for `NotifyPropertyChangedWrapper<T>`:**
   - **Parameters:**
     - `source`: An instance of the `NotifyPropertyChangedWrapper<T>` class on which to observe property changes.
     - `propertySelector`: An expression specifying the property to observe changes for.
   - **Returns:**
     - An observable stream (`IObservable<TProperty>`) that emits events whenever the specified property changes.
   - **Usage Example:**
     ```csharp
     var disposables = new CompositeDisposable();
     
     var wrapper = NotifyPropertyChangedWrapper<T>.Wrap(myObject);
     var observable = wrapper.WhenAny(obj => obj.MyProperty);
     observable.Subscribe(newValue => {
         // Handle property change
     })
     .DisposeWith(disposables); //add an `IDisposable` object to a `CompositeDisposable`
     ```

3. **`WhenAny<T>` Method for `NotifyPropertyChangedWrapper<T>` (No Property Selector):**
   - **Parameters:**
     - `source`: An instance of the `NotifyPropertyChangedWrapper<T>` class on which to observe any property changes.
   - **Returns:**
     - An observable stream (`IObservable<T>`) that emits events whenever any property changes.
   - **Usage Example:**
     ```csharp
     var disposables = new CompositeDisposable();

     var wrapper = NotifyPropertyChangedWrapper<T>.Wrap(myObject);
     var observable = wrapper.WhenAny();
     observable.Subscribe(newValue => {
         // Handle any property change
     })
     .DisposeWith(disposables); //add an `IDisposable` object to a `CompositeDisposable`
     ```
## DisposeWith Method for Dispose Management

Venn provides you with a `DisposeWith` extension method that simplifies the management of disposable resources. This method allows you to add an `IDisposable` object to a `CompositeDisposable`, facilitating the coordinated disposal of multiple objects.

### Method Overview:

1. **`DisposeWith` Method:**
   - **Signature:**
     ```csharp
     public static IDisposable DisposeWith(this IDisposable disposable, CompositeDisposable compositeDisposable)
     ```
   - **Parameters:**
     - `disposable` (type: `IDisposable`): The object to be added to the `compositeDisposable`.
     - `compositeDisposable` (type: `CompositeDisposable`): The container of disposable objects.
   - **Exceptions:**
     - `ArgumentNullException`: Thrown if `disposable` or `compositeDisposable` is null.
   - **Returns:**
     - The original `IDisposable` object.
   - **Usage Example:**
     ```csharp
     var disposableObject = //... create your IDisposable object
     var compositeDisposable = new CompositeDisposable();

     // Add the IDisposable object to the CompositeDisposable using DisposeWith
     disposableObject.DisposeWith(compositeDisposable);

     // Dispose of all IDisposable objects in the CompositeDisposable when needed
     compositeDisposable.Dispose();
     ```

### Important Notes:
- The `DisposeWith` method is particularly useful when you want to manage the disposal of multiple objects in a coordinated manner.
- Ensure that the object and the `CompositeDisposable` are not null before using this method to prevent potential null reference exceptions.
- Dispose of the `CompositeDisposable` when it's appropriate to release the resources held by the disposable objects.

This extension method follows a similar pattern to the `WhenAny` methods, providing a clean and concise way to handle the disposal of disposable resources.


## Important Notes:
- These extension methods simplify the process of observing property changes using reactive programming and can be particularly useful in scenarios where real-time updates are required based on property modifications.
- Ensure that the object or wrapper instance is not null before using these methods to prevent potential null reference exceptions.
