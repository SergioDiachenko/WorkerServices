using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.QueueService
{
    public interface IBackgroundTaskQueue
    {
        ValueTask QueueBackgroundItemAsync(
            Func<CancellationToken, ValueTask> workItem);

        ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(
            CancellationToken cancellationToken);
    }
}
