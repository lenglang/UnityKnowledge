using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
//using System.Threading.Tasks;

namespace LINQ
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //初始化武林高手
            //var masterList = new List<MartialArtsMaster>()
            //{
            //    new MartialArtsMaster() {Id = 1, Name = "黄蓉", Age = 18, Menpai = "丐帮", Kongfu = "打狗棒法", Level = 9},
            //    new MartialArtsMaster() {Id = 2, Name = "洪七公", Age = 70, Menpai = "丐帮", Kongfu = "打狗棒法", Level = 10},
            //    new MartialArtsMaster() {Id = 3, Name = "郭靖", Age = 22, Menpai = "丐帮", Kongfu = "降龙十八掌", Level = 10},
            //    new MartialArtsMaster() {Id = 4, Name = "任我行", Age = 50, Menpai = "明教", Kongfu = "葵花宝典", Level = 1},
            //    new MartialArtsMaster() {Id = 5, Name = "东方不败", Age = 35, Menpai = "明教", Kongfu = "葵花宝典", Level = 10},
            //    new MartialArtsMaster() {Id = 6, Name = "林平之", Age = 23, Menpai = "华山", Kongfu = "葵花宝典", Level = 7},
            //    new MartialArtsMaster() {Id = 7, Name = "岳不群", Age = 50, Menpai = "华山", Kongfu = "葵花宝典", Level = 8},
            //    new MartialArtsMaster() {Id = 8, Name = "令狐冲", Age = 23, Menpai = "华山", Kongfu = "独孤九剑", Level = 10},
            //    new MartialArtsMaster() {Id = 9, Name = "梅超风", Age = 23, Menpai = "桃花岛", Kongfu = "九阴真经", Level = 8},
            //    new MartialArtsMaster() {Id = 10, Name = "黄药师", Age = 23, Menpai = "梅花岛", Kongfu = "弹指神通", Level = 10},
            //    new MartialArtsMaster() {Id = 11, Name = "风清扬", Age = 23, Menpai = "华山", Kongfu = "独孤九剑", Level = 10}
            //};
            //初始化武学
            //var kongfuList = new List<Kongfu>()
            //{
            //    new Kongfu() {Id = 1, Name = "打狗棒法", Power = 90},
            //    new Kongfu() {Id = 2, Name = "降龙十八掌", Power = 95},
            //    new Kongfu() {Id = 3, Name = "葵花宝典", Power = 100},
            //    new Kongfu() {Id = 4, Name = "独孤九剑", Power = 100},
            //    new Kongfu() {Id = 5, Name = "九阴真经", Power = 100},
            //    new Kongfu() {Id = 6, Name = "弹指神通", Power = 100}
            //};
            //查询所有武学级别大于8的武林高手
            //var res = new List<MartialArtsMaster>();
            //foreach (var temp in masterList)
            //{
            //    if (temp.Level > 8)
            //    {
            //        res.Add(temp);
            //    }   
            //}
            //1,使用LINQ做查询( 表达式写法)
            //var res = from m in masterList
            //          //from后面设置查询的集合
            //          where m.Level > 8 && m.Menpai=="丐帮" //通过&&添加并列的条件
            //          //where后面跟上查询的条件
            //          select m;//表示m的结果结合返回
            //2,扩展方法的写法
            //var res = masterList.Where(Test1);
            //var res = masterList.Where(m => m.Level > 8 && m.Menpai=="丐帮" );


            //3,LINQ 联合查询
            //取得所学功夫的杀伤力大于90 的武林高手
            //var res = from m in masterList
            //    from k in kongfuList
            //    where m.Kongfu == k.Name && k.Power > 90
            //    //select new {master = m, kongfu = k};
            //    select m;
            //扩展方法用法
            //var res =
            //    masterList.SelectMany(m => kongfuList, (m, k) => new {master = m, kongfu = k})
            //        .Where(x => x.master.Kongfu == x.kongfu.Name && x.kongfu.Power>90 );

            //4,对查询结果做排序 orderby (descending)
            //var res = from m in masterList
            //          //from后面设置查询的集合
            //          where m.Level > 8 && m.Menpai == "丐帮" //通过&&添加并列的条件
            //          //orderby m.Age descending
            //          orderby m.Level,m.Age //按照多个字段进行排序，如果字段的属性相同，就按照第二个属性排序
            //          //where后面跟上查询的条件
            //          select m;//表示m的结果结合返回

            //var res = masterList.Where(m => m.Level > 8 && m.Menpai == "丐帮").OrderBy(m => m.Age);
            //var res = masterList.Where(m => m.Level > 8).OrderBy(m => m.Level).ThenBy(m => m.Age);


            //5,join on 集合联合
            //var res = from m in masterList
            //    join k in kongfuList on m.Kongfu equals k.Name
            //    where k.Power>90
            //    select new {master = m, kongfu = k};

            //6,分组查询 into groups (把武林高手按照所学功夫分类，看一下那个功夫修炼的人数最多)
            //var res = from k in kongfuList
            //    join m in masterList on k.Name equals m.Kongfu
            //    into  groups
            //    orderby groups.Count()
            //    select new {kongfu = k, count = groups.Count() };
            

            //7,按照自身字段分组 group
            //var res = from m in masterList
            //    group m by m.Kongfu
            //    into g
            //    select new {count = g.Count(), key = g.Key};//g.Key Key表示是按照那个属性分的组

            //8,量词操作符 any all 判断集合中是否满足某个条件
            //bool res  = masterList.Any(m => m.Menpai == "长留");
            //Console.WriteLine(res);
            //bool res = masterList.All(m => m.Menpai == "丐帮");
            //Console.WriteLine(res);


            //foreach (var temp in res)
            //{
            //    Console.WriteLine(temp);
            //}
        }

        //过滤方法
        static bool Test1(MartialArtsMaster master)
        {
            if (master.Level > 8) return true;
            return false;
        }
    }
}
