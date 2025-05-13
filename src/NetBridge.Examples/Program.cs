using Microsoft.Extensions.DependencyInjection;
using NetBridge.Abstractions.Dispatching;
using NetBridge.Dispatching.DependencyInjection;
using NetBridge.Examples.Commands;
using NetBridge.Examples.Events;
using NetBridge.Examples.Queries;

namespace NetBridge.Examples
{
    internal class Program
    {
        private static async Task Main()
        {
            var services = new ServiceCollection();

            // Регистрируем все зависимости через DI
            services
                .AddDispatching()
                .AddHandlersFromAssembly(typeof(MyCommand).Assembly);

            var provider = services.BuildServiceProvider();

            var dispatcher = provider.GetRequiredService<IRequestDispatcher>();
            var eventDispatcher = provider.GetRequiredService<IEventDispatcher>();


            // Создаём и отправляем команду
            var command = new MyCommand { Message = "Hello from MyCommand!" };
            await dispatcher.SendAsync(command); // Отправляем команду

            // Создаём и отправляем запрос
            var query = new MyQuery { QueryParameter = "Test Parameter" };
            var result = await dispatcher.QueryAsync<MyQuery, string>(query); // Отправляем запрос
            Console.WriteLine(result);  // Выводим результат запроса

            // Публикация события
            var myEvent = new MyEvent { Payload = "Something happened!" };
            await eventDispatcher.PublishAsync(myEvent);
        }
    }
}
