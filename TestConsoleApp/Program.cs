using SqlServerUtilitiesLibrary;

namespace TestConsoleApp;

internal partial class Program
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    static async Task Main(string[] args)
    {
        CancellationTokenSource cts = new(TimeSpan.FromSeconds(1));

        var identifier = 1;
        var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"blub1{DateTime.Now.Millisecond}.png");
        var (success, exception) = await DataOperations.ReadFileFromDatabaseTableSimple(identifier, fileName,cts.Token);
        if (success)
        {
            AnsiConsole.MarkupLine($"[yellow]Downloaded image for record with id[/] [cyan]" +
                                   $"{identifier}[/][yellow] as [/][cyan]{Path.GetFileName(fileName)}[/][yellow] to the app folder[/]");

        }
        else
        {
            AnsiConsole.MarkupLine($"[red]{exception.Message}[/]");
        }

        Console.ReadLine();
    }
}