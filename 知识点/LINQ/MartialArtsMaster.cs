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
namespace LINQ
{
    /// <summary>
    /// 武林高手
    /// </summary>
    class MartialArtsMaster {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Menpai { get; set; }
        public string Kongfu { get; set; }
        public int Level { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}, Age: {2}, Menpai: {3}, Kongfu: {4}, Level: {5}", Id, Name, Age, Menpai, Kongfu, Level);
        }
    }
}
