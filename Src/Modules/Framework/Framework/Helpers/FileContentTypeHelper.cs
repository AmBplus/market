using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Helpers;

public static class FileContentTypeHelper
{
    private static readonly Dictionary<string, string> ContentTypeMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        // Images
        { ".jpg", "image/jpeg" },
        { ".jpeg", "image/jpeg" },
        { ".png", "image/png" },
        { ".gif", "image/gif" },
        { ".bmp", "image/bmp" },
        { ".webp", "image/webp" },
        { ".svg", "image/svg+xml" },
        { ".tiff", "image/tiff" },
        // Documents
        { ".pdf", "application/pdf" },
        { ".doc", "application/msword" },
        { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
        { ".xls", "application/vnd.ms-excel" },
        { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
        { ".ppt", "application/vnd.ms-powerpoint" },
        { ".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
        { ".txt", "text/plain" },
        { ".csv", "text/csv" },
        { ".rtf", "application/rtf" },

        // Audio
        { ".mp3", "audio/mpeg" },
        { ".wav", "audio/wav" },
        { ".wma", "audio/x-ms-wma" },
        { ".ogg", "audio/ogg" },
        { ".m4a", "audio/mp4" },

        // Video
        { ".mp4", "video/mp4" },
        { ".avi", "video/x-msvideo" },
        { ".mov", "video/quicktime" },
        { ".wmv", "video/x-ms-wmv" },
        { ".mkv", "video/x-matroska" },

        // Compressed
        { ".zip", "application/zip" },
        { ".rar", "application/x-rar-compressed" },
        { ".7z", "application/x-7z-compressed" },
        { ".gz", "application/gzip" },
        { ".tar", "application/x-tar" },

        // Code and Web
        { ".html", "text/html" },
        { ".htm", "text/html" },
        { ".css", "text/css" },
        { ".js", "application/javascript" },
        { ".json", "application/json" },
        { ".xml", "application/xml" },

        // Others
        { ".exe", "application/octet-stream" },
        { ".bin", "application/octet-stream" }
    };
    public static string GetContentType(this string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            return "";

        string extension = Path.GetExtension(fileName);

        if (string.IsNullOrEmpty(extension))
            return "";

        return ContentTypeMappings.TryGetValue(extension, out var contentType)
            ? contentType
            : "application/octet-stream";
    }
}
