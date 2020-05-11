![.NET Core](https://github.com/gobixm/bootstrap/workflows/Build/badge.svg?branch=master)
# Description

Runs bootstrap actions asynchronously after application starts, returning 503 while bootstrap in progress.

# Usage

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddBootstrap();
    services.AddControllers();
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseBootstrap(async (provider, state, cancel) =>
    {
        state.Progress = "Touching untouchable";
        await Task.Delay(TimeSpan.FromSeconds(10), cancel);
        state.Progress = "Breaking unbreakable";
        await Task.Delay(TimeSpan.FromSeconds(10), cancel);
    });
    
    app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
}
```