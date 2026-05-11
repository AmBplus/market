using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.NumberHelper;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Services.FileManger; 


public class FileSaveResult
{
    public bool? IsCorrectType { get; set; }
    public FileInformation FileInformation { get; set; }
}

public class FileInformation
{

    public string FileName { get; set; }
    public string FileNameWithOutExtension { get; set; }
    public string Path { get; set; }
    public string FileExtension { get; set; }
}


public enum ImageType
{
    Webp = 0,
    Jpeg = 1,
    Jpg = 2,
    png = 3
}
public class ImageTypeExtensionConst
{
    public const string Webp = ".webp";
    public const string Jpeg = ".jpeg";
    public const string Jpg = ".jpg";
    public const string Png = ".png";
    public const string Jfif = ".jfif";
    public const string Gif = ".gif";
    public const string Svg = ".svg";
    public const string Tif = ".tif";
    public const string Tiff = ".tiff";
    public const string Ico = ".ico";
    public const string Svgz = ".svgz";
    public const string Pjpeg = ".pjpeg";
    public const string Pjp = ".pjp";

    public const string WebpMime = "image/webp";
    public const string JpegMime = "image/jpeg";
    public const string PngMime = "image/png";
    public const string GifMime = "image/gif";
    public const string SvgMime = "image/svg+xml";
    public const string TiffMime = "image/tiff";
    public const string IcoMime = "image/x-icon";

    public const string AllImageMimes = WebpMime + "," +
                                JpegMime + "," +
                                PngMime + "," +
                                GifMime + "," +
                                SvgMime + "," +
                                TiffMime + "," +
                                IcoMime;


    public static readonly HashSet<string> ValidImageExtensions = new HashSet<string>
    {
        Webp,
        Jpeg,
        Jpg,
        Png,
        Jfif,
        Gif,
        Svg,
        Tif,
        Tiff,
        Ico,
        Svgz,
        Pjpeg,
        Pjp

    };
}
public class VideoTypeExtensionConst
{
    public const string M4v = ".m4v";
    public const string Mp4 = ".mp4";
    public const string Mkv = ".mkv";
    public const string Mpeg = ".mpeg";
    public const string Avi = ".avi";
    public const string Mov = ".mov";
}

public enum FileTypeEnum :int
{
    Pdf,
    Image,
    Video,
    Rar,
    Zip
}
