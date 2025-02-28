using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Modularity;

namespace KekikStream.Webtop.Hubs
{
    public class EventBusHub: AbpModule
    {
        private readonly ILocalEventBus _localEventBus;

        public EventBusHub(ILocalEventBus localEventBus)
        {
            _localEventBus = localEventBus;
        }

        public async Task SendMessageAsync(string from, string message, string type)
        {
            await _localEventBus.PublishAsync(new MessageEvent(from, message, type));
        }
    }

    public class MessageEvent
    {
        public string From { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }

        public MessageEvent(string from, string message, string type)
        {
            From = from;
            Message = message;
            Type = type;
        }
    }
}
