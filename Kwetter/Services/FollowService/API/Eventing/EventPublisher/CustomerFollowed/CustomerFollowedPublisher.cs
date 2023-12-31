﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace FollowService.API.Eventing.EventPublisher.CustomerFollowed
{
    public class CustomerFollowedPublisher : IRequestHandler<CustomerFollowedEvent>
    {
        private readonly IConnection _connection;

        public CustomerFollowedPublisher(IConnection connection)
        {
            _connection = connection;
        }

        public async Task Handle(CustomerFollowedEvent request, CancellationToken cancellationToken)
        {
            await PublishEvent(request);
        }

        private async Task<bool> PublishEvent(CustomerFollowedEvent @event)
        {
            var exchangeName = "customer-followed-exchange";
            var routingKey = "customer.followed";
            var body = await Task.Run(() => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event)));
            using (var channel = _connection.CreateModel())
            {
                channel.BasicPublish(exchangeName, routingKey, null, body);
            }
            await Task.CompletedTask;
            return true;
        }
    }
}
