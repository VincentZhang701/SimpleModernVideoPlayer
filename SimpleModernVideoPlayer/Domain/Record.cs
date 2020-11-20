using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SimpleModernVideoPlayer.Domain
{/// <summary>
 /// 记录用户观看的某一条记录 包括视频名称、播放进度等信息
 /// </summary>
    [DataContract]
    [Serializable]
    class Record
    {
        [DataMember]
        public int UID { get; set; }//用户ID这是谁的播放记录
        [DataMember]
        public string videoname { get; set; }//文件名称
        [DataMember]
        public string record { get; set; }//播放进度
        [DataMember]
        public string location { get; set; }//文件路径
        [DataMember]
        public int bytes { get; set; }//文件大小      
        [DataMember]
        public string LastOpened { get; set; } //上次打开的时间

        public override string ToString()
        {
            string retString = string.Format("UID:{0},Videoname:{1},record:{2},LastOpened:{3},location:{4},bytes:{5}",
                UID, videoname, record, LastOpened, location, bytes);
            return retString;
        }
    }
}