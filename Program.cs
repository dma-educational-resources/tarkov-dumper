using Spectre.Console;
using System.Diagnostics;
using TarkovDumper.Implementations;

namespace TarkovDumper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            AnsiConsole.Profile.Width = 420;
            AnsiConsole.Status().Start("Starting...", ctx => {
                ctx.Spinner(Spinner.Known.Dots);
                ctx.SpinnerStyle(Style.Parse("green"));

                Processor processor = null;
                try
                {
                    processor = new EFTProcessor();
                    processor.Run(ctx);
                    GC.Collect();
                    Pause();
                    processor = new ArenaProcessor();
                    processor.Run(ctx);
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
                finally
                {
                    Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;
                    Thread.CurrentThread.Priority = ThreadPriority.Normal;
                }
            });

            Pause();
        }

        private static void Pause()
        {
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
