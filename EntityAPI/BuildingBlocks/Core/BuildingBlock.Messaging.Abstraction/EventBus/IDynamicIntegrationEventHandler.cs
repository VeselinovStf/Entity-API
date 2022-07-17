using System.Threading.Tasks;

namespace BuildingBlock.Messaging.Abstraction.EventBus
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}