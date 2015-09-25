using System;
using System.Collections;
using System.IO;

namespace TextComparer
{
	public class TextLine : IComparable
	{
		private string _line;
		private readonly int _hash;

		public TextLine(string str)
		{
			_line = str.Replace("\t", "    ");
			_hash = str.GetHashCode();
		}
		#region IComparable Members

		public int CompareTo(object obj)
		{
			return _hash.CompareTo(((TextLine)obj)._hash);
		}

		#endregion
	}

	public class DifferenceListTextFile : IDifferenceList
	{
		private readonly ArrayList _lines;

		public DifferenceListTextFile(string text)
		{
			_lines = new ArrayList();
            using (StringReader sr = new StringReader(text)) 
			{
                const char endOfString = char.MaxValue;
				char singleChar;

				// Read and display lines from the file until the end of 
				// the file is reached.
                while ((singleChar = (char)sr.Read()) != endOfString) 
				{
                   _lines.Add(new TextLine(new string(singleChar, 1)));
				}
			}
		}
		#region IDifferenceList Members

		public int Count()
		{
			return _lines.Count;
		}

		public IComparable GetByIndex(int index)
		{
			return (TextLine)_lines[index];
		}

		#endregion
	}
}