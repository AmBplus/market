using System.Text;

namespace Framework.ResultHelper;

/// <summary>
/// برگرداندن نتیجه عملیات انجام شده - بدون خروجی دیتا
/// </summary>
public class ResultOperation
{
    private ResultOperation()
    {
        Message = new List<string>();
    }

    public bool IsSuccess { get; private set; }
    public List<string>? Message { get; private set; }
    public string MessageSingle => GetMessages();
    public int Status { get; private set; }

    private string GetMessages()
    {
        if (Message == null || Message.Count == 0)
            return "";

        var sb = new StringBuilder();
        foreach (var message in Message)
            sb.Append(message);
        return sb.ToString();
    }

    public static ResultOperation ToSuccessResult(string message) => new()
    {
        IsSuccess = true,
        Message = new List<string> { message },
        Status = 200
    };

    public static ResultOperation ToSuccessResult() => new()
    {
        IsSuccess = true,
        Message = new List<string>(),
        Status = 200
    };

    public static ResultOperation ToFailedResult() => new()
    {
        IsSuccess = false,
        Message = new List<string>()
    };

    public static ResultOperation ToFailedResult(int status) => new()
    {
        IsSuccess = false,
        Message = new List<string>(),
        Status = status
    };

    public static ResultOperation ToFailedResult(string message) => new()
    {
        IsSuccess = false,
        Message = new List<string> { message }
    };

    public static ResultOperation ToFailedResult(string message, int status) => new()
    {
        IsSuccess = false,
        Message = new List<string> { message },
        Status = status
    };

    public static ResultOperation ToFailedResult(List<string> message) => new()
    {
        IsSuccess = false,
        Message = message
    };

    public static ResultOperation ToFailedResult(List<string> message, int status) => new()
    {
        IsSuccess = false,
        Message = message,
        Status = status
    };
}

/// <summary>
/// برگرداندن نتیجه عملیات به همراه دیتای خروجی
/// </summary>
public class ResultOperation<T>
{
    private ResultOperation()
    {
        Message = new List<string>();
    }

    public bool IsSuccess { get; private set; }
    public List<string>? Message { get; private set; }
    public T Data { get; private set; } = default!;
    public string MessageSingle => GetMessages();

    private string GetMessages()
    {
        if (Message == null || Message.Count == 0)
            return "";

        var sb = new StringBuilder();
        foreach (var message in Message)
            sb.Append(message);
        return sb.ToString();
    }

    public static ResultOperation<T> ToSuccessResult(T data) => new()
    {
        Data = data,
        IsSuccess = true,
        Message = new List<string>()
    };

    public static ResultOperation<T> ToSuccessResult(string message, T data) => new()
    {
        Data = data,
        IsSuccess = true,
        Message = new List<string> { message }
    };

    public static ResultOperation<T> ToSuccessResult(List<string> message, T data) => new()
    {
        Data = data,
        IsSuccess = true,
        Message = message
    };

    public static ResultOperation<T> ToFailedResult() => new()
    {
        IsSuccess = false,
        Message = new List<string>()
    };

    public static ResultOperation<T> ToFailedResult(string message) => new()
    {
        IsSuccess = false,
        Message = new List<string> { message }
    };

    public static ResultOperation<T> ToFailedResult(List<string> message) => new()
    {
        IsSuccess = false,
        Message = message
    };

    public static ResultOperation<T> ToFailedResult(string message, T data) => new()
    {
        Data = data,
        IsSuccess = false,
        Message = new List<string> { message }
    };

    public static ResultOperation<T> ToFailedResult(List<string> message, T data) => new()
    {
        Data = data,
        IsSuccess = false,
        Message = message
    };
}

/// <summary>
/// اکستنشن‌های کمکی برای ResultOperation
/// </summary>
public static class ResultOperationExtensions
{
    public static ResultOperation GetResultOperation<T>(this ResultOperation<T> result)
    {
        return result.IsSuccess
            ? ResultOperation.ToSuccessResult(result.MessageSingle!)
            : ResultOperation.ToFailedResult(result.MessageSingle);
    }

    public static string? ToSingleMessage(this List<string> messages)
    {
        var sb = new StringBuilder();
        foreach (var message in messages)
            sb.Append(message);
        return sb.ToString();
    }

    public static ResultOperation<T> ToSuccessResult<T>(this T entity) =>
        ResultOperation<T>.ToSuccessResult(entity);

    public static ResultOperation<T> ToSuccessResult<T>(this T entity, string message) =>
        ResultOperation<T>.ToSuccessResult(message, entity);

    public static ResultOperation<T> ToFailedResult<T>(this T entity, string message) =>
        ResultOperation<T>.ToFailedResult(message, entity);

    public static ResultOperation<T> ToFailedResult<T>(this T entity, List<string> messages) =>
        ResultOperation<T>.ToFailedResult(messages, entity);

    public static ResultOperation<object> ToFailedResult(this object entity, string message) =>
        ResultOperation<object>.ToFailedResult(message, entity);

    public static ResultOperation<object> ToFailedResult(this object entity) =>
        ResultOperation<object>.ToFailedResult();
}
