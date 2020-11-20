using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SimpleModernVideoPlayer.Domain
{/// <summary>
/// 查询时候用到的类，以此类为参数，查询用户信息
/// </summary>
    [DataContract]
    [Serializable]
    class Info
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string password { get; set; }//用户密码
        public override string ToString()
        {
            return "ID:" + ID + ",name:" + name;
        }
    }
}

