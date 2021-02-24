using grpcCrud;
using library.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace library.Client.Services
{
    public class AuthService : IAuthService
    {
        GrpcServ.GrpcServClient kli;
        HttpClient http;
        public AuthService(GrpcServ.GrpcServClient cli, HttpClient httpClient)
        {
            kli = cli;
            http = httpClient;
        }
        public async Task Login(RegMsg l)
        {
            var res = await http.PostAsJsonAsync("api/userinfo/login", l);
            res.EnsureSuccessStatusCode();
        }

        public async Task Logout() => await http.GetAsync("api/userinfo/logout");

        public async Task Registration(RegMsg r)
        {
            await http.PostAsJsonAsync("api/userinfo/registration", r);
        }
        public async Task<User> CheckUser()
        {
            var k = await http.GetFromJsonAsync<User>("api/userinfo/check");
            Console.WriteLine("--------------");
            k.Claims.ToList().ForEach(k => Console.WriteLine($"{k.Key} ----- {k.Value}"));
            Console.WriteLine("--------------");
            return k;
        }
    }
}
