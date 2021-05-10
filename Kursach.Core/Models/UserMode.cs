using System.ComponentModel;

namespace ISTraining_Part.Core.Models
{
    /// <summary>
    /// 用户权限
    /// </summary>
    public enum UserMode
    {
        [Description("管理员")]
        Admin = 0,

        [Description("只读")]
        Read = 1,

        [Description("读写")]
        ReadWrite = 2
    }
}
