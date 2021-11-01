using PlaylistMaker.Models;
using Prism.Events;

namespace PlaylistMaker.Events
{
    public class PlaylistEvent : PubSubEvent<PlaylistEventInfo> { }
}
