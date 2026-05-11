using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Helpers
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ExportColumnAttribute : Attribute
    {
        /// <summary>
        /// آیا این ستون در خروجی باشد یا نه
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// عنوان نمایشی در خروجی (Excel / CSV)
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// ترتیب ستون در خروجی
        /// </summary>
        public int Order { get; set; } = int.MaxValue;

        /// <summary>
        /// فرمت خروجی (مثلاً تاریخ یا عدد)
        /// </summary>
        public string? Format { get; set; }

        // پیش‌فرض: اگر DateTime باشد، شمسی خروجی بده
        public bool PersianDate { get; set; } = true;
    }

}
