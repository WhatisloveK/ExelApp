using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowsFormsApp5;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {


        private string ToAssertableString(IDictionary<string, List<string>> dictionary)
        {
            var pairStrings = dictionary.OrderBy(p => p.Key)
                                        .Select(p => p.Key + ": " + string.Join(", ", p.Value));
            return string.Join("; ", pairStrings);
        }
        [TestMethod]
        public void CreatingDependentDictionary()
        {
            //arrange
            Form1 form = new Form1();
            
            string formula = "4+A.0+B.1";
            string cell = "B.0";
            Dictionary<string, List<string>> ExpectedDependentVariables = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> ActualDependentVariables = new Dictionary<string, List<string>>();
            ExpectedDependentVariables.Add(cell, new List<string>());
            ExpectedDependentVariables[cell].Add("A.0");
            ExpectedDependentVariables[cell].Add("B.1");
            //act           
            ActualDependentVariables = form.TestCalculatingDependentVariables(cell, formula);

            //assert
            Assert.AreEqual(ToAssertableString(ExpectedDependentVariables), ToAssertableString(ActualDependentVariables));

        }


        [TestMethod]
        public void CreatingDependencisByVariableDictionary()
        {
            //arrange
            Form1 form = new Form1();
            string formula = "4+C.0+D.1";
            string cell = "B.0";
            Dictionary<string, List<string>> ExpectedDependencisByVariables = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> ActualDependencisByVariables = new Dictionary<string, List<string>>();
            ExpectedDependencisByVariables.Add("D.1", new List<string>());
            ExpectedDependencisByVariables.Add("C.0", new List<string>());
            ExpectedDependencisByVariables["C.0"].Add("B.0");
            ExpectedDependencisByVariables["D.1"].Add("B.0");

            //act           
             form.TestCalculatingDependentVariables(cell, formula);
             ActualDependencisByVariables = form.TestCalculatingDependencisByVariables();

            //assert
            Assert.AreEqual(ToAssertableString(ExpectedDependencisByVariables), ToAssertableString(ActualDependencisByVariables));
        }

        [TestMethod]
        public void TestRecursion()
        {
            //arrange
            Form1 form = new Form1();
            Dictionary<string, List<string>> DependentVariables = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> DependencisByVariable = new Dictionary<string, List<string>>();
            string formula = "4+A.0+B.0";
            string cell = "B.0";
            DependentVariables = form.TestCalculatingDependentVariables(cell, formula);
            DependencisByVariable = form.TestCalculatingDependencisByVariables();
            string expected = "B.0";
            //act
           string actual = form.TestRecursion();
            //assert
            Assert.AreEqual(expected, actual);

        }
    }
}
