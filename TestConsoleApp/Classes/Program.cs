using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace
namespace TestConsoleApp;
internal partial class Program
{
    [ModuleInitializer]
    public static void Init()
    {

        Console.Title = "Code sample: working with storing images in SQL-Server";
        WindowUtility.SetConsoleWindowPosition(WindowUtility.AnchorWindow.Center);

        var files = Directory.GetFiles(".", "blub*.png");
        foreach (var file in files)
        {
            File.Delete(file);
        }
    }
}
