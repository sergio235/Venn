# Venn.Generators

Venn.Generators is a C# source generator library designed to simplify the implementation of the `INotifyPropertyChanged` interface in C# classes. It automatically generates property change notification code based on attributes applied to fields, reducing the boilerplate code typically required for implementing property change notifications.

## Purpose

The purpose of this project is to streamline the process of implementing property change notifications in C# classes, particularly those used in UI development frameworks like WPF, Xamarin.Forms, or UWP. By automating the generation of property change notification code, developers can focus more on the core logic of their classes and spend less time writing repetitive code for handling property changes.

## How to Use

To use Venn.Generators in your C# projects, follow these steps:

1. **Install the Package:** Install the Venn.Generators package from NuGet Package Manager by running the following command in the Package Manager Console or terminal:

   ```bash
   dotnet add package Venn.Generators
   ```

2. **Annotate Fields:** In your C# classes where you want to implement property change notifications, annotate the relevant fields with the `VennBindable` attribute. Optionally, you can specify a custom name for the generated property using the `PropertyName` parameter.

    ```csharp
    using Venn.Attributes;

    public class MyClass
    {
        [VennBindable(PropertyName = "MyProperty")]
        private string _myField;
    }
    ```

3. **Compile the Project:** Once you've annotated your fields, compile your project. During the compilation process, the `VennBindableGenerator` will automatically generate the necessary code to implement `INotifyPropertyChanged` and handle property change notifications.

4. **Use Generated Properties:** After the code is generated, you can use the generated properties in your code like any other normal property. The `PropertyChanged` event will be automatically triggered when the value of the associated fields changes.

    ```csharp
    var obj = new MyClass();
    obj.MyProperty = "New Value"; // Property change notification is handled automatically.
    ```

## Customization

You can customize the behavior of the generated properties by specifying different parameters in the `VennBindable` attribute, such as `PropertyName` to specify custom property names.

## Contribution

Contributions to Venn.Generators are welcome! If you encounter any issues, have feature requests, or want to contribute code, feel free to submit them via GitHub.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
