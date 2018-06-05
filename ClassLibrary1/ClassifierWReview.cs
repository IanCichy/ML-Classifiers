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
    public class ClassifierWReview
    {
        //Outlook 5 possible values (sunny, overcast, rain, snow, sleet)
        //Temperature 4 possible values (hot, mild, cool, cold)  
        //Humidity 3 possible values (high, normal, low)    
        //Wind 2 possible values (weak, strong) 
        //SprintReview 2 possible values (yes, no)

        public Accord.Statistics.Filters.Codification codebook { get; }
        public DecisionTree tree { get; }

        public ClassifierWReview()
        {
            //runData2 and runData2_1
            //string filedata = System.IO.File.ReadAllText("../runData2.txt");
            string filedata = System.IO.File.ReadAllText("../runData2_1.txt");

            string[] inputColumns =
            {
                "Day","Outlook","Temperature","Humidity","Wind","SprintReview"
            };

            string outputColumn = "GoRun";

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
            int[][] inputs = symbols.ToJagged<int>("Outlook", "Temperature", "Humidity", "Wind", "SprintReview");
            int[] outputs = symbols.ToArray<int>("GoRun");

            string[] decisionVariables = { "Outlook", "Temperature", "Humidity", "Wind", "SprintReview" };
            DecisionVariable[] attributes = DecisionVariable.FromCodebook(codebook, decisionVariables);
            // Create a teacher ID3 algorithm
            var id3learning = new ID3Learning(attributes);

            tree = id3learning.Learn(inputs, outputs);
            // Compute the training error when predicting training instances
            double error = new ZeroOneLoss(outputs).Loss(tree.Decide(inputs));

        }
    }
}
