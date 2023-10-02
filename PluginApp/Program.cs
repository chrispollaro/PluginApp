using System.Runtime.Loader;
using PluginSDK;

namespace PluginApp;

class Program
{
    private static string _pluginDirectory = @"K:\plugins";
    private static Dictionary<string, IStepProcessor> _plugins = new();

    static async Task Main(string[] args)
    
    
    {
        await LoadPlugins();
        await Initialize();
        await Process();
        
    }

    static async Task Initialize()
    {
        Console.WriteLine("Program initializing...");

        await Task.Delay(1000);

        Console.WriteLine("Initializing plugins...");
        await Task.Delay(1000);
        foreach(var plugin in _plugins.Values)
        {
            plugin.Initialize();
        }
    }

    static Task Process()
    {
        Console.WriteLine("Processing...");

        foreach (var plugin in _plugins.Values)
        {
            plugin.Process();
        }

        return Task.CompletedTask;
    }

    static async Task LoadPlugins()
    {
        AssemblyLoadContext alc = new AssemblyLoadContext("plugins");

        foreach (var dll in Directory.GetFiles(_pluginDirectory, "*.dll"))
        {
            var assembly = alc.LoadFromAssemblyPath(dll);
            var asmTypes = assembly.GetTypes();

            foreach (var type in asmTypes)
            {
                var intss = type?.GetInterfaces();
                foreach (var i in intss)
                {
                    if (i.Name == "IStepProcessor")
                    {
                        IStepProcessor? p = Activator.CreateInstance(type!) as IStepProcessor;
                        if (p is not null)
                        {
                            _plugins.Add(type!.FullName ?? type.Name, p);
                        }
                    }
                }
               
            }
        }

    }

}