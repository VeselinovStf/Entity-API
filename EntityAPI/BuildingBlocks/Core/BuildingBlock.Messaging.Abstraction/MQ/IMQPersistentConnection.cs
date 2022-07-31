using RabbitMQ.Client;
using System;

namespace BuildingBlock.Messaging.Abstraction.MQ
{
    public interface IMQPersistentConnection
        : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
