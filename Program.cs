using Spectre.Console;
using System.Diagnostics;
using System.Threading;
using TarkovDumper.Implementations;

namespace TarkovDumper
{
    internal class Program
    {
        private enum GameChoice { EFT = 1, Arena = 2, Both = 3 }

        static void Main(string[] args)
        {
            AnsiConsole.Profile.Width = 420;

            var choice = AskChoice();

            AnsiConsole.Status().Start("Starting...", ctx =>
            {
                ctx.Spinner(Spinner.Known.Dots);
                ctx.SpinnerStyle(Style.Parse("green"));

                // Elevate priorities for the whole status scope
                var proc = Process.GetCurrentProcess();
                var prevProcPrio = proc.PriorityClass;
                var prevThreadPrio = Thread.CurrentThread.Priority;

                try
                {
                    proc.PriorityClass = ProcessPriorityClass.High;
                    Thread.CurrentThread.Priority = ThreadPriority.Highest;

                    if (choice == GameChoice.EFT || choice == GameChoice.Both)
                    {
                        RunProcessor(ctx, new EFTProcessor());
                    }

                    if (choice == GameChoice.Both)
                    {
                        Pause();
                    }

                    if (choice == GameChoice.Arena || choice == GameChoice.Both)
                    {
                        RunProcessor(ctx, new ArenaProcessor());
                    }
                }
                finally
                {
                    // restore priorities
                    proc.PriorityClass = prevProcPrio;
                    Thread.CurrentThread.Priority = prevThreadPrio;
                }
            });

            Pause();
        }

        private static GameChoice AskChoice()
        {
            // Numeric prompt (1,2,3) as requested
            while (true)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[bold]Select target:[/]");
                AnsiConsole.MarkupLine("[green]1[/] = EFT");
                AnsiConsole.MarkupLine("[green]2[/] = Arena");
                AnsiConsole.MarkupLine("[green]3[/] = Both");
                AnsiConsole.Write(">");
                var input = Console.ReadLine()?.Trim();

                if (input == "1") return GameChoice.EFT;
                if (input == "2") return GameChoice.Arena;
                if (input == "3") return GameChoice.Both;

                AnsiConsole.MarkupLine("[red]Invalid selection. Enter 1, 2, or 3.[/]");
            }
        }

        private static void RunProcessor(StatusContext ctx, Processor processor)
        {
            try
            {
                processor.Run(ctx);
                GC.Collect();
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteLine();
                if (processor != null)
                    AnsiConsole.MarkupLine($"[bold yellow]Exception thrown while processing step -> {processor.LastStepName}[/]");

                AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
                if (ex.StackTrace != null)
                {
                    AnsiConsole.MarkupLine("[bold yellow]==========================Begin Stack Trace==========================[/]");
                    AnsiConsole.WriteLine(ex.StackTrace);
                    AnsiConsole.MarkupLine("[bold yellow]===========================End Stack Trace===========================[/]");
                }
            }
        }

        private static void Pause()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
