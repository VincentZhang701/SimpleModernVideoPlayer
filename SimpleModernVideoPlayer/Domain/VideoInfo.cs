using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SimpleModernVideoPlayer.Domain
{/// <summary>
/// 用于删除记录的查询类
/// </summary>
///  
    [DataContract]
    [Serializable]
    class VideoInfo
    {
        [DataMember]
        public int UID { get; set; }
        [DataMember]
        public string videoname { get; set; }
    }
}
