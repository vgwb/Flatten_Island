using System;
using System.Collections.Generic;

namespace Messages
{
	public class Message
	{
		public Object sender;
		public List<Object> receivers;   //this should be an interface IMessageReceiver

		public long timestamp { get; private set; }
		public MessageType type { get; private set; }
		public bool nullified { get; private set; }

		protected Message(Object sender)
		{
			this.sender = sender;
			timestamp = DateTime.UtcNow.Ticks;
			SetMessageType(MessageType.UNDEFINED);
			nullified = false;
			receivers = new List<Object>();
		}

		public void AddReceiver(Object receiver)
		{
			receivers.Add(receiver);
		}

		public void SetMessageType(MessageType messageType)
		{
			type = messageType;
		}

		public void SetNullified()
		{
			nullified = true;
		}
	}
}