using FileTypeChecker;
using FileTypeChecker.Abstracts;
using Framework.Resources;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Http;
using Services.FileManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetMng.Service
{
    public static class AssetHelper
    {
        public static bool CheckIsValidFile(this IFormFile formFile)
        {
            if (formFile == null || formFile.Length <= 0)
                return false;

            return true;
        }
        /// <summary>
        /// This Method Check The IFormFile Is Image And If It Is Image Return Image Type
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns>Return ResultOperation With Image Type If Is Valid</returns>
        public static ResultOperation<string> IsImage(this IFormFile formFile)
        {
            // Check Simply By FileName Extension Name
            var fileExtension = Path.GetExtension(formFile.FileName).ToLowerInvariant();
            if (!ImageTypeExtensionConst.ValidImageExtensions.Contains(fileExtension.ToLowerInvariant()))
            {
                return ResultOperation<string>.ToFailedResult();
            }
            // Check By Signature Of File
            using var fileStream = formFile.OpenReadStream();
            IFileType fileType = FileTypeValidator.GetFileType(fileStream);
            fileStream.Position = 0;
            var fileTypeExtension = "." + fileType.Extension;
      
            if (!ImageTypeExtensionConst.ValidImageExtensions.Contains(fileTypeExtension.ToLowerInvariant()))
            {
                return ResultOperation<string>.ToFailedResult();
            }

            return fileTypeExtension.ToSuccessResult();     

        }
    }
}
