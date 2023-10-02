
namespace MyPlugin;

public class StepPlugin : PluginSDK.IStepProcessor
{
    private DateTime _date;

    public void Initialize()
    {
        _date = DateTime.Now;
    }

    public void Process()
    {
        Console.WriteLine($"Plugin initialized at {_date}.");
    }
}