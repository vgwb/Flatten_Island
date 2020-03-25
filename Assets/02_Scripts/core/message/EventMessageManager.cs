using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Messages
{
	public class EventMessageManager : Singleton
	{
		public int processActiveQueueMaxTimeMilliseconds { get; set; }
		private const int DEFAULT_PROCESS_ACTIVE_QUEUE_MAX_TIME_IN_MILLISECONDS = 500;

		private Dictionary<string, List<EventMessageHandler>> eventHandlerMap;

		private const int NUMBER_OF_MESSAGE_QUEUES = 2;
		private RemovableQueue<EventMessage>[] messageQueues = new RemovableQueue<EventMessage>[NUMBER_OF_MESSAGE_QUEUES];
		private int activeQueueIndex;

		public static EventMessageManager instance
		{
			get
			{
				return GetInstance<EventMessageManager>();
			}
		}

		public EventMessageManager()
		{
			eventHandlerMap = new Dictionary<string, List<EventMessageHandler>>();
			activeQueueIndex = 0;
			processActiveQueueMaxTimeMilliseconds = DEFAULT_PROCESS_ACTIVE_QUEUE_MAX_TIME_IN_MILLISECONDS;

			for (int i = 0; i < NUMBER_OF_MESSAGE_QUEUES; i++)
			{
				messageQueues[i] = new RemovableQueue<EventMessage>();
			}
		}

		public void Update()
		{
			ProcessActiveQueue();
		}

		private void ProcessActiveQueue()
		{
			RemovableQueue<EventMessage> queueToProcess = messageQueues[activeQueueIndex];

			/*
			if (queueToProcess.Count > 0)
			{
				UnityEngine.Debug.Log("Time: " + UnityEngine.Time.realtimeSinceStartup + " - ProcessActiveQueue - Processing active index: " + activeQueueIndex + " - count:" + queueToProcess.Count);
			}
			*/

			activeQueueIndex = (activeQueueIndex + 1) % NUMBER_OF_MESSAGE_QUEUES;
			messageQueues[activeQueueIndex].Clear();

			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Start();

			while (queueToProcess.Count > 0)
			{
				EventMessage message = queueToProcess.Dequeue();
				SendMessage(message);

				if (stopWatch.ElapsedMilliseconds >= processActiveQueueMaxTimeMilliseconds)
				{
					stopWatch.Reset();
					break;
				}
			}

			while (queueToProcess.Count > 0)
			{
				// To preserve event all events sequencing, we go back-to-front, 
				// inserting them at the head of the active queue.
				EventMessage eventMessage = queueToProcess.DequeueBack();
				messageQueues[activeQueueIndex].EnqueueFront(eventMessage);
			}
		}

		public void SendMessage(EventMessage eventMessage)
		{
			//UnityEngine.Debug.Log("Time: " + UnityEngine.Time.realtimeSinceStartup + " EventMessageManager.SendMessage called:" + eventMessage.eventObject.name);
			List<EventMessageHandler> messageHandlers;
			if (eventHandlerMap.TryGetValue(eventMessage.eventObject.name, out messageHandlers))
			{
				if (messageHandlers != null)
				{
					foreach (EventMessageHandler messageHandler in messageHandlers)
					{
						switch (eventMessage.type)
						{
							case MessageType.MULTICAST:
								if (eventMessage.receivers.Contains(messageHandler.handler))
								{
									//UnityEngine.Debug.Log("Time: " + UnityEngine.Time.realtimeSinceStartup + " EventMessageManager.SendMessage MULTICAST:" + eventMessage.eventObject.name + " to " + messageHandler.handler.ToString() + " action is:" + messageHandler.action.Method.Name);
									messageHandler.action.Invoke(eventMessage);
								}
								break;
							case MessageType.BROADCAST:
								//UnityEngine.Debug.Log("Time: " + UnityEngine.Time.realtimeSinceStartup + " EventMessageManager.SendMessage BROADCAST:" + eventMessage.eventObject.name + " to " + messageHandler.handler.ToString() + " action is:" + messageHandler.action.Method.Name);
								messageHandler.action.Invoke(eventMessage);
								break;
						}
					}
				}
			}
		}

		public void AddHandler(string eventName, EventMessageHandler messageHandler)
		{
			Monitor.Enter(eventHandlerMap);
			if (eventHandlerMap.ContainsKey(eventName))
			{
				if (!eventHandlerMap[eventName].Contains(messageHandler))
				{
					eventHandlerMap[eventName].Add(messageHandler);
				}
			}
			else
			{
				eventHandlerMap[eventName] = new List<EventMessageHandler>();
				eventHandlerMap[eventName].Add(messageHandler);
			}
			Monitor.Exit(eventHandlerMap);
		}

		public void RemoveHandler(string eventName, Object handler)
		{
			Monitor.Enter(eventHandlerMap);
			List<EventMessageHandler> messageHandlers;
			if (eventHandlerMap.TryGetValue(eventName, out messageHandlers))
			{
				if (messageHandlers != null)
				{
					RemoveHandler(messageHandlers, handler);

					if (messageHandlers.Count == 0)
					{
						eventHandlerMap.Remove(eventName);
					}

					RemoveMessagesWithHandler(handler);
				}
			}
			Monitor.Exit(eventHandlerMap);
		}

		private void RemoveHandler(List<EventMessageHandler> messageHandlers, Object handler)
		{
			EventMessageHandler messageHandlerToRemove = messageHandlers.Find(messageHandler => messageHandler.handler == handler);
			if (messageHandlerToRemove != null)
			{
				messageHandlers.Remove(messageHandlerToRemove);
			}
		}

		private void RemoveMessagesWithHandler(object handler)
		{
			//UnityEngine.Debug.Log("Time: " + UnityEngine.Time.realtimeSinceStartup + " - RemoveMessagesWithHandler called() - handler: " + handler.ToString());
			for (int i = 0; i < messageQueues.Length; i++)
			{
				RemovableQueue<EventMessage> queue = messageQueues[i];
				foreach (EventMessage queuedMessage in queue)
				{
					queuedMessage.receivers.Remove(handler);

					if (queuedMessage.type == MessageType.MULTICAST && queuedMessage.receivers.Count == 0)
					{
						//UnityEngine.Debug.Log("Time: " + UnityEngine.Time.realtimeSinceStartup + " - RemoveMessagesWithHandler Nullifying message: " + queuedMessage.eventObject.name + " queue index:" + i);
						queuedMessage.SetNullified();
					}
				}

				//UnityEngine.Debug.Log("Time: " + UnityEngine.Time.realtimeSinceStartup + " - RemoveMessagesWithHandler BEFORE - queue size:" + queue.Count + " - queue index:" + i);
				queue.RemoveAll(queuedMessage => (queuedMessage.nullified == true));
				//UnityEngine.Debug.Log("Time: " + UnityEngine.Time.realtimeSinceStartup + " - RemoveMessagesWithHandler AFTER - queue size:" + queue.Count + " - queue index:" + i);
			}
		}

		public void QueueMessage(EventMessage eventMessage)
		{
			Debug.Assert(eventMessage.sender != null, "EventMessageManager: eventMessage with event " + eventMessage.eventObject.name + " has NULL sender");
			Debug.Assert(activeQueueIndex >= 0, "EventMessageManager: activeQueueIndex(" + activeQueueIndex + ") < 0");
			Debug.Assert(activeQueueIndex < NUMBER_OF_MESSAGE_QUEUES, "EventMessageManager: activeQueueIndex(" + activeQueueIndex + ") >= NUMBER_OF_EVENT_QUEUES");

			//UnityEngine.Debug.Log("Time: " + UnityEngine.Time.realtimeSinceStartup + " - QueueMessage called() - event: " + eventMessage.eventObject.name + " activeQueueIndex:" + activeQueueIndex + " size:" + messageQueues[activeQueueIndex].Count);

			//We queue the message only if there is any handler
			if (eventHandlerMap.ContainsKey(eventMessage.eventObject.name))
			{
				messageQueues[activeQueueIndex].Enqueue(eventMessage);
				//UnityEngine.Debug.Log("Time: " + UnityEngine.Time.realtimeSinceStartup + " - QueueMessage Enqueing - event: " + eventMessage.eventObject.name + " - activeQueueIndex:" + activeQueueIndex + " size:" + messageQueues[activeQueueIndex].Count);
			}
		}

		public void AbortMessage(string eventName)
		{
			Debug.Assert(activeQueueIndex >= 0, "EventMessageManager: activeQueueIndex(" + activeQueueIndex + ") < 0");
			Debug.Assert(activeQueueIndex < NUMBER_OF_MESSAGE_QUEUES, "EventMessageManager: activeQueueIndex(" + activeQueueIndex + ") >= NUMBER_OF_EVENT_QUEUES");

			if (eventHandlerMap.ContainsKey(eventName))
			{
				RemovableQueue<EventMessage> activeQueue = messageQueues[activeQueueIndex];
				activeQueue.RemoveAll(queuedMessage => queuedMessage.eventObject.name == eventName);
			}
		}
	}
}