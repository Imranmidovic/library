using grpcCrud;
using library.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace library.Client.Services
{
    public class AuthProvider : AuthenticationStateProvider
    {
        private User _user;
        private IAuthService api;
        public AuthProvider(IAuthService apii)
        {
            api = apii;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var id = new ClaimsIdentity();
            _user = await GetUser();
            if (_user.LoggedIn)
            {
                var claims = new[] { new Claim(ClaimTypes.Name, _user.Name) }
                .Concat(_user.Claims.Select(k => new Claim(k.Key, k.Value)));
                id = new ClaimsIdentity(claims, "Server authentication");
            }
            return new AuthenticationState(new ClaimsPrincipal(id));
        }
        private async Task<User> GetUser()
        => _user is not null && _user.LoggedIn ? _user : await api.CheckUser();
        public async Task Registration(RegMsg r)
        {
            await api.Registration(r);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        public async Task Login(RegMsg l)
        {
            await api.Login(l);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        public async Task Logout()
        {
            await api.Logout();
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
