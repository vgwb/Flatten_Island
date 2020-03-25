using System;
using System.Collections;
using System.Collections.Generic;

public class RemovableQueue<T> : IEnumerable<T>
{
	LinkedList<T> list = new LinkedList<T>();

	public int Count { get { return list.Count; } }

	public void Clear()
	{
		list.Clear();
	}

	public void Enqueue(T t)
	{
		list.AddLast(t);
	}

	public void EnqueueFront(T t)
	{
		list.AddFirst(t);
	}

	public T Dequeue()
	{
		var result = list.First.Value;
		list.RemoveFirst();
		return result;
	}

	public T DequeueBack()
	{
		var result = list.Last.Value;
		list.RemoveLast();
		return result;
	}

	public T Peek()
	{
		return list.First.Value;
	}

	public bool Remove(T t)
	{
		return list.Remove(t);
	}

	public void RemoveAll(Func<T, bool> predicate)
	{
		for (LinkedListNode<T> node = list.First; node != null;)
		{
			LinkedListNode<T> next = node.Next;
			if (predicate(node.Value))
			{
				list.Remove(node);
			}

			node = next;
		}
	}

	public bool Contains(T t)
	{
		return list.Contains(t);
	}

	public IEnumerator<T> GetEnumerator()
	{
		return list.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return list.GetEnumerator();
	}
}