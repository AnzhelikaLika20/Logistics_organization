using System.Threading.RateLimiting;

namespace Workshop.Api;

public sealed class Program
{
    public static void Main()
    {
        var builder = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(x => x.UseStartup<Startup>());
        builder.Build().Run();
    }
}