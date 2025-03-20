using BackendLibrary;
using CoreFrontend.Classes;
using static CoreFrontend.Classes.SpectreConsoleHelpers;

namespace CoreFrontend;

/// <summary>
/// Insert image into table
/// There are no checks to see if the image has already been
/// added so if it has it is inserted again and the extraction
/// gets the first one.
/// </summary>
internal partial class Program
{
    static void Main(string[] args)
    {
        AnsiConsole.MarkupLine("[white on blue]Working[/]");

        DataOperations operations = new();

        // physical file to insert into table
        var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Burning.png");
        // new id after insert.
        var identifier = 0;

        var (success, exception) = operations.InsertFileSimple(fileName, "Burning.png", ref identifier);

        if (success)
        {
            AnsiConsole.MarkupLine($"[green]Burning image inserted[/] [yellow]Id {identifier}[/]");
            /*
             * Extract first instance of Burning.png as Burning1.png to the
             * application folder
             */
            var (successOut, exceptionOut) = operations.ReadFileFromDatabaseTableSimple("Burning.png", "BurningOut.png");
            if (successOut)
            {
                AnsiConsole.MarkupLine("[yellow]Burning image extracted[/]");
            }
            else
            {
                ExceptionHelpers.ColorStandard(exceptionOut);
            }
        }
        else
        {
            ExceptionHelpers.ColorStandard(exception);
        }

        /*
         * Read back image by primary key
         */
        var (success2, exception1) = operations.ReadFileFromDatabaseTableById(1, "blub1.png");
        if (success2)
        {
            AnsiConsole.MarkupLine("[yellow]blub image extracted[/]");
        }
        else
        {
            ExceptionHelpers.ColorStandard(exception);
        }

        ExitPrompt();

    }
}