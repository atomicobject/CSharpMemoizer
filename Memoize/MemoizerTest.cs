using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Memoizer
{
    public class DoThings
    {
        public Func<string> WithNoInput = Memoizer.Memoize(
            () =>
                {
                    Console.Out.WriteLine("Real WithNoInput method ran");
                    return "the value";
                }
            );

        public Func<string, string> WithOneInput = Memoizer.Memoize(
            (string x) =>
                {
                    Console.Out.WriteLine("Real WithOneInput method ran");
                    return String.Format("Hello, {0}", x);
                }
            );

        public Func<string, Dictionary<string, string>, Dictionary<string, string>> WithComplexInput = Memoizer.Memoize(
            (string x, Dictionary<string, string> y) =>
                {
                    Console.Out.WriteLine("Real WithComplexInput method ran");
                    var result = new Dictionary<string, string>();
                    foreach (string key in y.Keys)
                    {
                        result[key] = y[key] + x;
                    }
                    return result;
                }
            );
    }

    public class TestIt
    {
        [Test]
        public void The_no_input_case_only_runs_the_real_function_once()
        {
            // You should see only one "Real WithNoInput method ran" message in the console log
            Console.Out.WriteLine("You should only see one 'Real WithNoInput method ran' message in the log below");
            var doThings = new DoThings();
            Assert.AreEqual("the value", doThings.WithNoInput());
            Assert.AreEqual("the value", doThings.WithNoInput());
            Assert.AreEqual("the value", doThings.WithNoInput());
        }

        [Test]
        public void The_one_input_case_only_runs_the_real_function_once_when_the_input_is_the_same()
        {
            var doThings = new DoThings();

            // You should only see two "Real WithOneInput method ran" messages in the console log
            Console.Out.WriteLine("You should only see two 'Real WithOneInput method ran' messages in the log below");
            Assert.AreEqual("Hello, Dan.", doThings.WithOneInput("Dan."));
            Assert.AreEqual("Hello, David.", doThings.WithOneInput("David."));
            Assert.AreEqual("Hello, David.", doThings.WithOneInput("David."));
            Assert.AreEqual("Hello, Dan.", doThings.WithOneInput("Dan."));
        }

        [Test]
        public void The_complex_input_case_only_runs_the_real_function_once_when_the_input_is_the_same()
        {
            var doThings = new DoThings();

            // You should see two "Real WithComplexInput method ran" messages in the console log
            Console.Out.WriteLine("You should only see two 'Real WithComplexInput method ran' messages in the log below");

            var expected = new Dictionary<string, string>() {{"one", "1-digit"}, {"two", "2-digit"}};
            var input = new Dictionary<string, string>() {{"one", "1"}, {"two", "2"}};
            var append = "-digit";
            Assert.AreEqual(expected, doThings.WithComplexInput(append, input));
            Assert.AreEqual(expected, doThings.WithComplexInput(append, input));
            Assert.AreEqual(expected, doThings.WithComplexInput(append, input));

            var doThings2 = new DoThings();
            Assert.AreEqual(expected, doThings2.WithComplexInput(append, input));
            Assert.AreEqual(expected, doThings2.WithComplexInput(append, input));
        }
    }

}

