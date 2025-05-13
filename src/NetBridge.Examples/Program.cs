using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetBridge.Abstractions.Dispatching;
using NetBridge.Dispatching.DependencyInjection;
using NetBridge.Examples.Commands;
using NetBridge.Examples.Events;
using NetBridge.Examples.Queries;
using NetBridge.Messaging.DependencyInjection;

namespace NetBridge.Examples
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            // Конфигурация для локального использования
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // Конфигурация диспетчера для обработки команд и запросов
                    services
                        .AddDispatching()
                        .AddHandlersFromAssembly(typeof(MyCommand).Assembly);
                })
                .Build();

            var dispatcher = host.Services.GetRequiredService<IRequestDispatcher>();
            var eventDispatcher = host.Services.GetRequiredService<IEventDispatcher>();

            // Создаём и отправляем команду
            var command = new MyCommand { Message = "Hello from MyCommand!" };
            await dispatcher.SendAsync(command); // Отправляем команду

            // Создаём и отправляем запрос
            var query = new MyQuery { QueryParameter = "Test Parameter" };
            var result = await dispatcher.QueryAsync<MyQuery, MyQueryResponse>(query); // Отправляем запрос
            Console.WriteLine(result.Value);  // Выводим результат запроса

            // Публикация события
            var myEvent = new MyEvent { Payload = "Something happened!" };
            await eventDispatcher.PublishAsync(myEvent);


            // Конфигурация для использования с RabbitMQ
            host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddMessagingDispatchers()
                        .AddMessagingWithRabbitMq(context.Configuration,
                            config =>
                            {
                                // Регистрируем потребителя команды
                                config.AddDispatchingCommand<MyCommand, MyCommandHandler>();
                                // Регистрируем потребителя запроса
                                config.AddDispatchingQuery<MyQuery, MyQueryResponse, MyQueryHandler>();
                                // Регистрируем consumer для события
                                config.AddDispatchingEvent<MyEvent, MyEventHandler>();
                            })
                        .AddHandlersFromAssembly(typeof(MyCommand).Assembly);   // если есть локальные хендлеры
                })
                .Build();

            await host.StartAsync();

            dispatcher = host.Services.GetRequiredService<IRequestDispatcher>();
            eventDispatcher = host.Services.GetRequiredService<IEventDispatcher>();

            // Создаём и отправляем команду
            command = new MyCommand { Message = "Command from RabbitMQ!" };
            await dispatcher.SendAsync(command);

            // Создаём и отправляем запрос
            query = new MyQuery { QueryParameter = "RabbitMQ Query" };
            var qResult = await dispatcher.QueryAsync<MyQuery, MyQueryResponse>(query);
            Console.WriteLine(qResult.Value);  // Output: Query processed: RabbitMQ Query

            // Публикация события
            myEvent = new MyEvent { Payload = "Event from RabbitMQ" };
            await eventDispatcher.PublishAsync(myEvent);

            Console.WriteLine("Ожидаем обработку...");
            Console.ReadLine(); // не даем завершиться приложению

            await host.StopAsync();
        }
    }
}
