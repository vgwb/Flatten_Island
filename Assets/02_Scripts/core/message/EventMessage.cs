using UnityEngine;

namespace Messages
{
	public class EventMessage : Message
	{
		public EventObject eventObject { get; private set; }

		public EventMessage(Object sender, EventObject eventObject) : base(sender)
		{
			SetMessageType(MessageType.MULTICAST);
			this.eventObject = eventObject;
		}
	}
}
