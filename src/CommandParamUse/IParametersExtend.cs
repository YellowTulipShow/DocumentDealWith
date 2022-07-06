using System;
using System.Collections.Generic;
using System.Reflection;

namespace CommandParamUse
{
    /// <summary>
    /// 接口: 命令参数
    /// </summary>
    public static class IParametersExtend
    {
        /// <summary>
        /// 写入日志参数
        /// </summary>
        /// <param name="param">数据对象</param>
        /// <param name="logArgs">日志参数队列</param>
        public static void WriteLogArgs(this object param, IDictionary<string, object> logArgs)
        {
            Type modelType = param.GetType();
            foreach (PropertyInfo property in modelType.GetProperties())
            {
                string name = property.Name;
                object value = property.GetValue(param);
                if (value == null)
                {
                    logArgs[$"{name}"] = "<NULL Value>";
                    continue;
                }
                if (property.PropertyType.IsEnum)
                {
                    logArgs[$"{name}"] = value.ToString();
                    continue;
                }
                if (property.PropertyType.IsValueType)
                {
                    logArgs[$"{name}"] = value.ToString();
                    continue;
                }
                if (value is string str)
                {
                    logArgs[$"{name}"] = str;
                    continue;
                }
                if (property.PropertyType.IsArray)
                {
                    logArgs[$"{name}"] = "<IsArray>";
                    continue;
                }
                logArgs[$"{name}.GetType().FullName"] = value.GetType().FullName;
            }
        }
    }
}
