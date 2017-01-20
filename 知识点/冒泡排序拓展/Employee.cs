using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

/// <summary>
/// 作者:siki
/// 微信公众账号：devsiki
/// QQ:804632564
/// 请关注微信公众号，关注最新的视频和文章教程信息！O(∩_∩)O
/// </summary>
namespace 冒泡排序拓展
{
    class Employee {
        public string Name { get; private set; }
        public int Salary { get; private set; }

        public Employee(string name, int salary)
        {
            this.Name = name;
            this.Salary = salary;
        }
        //如果e1大于e2的话，返回true，否则返回false
        public static bool Compare(Employee e1, Employee e2)
        {
            if (e1.Salary > e2.Salary) return true;
            return false;
        }

        public override string ToString()
        {
            return Name + ":" + Salary;
        }
    }
}
