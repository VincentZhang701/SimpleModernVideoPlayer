
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Configuration;
using SimpleModernVideoPlayer.Domain;
using SimpleModernVideoPlayer.Dao;
using SimpleModernVideoPlayer.Utils;

namespace SimpleModernVideoPlayer.Service
{/// <summary>
 /// 静态类 包括
 /// GetUserNameById
 /// GetUserByInfo
 ///UpdateUser
 ///CreateUser
 ///DeleteUser
 /// 这些方法
 /// </summary>
    class RESTClient
    {/// <summary>
     /// HTTPclient 类 静态变量 只读
     /// </summary>
        private static readonly HttpClient client = new HttpClient();
        /// <summary>
        /// 要访问的地址
        /// </summary>
        public Uri uri { get; set; }
        #region 关于用户的操作
        /// <summary>
        /// 静态方法 异步取得用户名字 采用GET方法
        /// </summary>
        /// <param name="ID">用户ID 之后TOSTRING</param>
        /// <returns>用户NAME</returns>
        public static async Task<string> GetUserNameById(int ID)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            var streamTask = client.GetStringAsync("http://39.99.164.175/RestService/" + ID.ToString());
            var usrName = await streamTask;
            return usrName.ToString();
        }
        /// <summary>
        /// 取得User对象 在bottomclick里面添加 了 Taks.run(()=>getuserbyname())方法异步执行
        /// </summary>
        /// <param name="info">名字查询</param>
        /// <returns></returns>
        public static User GetUserByInfo(Info info)
        {
            string rtstr = (string)Request.HttpRequest(info, "POST", "Info");
            return (User)JsonConvert.DeserializeObject(rtstr, typeof(User));

        }
        /// <summary>
        /// 创建一个用户
        /// </summary>
        /// <param name="usr">要创建的用户信息</param>
        /// <returns>创建是否成功</returns>
        public static string CreateUser(User usr)
        {
            if (!UserCheck.CheckUsername(usr.name)) { return "用户名格式不对"; }
            if (!UserCheck.CheckUserpswd(usr.password)) { return "用户密码格式不对"; }
            int result = int.Parse((string)Request.HttpRequest(usr, "PATCH", "CreateUser"));
            return result.ToString();//ID
        }
        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="usr">要更新的用户信息</param>
        /// <returns>更新是否成功</returns>
        public static string UpdateUser(User usr)
        {
            if (!UserCheck.CheckUsername(usr.name)) { return "用户名格式不对"; }
            if (!UserCheck.CheckUserpswd(usr.password)) { return "用户密码格式不对"; }
            string result = (string)Request.HttpRequest(usr, "PUT", "UpdateUser");
            if (result == "true") { return "true"; }
            else { return "false"; }
        }
        /// <summary>
        /// 删除一个用户（通过用户info）
        /// </summary>
        /// <param name="info">用户信息类</param>
        /// <returns>是否成功</returns>
        public static bool DeleteUser(Info info)
        {
            string result = (string)Request.HttpRequest(info, "DELETE", "DeleteUser");
            if(result == "true") { return true; }
            else { return false; }
        }
        #endregion

        #region 关于记录的操作
        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="record">要创建的记录</param>
        /// <returns>是否成功</returns>
        public static bool CreateRecord(Record record)
        {
            string result = (string)Request.HttpRequest(record, "PATCH", "CreateRecord");
            if (result == "true") { return true; }
            else { return false; }
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="record">要更新的记录</param>
        /// <returns>是否成功</returns>
        public static bool UpdateRecord(Record record)
        {
            string result = (string)Request.HttpRequest(record, "PUT", "UpdateRecord");
            if (result == "true") { return true; }
            else { return false; }
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="record">要删除的记录信息 应该包含主键ID和videoname</param>
        /// <returns>是否成功</returns>
        public static bool DeleteRecord(VideoInfo videoInfo)
        {
            string result = (string)Request.HttpRequest(videoInfo, "DELETE", "DeleteRecord");
            if (result == "true") { return true; }
            else { return false; }
        }
        /// <summary>
        /// 更新一个用户的播放记录
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public static bool UpdateRecordList(List<Record> records)
        {
            string result = (string)Request.HttpRequest(records, "PUT", "UpdateRecordList");
            if (result == "true") { return true; }
            else { return false; }
        }
        /// <summary>
        /// 返回某个用户的观看记录 
        /// </summary>
        /// <param name="ID">用户ID</param>
        /// <returns>用户观看记录列表</returns>
        public static List<Record> GetRecordByID(int ID)
        {
            string rtstr = (string)Request.HttpRequest(ID, "POST", "GetRecordByID");
            Console.WriteLine(rtstr);
            return (List<Record>)JsonConvert.DeserializeObject(rtstr, typeof(List<Record>));
        }

        /// <summary>
        /// 精确查找用户的观看记录
        /// </summary>
        /// <param name="videoInfo">记录信息 应该包含主键ID和videoname</param>
        /// <returns>一个Record对象</returns>
        public static Record GetRecordByInfo(VideoInfo videoInfo)
        {
            string rtstr = (string)Request.HttpRequest(videoInfo, "POST", "GetRecordByInfo");
            Console.WriteLine(rtstr);
            return (Record)JsonConvert.DeserializeObject(rtstr, typeof(Record));
        }
        #endregion
    }
}



