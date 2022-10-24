п»їusing System.Linq;

namespace ReversePolishNotation;

public static class NotationElement
{
    public static sbyte GetPriority(this string element)
    {
        switch (element)
        {
            case "+":
            case "-": return 1;

            case "*":
            case "/": return 2;

            case "^": return 3;

            case "sin":
            case "cos":
            case "abs": return 4;

            default: return -1;
        }
    }

    public static bool GetDouble(this string element, out double value)
    {
        return Double.TryParse(element, out value);
    }

    public static bool IsDouble(this string element)
    {
        double val;
        return GetDouble(element, out val);
    }

    public static bool IsOperator(this string element)
    {
        return GetPriority(element) != -1;
    }

    public static bool IsBinaryOperator(this string element)
    {
        switch (element)
        {
            case "+":
            case "-": 
            case "*":
            case "/": return true;

            default: return false;
        }
    }
}

delegate decimal BinaryOperation(decimal a, decimal b);
delegate decimal UnaryOperation(decimal a);
public class RPN
{
    List<string> rpn;

    public RPN(string rpnNotation)
    {
        rpn = new List<string>();
        rpn.AddRange(rpnNotation.Split(" "));
    }

    public static RPN FromInfixNotation(string InfixNotation)
    {
        var stack = new Stack<string>();
        string output = "";
        List<string> elements = new List<string>();
        elements.AddRange(InfixNotation.Split(" "));
        for (int i = 0; i < elements.Count;)
        {
            var s = elements[i];
            if (s == "(")
                stack.Push(s);
            else if (s == ")")
            {
                while (stack.Peek() != "(")
                    output += " " + stack.Pop();//TODO РїСЂРѕРІРµСЂРёС‚СЊ С‡С‚Рѕ РµСЃР»Рё СЃС‚РµРє РїСѓСЃС‚РѕР№, С‚Рѕ СЌС‚Рѕ РѕС€РёР±РєР°
                stack.Pop();
            }
            else if (s.IsDouble())
                output += " " + s;
            else if (stack.Count == 0 || stack.Peek() == "(")
                stack.Push(s);
            else
            {
                if (stack.Peek().GetPriority() < s.GetPriority())
                    stack.Push(s);
                else
                {
                    output += " " + stack.Pop();
                    continue; //TOTO РµСЃР»Рё РІСЃС‚СЂРµС‡Р°РµС‚СЃСЏ СѓРЅР°СЂРЅС‹Р№ РјРёРЅСѓСЃ, (РЅР°РїСЂРёРјРµСЂ "-8+7*(-6+2)), С‚Рѕ СѓС‡РёС‚С‹РІР°С‚СЊ РµРіРѕ РІ РЅР°С‡Р°Р»Рµ СЃС‚СЂРѕРєРё Рё СЃСЂР°Р·Сѓ РїРѕСЃР»Рµ РѕС‚РєСЂС‹РІР°СЋС‰РµР№ СЃРєРѕР±РєРё, РґРѕР±Р°РІР»СЏСЏ 0 РІ РћРџР—. РјРѕР¶РЅРѕ РІРѕ РІС…РѕРґРЅРѕР№ СЃС‚СЂРѕРєРµ СЃСЂР°Р·Сѓ Р·Р°РјРµРЅРёС‚СЊ "(-" РЅР° "(0 - " Рё РІ РЅР°С‡Р°Р»Рµ СЃС‚СЂРѕРєРё С‚РѕР¶Рµ РЅР° "0 - ".
                }
            }
            i++;
        }

        while (stack.Count > 0)
            output += " " + stack.Pop();

        return new RPN(output.Trim());
    }

    public override string ToString()
    {
        return string.Join(" ", rpn);
    }

    public double Evaluate(double? x = null)
    {
        var stack = new Stack<double>();

        foreach (string op in rpn)
        {
            if (!op.IsOperator())
                if (op == "x")
                    if (x != null)
                        stack.Push((double)x);
                    else
                        throw new ArithmeticException("x РЅРµ Р·Р°РґР°РЅ");
                else
                    stack.Push(double.Parse(op));
            else
            {
                double b;
                switch (op)
                {
                    case "+": stack.Push(stack.Pop() + stack.Pop()); break;
                    case "-":
                        /*if (CalcStack.Count == 1)
                            CalcStack.Push(-CalcStack.Pop());
                        else*/
                        {
                            b = stack.Pop();
                            stack.Push(stack.Pop() - b);
                        }
                        break;
                    case "*": stack.Push(stack.Pop() * stack.Pop()); break;
                    case "/": b = stack.Pop(); stack.Push(stack.Pop() / b); break;
                    case "^": b = stack.Pop(); stack.Push(Math.Pow(stack.Pop(), b)); break;
                    case "sin": stack.Push(Math.Sin(stack.Pop())); break;
                    case "cos": stack.Push(Math.Cos(stack.Pop())); break;
                    case "abs": stack.Push(Math.Abs(stack.Pop())); break;
                    default: throw new ArithmeticException("РћС€РёР±РєР° РІС‹СЂР°Р¶РµРЅРёСЏ");
                }
            }
        }
        if (stack.Count == 1)
            return stack.Pop();
        else
            throw new ArithmeticException("РћС€РёР±РєР° РІС‹СЂР°Р¶РµРЅРёСЏ");
    }
}
