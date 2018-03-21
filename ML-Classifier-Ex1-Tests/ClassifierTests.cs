using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ML_Classifier_Ex1_Tests
{
    [TestClass]
    public class ClassifierTests
    {
        [TestMethod]
        public void TestResultsNoReview()
        {
            var clasif = new Classifier_Ex1.Classifier();
            var codebook = clasif.codebook;
            var tree = clasif.tree;


            int[] query = codebook.Transform(new[,]
            {
                { "Outlook",     "sunny"  },
                { "Temperature", "hot"    },
                { "Humidity",    "high"   },
                { "Wind",        "strong" }
            });
            int predicted = tree.Decide(query);
            string answer = codebook.Revert("PlayTennis", predicted);


            int[] query2 = codebook.Transform(new[,]
            {
                { "Outlook",     "overcast" },
                { "Temperature", "mild"     },
                { "Humidity",    "low"      },
                { "Wind",        "weak"     }
            });
            int predicted2 = tree.Decide(query2);
            string answer2 = codebook.Revert("PlayTennis", predicted2);


            int[] query3 = codebook.Transform(new[,]
            {
                { "Outlook",     "snow"     },
                { "Temperature", "cold"     },
                { "Humidity",    "normal"   },
                { "Wind",        "weak"     }
            });
            int predicted3 = tree.Decide(query3);
            string answer3 = codebook.Revert("PlayTennis", predicted3);
        }

        [TestMethod]
        public void TestResultsWReview()
        {
            var clasif = new Classifier_Ex1.ClassifierWReview();
            var codebook = clasif.codebook;
            var tree = clasif.tree;


            int[] query = codebook.Transform(new[,]
            {
                { "Outlook",     "sunny"    },
                { "Temperature", "hot"      },
                { "Humidity",    "high"     },
                { "Wind",        "strong"   },
                { "SprintReview","yes"      }
            });
            int predicted = tree.Decide(query);
            string answer = codebook.Revert("PlayTennis", predicted);

            int[] query2 = codebook.Transform(new[,]
            {
                { "Outlook",     "overcast" },
                { "Temperature", "mild"     },
                { "Humidity",    "low"      },
                { "Wind",        "weak"     },
                { "SprintReview","yes"      }
            });
            int predicted2 = tree.Decide(query2);
            string answer2 = codebook.Revert("PlayTennis", predicted2);

            int[] query2_1 = codebook.Transform(new[,]
            {
                { "Outlook",     "overcast" },
                { "Temperature", "mild"     },
                { "Humidity",    "low"      },
                { "Wind",        "weak"     },
                { "SprintReview","no"      }
            });
            int predicted2_1 = tree.Decide(query2_1);
            string answer2_1 = codebook.Revert("PlayTennis", predicted2_1);

            int[] query3 = codebook.Transform(new[,]
            {
                { "Outlook",     "snow"     },
                { "Temperature", "cold"     },
                { "Humidity",    "normal"   },
                { "Wind",        "weak"     },
                { "SprintReview","no"       }
            });
            int predicted3 = tree.Decide(query3);
            string answer3 = codebook.Revert("PlayTennis", predicted3);
        }


        [TestMethod]
        public void TestResultsTitanic()
        {
            var clasif = new Classifier_Ex1.ClassifierTitanic();
            //var codebook = clasif.codebook;
            //var tree = clasif.tree;
        }


    }
}
