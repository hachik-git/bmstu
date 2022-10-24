п»їusing Allure.Commons;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using ReversePolishNotation;

namespace Matrices.Tests.NUnit;

[AllureNUnit]
[TestFixture(TestOf = typeof(RPN), Description = "RPN tests")]

public class RPNTest
{
    [AllureStep("Evaluating {0}. Expected {1}")]
    public static void CheckCalc(string rpnExpression, double expectedResult, double? x = null)
    {
        Assert.That(expectedResult, Is.EqualTo(new RPN(rpnExpression).Evaluate(x)));
    }
    
    [Test]
    [AllureSeverity(SeverityLevel.critical)]
    public void TestCalculation()
    {
        CheckCalc("25 8 70 * + 6 2 / -", 582);
        CheckCalc("25 8 x * + 6 2 / -", 582, 70);
        /*CheckCalc("25 - 8 70 * + 6 2 / -", 532);
        CheckCalc("25 - 8 70 * + 6 2 - / -", 538);
        CheckCalc("6 2 ^", 36);
        CheckCalc("25 - 8 70 * + 6 2 ^ 2 - / -", 553);
        CheckCalc("25 - 8 70 * + 6 2 ^ 2 - / -", 553);*/
    }

    [Test]
    [AllureSeverity(SeverityLevel.critical)]
    public void TestConvertation()
    {
        Assert.That(RPN.FromInfixNotation("25 + 8").ToString() == "25 8 +");
        Assert.That(RPN.FromInfixNotation("25 + 8 * 70").ToString() == "25 8 70 * +");
        Assert.That(RPN.FromInfixNotation("25 + 8 * 70 - 6").ToString() == "25 8 70 * + 6 -");
        Assert.That(RPN.FromInfixNotation("25 + 8 * 70 - 6 / 2").ToString() == "25 8 70 * + 6 2 / -");
        Assert.That(RPN.FromInfixNotation("25 + 8 * 70 - 6 ^ 2 + 55 / 5").ToString() == "25 8 70 * + 6 2 ^ - 55 5 / +");
        Assert.That(RPN.FromInfixNotation("25 + 8 * 70 - 6 ^ 2 + 55 / ( 2 + 3 )").ToString() == "25 8 70 * + 6 2 ^ - 55 2 3 + / +");    
    }
}