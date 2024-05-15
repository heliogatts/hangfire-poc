using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using HanfirePoc;

var builder = WebApplication.CreateBuilder(args);

var supportedCultures = new[]
{
    new CultureInfo("en-US"),
};

var mongoConnection = builder.Configuration.GetConnectionString("Hangfire");

var options = new MongoStorageOptions
{
    MigrationOptions = new MongoMigrationOptions
    {
        MigrationStrategy = new DropMongoMigrationStrategy(),
        BackupStrategy = new NoneMongoBackupStrategy()
    },
    CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection,
    Prefix = "todo.Hangfire",
    CheckConnection = false
    
};

builder.Services.AddScoped<ExampleJob>();

builder.Services.AddControllers();

builder.Services.AddHangfire(x =>
{
    x.UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseMongoStorage(mongoConnection, "Hangfire", options);
});

builder.Services.AddHangfireServer(x =>
{
    x.SchedulePollingInterval = TimeSpan.FromSeconds(30);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfireServer(serverOptions =>
{
    serverOptions.ServerName = "Todo.Hangfire";
});

var app = builder.Build();

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    // Formatting numbers, dates, etc.
    SupportedCultures = supportedCultures,
    // UI strings that we have localized.
    SupportedUICultures = supportedCultures
});


app.UseHangfireDashboard();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();