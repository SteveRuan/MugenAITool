using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MugenAITool
{
    class RegularExpressions
    {
        public void AOZ(string anim)
        {
            /*
            //anim语句
            //var anim="600-300+ifElse(var(x),2,ifelse(var(59),4,3))-590";
            //匹配ifelse之外的状态号规则
            var outstate = @"(^\d{1,})|((\+|\-|\/){1,}\d{1,})";
            //匹配ifelse内的状态号规则
            var instate = @"\,\d";
            //正则“，”偏移
            var rex = new Regex(@"\,");
            //正则“+ - * /”偏移
            var rex2 = new Regex(@"\+|\-|\*|\/");
            //记录ifelse外状态号的总和
            var sum = 0;
            //打印返回数组测试用
            //var i = 0;
            //状态号列表
            ArrayList liststate = new ArrayList();
            //匹配获取列表
            MatchCollection mathesone = Regex.Matches(anim, outstate);
            MatchCollection mathestwo = Regex.Matches(anim, instate);
            //获取最外面的ifelse外的运算符
            var IfleseOperator = Regex.Match(anim, @"(\+|\-|\/){1}[iI]f[Ee]lse");
            //string转义算式
            MSScriptControl.ScriptControl sc = new MSScriptControl.ScriptControlClass();
            sc.Language = "JavaScript";

            foreach (Match item in mathesone)
            {

                var Operator = Regex.Match(item.Value, @"(\+|\-|\*|\/)").ToString();
                if (Operator == "")
                {
                    Operator = "+";
                }
                sum = int.Parse(sc.Eval(sum.ToString() + Operator + rex2.Replace(item.Value, "").ToString()).ToString());
                //Console.WriteLine(sum);
                //Console.WriteLine(item.Value);
                //Console.WriteLine(rex2.Replace(item.Value, ""));

            }
           foreach (Match item in mathestwo)
            {
                var Operator = Regex.Match(IfleseOperator.ToString(), @"(\+|\-|\*|\/)").ToString();
                liststate.Add(sc.Eval(sum.ToString() + Operator + rex.Replace(item.Value, "").ToString()).ToString());
                //Console.WriteLine(liststate[i]);
                //Console.WriteLine(rex.Replace(item.Value,""));
                //i++;
            }
           */
        }
    }
}
