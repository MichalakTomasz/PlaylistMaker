using PlaylistMaker.Models;
using Prism.Events;

namespace PlaylistMaker.Events
{
    public class StatusBarEvent : PubSubEvent<StatusBarInfo> { }
}
