using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TextComparer
{
    public partial class TextComparer : Form
    {
        public TextComparer()
        {
            InitializeComponent();
        }

        private int GetNumberOfWordsInString(string stringToCount)
        {
            int count = 0;

            // SimplifyText(stringToCount);
            
            if (stringToCount.Length == 0) return 0;
            
            var diffWords = stringToCount.Split(' ');

            if (diffWords != null)
            {
                count = diffWords.Length;
            }

            return count;
        }

        public static void SimplifyText(ref string inputText)
        {
            inputText = inputText.ToLower();
            inputText = inputText.Replace("\r", " ");
            inputText = inputText.Replace("\n", " ");
            inputText = inputText.Trim(' ');

            string oldString = string.Empty;
            while (oldString != inputText)
            {
                oldString = inputText;
                inputText = inputText.Replace("  ", " ");
            }
        }

        private void buttonCompare_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Started comparing differences");

            DateTime startTime = DateTime.Now;

            string textUncorrected = textBoxUncorrected.Text;
            string textCorrected = textBoxCorrected.Text;

            int numberOfDeletions = 0;
            int numberOfAdditions = 0;
            int numberOfReplacements = 0;

            string accuracyPercent;
            if (radioButtonDiffEngine.Checked)
            {
                accuracyPercent = ProcessDifferences(textUncorrected, textCorrected, ref numberOfDeletions,
                                                         ref numberOfAdditions, ref numberOfReplacements);
            }
            else
            {
                WordSequenceAligner werEval = new WordSequenceAligner();
                WordSequenceAligner.Alignment alignment = werEval.align(textCorrected.Split(' '), textUncorrected.Split(' '));

                numberOfDeletions = alignment.numInsertions;
                numberOfAdditions = alignment.numDeletions;
                numberOfReplacements = alignment.numSubstitutions;

                accuracyPercent = ((float)alignment.getNumCorrect() / (float)alignment.getReferenceLength() * 100.0).ToString("0.0");
            }
            TimeSpan timeTook = DateTime.Now - startTime;

            string message = String.Format("Number of deletions: {0}, ", numberOfDeletions);
            message       += String.Format("additions: {0}, ", numberOfAdditions);
            message       += String.Format("replacements: {0}{1}", numberOfReplacements, Environment.NewLine);
            message       += String.Format("Percentage accuracy: {0}%{1}", accuracyPercent, Environment.NewLine);
            message       += String.Format("Processing took {0} s", timeTook.TotalSeconds.ToString("0.000", CultureInfo.InvariantCulture));

            labelNumberOfDifferences.Text = message;
        }

        private string ProcessDifferences(string textSource, string textDestination, ref int numberOfDeletions,
                                          ref int numberOfAdditions, ref int numberOfReplacements)
        {
            SimplifyText(ref textSource);
            SimplifyText(ref textDestination);

            try
            {
                DifferenceEngine differenceEngine = new DifferenceEngine();

                DifferenceListWordList sourceFile = new DifferenceListWordList(textSource);
                DifferenceListWordList destinationFile = new DifferenceListWordList(textDestination);

                differenceEngine.ProcessDifferences(sourceFile, destinationFile, DifferenceEngineLevel.SlowPerfect);

                ArrayList diffReport = differenceEngine.DifferenceReport();

                foreach (DifferenceResultSpan difference in diffReport)
                {
                    switch (difference.Status)
                    {
                        case DifferenceResultStatus.DeleteSource:
                            PrintDifferences(difference.SourceIndex, difference.Length, sourceFile, "DeleteSource:");
                            numberOfDeletions += difference.Length;
                            break;
                        case DifferenceResultStatus.AddDestination:
                            PrintDifferences(difference.DestIndex, difference.Length, destinationFile, "AddDestination:");
                            numberOfAdditions += difference.Length;
                            break;
                        case DifferenceResultStatus.Replace:
                            PrintDifferences(difference.DestIndex, difference.Length, destinationFile, "Replace:");
                            numberOfReplacements += difference.Length;
                            break;
                        case DifferenceResultStatus.NoChange:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to process the differences in the transcription. Error: " + ex.Message);
            }

            double accuracy = 1.0 -
                              ((double) (numberOfAdditions + numberOfReplacements)/
                               (double) GetNumberOfWordsInString(textBoxCorrected.Text));
            string accuracyPercent = (accuracy*100.0).ToString("0.0");
            return accuracyPercent;
        }

        private static void PrintDifferences(int index, int length, DifferenceListWordList sourceFile, string differenceName)
        {
            Debug.WriteLine(differenceName);
            for (int i = 0; i < length; i++)
            {
                var word = sourceFile.GetByIndex(index + i);
                Debug.WriteLine(((Word) word).WordProp);
            }
        }

        private void textBoxSource_TextChanged(object sender, EventArgs e)
        {
            string text = textBoxUncorrected.Text;
            SimplifyText(ref text);
            labelSourceNoWords.Text = string.Format("Words: {0}", GetNumberOfWordsInString(text));
        }

        private void textBoxDestination_TextChanged(object sender, EventArgs e)
        {
            string text = textBoxCorrected.Text;
            SimplifyText(ref text);
            labelDestinationNoWords.Text = string.Format("Words: {0}", GetNumberOfWordsInString(text));
        }
    }
}
