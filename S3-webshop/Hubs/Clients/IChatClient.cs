using S3_webshop.Resources;
using System.Threading.Tasks;

namespace S3_webshop.Hubs.Clients
{
    public interface IChatClient
    {
        Task ReceiveMessage(ChatMessage message);
    }
}
