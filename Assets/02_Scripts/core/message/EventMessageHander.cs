using System;

namespace Messages
{
	public class EventMessageHandler
	{
		public Object handler { get; private set; }
		public Action<EventMessage> action { get; private set; }

		public EventMessageHandler(Object handler, Action<EventMessage> action)
		{
			this.handler = handler;
			this.action = action;
		}
	}
}