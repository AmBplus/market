using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.ResultHelper
{
    /// <summary>
    /// برگرداندن نتیجه عملیات انجام شده
    /// </summary>
    public class ResultOperation
    {
        private ResultOperation()
        {
            Message = new List<string>();
        }
        public bool IsSuccess { get; private set; }
        public List<string>? Message { get; private set; }
        public string MessageSingle { get => GetMessages(); }

        private string GetMessages()
        {
            if (Message == null || Message.Count == 0)
            {
                return "";
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var message in Message)
                {
                    stringBuilder.Append(message);
                }
                return stringBuilder.ToString();
            }
        }

        public int Status { get; private set; }

        public static ResultOperation ToSuccessResult(string message)
        {
            return new ResultOperation()
            {
                IsSuccess = true,
                Message = new List<string>() { message },
                Status = 200
            };
        }

        public static ResultOperation ToSuccessResult()
        {
            return new ResultOperation()
            {
                IsSuccess = true,
                Message = new List<string>(),
                Status = 200
            };
        }
        public static ResultOperation ToFailedResult()
        {
            return new ResultOperation()
            {
                IsSuccess = false,
                Message = new List<string>() { },
            };
        }
        public static ResultOperation ToFailedResult(int status)
        {
            return new ResultOperation()
            {
                Message = new List<string>() { },
                IsSuccess = false,
                Status = status
            };
        }
        public static ResultOperation ToFailedResult(string message)
        {
            return new ResultOperation()
            {
                IsSuccess = false,
                Message = new List<string>() { message },
            };
        }
        public static ResultOperation ToFailedResult(string message, int status)
        {
            return new ResultOperation()
            {
                IsSuccess = false,
                Message = new List<string>() { message },
                Status = status
            };
        }
        public static ResultOperation ToFailedResult(List<string> message)
        {
            return new ResultOperation()
            {
                IsSuccess = false,
                Message = message,
            };
        }
        public static ResultOperation ToFailedResult(List<string> message, int status)
        {
            return new ResultOperation()
            {
                IsSuccess = false,
                Message = message,
                Status = status
            };
        }

    }


}