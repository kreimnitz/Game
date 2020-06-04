
namespace Utilities
{
    public interface IMessageReceivedHandler
    {
        void HandleStateMessage(State state, object sender);
    }
}
