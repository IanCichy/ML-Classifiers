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
        public Accord.Statistics.Filters.Codification codebook { get; }
        public DecisionTree tree { get; }
        DataTable data { get; set; }

        public ClassifierTitanic()
        {
            data = new DataTable("Titanic Data");
            //"Pclass","Title","LastName","FirstName","Name","Sex","Age","SibSp","Parch","Ticket","Fare","Cabin","Embarked"


            string filedata = System.IO.File.ReadAllText("../titanicData.txt");
            string[] dataColumns = System.IO.File.ReadAllText("../titanicColumns.txt").Split(',');

            //Input columns are to be learned from
            string[] inputColumns = new string[dataColumns.Length - 1];
            Array.Copy(dataColumns, 0, inputColumns, 0, dataColumns.Length - 1);

            //Output is what we are trying to predict
            string outputColumn = dataColumns[dataColumns.Length-1];

            //Create an easy way to store and manipulate data
            data.Columns.Add(inputColumns);
            data.Columns.Add(outputColumn);

            string[] lines = filedata.Split(
                new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var line in lines)
                data.Rows.Add(line.Split(','));


            //Clean up data representation and missing data
            data = cleanData(data);




            //create codebook to turn the strings into number representations
            codebook = new Accord.Statistics.Filters.Codification(data);

            // Translate our training data into integer symbols using our codebook:
            DataTable symbols = codebook.Apply(data);
            int[][] inputs = symbols.ToJagged<int>("Outlook", "Temperature", "Humidity", "Wind");
            int[] outputs = symbols.ToArray<int>("PlayTennis");

            string[] decisionVariables = { "Outlook", "Temperature", "Humidity", "Wind" };
            DecisionVariable[] attributes = DecisionVariable.FromCodebook(codebook, decisionVariables);
            // Create a teacher ID3 algorithm
            var id3learning = new ID3Learning(attributes);

            tree = id3learning.Learn(inputs, outputs);

            // Compute the training error when predicting training instances
            double error = new ZeroOneLoss(outputs).Loss(tree.Decide(inputs));

        }


        private DataTable cleanData(DataTable table)
        {
            DataTable newtable = new DataTable();

            //bool addrow;
            //foreach(DataRow row in table.Rows)
            //{
            //    addrow = true;
            //    foreach(var x in row.ItemArray)
            //    {
            //        if( x == null)
            //        {
            //            addrow = false;
            //        }

            //    }
            //    if (addrow)
            //    {
            //        newtable.Rows.Add(row.ItemArray);
            //    }

            //}


            return newtable;
        }



    }
}
