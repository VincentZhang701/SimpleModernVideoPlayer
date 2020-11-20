using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleModernVideoPlayer.Utils
{
    class UserCheck
    {
        public static bool CheckUsername(string userName)
        {
            Regex reg_usr_name = new Regex(@"^.{4,40}$");

            return reg_usr_name.IsMatch(userName);
        }
        public static bool CheckUserpswd(string passWord)
        {   //6到12位的密码 只允许使用大小写和数字 不能使用下划线中文等等
            Regex reg_paswd = new Regex(@"^[A-Za-z0-9]{6,12}$");
            return reg_paswd.IsMatch(passWord);
        }
    }
}
