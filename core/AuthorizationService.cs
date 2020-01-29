using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace EfConfigurationProvider.Core
{
    public class AuthorizationService
    {
        private readonly Options options;
        private readonly IAuthorizationService authorizationService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthorizationService(Options options, IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor)
        {
            this.options = options;
            this.authorizationService = authorizationService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task<bool> ReadAllAllowed()
        {
            var policy = options.ReadAllPolicy ?? options.GlobalPolicy;
            return Test(policy);
        }

        public Task<bool> UpdateAllAllowed()
        {
            var policy = options.WriteAllPolicy ?? options.GlobalPolicy;
            return Test(policy);
        }

        public Task<bool> ReadPartialAllowed(string path)
        {
            var policy = options.ReadAllPolicy ?? options.GlobalPolicy;
            return Test(policy);
        }

        public Task<bool> UpdatePartialAllowed(string path)
        {
            var policy = options.GetWriteReadPolicy(path) ?? options.GlobalPolicy;
            return Test(policy);
        }

        private async Task<bool> Test(string policy)
        {
            if (policy != null)
            {
                AuthorizationResult result = await authorizationService.AuthorizeAsync(httpContextAccessor.HttpContext.User, policy);
                return result.Succeeded;
            }
            return true;
        }
    }
}
