﻿using MediatR;

namespace KweetService.API.Temp
{
    public class CreateKwetCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Kweet { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
