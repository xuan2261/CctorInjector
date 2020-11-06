using System;

using dnlib.DotNet;
using dnlib.DotNet.Writer;

namespace CctorInjector.CLI
{
    class Program
    {
        static void Main(string[] args) => new Program().Run(args);

        public void Run(string[] args)
        {
            if (args.Length is 0) Environment.Exit(0);
            ModuleDef module = ModuleDefMD.Load(args[0]);

            Injector Injector = new Injector(module, typeof(Test));
            Injector.Inject();

            if (Injector.hasInjected)
                Injector.AddCall("ExecuteMePlease");

            SaveFile(module);
            Console.ReadLine();
        }

        public void SaveFile(ModuleDef module)
        {
            var opts = new ModuleWriterOptions(module)
            {
                Logger = DummyLogger.NoThrowInstance
            };
            module.Write("Test.exe", opts);
            Console.WriteLine("Module Saved.");
        }
    }

    public class Test
    {
        public static void ExecuteMePlease()
        {
            Console.WriteLine("Thanks!");
        }
    }
}