using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ApiRes
{
    /// <summary>
    /// 接口统一返回格式
    /// </summary>
    public  class ApiResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 返回结果
        /// </summary>
        public object Result { get; set; }
        /// <summary>
        /// 返回信息提示
        /// </summary>
        public string Msg { get; set; }
    }
}
