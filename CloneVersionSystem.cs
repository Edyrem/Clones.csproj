using System;
using System.Collections.Generic;

namespace Clones
{
	public class NewStackItem<T>
    {
		public T Value { get; set; }
		public NewStackItem<T> Previous { get; set; }
    }

	public class NewStack<T>
    {
		NewStackItem<T> Head { get; set; }
		NewStackItem<T> Tail { get; set; }

		public int Count { get; private set; }

		public void Push(T value)
		{
			if (Head == null)
				Head = Tail = new NewStackItem<T>() { Value = value, Previous = null };
			else
            {
				var item = new NewStackItem<T>() { Value = value, Previous = Tail };
				Tail = item;
            }
			Count++;
		}

		public T Pop()
        {
			if (Head == null) throw new InvalidOperationException();
			var result = Tail;
			Tail = Tail.Previous;
			if (Tail == null) Head = null;
			Count = Count > 0 ? Count-- : 0;
			return result.Value;
		}

		public T Peek()
        {
			if (Head == null) throw new InvalidOperationException();
			return Tail.Value;
		}

		public void Clear()
        {
			Head = Tail = new NewStackItem<T>() { Value = default(T), Previous = null };
			Count = 0;
		}

		public NewStack<T> Clone()
        {
			var newStack = this;
			var stack = new NewStack<T>();
			while(newStack.Head != null)
				stack.Push(newStack.Pop());
			var head = stack.Head;
			var tail = stack.Tail;
			newStack.Head = tail;
			newStack.Tail = head;
			return newStack;
        }
	}

	public class Clone
    {
		public NewStack<int> LearningProgramm = new NewStack<int>();
		public NewStack<int> RollbackedProgramm = new NewStack<int>();

		public void Learn(int programm)
		{
			LearningProgramm.Push(programm);
			RollbackedProgramm.Clear();
		}

		public void Rollback()
		{
			RollbackedProgramm.Push(LearningProgramm.Pop());
		}

		public void Relearn()
		{
			LearningProgramm.Push(RollbackedProgramm.Pop());
		}

		public string Check()
		{
			return LearningProgramm.Count > 0 ? LearningProgramm.Peek().ToString() : "basic";
		}			
	}

	public class CloneVersionSystem : ICloneVersionSystem
	{
		public List<Clone> clone = new List<Clone>();		

		public CloneVersionSystem()
        {
			clone.Add(new Clone());
        }

		public string Execute(string query)
		{
			var splittedQuery = query.Split(' ');
			var command = splittedQuery[0];
			var cloneId = int.Parse(splittedQuery[1]) - 1;

			switch(command)
            {
				case "learn": 
					{
						var programm = int.Parse(splittedQuery[2]);
						clone[cloneId].Learn(programm);
						break;
					}
				case "rollback":
					{
						clone[cloneId].Rollback();
						break;
					}
				case "relearn":
					{
						clone[cloneId].Relearn();
						break;
					}
				case "check":
					{
						return clone[cloneId].Check();
					}
				case "clone":
					{
						CloneClone(cloneId);
						break;
					}
				default: throw new Exception("Unknown command");
			}
			return null;
		}		

		public void CloneClone(int cloneId)
		{
			var newClone = new Clone()
			{
				LearningProgramm = clone[cloneId].LearningProgramm.Clone(),
				RollbackedProgramm = clone[cloneId].RollbackedProgramm.Clone()
			};
			//var newClone = new Clone()
			//{
			//	LearningProgramm = ReturnStack(clone[cloneId].LearningProgramm),
			//	RollbackedProgramm = ReturnStack(clone[cloneId].RollbackedProgramm)
			//};
			clone.Add(newClone);
		}

		public Stack<int> ReturnStack(Stack<int> stack)
        {
			var newStack = new Stack<int>();
			var stackArray = stack.ToArray();
            for (int i = stackArray.Length - 1; i >= 0; i--)
            {
                newStack.Push(stackArray[i]);
			}
			return newStack;
        }
	}
}
