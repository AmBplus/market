using AssetMng.Service;
using FileTypeChecker;
using FileTypeChecker.Abstracts;
using Framework.AppPathHelper;
using Framework.Resources;
using Framework.ResultHelper;
using Framework.Settings.AppSettings;
using Framework.TextHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;


namespace Services.FileManger
{
    public class AssetMangerToDisk : IAssetManger
    {
        public AssetMangerToDisk(ILogger<AssetMangerToDisk> logger
            , IApplicationPathHelper filePathHelper 
            , ApplicationSettings settings
           )
        {
            Logger = logger;
            FilePathHelper = filePathHelper;
            Settings = settings;
            UploadUrl = settings.AvatarUploadUrl;
        }

        public ILogger<AssetMangerToDisk> Logger { get; }
        public IApplicationPathHelper FilePathHelper { get; }
        public ApplicationSettings Settings { get; }
        public HttpContext Context { get; }

        public async Task<string> GetProfileBase64Async(string fileName)
        {
         if(string.IsNullOrWhiteSpace(fileName))
            {
                return string.Empty;
            }
            var relativeFilePath = Path.Combine(AssetsPath.AvatarPath);
            var filePath = Path.Combine(FilePathHelper.WebRootPath, relativeFilePath , fileName);

                if(File.Exists(filePath))
            {
                // Read the image file into a byte array
                byte[] imageBytes = await File.ReadAllBytesAsync(filePath);

                // Convert the byte array to a Base64 string
                string base64String = Convert.ToBase64String(imageBytes);

                return base64String;
            }
            Logger.LogError($"image not exits {fileName} \n with full path : {filePath}");
            return string.Empty;
        }
        public async Task<string> GetProfileAsync(string profileName)
        {
            if (string.IsNullOrWhiteSpace(profileName))
            {
                return string.Empty;
                
            }
            //var filePath = Path.Combine(FilePathHelper.WebRootPath, relativeFilePath, profileName);
           
            return $"{Settings.AvatarBaseRootPath}/{profileName}";

            //var profilePath = $"{Settings.AssetBasePath}/{AssetsPath.AvatarPath}/{profileName}";
            //return  profilePath;
        }
        public async Task<string> GetCourseImageLinkAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                  return "/img/backgrounds/blank_course.webp";
            }
            var relativeFilePath = $"{Settings.AssetBasePath}/{AssetsPath.CourseImages}/{fileName}";
            return relativeFilePath;
        }
        private  readonly string Token = "326677C0D0A3"; // توکن ثابت مطابق کنترلر
        private readonly string UploadUrl; // جایگزین با آدرس واقعی API
        public async Task<ResultOperation<FileSaveResult>> UpdateOrSetProfilAsync(IFormFile formFile, long idUser)
        {
            try
            {
                if (!formFile.CheckIsValidFile())
                    return ResultOperation<FileSaveResult>.ToFailedResult(ErrorMessages.NotValidFile);


                var result = formFile.IsImage();

                if (!result.IsSuccess)
                {
                    return ResultOperation<FileSaveResult>.ToFailedResult(string.Format(ErrorMessages.OnlySeletedFileAllowedWithParam, $"{ImageTypeExtensionConst.Jpg}{ImageTypeExtensionConst.Jpeg}{ImageTypeExtensionConst.Png}{ImageTypeExtensionConst.Webp}"));
                }

                var fileExtension = result.Data;
                var fileName = $"{TextUtilHelper.GenerateFileName(idUser.ToString())}{fileExtension}";
                //var filePath = Path.Combine(FilePathHelper.WebRootPath, AssetsPath.AvatarPath);
                //var fullPath = GenerateFilePath(filePath, fileName);
                //// Path  Of File 

                //await SaveImage(formFile, fullPath, fileExtension);

                // Create Response 
                // Set Stream Postion At Start
                byte[] imageBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await formFile.CopyToAsync(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }
                // ایجاد شیء درخواست
                var request = new UploadImageRequest
                {
                    token = Token,
                    imageName = fileName,
                    image = imageBytes
                };


                // ارسال درخواست به کنترلر
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsJsonAsync(UploadUrl, request);

                    // بررسی پاسخ
                    if (response.IsSuccessStatusCode)
                    {
                        var resultUpload = await response.Content.ReadFromJsonAsync<UploadResponse>();
                        Console.WriteLine(result?.Message); // یا لاگ کردن پیام موفقیت
                    }
                    else
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        throw new Exception($"خطا در آپلود تصویر: {errorMessage}");
                    }
                }
                var fileSaveResult = CreateFileSaveResult(fileName);
                return fileSaveResult.ToSuccessResult();
                // Check Simply By FileName Extension Name


                throw new Exception();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return ResultOperation<FileSaveResult>.ToFailedResult(ErrorMessages.ErrorOccuredOnUploadFile);
            }
        }

        public async Task<ResultOperation<FileSaveResult>> UploadImageAsync(IFormFile formFile, string path, ImageType imageDestinationType)
        {
            try
            {

                if (!formFile.CheckIsValidFile())
                    return ResultOperation<FileSaveResult>.ToFailedResult(ErrorMessages.NotValidFile);


                var result = formFile.IsImage();

                if (!result.IsSuccess)
                {
                    return ResultOperation<FileSaveResult>.ToFailedResult(string.Format(ErrorMessages.OnlySeletedFileAllowedWithParam, $"{ImageTypeExtensionConst.Jpg}{ImageTypeExtensionConst.Jpeg}{ImageTypeExtensionConst.Png}{ImageTypeExtensionConst.Webp}"));
                }

                var fileExtension = result.Data;
                var fileName = $"{TextUtilHelper.GenerateFileName()}{fileExtension}";
                // Set Stream Postion At Start
                // New FileName

                var filePath_out = GenerateFilePath(path, fileName);
                // Path  Of File 

                await SaveImage(formFile, filePath_out, fileExtension);

                // Create Response 
                
                var fileSaveResult =   CreateFileSaveResult(fileName) ;
                return fileSaveResult.ToSuccessResult();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return ResultOperation<FileSaveResult>.ToFailedResult(ErrorMessages.ErrorOccuredOnUploadFile);
            }


        }

        public async Task<ResultOperation<FileSaveResult>> UploadPdfAsync(IFormFile formFile, string path)
        {
            try
            {
                // Check if the directory exists, if not, create it
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (formFile == null || formFile.Length <= 0)
                    return ResultOperation<FileSaveResult>.ToFailedResult("No file selected.");

                var fileExtension = Path.GetExtension(formFile.FileName).ToLowerInvariant();
                if (!fileExtension.Equals(".pdf", StringComparison.InvariantCultureIgnoreCase))
                    return ResultOperation<FileSaveResult>.ToFailedResult("Only pdf files are allowed.");


                using var fileStream = formFile.OpenReadStream();
                var isRecognizableType = FileTypeValidator.IsTypeRecognizable(fileStream);

                if (!isRecognizableType)
                    return ResultOperation<FileSaveResult>.ToFailedResult("Unknown file");


                IFileType fileType = FileTypeValidator.GetFileType(fileStream);
                if (!fileType.Extension.Equals("pdf", StringComparison.InvariantCultureIgnoreCase))
                    return ResultOperation<FileSaveResult>.ToFailedResult("FiletypeChecker: Only pdf files are allowed.");

                var fileName = $"{TextUtilHelper.GenerateFileName()}.pdf";
                var filePath_out = Path.Combine(path, fileName);


                using var fileStreamWebp = new FileStream(Path.Combine(path, fileName), FileMode.Create);
                await formFile.CopyToAsync(fileStreamWebp);

                var fileInformation = new FileInformation()
                {
                    FileName = fileName,
                    FileNameWithOutExtension = Path.GetFileNameWithoutExtension(fileName),
                    Path = filePath_out,
                    FileExtension = Path.GetExtension(fileName)
                };
                var fileSaveResult = new FileSaveResult() { IsCorrectType = true, FileInformation = fileInformation };
                return fileSaveResult.ToSuccessResult();

            }
            catch (Exception ex)
            {
                return ResultOperation<FileSaveResult>.ToFailedResult($"Error uploading pdf file: {ex.Message}");
            }
        }

        public async Task<ResultOperation<FileSaveResult>> UploadVideoAsync(IFormFile formFile, string path)
        {
            try
            {

                // Check Simply By FileName Extension Name
                var fileExtension = Path.GetExtension(formFile.FileName).ToLowerInvariant();
                if (fileExtension is not
                    (VideoTypeExtensionConst.Mov or
                    VideoTypeExtensionConst.Avi or VideoTypeExtensionConst.Mpeg or
                    VideoTypeExtensionConst.M4v or VideoTypeExtensionConst.Mp4 or
                    VideoTypeExtensionConst.Mkv
                    ))
                    return ResultOperation<FileSaveResult>
                        .ToFailedResult
                        (string.Format(
                            ErrorMessages.OnlySeletedFileAllowedWithParam,
                            $"{VideoTypeExtensionConst.Mov},{VideoTypeExtensionConst.Avi}," +
                            $"{VideoTypeExtensionConst.Mpeg},{VideoTypeExtensionConst.M4v}" +
                            $"{VideoTypeExtensionConst.Mp4},{VideoTypeExtensionConst.Mkv}"));

                using var fileStream = formFile.OpenReadStream();
                // Check By Signature Of File
                IFileType fileType = FileTypeValidator.GetFileType(fileStream);
                var fileTypeExtension = "." + fileType.Extension;
                if (fileTypeExtension.ToLowerInvariant() is not (VideoTypeExtensionConst.Mov or
                    VideoTypeExtensionConst.Avi or VideoTypeExtensionConst.Mpeg or
                    VideoTypeExtensionConst.M4v or VideoTypeExtensionConst.Mp4 or
                    VideoTypeExtensionConst.Mkv))
                {
                    return ResultOperation<FileSaveResult>
               .ToFailedResult
               (string.Format(
                   ErrorMessages.OnlySeletedFileAllowedWithParam,
                   $"{VideoTypeExtensionConst.Mov},{VideoTypeExtensionConst.Avi}," +
                   $"{VideoTypeExtensionConst.Mpeg},{VideoTypeExtensionConst.M4v}," +
                   $"{VideoTypeExtensionConst.Mp4},{VideoTypeExtensionConst.Mkv}"));
                }


                var fileName = $"{TextUtilHelper.GenerateFileName()}{fileExtension}";
                var filePath_out = Path.Combine(path, fileName);

                // Check if the directory exists, if not, create it
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using var fileStreamWebp = new FileStream(Path.Combine(path, fileName), FileMode.Create);
                await formFile.CopyToAsync(fileStreamWebp);
                // Create Response 
                var fileInformation = new FileInformation()
                {
                    FileName = fileName,
                    FileNameWithOutExtension = Path.GetFileNameWithoutExtension(fileName),
                    Path = filePath_out,
                    FileExtension = Path.GetExtension(fileName)
                };
                var fileSaveResult = new FileSaveResult() { IsCorrectType = true, FileInformation = fileInformation };
                return fileSaveResult.ToSuccessResult();
                //return ResultOperation<FileSaveResult>.ToSuccessResult(fileSaveResult);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                return ResultOperation<FileSaveResult>.ToFailedResult(ErrorMessages.ErrorOccuredOnUploadFile);
            }

        }

        
       /// <summary>
        /// supported video , pdf , image
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="idUser"></param>
        /// <param name="fileTypeEnum"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<ResultOperation<FileSaveResult>> UpdateSetUserFileAsync(IFormFile formFile, long idUser, FileTypeEnum fileTypeEnum)
        {
            if (idUser <=0)
            {
                throw new NotSupportedException();
            }
            var path = $"{FilePathHelper.WebRootPath}/{AssetsPath.userFile}/{idUser.ToString()}";
            if(fileTypeEnum == FileTypeEnum.Pdf)
            {
                return  await UploadPdfAsync(formFile, path);
            }
            else if (fileTypeEnum == FileTypeEnum.Video)
            {
                return await UploadVideoAsync(formFile, path);
            }
            else if (fileTypeEnum == FileTypeEnum.Image)
            {
                return await UploadImageAsync(formFile, path , ImageType.Webp);
            }
            else
            {
                throw new NotSupportedException();
            }
            
        }

       
        
        private async Task SaveImage(IFormFile formFile, string path, string fileExtension)
        {

            // If File Are Webp Save It Without Change
            //if (fileExtension.Equals(ImageTypeExtensionConst.Webp, StringComparison.InvariantCultureIgnoreCase))
            //{
            //    using var fileStreamWebp = new FileStream(path, FileMode.Create);
            //    await formFile.CopyToAsync(fileStreamWebp);
            //}
            //// If File Not A Webp ,So Save As Webp 
            //else
            //{
            //    using var image = await SixLabors.ImageSharp.Image.LoadAsync(formFile.OpenReadStream());
            //    var webpEncoder = new WebpEncoder();
            //    // Create a new image with the Rgba32 pixel format
            //    using var rgba32Image = image.CloneAs<Rgba32>();
            //    using var outputStream = new FileStream(path, FileMode.Create);
            //    await webpEncoder.EncodeAsync(rgba32Image, outputStream);
            //}
            using var fileStreamWebp = new FileStream(path, FileMode.Create);
            await formFile.CopyToAsync(fileStreamWebp);
        }


        private FileSaveResult CreateFileSaveResult(string path)
        {

            // Create Response 
            var fileInformation = new FileInformation()
            {
                FileName = path,
                FileNameWithOutExtension = Path.GetFileNameWithoutExtension(path),
                Path = path,
                FileExtension = Path.GetExtension(path)
            };
            var fileSaveResult = new FileSaveResult() { IsCorrectType = true, FileInformation = fileInformation };
            return fileSaveResult;
        }

        private string GenerateFilePath(string basePath, string filePath)
        {
            var filePath_out = $"{basePath}/{filePath}";
            // Check Exit Or Create Path
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            return filePath_out;
        }


    }
}
