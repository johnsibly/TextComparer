using System;
using System.Collections;

namespace TextComparer
{
	public enum DifferenceEngineLevel
	{
		FastImperfect,
		Medium,
		SlowPerfect
	}

	public class DifferenceEngine
	{
		private IDifferenceList _source;
		private IDifferenceList _destination;
		private ArrayList _matchList;

		private DifferenceEngineLevel _level;

		private DifferenceStateList _stateList;

		public DifferenceEngine() 
		{
			_source = null;
			_destination = null;
			_matchList = null;
			_stateList = null;
			_level = DifferenceEngineLevel.FastImperfect;
		}

		private int GetSourceMatchLength(int destIndex, int sourceIndex, int maxLength)
		{
			int matchCount;
			for (matchCount = 0; matchCount < maxLength; matchCount++)
			{
				if (_destination.GetByIndex(destIndex + matchCount).CompareTo(_source.GetByIndex(sourceIndex + matchCount)) != 0)
				{
					break;
				}
			}

			return matchCount;
		}

		private void GetLongestSourceMatch(DifferenceState curItem, int destIndex, int destEnd, int sourceStart, int sourceEnd)
		{
			int maxDestLength = (destEnd - destIndex) + 1;
			int currentBestLength = 0;
			int currentBestIndex = -1;
			for (int sourceIndex = sourceStart; sourceIndex <= sourceEnd; sourceIndex++)
			{
				int maxLength = Math.Min(maxDestLength, (sourceEnd - sourceIndex) + 1);
				if (maxLength <= currentBestLength)
				{
					// No chance to find a longer one any more
					break;
				}

				int currentLength = GetSourceMatchLength(destIndex, sourceIndex, maxLength);
				if (currentLength > currentBestLength)
				{
					// This is the best match so far
					currentBestIndex = sourceIndex;
					currentBestLength = currentLength;
				}

				// Jump over the match
				sourceIndex += currentBestLength; 
			}

			// DifferenceState current = _stateList.GetByIndex(destIndex);
			if (currentBestIndex == -1)
			{
				curItem.SetNoMatch();
			}
			else
			{
				curItem.SetMatch(currentBestIndex, currentBestLength);
			}
		}

		private void ProcessRange(int destStart, int destEnd, int sourceStart, int sourceEnd)
		{
			int curBestIndex = -1;
			int curBestLength = -1;
			DifferenceState curItem;
			DifferenceState bestItem = null;

			for (int destIndex = destStart; destIndex <= destEnd; destIndex++)
			{
				int maxPossibleDestLength = (destEnd - destIndex) + 1;
				if (maxPossibleDestLength <= curBestLength)
				{
					// we won't find a longer one even if we looked
					break;
				}

				curItem = _stateList.GetByIndex(destIndex);
				
				if (!curItem.HasValidLength(sourceStart, sourceEnd, maxPossibleDestLength))
				{
					// recalc new best length since it isn't valid or has never been done.
					GetLongestSourceMatch(curItem, destIndex, destEnd, sourceStart, sourceEnd);
				}

				if (curItem.Status == DifferenceStatus.Matched)
				{
					switch (_level)
					{
						case DifferenceEngineLevel.FastImperfect:
							if (curItem.Length > curBestLength)
							{
								// this is longest match so far
								curBestIndex = destIndex;
								curBestLength = curItem.Length;
								bestItem = curItem;
							}

							// Jump over the match 
							destIndex += curItem.Length - 1; 
							break;
						case DifferenceEngineLevel.Medium: 
							if (curItem.Length > curBestLength)
							{
								// this is longest match so far
								curBestIndex = destIndex;
								curBestLength = curItem.Length;
								bestItem = curItem;

								// Jump over the match 
								destIndex += curItem.Length - 1; 
							}

							break;
						default:
							if (curItem.Length > curBestLength)
							{
								// this is longest match so far
								curBestIndex = destIndex;
								curBestLength = curItem.Length;
								bestItem = curItem;
							}

							break;
					}
				}
			}

			if (curBestIndex < 0)
			{
				// we are done - there are no matches in this span
			}
			else
			{
				if (bestItem != null)
				{
					int sourceIndex = bestItem.StartIndex;
					_matchList.Add(DifferenceResultSpan.CreateNoChange(curBestIndex, sourceIndex, curBestLength));
					if (destStart < curBestIndex)
					{
						// Still have more lower destination data
						if (sourceStart < sourceIndex)
						{
							// Still have more lower source data
							// Recursive call to process lower indexes
							ProcessRange(destStart, curBestIndex - 1, sourceStart, sourceIndex - 1);
						}
					}

					int upperDestStart = curBestIndex + curBestLength;
					int upperSourceStart = sourceIndex + curBestLength;
					if (destEnd > upperDestStart)
					{
						// we still have more upper dest data
						if (sourceEnd > upperSourceStart)
						{
	 						// set still have more upper source data
							// Recursive call to process upper indexes
							ProcessRange(upperDestStart, destEnd, upperSourceStart, sourceEnd);
						}
					}
				}
			}
		}

		public double ProcessDifferences(IDifferenceList source, IDifferenceList destination, DifferenceEngineLevel level)
		{
			_level = level;
			return ProcessDifferences(source, destination);
		}

		public double ProcessDifferences(IDifferenceList source, IDifferenceList destination)
		{
			DateTime dt = DateTime.Now;
			_source = source;
			_destination = destination;
			_matchList = new ArrayList();
			
			int dcount = _destination.Count();
			int scount = _source.Count();

			if ((dcount > 0) && (scount > 0))
			{
				_stateList = new DifferenceStateList(dcount);
				ProcessRange(0, dcount - 1, 0, scount - 1);
			}

			TimeSpan ts = DateTime.Now - dt;
			return ts.TotalSeconds;
		}

		private bool AddChanges(ArrayList report, int curDest, int nextDest, int curSource, int nextSource)
		{
			bool retval = false;
			int diffDest = nextDest - curDest;
			int diffSource = nextSource - curSource;
			if (diffDest > 0)
			{
				if (diffSource > 0)
				{
					int minDiff = Math.Min(diffDest, diffSource);
					report.Add(DifferenceResultSpan.CreateReplace(curDest, curSource, minDiff));
					if (diffDest > diffSource)
					{
						curDest += minDiff;
						report.Add(DifferenceResultSpan.CreateAddDestination(curDest, diffDest - diffSource));
					}
					else
					{
						if (diffSource > diffDest)
						{
							curSource += minDiff;
							report.Add(DifferenceResultSpan.CreateDeleteSource(curSource, diffSource - diffDest));
						}
					}	
				}
				else
				{
					report.Add(DifferenceResultSpan.CreateAddDestination(curDest, diffDest)); 
				}

				retval = true;
			}
			else
			{
				if (diffSource > 0)
				{
					report.Add(DifferenceResultSpan.CreateDeleteSource(curSource, diffSource));
					  
					retval = true;
				}
			}

			return retval;
		}

		public ArrayList DifferenceReport()
		{
			ArrayList retval = new ArrayList();
			int dcount = _destination.Count();
			int scount = _source.Count();
			
			// Deal with the special case of empty files
			if (dcount == 0)
			{
				if (scount > 0)
				{
					retval.Add(DifferenceResultSpan.CreateDeleteSource(0, scount));
				}

				return retval;
			}
			else
			{
				if (scount == 0)
				{
					retval.Add(DifferenceResultSpan.CreateAddDestination(0, dcount));
					return retval;
				}
			}

			_matchList.Sort();
			int curDest = 0;
			int curSource = 0;
			DifferenceResultSpan last = null;

			// Process each match record
			foreach (DifferenceResultSpan drs in _matchList)
			{
				if ((!AddChanges(retval, curDest, drs.DestIndex, curSource, drs.SourceIndex)) &&
				    (last != null))
				{
					last.AddLength(drs.Length);
				}
				else
				{
					retval.Add(drs);
				}

				curDest = drs.DestIndex + drs.Length;
				curSource = drs.SourceIndex + drs.Length;
				last = drs;
			}
			
			// Process any tail end data
			AddChanges(retval, curDest, dcount, curSource, scount);

			return retval;
		}
	}
}
