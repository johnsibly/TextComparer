using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Collections;

namespace TextComparer
{
    public class WordSequenceAligner 
    {
	    /** Cost of a substitution string edit operation applied during alignment. 
	     * From edu.cmu.sphinx.util.NISTAlign, which should be referencing the NIST sclite utility settings. */
	    public const int DEFAULT_SUBSTITUTION_PENALTY = 100;
	
	    /** Cost of an insertion string edit operation applied during alignment. 
	     * From edu.cmu.sphinx.util.NISTAlign, which should be referencing the NIST sclite utility settings. */
	    public const int DEFAULT_INSERTION_PENALTY = 75;
	
	    /** Cost of a deletion string edit operation applied during alignment. 
	     * From edu.cmu.sphinx.util.NISTAlign, which should be referencing the NIST sclite utility settings. */
	    public const int DEFAULT_DELETION_PENALTY = 75;

	    /** Substitution penalty for reference-hypothesis string alignment */
	    private int substitutionPenalty;
			 
	    /** Insertion penalty for reference-hypothesis string alignment */
	    private int insertionPenalty;
	
        /** Deletion penalty for reference-hypothesis string alignment */
	    private int deletionPenalty;
	
	
	    /**
	     * Result of an alignment.
	     * Has a {@link #toString()} method that pretty-prints human-readable metrics.
	     *  
	     * @author romanows
	     */
	    public class Alignment {
		    /** Reference words, with null elements representing insertions in the hypothesis sentence and upper-cased words representing an alignment mismatch */
		    public String [] reference;
		
		    /** Hypothesis words, with null elements representing deletions (missing words) in the hypothesis sentence and upper-cased words representing an alignment mismatch */
		    public String [] hypothesis;
		
		    /** Number of word substitutions made in the hypothesis with respect to the reference */
		    public int numSubstitutions;
		
		    /** Number of word insertions (unnecessary words present) in the hypothesis with respect to the reference */
		    public int numInsertions;
		
		    /** Number of word deletions (necessary words missing) in the hypothesis with respect to the reference */
		    public int numDeletions;
	
		
		    /**
		     * Constructor.
		     * @param reference reference words, with null elements representing insertions in the hypothesis sentence
		     * @param hypothesis hypothesis words, with null elements representing deletions (missing words) in the hypothesis sentence
		     * @param numSubstitutions Number of word substitutions made in the hypothesis with respect to the reference 
		     * @param numInsertions Number of word insertions (unnecessary words present) in the hypothesis with respect to the reference
		     * @param numDeletions Number of word deletions (necessary words missing) in the hypothesis with respect to the reference
		     */
		    public Alignment(String [] reference, String [] hypothesis, int numSubstitutions, int numInsertions, int numDeletions) 
            {
			    if(reference == null || hypothesis == null || reference.Length != hypothesis.Length || numSubstitutions < 0 || numInsertions < 0 || numDeletions < 0) 
                {
				    throw new ArgumentException();
			    }
			    this.reference = reference;
			    this.hypothesis = hypothesis;
			    this.numSubstitutions = numSubstitutions;
			    this.numInsertions = numInsertions;
			    this.numDeletions = numDeletions;
		    }
		
		    /**
		     * Number of word correct words in the aligned hypothesis with respect to the reference.
		     * @return number of word correct words 
		     */
		    public int getNumCorrect() 
            {
			    return getHypothesisLength() - (numSubstitutions + numInsertions);  // Substitutions are mismatched and not correct, insertions are extra words that aren't correct
		    }
		
		    /** @return true when the hypothesis exactly matches the reference */
		    public bool isSentenceCorrect() 
            {
			    return numSubstitutions == 0 && numInsertions == 0 && numDeletions == 0;
		    }
		
		    /**
		     * Get the length of the original reference sequence.
		     * This is not the same as {@link #reference}.Length(), because that member variable may have null elements 
		     * inserted to mark hypothesis insertions.
		     * 
		     * @return the length of the original reference sequence
		     */
		    public int getReferenceLength() 
            {
			    return reference.Length - numInsertions;
		    }
		
		    /**
		     * Get the length of the original hypothesis sequence.
		     * This is not the same as {@link #hypothesis}.length(), because that member variable may have null elements 
		     * inserted to mark hypothesis deletions.
		     * 
		     * @return the length of the original hypothesis sequence
		     */
		    public int getHypothesisLength() 
            {
			    return hypothesis.Length - numDeletions;
		    }
		
		    /*
		     * (non-Javadoc)
		     * @see java.lang.Object#toString()
		     */
            override public String ToString() 
            {
			    StringBuilder refText = new StringBuilder();
			    StringBuilder hypText = new StringBuilder();
			    refText.Append("REF:\t");
			    hypText.Append("HYP:\t");
			    for(int i=0; i < reference.Length; i++) {
				    if(reference[i] == null) {
					    for(int j=0; j < hypothesis[i].Length; j++) {
						    refText.Append("*");
					    }
				    } else {
					    refText.Append(reference[i]);
				    }
				
				    if(hypothesis[i] == null) {
					    for(int j=0; j < reference[i].Length; j++) {
						    hypText.Append("*");
					    }
				    } else {
					    hypText.Append(hypothesis[i]);
				    }

				    if(i != reference.Length - 1) {
					    refText.Append("\t");
					    hypText.Append("\t");
				    }
			    }
			
			    StringBuilder sb = new StringBuilder();
			    sb.Append("\t");
			    sb.Append("# seq").Append("\t");
			    sb.Append("# ref").Append("\t");
			    sb.Append("# hyp").Append("\t");
			    sb.Append("# cor").Append("\t");
			    sb.Append("# sub").Append("\t");
			    sb.Append("# ins").Append("\t");
			    sb.Append("# del").Append("\t");
			    sb.Append("acc").Append("\t");
			    sb.Append("WER").Append("\t");
			    sb.Append("# seq cor").Append("\t");

			    sb.Append("\n");
			    sb.Append("STATS:\t");
			    sb.Append(1).Append("\t");
			    sb.Append(getReferenceLength()).Append("\t");
			    sb.Append(getHypothesisLength()).Append("\t");
			    sb.Append(getNumCorrect()).Append("\t");
			    sb.Append(numSubstitutions).Append("\t");
			    sb.Append(numInsertions).Append("\t");
			    sb.Append(numDeletions).Append("\t");
			    sb.Append(getNumCorrect() / (float) getReferenceLength()).Append("\t");
			    sb.Append((numSubstitutions + numInsertions + numDeletions) / (float) getReferenceLength()).Append("\t");
			    sb.Append(isSentenceCorrect() ? 1 : 0);

			    sb.Append("\n");
			    sb.Append("-----\t");
			    sb.Append("-----\t");
			    sb.Append("-----\t");
			    sb.Append("-----\t");
			    sb.Append("-----\t");
			    sb.Append("-----\t");
			    sb.Append("-----\t");
			    sb.Append("-----\t");
			    sb.Append("-----\t");
			    sb.Append("-----\t");
			    sb.Append("-----\t");

			    sb.Append("\n");
			    sb.Append(refText).Append("\n").Append(hypText);
			
			    return sb.ToString();
		    }
	    }
	
	
	    /**
	     * Collects several alignment results.
	     * Has a {@link #toString()} method that pretty-prints a human-readable summary metrics for the collection of results.
	     *  
	     * @author romanows
	     */
	    public class SummaryStatistics 
        {
		    /** Number of correct words in the aligned hypothesis with respect to the reference */
		    private int numCorrect;

		    /** Number of word substitutions made in the hypothesis with respect to the reference */
		    private int numSubstitutions;
		
		    /** Number of word insertions (unnecessary words present) in the hypothesis with respect to the reference */
		    private int numInsertions;
		
		    /** Number of word deletions (necessary words missing) in the hypothesis with respect to the reference */
		    private int numDeletions;
		
		    /** Number of hypotheses that exactly match the associated reference */
		    private int numSentenceCorrect;

		    /** Total number of words in the reference sequences */
		    private int numReferenceWords;
		
		    /** Total number of words in the hypothesis sequences */
		    private int numHypothesisWords;
		
		    /** Number of sentences */
		    private int numSentences;
		
		
		    /**
		     * Constructor.
		     * @param alignments collection of alignments
		     */
		    public SummaryStatistics(Collection<Alignment> alignments) 
            {
			    foreach(Alignment a in alignments) 
                {
				    add(a);
			    }
		    }
		
		    /**
		     * Add a new alignment result
		     * @param alignment result to add
		     */
		    public void add(Alignment alignment) 
            {
			    numCorrect += alignment.getNumCorrect();
			    numSubstitutions += alignment.numSubstitutions;
			    numInsertions += alignment.numInsertions;
			    numDeletions += alignment.numDeletions;
			    numSentenceCorrect += alignment.isSentenceCorrect() ? 1 : 0;
			    numReferenceWords += alignment.getReferenceLength();
			    numHypothesisWords += alignment.getHypothesisLength();
			    numSentences++;
		    }
		
		    public int getNumSentences() 
            {
			    return numSentences;
		    }

		    public int getNumReferenceWords() 
            {
			    return numReferenceWords;
		    }
		
		    public int getNumHypothesisWords() 
            {
			    return numHypothesisWords;
		    }
		
		    public float getCorrectRate() 
            {
			    return numCorrect / (float) numReferenceWords;
		    }
		
		    public float getSubstitutionRate() 
            {
			    return numSubstitutions / (float) numReferenceWords;
		    }

		    public float getDeletionRate() 
            {
			    return numDeletions / (float) numReferenceWords;
		    }

		    public float getInsertionRate() 
            {
			    return numInsertions / (float) numReferenceWords;
		    }
		
		    /** @return the word error rate of this collection */
		    public float getWordErrorRate() 
            {
			    return (numSubstitutions + numDeletions + numInsertions) / (float) numReferenceWords;
		    }
		
		    /** @return the sentence error rate of this collection */
		    public float getSentenceErrorRate() 
            {
			    return (numSentences - numSentenceCorrect) / (float) numSentences;
		    }
		
		    /*
		     * (non-Javadoc)
		     * @see java.lang.Object#toString()
		     */
            override public String ToString() 
            {
			    StringBuilder sb = new StringBuilder();
			    sb.Append("# seq").Append("\t");
			    sb.Append("# ref").Append("\t");
			    sb.Append("# hyp").Append("\t");
			    sb.Append("cor").Append("\t");
			    sb.Append("sub").Append("\t");
			    sb.Append("ins").Append("\t");
			    sb.Append("del").Append("\t");
			    sb.Append("WER").Append("\t");
			    sb.Append("SER").Append("\t");
			    sb.Append("\n");

			    sb.Append(numSentences).Append("\t");
			    sb.Append(numReferenceWords).Append("\t");
			    sb.Append(numHypothesisWords).Append("\t");
			    sb.Append(getCorrectRate()).Append("\t");
			    sb.Append(getSubstitutionRate()).Append("\t");
			    sb.Append(getInsertionRate()).Append("\t");
			    sb.Append(getDeletionRate()).Append("\t");
			    sb.Append(getWordErrorRate()).Append("\t");
			    sb.Append(getSentenceErrorRate());
			    return sb.ToString();
		    }
	    }
	
	
	    /**
	     * Constructor.
	     * Creates an object with default alignment penalties.  
	     */
	    public WordSequenceAligner() : this(DEFAULT_SUBSTITUTION_PENALTY, DEFAULT_INSERTION_PENALTY, DEFAULT_DELETION_PENALTY)
        {
		
	    }
	
	
	    /**
	     * Constructor. 
	     * @param substitutionPenalty substitution penalty for reference-hypothesis string alignment
	     * @param insertionPenalty insertion penalty for reference-hypothesis string alignment
	     * @param deletionPenalty deletion penalty for reference-hypothesis string alignment
	     */
	    public WordSequenceAligner(int substitutionPenalty, int insertionPenalty, int deletionPenalty) 
        {
		    this.substitutionPenalty = substitutionPenalty;
		    this.insertionPenalty = insertionPenalty;
		    this.deletionPenalty = deletionPenalty;
	    }
	
	
	    /**
	     * Produce alignment results for several pairs of sentences.
	     * @see #align(String[], String[])
	     * @param references reference sentences to align with the given hypotheses 
	     * @param hypotheses hypothesis sentences to align with the given references
	     * @return collection of per-sentence alignment results
	     */
	    public List<Alignment> align(List<String []> references, List<String []> hypotheses) 
        {
            if (references.Count() != hypotheses.Count())
            {
			    throw new ArgumentException();
		    }
            if (references.Count() == 0)
            {
			    return new List<WordSequenceAligner.Alignment>();
		    }
		
		    List<Alignment> alignments = new List<WordSequenceAligner.Alignment>();
            IEnumerator<String[]> refIt = references.GetEnumerator();
            IEnumerator<String[]> hypIt = hypotheses.GetEnumerator();

            while (refIt.MoveNext() /*refIt.hasNext()*/ && hypIt.MoveNext()) 
            {
			    alignments.Add(align(refIt.Current, hypIt.Current));
		    }
		    return alignments;
	    }

	
	    /**
	     * Produces {@link Alignment} results from the alignment of the hypothesis words to the reference words.
	     * Alignment is done via weighted string edit distance according to {@link #substitutionPenalty}, {@link #insertionPenalty}, {@link #deletionPenalty}.
	     * 
	     * @param reference sequence of words representing the true sentence; will be evaluated as lowercase.
	     * @param hypothesis sequence of words representing the hypothesized sentence; will be evaluated as lowercase.
	     * @return results of aligning the hypothesis to the reference 
	     */
	    public Alignment align(String [] reference, String [] hypothesis) 
        {
		    // Values representing string edit operations in the backtrace matrix
		    const int OK = 0;  
		    const int SUB = 1;
		    const int INS = 2;
		    const int DEL = 3;

		    /* 
		     * Next up is our dynamic programming tables that track the string edit distance calculation.
		     * The row address corresponds to an index within the sequence of reference words.
		     * The column address corresponds to an index within the sequence of hypothesis words.
		     * cost[0][0] addresses the beginning of two word sequences, and thus always has a cost of zero.  
		     */
		
		    /** cost[3][2] is the minimum alignment cost when aligning the first two words of the reference to the first word of the hypothesis */
		    int[,] cost = new int[reference.Length + 1, hypothesis.Length + 1];
		
		    /** 
		     * backtrace[3][2] gives information about the string edit operation that produced the minimum cost alignment between the first two words of the reference to the first word of the hypothesis.
		     * If a deletion operation is the minimum cost operation, then we say that the best way to get to hyp[1] is by deleting ref[2].
		     */
		    int[,] backtrace = new int[reference.Length + 1, hypothesis.Length + 1];
		
		    // Initialization
		    InitialiseCosts(cost, backtrace, OK, DEL, INS);

	        // For each next column, go down the rows, recording the min cost edit operation (and the cumulative cost). 
		    RecordMinCostEditOperation(reference, hypothesis, cost, OK, SUB, backtrace, INS, DEL);
		
		    // Now that we have the minimal costs, find the lowest cost edit to create the hypothesis sequence
		    LinkedList<String> alignedReference = new LinkedList<String>();
		    LinkedList<String> alignedHypothesis = new LinkedList<String>();
		    int numSub = 0;
		    int numDel = 0;
		    int numIns = 0;
		    int i = cost.GetLength(0) - 1;
            int j = cost.GetLength(1) - 1; // int j = cost[0].Length - 1; Not sure correct
		    while(i > 0 || j > 0) 
            {
			    switch(backtrace[i,j]) 
                {
			    case OK: 
                    alignedReference.AddFirst(reference[i-1].ToLower()); 
                    alignedHypothesis.AddFirst(hypothesis[j-1].ToLower()); 
                    i--; 
                    j--; 
                    break;
			    case SUB: 
                    alignedReference.AddFirst(reference[i-1].ToUpper()); 
                    alignedHypothesis.AddFirst(hypothesis[j-1].ToUpper()); 
                    i--; 
                    j--; 
                    numSub++; 
                    break;
			    case INS: 
                    alignedReference.AddFirst(String.Empty); // alignedReference.AddFirst(null); 
                    alignedHypothesis.AddFirst(hypothesis[j-1].ToUpper()); 
                    j--; numIns++; 
                    break;
			    case DEL: 
                    alignedReference.AddFirst(reference[i-1].ToUpper()); 
                    alignedHypothesis.AddFirst(String.Empty); i--; numDel++; // alignedHypothesis.AddFirst(null); i--; numDel++; 
                    break;
			    }
		    }
		
		    return new Alignment(alignedReference.ToArray(), alignedHypothesis.ToArray(), numSub, numIns, numDel);
	    }

        private void InitialiseCosts(int[,] cost, int[,] backtrace, int OK, int DEL, int INS)
        {
            cost[0, 0] = 0;
            backtrace[0, 0] = OK;

            // First column represents the case where we achieve zero hypothesis words by deleting all reference words.
            for (int i = 1; i < cost.GetLength(0); i++)
            {
                cost[i, 0] = deletionPenalty*i;
                backtrace[i, 0] = DEL;
            }

            // First row represents the case where we achieve the hypothesis by inserting all hypothesis words into a zero-length reference.
            for (int j = 1; j < cost.GetLength(1); j++)
            {
                cost[0, j] = insertionPenalty*j;
                backtrace[0, j] = INS;
            }
        }

        private void RecordMinCostEditOperation(string[] reference, string[] hypothesis, int[,] cost, int OK, int SUB,
                                                int[,] backtrace, int INS, int DEL)
        {
            for (int i = 1; i < cost.GetLength(0); i++)
            {
                for (int j = 1; j < cost.GetLength(1); j++)
                {
                    int subOp, cs; // it is a substitution if the words aren't equal, but if they are, no penalty is assigned.
                    if (reference[i - 1].ToLower().Equals(hypothesis[j - 1].ToLower()))
                    {
                        subOp = OK;
                        cs = cost[i - 1, j - 1];
                    }
                    else
                    {
                        subOp = SUB;
                        cs = cost[i - 1, j - 1] + substitutionPenalty;
                    }
                    int ci = cost[i, j - 1] + insertionPenalty;
                    int cd = cost[i - 1, j] + deletionPenalty;

                    int mincost = Math.Min(cs, Math.Min(ci, cd));
                    if (cs == mincost)
                    {
                        cost[i, j] = cs;
                        backtrace[i, j] = subOp;
                    }
                    else if (ci == mincost)
                    {
                        cost[i, j] = ci;
                        backtrace[i, j] = INS;
                    }
                    else
                    {
                        cost[i, j] = cd;
                        backtrace[i, j] = DEL;
                    }
                }
            }
        }
    }
}
