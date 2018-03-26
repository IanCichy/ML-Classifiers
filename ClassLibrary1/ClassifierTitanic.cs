using Accord;
using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.Math;
using Accord.Math.Optimization.Losses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier_Ex1
{
    public class ClassifierTitanic
    {
        /* 
        * -----Explanatory Variables-----
        * Pclass - 3 possible values (1,2,3)
        * Title - Needs custom rules
        * LastName - Truncate
        * FirstName - Truncate
        * Sex - 2 possible values (male, female)
        * Age - Continuous 
        * SibSp - Continuous
        * Parch - Continuous
        * Ticket - Truncate
        * Fare - Continuous
        * Cabin  - Needs custom rules
        * Embarked - 4 possible variables (C, S, Q, None)
        * 
        * -----Outcome Variable-----
        * Survived - 2 possible values (yes, no)
        */

        public Accord.Statistics.Filters.Codification codebook { get; }
        public DecisionTree tree { get; }

        DataTable rawData { get; set; }
        DataTable trainingData { get; set; }
        DataTable testingData { get; set; }


        public ClassifierTitanic()
        {
            rawData = new DataTable("Titanic Data");
            trainingData = new DataTable();
            testingData = new DataTable();

            string filedata = System.IO.File.ReadAllText("../titanicData.txt");
            string[] dataColumns = System.IO.File.ReadAllText("../titanicColumns.txt").Split(',');

            //Input columns are to be learned from
            string[] inputColumns = new string[dataColumns.Length - 1];
            Array.Copy(dataColumns, 0, inputColumns, 0, dataColumns.Length - 1);

            //Output is what we are trying to predict
            string outputColumn = dataColumns[dataColumns.Length - 1];

            //Create an easy way to store and manipulate data
            rawData.Columns.Add(inputColumns);
            rawData.Columns.Add(outputColumn);

            trainingData.Columns.Add(inputColumns);
            trainingData.Columns.Add(outputColumn);

            testingData.Columns.Add(inputColumns);
            testingData.Columns.Add(outputColumn);

            string[] lines = filedata.Split(
                new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
                rawData.Rows.Add(line.Split(','));


            //Clean up data representation and missing data
            rawData = cleanData(rawData);

            DataTable[] dt = splitDataForTraining(rawData, .8);
            trainingData = dt[0];
            testingData = dt[1];



            //int[][] inputs = symbols.ToJagged<int>("???");
            //int[] outputs = symbols.ToArray<int>("Survived");

            //string[] decisionVariables = { "???" };
            //DecisionVariable[] attributes = DecisionVariable.FromCodebook(codebook, decisionVariables);
            //// Create a teacher ID3 algorithm
            //var id3learning = new ID3Learning(attributes);

            //tree = id3learning.Learn(inputs, outputs);

            //// Compute the training error when predicting training instances
            //double error = new ZeroOneLoss(outputs).Loss(tree.Decide(inputs));

        }


        private DataTable cleanData(DataTable table)
        {
            cleanTitles(table);
            cleanFare(table);
            //cleanCabin(table);

            //DataTable newtable = new DataTable();

            //TODO: Copy only data columns we want into final data set to learn from

            return table;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        private void cleanCabin(DataTable dt)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Remove any decimal portion of the fare
        /// </summary>
        /// <param name="dt"></param>
        private void cleanFare(DataTable dt)
        {
            int fareColumnNumber = dt.Columns["Fare"].Ordinal;

            foreach (DataRow row in dt.Rows)
            {
                row[fareColumnNumber] =  Math.Round(double.Parse(row[fareColumnNumber].ToString()));
            }
        }

        /// <summary>
        /// Convert titles that are rare into more commmon ones
        /// </summary>
        /// <param name="dt"></param>
        private void cleanTitles(DataTable dt)
        {
            HashSet<string> titleMR = new HashSet<string>{
                "Don", "Major", "Capt", "Jonkheer", "Rev", "Col" };
            HashSet<string> titleMRS = new HashSet<string> {
                "Countess", "Mme", "Lady" };
            HashSet<string> titleMISS = new HashSet<string> {
                "Mlle", "Ms" };

            int titleColumnNumber = dt.Columns["Title"].Ordinal;
            int sexColumnNumber = dt.Columns["Sex"].Ordinal;

            foreach (DataRow row in dt.Rows)
            {
                if (titleMR.Contains(row[titleColumnNumber]))
                {
                    row[titleColumnNumber] = "Mr";
                }
                else if (titleMRS.Contains(row[titleColumnNumber]))
                {
                    row[titleColumnNumber] = "Mrs";
                }
                else if (titleMISS.Contains(row[titleColumnNumber]))
                {
                    row[titleColumnNumber] = "Miss";
                }
                else if (row[titleColumnNumber].Equals("Dr"))
                {
                    if (row[sexColumnNumber].Equals("male"))
                    {
                        row[titleColumnNumber] = "Mr";
                    }
                    else
                    {
                        row[titleColumnNumber] = "Mrs";
                    }
                }
            }
        }


        private DataTable[] splitDataForTraining(DataTable table, double amtToTrain)
        {
            DataTable trainingSet = new DataTable();
            DataTable testingSet = new DataTable();

            DataTable dt = Extensions.OrderRandomly(table.AsEnumerable()).CopyToDataTable();

            int numRowsTrain = (int)Math.Ceiling(dt.Rows.Count * amtToTrain);

            for (int x = 0; x < dt.Rows.Count; x++)
            {
                if(x <= numRowsTrain)
                {
                    trainingSet.Rows.Add(dt.Rows[x].ItemArray);
                }
                else
                {
                    testingSet.Rows.Add(dt.Rows[x].ItemArray);
                }
            }
            return new DataTable[] { trainingSet, testingSet };
        }
    }

    public static class Extensions
    {

        private static Random random = new Random();

        public static IEnumerable<T> OrderRandomly<T>(this IEnumerable<T> items)
        {
            List<T> randomly = new List<T>(items);

            while (randomly.Count > 0)
            {

                Int32 index = random.Next(randomly.Count);
                yield return randomly[index];

                randomly.RemoveAt(index);
            }
        }
    }

}
