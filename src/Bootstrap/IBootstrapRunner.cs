using System.Threading;
using System.Threading.Tasks;

namespace Gobi.Bootstrap
{
    public interface IBootstrapRunner
    {
        bool IsCompleted { get; }
        string Progress { get; }
        Task RunAsync(IBootstrap bootstrap, CancellationToken cancellationToken = default);
    }
}