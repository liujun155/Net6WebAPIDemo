using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Common.Helper
{
    public class LogUtil
    {
        private static readonly ILog log = LogManager.GetLogger("LogRepository", typeof(LogUtil));

        /// <summary>
        /// 组织调用方法-日志信息
        /// </summary>
        /// <returns></returns>
        private static string GetCallerNameAndMsg(string msg)
        {
            var callerMethod = new StackFrame(2, true)?.GetMethod();
            if (callerMethod == null)
            {
                return msg;
            }

            return $"{callerMethod.DeclaringType.FullName }.{callerMethod.Name}-{msg}";
        }

        /// <summary>
        /// 日常日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="obj"></param>
        public static void Info(string msg)
        {
            if (log.IsInfoEnabled && !string.IsNullOrEmpty(msg))
            {
                log.Info(GetCallerNameAndMsg(msg));
            }
        }
        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="obj"></param>
        public static void Warn(string msg)
        {
            if (log.IsWarnEnabled && !string.IsNullOrEmpty(msg))
            {
                log.Warn(GetCallerNameAndMsg(msg));
            }
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="obj"></param>
        public static void Error(string msg)
        {
            if (log.IsErrorEnabled && !string.IsNullOrEmpty(msg))
            {
                log.Error(GetCallerNameAndMsg(msg));
            }
        }
    }

}
