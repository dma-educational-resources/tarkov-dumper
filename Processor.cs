using dnlib.DotNet;
using ICSharpCode.Decompiler.CSharp;
using Spectre.Console;

namespace TarkovDumper
{
    public abstract class Processor
    {
        protected readonly ModuleDefMD _module;
        protected readonly Decompiler _decompiler_Basic;
        protected readonly Decompiler _decompiler_Async;
        protected readonly DnlibHelper _dnlibHelper;
        protected readonly DumpParser _dumpParser;

        public string LastStepName = "N/A";

        public Processor(string assemblyPath, string dumpPath)
        {
            try
            {
                _module = ModuleDefMD.Load(assemblyPath);
                _module.EnableTypeDefFindCache = true;
            }
            catch (Exception ex)
            {
                throw new Exception($"[bold yellow]Error loading assembly ~[/] [red]{ex.Message}[/]");
            }

            try
            {
                CSharpDecompiler CSharpDecompiler_basic = new(assemblyPath, new()
                {
                    AnonymousMethods = false,
                    ThrowOnAssemblyResolveErrors = false,
                    AsyncAwait = false,
                });
                _decompiler_Basic = new(CSharpDecompiler_basic);

                CSharpDecompiler CSharpDecompiler_async = new(assemblyPath, new()
                {
                    AnonymousMethods = false,
                    ThrowOnAssemblyResolveErrors = false,
                });
                _decompiler_Async = new(CSharpDecompiler_async);
            }
            catch (Exception ex)
            {
                throw new Exception($"[bold yellow]Error loading decompiler ~[/] [red]{ex.Message}[/]");
            }

            try { _dnlibHelper = new(_module); }
            catch (Exception ex)
            {
                throw new Exception($"[bold yellow]Error loading dnlib helper ~[/] [red]{ex.Message}[/]");
            }

            try { _dumpParser = new(dumpPath); }
            catch (Exception ex)
            {
                throw new Exception($"[bold yellow]Error loading dump parser ~[/] [red]{ex.Message}[/]");
            }
        }

        /// <summary>
        /// Run this Processor Job.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void Run(StatusContext ctx)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"Processing {this.GetType()} entries...");
        }
    }
}
