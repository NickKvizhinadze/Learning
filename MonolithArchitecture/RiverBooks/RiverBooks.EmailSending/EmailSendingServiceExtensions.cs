﻿using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using RiverBooks.EmailSending.EmailBackgroundService;
using RiverBooks.EmailSending.Integrations;
using Serilog;

namespace RiverBooks.EmailSending;

public static class EmailSendingServiceExtensions
{
    public static IServiceCollection AddEmailSendingModuleService(
        this IServiceCollection services,
        ConfigurationManager configuration,
        ILogger logger,
        List<Assembly> mediatRAssemblies)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
        services.AddMongoDb(configuration);
        
        services.AddTransient<ISendEmail, MimeKitEmailSender>();
        services.AddTransient<IQueueEmailsInOutboxService, MongoDbQueueEmailsOutboxService>();
        services.AddTransient<IGetEmailsFromOutboxService, MongoDbGetEmailsFromOutboxService>();
        services.AddTransient<ISendEmailsFromOutboxService, DefaultSendEmailsFromOutboxService>();


        mediatRAssemblies.Add(typeof(EmailSendingServiceExtensions).Assembly);

        // Add background worker
        services.AddHostedService<EmailSendingBackgroundService>();
        
        logger.Information("{Module} module services registered", "EmailSending");

        return services;
    }
    
    
    internal static IServiceCollection AddMongoDb(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddSingleton<IMongoClient>(serviceProvider =>
        {
            var settings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
            return new MongoClient(settings!.ConnectionString);
        });

        services.AddSingleton(serviceProvider =>
        {
            var settings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
            var client = serviceProvider.GetService<IMongoClient>();
            return client!.GetDatabase(settings!.DatabaseName);
        });

        services.AddTransient(serviceProvider =>
        {
            var database = serviceProvider.GetService<IMongoDatabase>();
            return database!.GetCollection<EmailOutboxEntity>("EmailOutboxEntityCollection");
        });

        return services;
    }
}