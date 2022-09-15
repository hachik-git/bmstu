п»їusing Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EditDistance.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(string s1, string s2)
        {
            var L = new Levenstein(s1, s2);

            ViewData["s1"] = L.s1;
            ViewData["s2"] = L.s2;
            ViewData["matrix"] = L.Matrix;
            ViewData["ldm"] = L.GetDistance(CalcMethod.Matrix);
            ViewData["ldr"] = L.GetDistance(CalcMethod.Recursive);
            ViewData["ldrc"] = L.GetDistance(CalcMethod.RecursiveCashed);
            ViewData["lddl"] = L.GetDistance(CalcMethod.Domerau);
            ViewData["path"] = new string(L.Path);

            var benchmark1 = new TimeSpan[4];
            
            foreach (var method in CalcMethod.GetValues<EditDistance.CalcMethod>())
                benchmark1[(int)method] = L.MeasureTime(method);

            ViewData["benchmark1"] = benchmark1;
            
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            
            int mc = 12;
            var b = new (string, TimeSpan, TimeSpan, TimeSpan)[mc*2];
            string w1, w2;
            Levenstein lev;
            for (int i = 1; i <= mc; i++)
            {
                w1 = "";
                for (int j = 1; j <= i; j++)
                    w1 += j.ToString();

                w2 = w1.Substring(0, w1.Length - 1);
                lev = new Levenstein(w1, w2);
                
                b[i * 2 - 2] = new(w1 + "-" + w2,
                    lev.MeasureTime(CalcMethod.Matrix),
                    lev.MeasureTime(CalcMethod.Recursive),
                    lev.MeasureTime(CalcMethod.RecursiveCashed));

                w2 = w1;
                
                lev = new Levenstein(w1, w2);
                b[i * 2 - 1] = new(w1 + "-" + w2,
                   lev.MeasureTime(CalcMethod.Matrix),
                   lev.MeasureTime(CalcMethod.Recursive),
                   lev.MeasureTime(CalcMethod.RecursiveCashed));
            }

            var s = "";

            for (int i = 0; i < mc*2; i++)
                s += $"[{i}, {b[i].Item2.Ticks}, {b[i].Item3.Ticks}, {b[i].Item4.Ticks}],";
            
            ViewData["benchmark2"] = s;
        }
    }
}