using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.ResultHelper { 

/// <summary>
/// اکستنشن هایی جهت راحت کردن استفاده از
/// resultOperation
/// </summary>
public static class UtilityResultExtension
{
    public static string? ToSingleMessage(this List<string> messages)
        {
            StringBuilder stringBuilder = new StringBuilder();  
            foreach(var message in messages)
            {
                stringBuilder.Append(message);
            }
            return stringBuilder.ToString();    
        }
    /// <summary>
    /// ساختن یک نتیجه عملیات موفق به صورت جنریک
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static ResultOperation<T> ToSuccessResult<T>(this T entity)
    {
        return ResultOperation<T>.ToSuccessResult(entity);
    }
    /// <summary>
    /// ساختن یک نتیجه عملیات موفق به صورت جنریک به همراه پیام
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static ResultOperation<T> ToSuccessResult<T>(this T entity, string message)
    {
        return ResultOperation<T>.ToSuccessResult(message, entity);
    }
    public static ResultOperation<T> ToFailedResult<T>(this T entity, string message)
    {
        return ResultOperation<T>.ToFailedResult(message,entity);
    }
    public static ResultOperation<T> ToFailedResult<T>(this T entity, List<string> messages)
    {
        return ResultOperation<T>.ToFailedResult(messages,entity);
    }
    public static ResultOperation<object> ToFailedResult(this object entity, string message)
    {
        return ResultOperation<object>.ToFailedResult(message,entity);
    }
    public static ResultOperation<object> ToFailedResult(this object entity)
    {
        return ResultOperation<object>.ToFailedResult();
    }
}
}