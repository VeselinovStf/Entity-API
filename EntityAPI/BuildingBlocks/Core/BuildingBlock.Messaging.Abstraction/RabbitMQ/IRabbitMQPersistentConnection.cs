﻿using RabbitMQ.Client;
using System;

namespace BuildingBlock.Messaging.Abstraction.RabbitMQ
{
    public interface IRabbitMQPersistentConnection
        : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
