using Common;
using KweetService.API.Temp;
using MediatR;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using System;

namespace KweetService.API.CommandHandler
{
    public class CreateKweetCommandHandler : IRequestHandler<CreateKweetCommand>
    {
        private readonly IConnection _connection;

        public CreateKweetCommandHandler(IConnection connection)
        {
            _connection = connection;
        }

        public async Task Handle(CreateKweetCommand request, CancellationToken cancellationToken)
        {
            var @event = new KweetCreateEvent
            {
                Id = request.Id,
                Kweet = request.Kweet,
                CreatedDate = request.CreatedDate
            };

            await PublishEvent(@event);
        }

        private async Task<bool> PublishEvent(KweetCreateEvent @event)
        {
            var channel = _connection.CreateModel();
            var exchangeName = "kweet-created-exchange"; //TODO: should be a provided variable
            var routingKey = "kweet.created";
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
            channel.BasicPublish(exchangeName, routingKey, null, body); //TODO: should be changed to be async

            return true;
        }
    }
}
