using PlaylistMaker.Models;
using Prism.Events;
using System.Collections.Generic;

namespace PlaylistMaker.Events
{
    public class SelectionEvent : PubSubEvent<IEnumerable<FileAudio>> { }
}
