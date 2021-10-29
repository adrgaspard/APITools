using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace APITools.ASP.Client
{
    /// <summary>
    /// Represents the base class for a hub client with the methods provided to handle some events.
    /// </summary>
    public abstract class HubClient : IDisposable
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="hubConnectionBuilder">The connection builder (with its parameters)</param>
        /// <param name="immediatelyStart">Whether the connection starts immediately after being built</param>
        /// <exception cref="ArgumentNullException">Occurs when <paramref name="hubConnectionBuilder"/> is null</exception>
        protected HubClient(HubConnectionBuilder hubConnectionBuilder, bool immediatelyStart)
        {
            if (hubConnectionBuilder is null)
            {
                throw new ArgumentNullException(nameof(hubConnectionBuilder));
            }
            Connection = hubConnectionBuilder.Build();
            Connection.Closed += OnDisconnected;
            Connection.Reconnecting += OnReconnecting;
            Connection.Reconnected += OnReconnected;
            if (immediatelyStart)
            {
                Task.Run(StartConnectionAsync);
            }
        }

        /// <summary>
        /// Gets or sets the hub connection used by the client.
        /// </summary>
        protected HubConnection Connection { get; init; }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public virtual void Dispose()
        {
            Connection.Closed -= OnDisconnected;
            Connection.Reconnecting -= OnReconnecting;
            Connection.Reconnected -= OnReconnected;
            Connection.StopAsync();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Starts the connection to the hub.
        /// </summary>
        /// <exception cref="InvalidOperationException">Occurs when the connection state is not "Disconnected"</exception>
        public async Task StartConnectionAsync()
        {
            if (Connection.State == HubConnectionState.Disconnected)
            {
                await Connection.StartAsync();
            }
            else
            {
                throw new InvalidOperationException($"The methode {nameof(StartConnectionAsync)} can't be called when the connection is on state {Connection.State}");
            }
        }

        /// <summary>
        /// Called when the client gets disconnected from the server. This can be a voluntary disconnection or an error (in the latter case, the exception can be retrieved in parameter).
        /// </summary>
        /// <param name="disconnectionException">The exception of the disconnection event (null if the disconnection is voluntary)</param>
        protected abstract Task OnDisconnected(Exception disconnectionException);

        /// <summary>
        /// Called when the client is reconnected to the server.
        /// </summary>
        /// <param name="connectionId">The new connection id or null if negotiation was skipped</param>
        protected abstract Task OnReconnected(string connectionId);

        /// <summary>
        /// Called when the client try to reconnect to the server after losing its underlying connection.
        /// </summary>
        /// <param name="disconnectionException">The exception that describes the circumstances of the disconnection</param>
        protected abstract Task OnReconnecting(Exception disconnectionException);
    }
}