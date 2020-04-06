using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A continuous shuffled sequence generator. Basically it's List refilling/reshuffling once it's empty.
/// Single elements can only reoccur once a pass is finished. So, if an element was only added
/// once it can be at most chosen twice: at the end of one pass and at the beginning of the next one.
/// Generic type T *SHOULD* be a Value Type or a read only value since it's performed only a shallow copy when refilling the list.
/// </summary>
public class ShuffleBag<T>
{
	private System.Random randomGenerator = null;
	private static readonly int DEFAULT_CAPACITY = 10;

	private List<T> bag;
	private List<T> bagCopy;

	public ShuffleBag(int capacity, System.Random foreignRandomGenerator = null)
	{
		if (foreignRandomGenerator != null)
		{
			randomGenerator = foreignRandomGenerator;
		}
		else
		{
			randomGenerator = new System.Random();
		}

		if (capacity <= 0)
		{
			bag = new List<T>(DEFAULT_CAPACITY);
			bagCopy = new List<T>(DEFAULT_CAPACITY);
		}
		else
		{
			bag = new List<T>(capacity);
			bagCopy = new List<T>(capacity);
		}
	}

	public bool IsBagEmpty()
	{
		if (bag != null)
		{
			return (bag.Count <= 0);
		}

		return true;
	}

	public int DistinctTypesCount()
	{
		IEnumerable<T> distinctTypes = bag.Distinct();
		return distinctTypes.Count();
	}

	public int Count()
	{
		if (bag != null)
		{
			return bag.Count;
		}

		return 0;
	}

	public void Add(T item, int amount)
	{
		if (amount > 0)
		{
			for (int i = 0; i < amount; ++i)
			{
				bag.Add(item);
				bagCopy.Add(item);
			}
		}
	}

	public void Refill(T item, int amount)
	{
		if (amount > 0)
		{
			var groups = bag.GroupBy(i => i);
			int currentAmount = 0;

			foreach (var type in groups)
			{
				if (item.Equals(type.Key))
				{
					currentAmount = type.Count();
				}
			}

			for (int i = currentAmount; i < amount; ++i)
			{
				bag.Add(item);
				bagCopy.Add(item);
			}
		}
	}

	public T Next()
	{
		if (bag.Count > 0)
		{
			T item = bag[0];
			bag.RemoveAt(0);
			return item;
		}
		else
		{
			bag = new List<T>(bagCopy.Count);

			bagCopy.ForEach((element) =>
			{
				bag.Add(element);
			});

			Shuffle(bag);

			if (bag.Count > 0)
			{
				T item = bag[0];
				bag.RemoveAt(0);
				return item;
			}
		}

		return default(T);
	}

	public void Shuffle()
	{
		Shuffle(bag);
	}

	private void Shuffle(List<T> list)
	{
		int count = list.Count;
		while (count > 1)
		{
			count--;
			int randomIndex = randomGenerator.Next(count + 1);
			T value = list[randomIndex];
			list[randomIndex] = list[count];
			list[count] = value;
		}
	}

	public void Clear()
	{
		bag.Clear();
		bagCopy.Clear();
	}
}