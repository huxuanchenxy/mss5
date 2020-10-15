using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
namespace MSS.API.Common
{
    public enum Code
    {
        [Description("接口调用成功")]
        Success = 0,
        [Description("接口调用失败")]
        Failure = 1,
        [Description("数据已存在")]
        DataIsExist = 2,
        [Description("数据不存在")]
        DataIsnotExist = 3,
        // 向不可添加子节点的节点添加节点
        [Description("数据校验失败")]
        CheckDataRulesFail= 4,
        [Description("绑定用户存在冲突")]
        BindUserConflict = 5,
        [Description("导入失败")]
        ImportError = 6
    }
    public class ApiResult
    {
        public Code code { get; set; }
        public string msg { get; set; }
        public object data { get; set; }
    }

    #region 常量
    public enum IsDeleted
    {
        no,
        yes
    }
    public class Const
    {
        public static int PAGESIZE = 10;

        //public static string REDISConn_AUTH;
        public static string REDIS_AUTH_KEY_ACTIONINFO = "Auth_ActionInfo";
        public static string REDIS_AUTH_KEY_ROLEACTION = "Auth_RoleAction";
        public static string REDIS_AUTH_KEY_USER = "Auth_User";

        public static int LINE = 3;
    }

    public static class FilePath
    {
        /// <summary>
        /// 基础绝对路径
        /// </summary>
        public static string BASEFILE = "D:/bin/eqp/";
        public static string SHAREFILE = "File/";
        public static string CSVPATH = @"\\10.89.36.152\uploads\";
        public static string PMPATH = "D:/services/uploadFile/PM/";
    }

    public enum UploadShowType
    {
        Cascader=1,
        List=2
    }

    /// <summary>
    /// 重要角色，不可删除，但可显示供用户配置
    /// </summary>
    public enum KeyRole
    {
        Dispatcher=24
    }
    #endregion

    #region 字典
    public static class MyDictionary
    {
        public enum SystemResource
        {
            EqpType = 25,
            Eqp=26,
            Expert=27,
            MaintainReg=28,
            EmergencyPlan=57,
            ConstructionPlan=109,
            TroubleReport=136,
            EqpRepair=190
        }

        public enum EqpHistoryType
        {
            Install = 39,
            MediumPM = 40,
            MajorPM = 41,
            TroublePM = 46,
            FirstWork = 43,
            SecondWork = 44,
            Change = 157,
            Maintenance = 189,
            Expiration = 45,
            AllChange = 205
        }

        public enum HealthType
        {
            Trouble= 199,
            PM= 200,
            Time= 201,
            MediumPM= 202,
            MajorPM= 203,
            EqpReplace= 204
        }

        public const double HEATHFULLVAL = 100;
        //程序自动更新数据库表时的创建更新人，目前仅在每天健康度衰减时使用
        public const int SYSTEM = 0;
    }

    public enum OrgType
    {
        Company = 1,
        Department = 2,
        Team = 3
    }
    #endregion


}
