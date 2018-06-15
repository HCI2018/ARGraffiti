using System.Collections;
using System.Collections.Generic;


namespace ARGraffiti.Utilities
{
/// Stack with capacity
public class CapStack<T>
{
	LinkedList<T> stackList;
	int capacity;

	/// try push an element into the stack with capacity
	/// if exceeds capacity, return true and modify oldest
	/// else, return false
	public bool Push(T x, out T oldest)
	{
		oldest = default(T);
		bool exceeded = false;

		if (stackList.Count >= capacity)
		{
			oldest = stackList.Last.Value;
			exceeded = true;
			stackList.RemoveLast();
		}

		stackList.AddFirst(x);

		return exceeded;
	}

	public T Pop()
	{
		T newest = stackList.First.Value;
		stackList.RemoveFirst();
		return newest;
	}

	public CapStack(int newCapacity)
	{
		capacity = newCapacity;
		stackList = new LinkedList<T>();
	}

	public int Count{
		get{
			return stackList.Count;
		}
	}

	public T[] DumpElements()
	{
		T[] elements = new T[stackList.Count];
		stackList.CopyTo(elements, 0);

		return elements;
	}
}


}	// end of namespace

