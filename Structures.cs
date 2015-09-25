using System;

namespace TextComparer
{
	internal enum DifferenceStatus 
	{
		Matched = 1,
		NoMatch = -1,
		Unknown = -2
	}

	internal class DifferenceState
	{
		private const int BadIndex = -1;
		private int _startIndex;
		private int _length;

		public int StartIndex
		{
			get { return _startIndex; }
		}

		public int EndIndex
		{
			get { return (_startIndex + _length) - 1; }
		}

		public int Length 
		{
			get
			{
				int len;
				if (_length > 0)
				{
					len = _length;
				}
				else
				{
					len = _length == 0 ? 1 : 0;
				}

				return len;
			}
		}

		public DifferenceStatus Status 
		{
			get
			{
				DifferenceStatus stat;
				if (_length > 0)
				{
					stat = DifferenceStatus.Matched; 
				}
				else
				{
					switch (_length)
					{
						case -1:
							stat = DifferenceStatus.NoMatch;
							break;
						default:
							stat = DifferenceStatus.Unknown;
							break;
					}
				}

				return stat;
			}
		}

		public DifferenceState()
		{
			SetToUnknown();
		}

		protected void SetToUnknown()
		{
			_startIndex = BadIndex;
			_length = (int)DifferenceStatus.Unknown;
		}

		public void SetMatch(int start, int length)
		{
			_startIndex = start;
			_length = length;
		}

		public void SetNoMatch()
		{
			_startIndex = BadIndex;
			_length = (int)DifferenceStatus.NoMatch;
		}

		public bool HasValidLength(int newStart, int newEnd, int maxPossibleDestLength)
		{
			if (_length > 0)
			{
				// have unlocked match
				if ((maxPossibleDestLength < _length) ||
				    ((_startIndex < newStart) || (EndIndex > newEnd)))
				{
					SetToUnknown();
				}
			}

			return _length != (int)DifferenceStatus.Unknown;
		}
	}

	internal class DifferenceStateList
	{
		private readonly DifferenceState[] _array;

		public DifferenceStateList(int destCount)
		{
			_array = new DifferenceState[destCount];
		}

		public DifferenceState GetByIndex(int index)
		{
			DifferenceState retval = _array[index];
			if (retval == null)
			{
				retval = new DifferenceState();
				_array[index] = retval;
			}

			return retval;
		}
	}

	public enum DifferenceResultStatus
	{
		NoChange,
		Replace,
		DeleteSource,
		AddDestination
	}

	public class DifferenceResultSpan : IComparable
	{
		private const int BadIndex = -1;
		private readonly int _destIndex;
		private readonly int _sourceIndex;
		private int _length;
		private readonly DifferenceResultStatus _status;

		public int DestIndex
		{
			get { return _destIndex; }
		}

		public int SourceIndex
		{
			get { return _sourceIndex; }
		}

		public int Length
		{
			get { return _length; }
		}

		public DifferenceResultStatus Status
		{
			get { return _status; }
		}

		protected DifferenceResultSpan(
			DifferenceResultStatus status,
			int destIndex,
			int sourceIndex,
			int length)
		{
			_status = status;
			_destIndex = destIndex;
			_sourceIndex = sourceIndex;
			_length = length;
		}

		public static DifferenceResultSpan CreateNoChange(int destIndex, int sourceIndex, int length)
		{
			return new DifferenceResultSpan(DifferenceResultStatus.NoChange, destIndex, sourceIndex, length);
		}

		public static DifferenceResultSpan CreateReplace(int destIndex, int sourceIndex, int length)
		{
			return new DifferenceResultSpan(DifferenceResultStatus.Replace, destIndex, sourceIndex, length);
		}

		public static DifferenceResultSpan CreateDeleteSource(int sourceIndex, int length)
		{
			return new DifferenceResultSpan(DifferenceResultStatus.DeleteSource, BadIndex, sourceIndex, length);
		}

		public static DifferenceResultSpan CreateAddDestination(int destIndex, int length)
		{
			return new DifferenceResultSpan(DifferenceResultStatus.AddDestination, destIndex, BadIndex, length);
		}

		public void AddLength(int i)
		{
			_length += i;
		}

		public override string ToString()
		{
			return string.Format("{0} (Dest: {1},Source: {2}) {3}", _status, _destIndex, _sourceIndex, _length);
		}
		#region IComparable Members

		public int CompareTo(object obj)
		{
			return _destIndex.CompareTo(((DifferenceResultSpan)obj)._destIndex);
		}

		#endregion
	}
}