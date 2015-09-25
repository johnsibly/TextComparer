using System;
using System.Collections.Generic;
using System.Text;

namespace TextComparer
{
	public interface IDifferenceList
	{
		int Count();
		IComparable GetByIndex(int index);
	}
}
