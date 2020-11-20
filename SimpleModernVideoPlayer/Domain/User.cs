
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SimpleModernVideoPlayer.Domain
{/// <summary>
/// 记录用户信息与数据库对接
/// </summary>
    [DataContract]
    [Serializable]
    class User
    {
        [DataMember]
        public int ID { get; set; }//用户ID 自增
        [DataMember]
        public string name { get; set; }//用户账号
        [DataMember]
        public string password { get; set; }//用户密码
        /*[DataMember]
        public List<Record> records { get; set; }//用户观看列表*/
        [DataMember]
        public string avatar { get; set; }//用户头像
        [DataMember]
        public string CreateTime { get; set; }//创建用户的时间
        [DataMember]
        public string LastModifiedTime { get; set; }//用户信息最后修改的时间

        public override string ToString()
        {
            string retString = string.Format("ID:{0},name:{1},password:{2},createtime:{3},lastmodifiedtime:{4},avatar:{5}\r\n",
                ID, name, password, CreateTime, LastModifiedTime, avatar);
            /*string recordtostr = "PlayRecords:\r\n";
            foreach (var rec in records)
            {
                recordtostr += string.Format("videoname:{0} record:{1}times:{2} location:{3} bytes:{4}\r\n",
                    rec.videoname, rec.record.ToString(), rec.times.ToString(), rec.location, rec.bytes.ToString());
            }*/
            return retString ;
        }
    }
}

