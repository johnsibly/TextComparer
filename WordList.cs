using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;

namespace TextComparer
{
    public class Word : IComparable
    {
        private string _word;
        private readonly int _hash;

        public string WordProp
        {
            get { return _word; }
        }

        public Word(string str)
        {
            _word = str;
            _hash = str.GetHashCode();
        }
        #region IComparable Members

        public int CompareTo(object obj)
        {
            return _hash.CompareTo(((Word)obj)._hash);
        }

        #endregion
    }

    public class DifferenceListWordList : IDifferenceList
    {
		private readonly ArrayList _words;

        public DifferenceListWordList(string text)
		{
            _words = new ArrayList();
            
            TextComparer.SimplifyText(ref text);

            var stringArray = text.Split(' ');

            foreach (var s in stringArray)
            {
                _words.Add(new Word(s));
            }
		}
		#region IDifferenceList Members

		public int Count()
		{
			return _words.Count;
		}

		public IComparable GetByIndex(int index)
		{
            return (Word)_words[index];
		}

		#endregion

    }
}
