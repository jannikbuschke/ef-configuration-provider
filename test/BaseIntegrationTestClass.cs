using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EfConfigurationProvider.Test
{
    [Collection("Database collection")]
    public abstract class BaseIntegrationTestClass : IDisposable
    {
        private readonly WebApplicationFactory<Startup> factory;
        protected readonly HttpClient client;
        private readonly IServiceScope scope;

        public BaseIntegrationTestClass(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            client = this.factory.CreateClient();
            scope = this.factory.Server.Host.Services.CreateScope();
        }

        public void Dispose()
        {
            scope.Dispose();
        }

        protected T GetRequiredService<T>()
        {
            return scope.ServiceProvider.GetRequiredService<T>();
        }

        protected Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetRequiredService<IMediator>().Send(request);
        }
    }
}
