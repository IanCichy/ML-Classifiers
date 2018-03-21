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
    public class Classifier
    {
        public Accord.Statistics.Filters.Codification codebook { get; }
        public DecisionTree tree { get; }

        public Classifier()
        {
            //Day,Outlook,Temperature,Humidity,Wind,PlayTennis
            string filedata = System.IO.File.ReadAllText("../runData1.txt");

            string[] inputColumns =
            {
                "Day","Outlook","Temperature","Humidity","Wind"
            };

            string outputColumn = "PlayTennis";

            // Let's populate a data table with this information.
            // 
            DataTable data = new DataTable("Internet Services Run Calculator");
            data.Columns.Add(inputColumns);
            data.Columns.Add(outputColumn);

            string[] lines = filedata.Split(
                new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
                data.Rows.Add(line.Split(','));

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
            //new DecisionVariable("Outlook",     5), // 5 possible values (sunny, overcast, rain, snow, sleet)
            //new DecisionVariable("Temperature", 4), // 4 possible values (hot, mild, cool, cold)  
            //new DecisionVariable("Humidity",    3), // 3 possible values (high, normal, low)    
            //new DecisionVariable("Wind",        2)  // 2 possible values (weak, strong) 

            tree = id3learning.Learn(inputs, outputs);

            // Compute the training error when predicting training instances
            double error = new ZeroOneLoss(outputs).Loss(tree.Decide(inputs));

        }
    }
}
