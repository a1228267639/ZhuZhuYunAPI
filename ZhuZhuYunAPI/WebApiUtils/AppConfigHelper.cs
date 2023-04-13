/***
*	Title：".NET Core WebApi" 项目
*		主题：程序配置帮助类
*	Description：
*		功能：
*		    1、初始化配置参数
*		    3、获取到不同类型的数据库的连接串
*	Date：2021
*	Version：0.1版本
*	Author：Coffee
*	Modify Recoder：
*/

using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiUtils
{
    public class AppConfigHelper
    {
        #region   基础参数
        //程序配置
        private static ConfigurationManager _appConfig;

        //数据库连接串
        private static string _dbStrCon = string.Empty;

        #endregion



        #region   公有方法

        /// <summary>
        /// 初始化配置参数
        /// </summary>
        /// <param name="configuration">程序配置</param>
        public static void InitAppConfig(ConfigurationManager configuration)
        {
            _appConfig = configuration;
        }

        /// <summary>
        /// 获取到不同类型的数据库的连接串
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <returns>返回当前数据库类型的连接串</returns>
        public static string GetDBStrCon(string dbType)
        {
            string field = $"ConnectionStrings:{dbType}";
            return GetAppConfigContext(_dbStrCon, field);
        }


        #endregion


        #region   私有方法
        /// <summary>
        /// 获取到配置内容
        /// </summary>
        /// <param name="appConfigVar">程序配置变量</param>
        /// <param name="appConfigField">程序配置字段</param>
        /// <returns>返回当前数据库类型的连接串</returns>
        private static string GetAppConfigContext(string appConfigVar, string appConfigField)
        {
            if (string.IsNullOrEmpty(appConfigVar))
            {
                appConfigVar = _appConfig.GetSection(appConfigField).Value;
            }
            return appConfigVar;
        }

        public static string GetToKey()
        {
            //return _appConfig.GetSection("AppSettings:Token").Value;
            return _appConfig.GetSection("JWTConfig:Key").Value;
        }
        public static string GetIssuer()
        {
            return _appConfig.GetSection("JWTConfig:Issuer").Value;
        }
        public static string GetAudience()
        {
            return _appConfig.GetSection("JWTConfig:Audience").Value;
        }
        public static int GetExpires()
        {
            return int.Parse(_appConfig.GetSection("JWTConfig:Expires").Value);
        }
        public static int GetTempRegTime()
        {
            return int.Parse(_appConfig.GetSection("RegConfig:TempRegTime").Value);
        }
        public static int GetRegTime()
        {
            return int.Parse(_appConfig.GetSection("RegConfig:RegTime").Value);
        }
        #endregion

    }//Class_end

}