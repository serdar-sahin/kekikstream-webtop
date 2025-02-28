using System.Threading.Tasks;
using KekikStream.Webtop.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Identity;

// https://docs.abp.io/en/abp/latest/SignalR-Integration
// https://github.com/abpframework/abp-samples/blob/master/SignalRDemo/src/SignalRDemo.Web/Pages/Chat.cshtml
namespace KekikStream.Webtop.Hubs
{
    //[Authorize]
    //[HubRoute("/socket-hub")]
    public class SocketHub : AbpHub
    {
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly ILookupNormalizer _lookupNormalizer;

        public SocketHub(IIdentityUserRepository identityUserRepository, ILookupNormalizer lookupNormalizer)
        {
            _identityUserRepository = identityUserRepository;
            _lookupNormalizer = lookupNormalizer;
        }

        //public async Task SendMessage(string name, string command, string message)
        //{
        //    await Clients.All.SendAsync("getMessage", name, command, message);
        //}

        public async Task SendMessage(string from, string to, SocketCommand command, string message)
        {
            await Clients.All.SendAsync("getMessage", from, to, command, message);
        }

        public async Task SendUserMessage(string targetUserName, string message)
        {
            var targetUser = await _identityUserRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName(targetUserName));
            //var txt = L["MyText"]; //Localization

            message = $"{CurrentUser.UserName}: {message}";

            await Clients
                .User(targetUser.Id.ToString())
                .SendAsync("getUserMessage", message);
        }
    }
}
