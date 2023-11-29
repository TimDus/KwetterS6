﻿using Common.Eventing;

namespace KweetService.API.Eventing.EventReceiver.CustomerCreated
{
    public class CustomerCreatedEvent : Event
    {
        public int CustomerId { get; set; }

        public string DisplayName { get; set; }

        public string CustomerName { get; set; }
    }
}
