namespace PersonnelManageSystem.Utils
{
    /// <summary>
    /// Api调用状态码 
    /// </summary>
    public enum StatusCode
    {
        /// <summary>
        /// 传递的参数无效
        /// 用于ReturnResult.Code字段
        /// </summary>
        InValidParameter = 10000,
        
        /// <summary>
        /// 账号或密码错误
        /// </summary>
        UserOrPasswordNoFond = 10001,
        /// <summary>
        /// 数据库异常
        /// </summary>
        UserDbException = 10002,
        
        /// <summary>
        /// 用户已经登录
        /// </summary>
        UserHasLogin = 10003,
        
        /// <summary>
        /// 无效或没有用户信息
        /// </summary>
        InValidUserInfo = 10004,
        
        /// <summary>
        /// 部门为空
        /// </summary>
        DepartInfoNull = 10005,
        
        /// <summary>
        /// 权限不足
        /// </summary>
        LowAuthority = 10006,
        
        /// <summary>
        /// 账号已经更新
        /// </summary>
        UserHasUpdate = 10007,
        
        /// <summary>
        /// 当你不知道是什么问题时
        /// </summary>
        UnKnowError = 10008,
        
        Success = 200,
    }
}