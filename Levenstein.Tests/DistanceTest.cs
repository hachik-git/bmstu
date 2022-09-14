namespace EditDistance.Tests;

[TestClass]
public class DistanceTest
{
    [TestMethod]
    public void Matrix_пусто_пусто_0()
    {
        var s1 = "";
        var s2 = "";

        int d = new Levenstein(s1, s2).GetDistance(CalcMethod.Matrix);
        int dp = new Levenstein(s1, s2).Distance;

        Assert.AreEqual(d, 0);
        Assert.AreEqual(d, dp);
    }
    [TestMethod]
    public void Recursive_пусто_пусто_0()
    {
        var s1 = "";
        var s2 = "";

        int d = new Levenstein(s1, s2).GetDistance(CalcMethod.Recursive);
        
        Assert.AreEqual(d, 0);
    }
    [TestMethod]
    public void RecursiveCashed_пусто_пусто_0()
    {
        var s1 = "";
        var s2 = "";

        int d = new Levenstein(s1, s2).GetDistance(CalcMethod.RecursiveCashed);

        Assert.AreEqual(d, 0);
    }

    [TestMethod]
    public void Matrix_пусто_кот_3()
    {
        var s1 = "";
        var s2 = "кот";

        int d = new Levenstein(s1, s2).GetDistance(CalcMethod.Matrix);
        int dp = new Levenstein(s1, s2).Distance;

        Assert.AreEqual(d, 3);
        Assert.AreEqual(d, dp);
    }
    [TestMethod]
    public void Matrix_пусто_гибралтар_9()
    {
        var s1 = "";
        var s2 = "гибралтар";

        int d = new Levenstein(s1, s2).GetDistance(CalcMethod.Matrix);
        int dp = new Levenstein(s1, s2).Distance;

        Assert.AreEqual(d, 9);
        Assert.AreEqual(d, dp);
    }
    [TestMethod]
    public void Recurcive_пусто_кот_3()
    {
        var s1 = "";
        var s2 = "кот";

        int d = new Levenstein(s1, s2).GetDistance(CalcMethod.Recursive);

        Assert.AreEqual(d, 3);
    }
    [TestMethod]
    public void Recurcive_пусто_гибралтар_9()
    {
        var s1 = "";
        var s2 = "гибралтар";

        int d = new Levenstein(s1, s2).GetDistance(CalcMethod.Recursive);

        Assert.AreEqual(d, 9);
    }
    [TestMethod]
    public void Recurcive_Cashed_пусто_кот_3()
    {
        var s1 = "";
        var s2 = "кот";

        int d = new Levenstein(s1, s2).GetDistance(CalcMethod.RecursiveCashed);

        Assert.AreEqual(d, 3);
    }
    [TestMethod]
    public void Recurcive_Cashed_пусто_гибралтар_9()
    {
        var s1 = "";
        var s2 = "гибралтар";

        int d = new Levenstein(s1, s2).GetDistance(CalcMethod.RecursiveCashed);

        Assert.AreEqual(d, 9);
    }

    [TestMethod]
    public void Matrix_кот_скат_2()
    {
        var s1 = "кот";
        var s2 = "cкaт";

        int d = new Levenstein(s1, s2).GetDistance(CalcMethod.Matrix);
        int dp = new Levenstein(s1, s2).Distance;

        Assert.AreEqual(d, 2);
        Assert.AreEqual(d, dp);

    }
    [TestMethod]
    public void Matrix_гибралтар_лабрадор_5()
    {
        var s1 = "гибралтар";
        var s2 = "лабрадор";

        int d = new Levenstein(s1, s2).GetDistance(CalcMethod.Matrix);
        int dp = new Levenstein(s1, s2).Distance;

        Assert.AreEqual(d, 5);
        Assert.AreEqual(d, dp);
    }
    [TestMethod]
    public void Matrix_кот_лабрадор_7()
    {
        var s1 = "кот";
        var s2 = "лабрадор";

        int d = new Levenstein(s1, s2).GetDistance(CalcMethod.Matrix);
        int dp = new Levenstein(s1, s2).Distance;

        Assert.AreEqual(d, 7);
        Assert.AreEqual(d, dp);

    }
    [TestMethod]
    public void Recurcive_кот_скат_2()
    {
        var s1 = "кот";
        var s2 = "cкaт";

        int d = new Levenstein(s1, s2).GetDistance(CalcMethod.Recursive);

        Assert.AreEqual(d, 2);

    }
    [TestMethod]
    public void Recurcive_гибралтар_лабрадор_5()
    {
        var s1 = "гибралтар";
        var s2 = "лабрадор";

        int d = new Levenstein(s1, s2).GetDistance(CalcMethod.Recursive);

        Assert.AreEqual(d, 5);

    }
    [TestMethod]
    public void Recurcive_кот_лабрадор_7()
    {
        var s1 = "кот";
        var s2 = "лабрадор";

        int d = new Levenstein(s1, s2).GetDistance(CalcMethod.Recursive);

        Assert.AreEqual(d, 7);
    }
    [TestMethod]
    public void Recurcive_Cashed_кот_скат_2()
    {
        var s1 = "кот";
        var s2 = "cкaт";

        int d = new Levenstein(s1, s2).GetDistance(CalcMethod.RecursiveCashed);

        Assert.AreEqual(d, 2);
    }
    [TestMethod]
    public void Recurcive_Cashed_гибралтар_лабрадор_5()
    {
        var s1 = "гибралтар";
        var s2 = "лабрадор";

        int d = new Levenstein(s1, s2).GetDistance(CalcMethod.RecursiveCashed);

        Assert.AreEqual(d, 5);
    }
    [TestMethod]
    public void Recurcive_Cashed_кот_лабрадор_7()
    {
        var s1 = "кот";
        var s2 = "лабрадор";

        int d = new Levenstein(s1, s2).GetDistance(CalcMethod.RecursiveCashed);

        Assert.AreEqual(d, 7);
    }

    [TestMethod]
    public void Path_кот_скат_IMRM()
    {
        var s1 = "кот";
        var s2 = "скат";

        char[] p = new Levenstein(s1, s2).Path;
        Assert.AreEqual("IMRM", new String(p));
    }

    [TestMethod]
    public void Path_скaт_кот_DMRM()
    {
        var s1 = "скат";
        var s2 = "кот";

        char[] p = new Levenstein(s1, s2).Path;
        Assert.AreEqual("DMRM", new String(p));
    }
    [TestMethod]
    public void Path_гибралтар_кот_DDDDRRMDD()
    {
        var s1 = "гибралтар";
        var s2 = "кот";

        char[] p = new Levenstein(s1, s2).Path;
        Assert.AreEqual("DDDDRRMDD", new String(p));
    }
    [TestMethod]
    public void Path_кот_гибралтар_IIIIRRMII()
    {
        var s1 = "кот";
        var s2 = "гибралтар";

        char[] p = new Levenstein(s1, s2).Path;
        Assert.AreEqual("IIIIRRMII", new String(p));
    }
    [TestMethod]
    public void Path_кот_пусто_DDD()
    {
        var s1 = "кот";
        var s2 = "";

        char[] p = new Levenstein(s1, s2).Path;
        Assert.AreEqual("DDD", new String(p));
    }
}