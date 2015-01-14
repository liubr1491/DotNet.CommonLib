using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.CommonLib.Helper
{
    public class DateTimeHelper
    {
        public void ConvertDateTime()
        {
            //今天   
            DateTime.Now.Date.ToShortDateString();
            //昨天，就是今天的日期减一   
            DateTime.Now.AddDays(-1).ToShortDateString();
            //明天，同理，加一   
            DateTime.Now.AddDays(1).ToShortDateString();

            //本周(要知道本周的第一天就得先知道今天是星期几，从而得知本周的第一天就是几天前的那一天，要注意的是这里的每一周是从周日始至周六止   
            DateTime.Now.AddDays(Convert.ToDouble((0 - Convert.ToInt16(DateTime.Now.DayOfWeek)))).ToShortDateString();
            DateTime.Now.AddDays(Convert.ToDouble((6 - Convert.ToInt16(DateTime.Now.DayOfWeek)))).ToShortDateString();
            //如果你还不明白，再看一下中文显示星期几的方法就应该懂了   
            //由于DayOfWeek返回的是数字的星期几，我们要把它转换成汉字方便我们阅读，有些人可能会用switch来一个一个地对照，其实不用那么麻烦的                 
            string[] Day = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            string dayStr = Day[Convert.ToInt16(DateTime.Now.DayOfWeek)];

            //上周，同理，一个周是7天，上周就是本周再减去7天，下周也是一样   
            DateTime.Now.AddDays(Convert.ToDouble((0 - Convert.ToInt16(DateTime.Now.DayOfWeek))) - 7).ToShortDateString();
            DateTime.Now.AddDays(Convert.ToDouble((6 - Convert.ToInt16(DateTime.Now.DayOfWeek))) - 7).ToShortDateString();
            //下周   
            DateTime.Now.AddDays(Convert.ToDouble((0 - Convert.ToInt16(DateTime.Now.DayOfWeek))) + 7).ToShortDateString();
            DateTime.Now.AddDays(Convert.ToDouble((6 - Convert.ToInt16(DateTime.Now.DayOfWeek))) + 7).ToShortDateString();
            //本月,很多人都会说本月的第一天嘛肯定是1号，最后一天就是下个月一号再减一天。当然这是对的   
            //一般的写法   
            string monthFirstDay = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + "1"; //第一天   
            DateTime.Parse(DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + "1").AddMonths(1).AddDays(-1).ToShortDateString();//最后一天   

            //巧用C#里ToString的字符格式化更简便   
            DateTime.Now.ToString("yyyy-MM-01");
            DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(1).AddDays(-1).ToShortDateString();

            //上个月，减去一个月份   
            DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-1).ToShortDateString();
            DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddDays(-1).ToShortDateString();
            //下个月，加去一个月份   
            DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(1).ToShortDateString();
            DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(2).AddDays(-1).ToShortDateString();
            //7天后   
            DateTime.Now.Date.ToShortDateString();
            DateTime.Now.AddDays(7).ToShortDateString();
            //7天前   
            DateTime.Now.AddDays(-7).ToShortDateString();
            DateTime.Now.Date.ToShortDateString();

            //本年度，用ToString的字符格式化我们也很容易地算出本年度的第一天和最后一天   
            DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).ToShortDateString();
            DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddYears(1).AddDays(-1).ToShortDateString();
            //上年度，不用再解释了吧   
            DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddYears(-1).ToShortDateString();
            DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddDays(-1).ToShortDateString();
            //下年度   
            DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddYears(1).ToShortDateString();
            DateTime.Parse(DateTime.Now.ToString("yyyy-01-01")).AddYears(2).AddDays(-1).ToShortDateString();

            //本季度，很多人都会觉得这里难点，需要写个长长的过程来判断。其实不用的，我们都知道一年四个季度，一个季度三个月   
            //首先我们先把日期推到本季度第一个月，然后这个月的第一天就是本季度的第一天了   
            DateTime.Now.AddMonths(0 - ((DateTime.Now.Month - 1) % 3)).ToString("yyyy-MM-01");
            //同理，本季度的最后一天就是下季度的第一天减一   
            DateTime.Parse(DateTime.Now.AddMonths(3 - ((DateTime.Now.Month - 1) % 3)).ToString("yyyy-MM-01")).AddDays(-1).ToShortDateString();
            //下季度，相信你们都知道了。。。。收工   
            DateTime.Now.AddMonths(3 - ((DateTime.Now.Month - 1) % 3)).ToString("yyyy-MM-01");
            DateTime.Parse(DateTime.Now.AddMonths(6 - ((DateTime.Now.Month - 1) % 3)).ToString("yyyy-MM-01")).AddDays(-1).ToShortDateString();
            //上季度   
            DateTime.Now.AddMonths(-3 - ((DateTime.Now.Month - 1) % 3)).ToString("yyyy-MM-01");
            DateTime.Parse(DateTime.Now.AddMonths(0 - ((DateTime.Now.Month - 1) % 3)).ToString("yyyy-MM-01")).AddDays(-1).ToShortDateString();
        }
    }
}