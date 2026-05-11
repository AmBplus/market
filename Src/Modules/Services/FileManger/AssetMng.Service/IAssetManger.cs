using Framework.ResultHelper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FileManger
{
    public interface IAssetManger
    {
        public Task<string> GetProfileBase64Async(string profileName);
        public Task<string> GetProfileAsync(string profileName);
        public Task<string> GetCourseImageLinkAsync(string fileName);
        
        

        public Task<ResultOperation<FileSaveResult>> UpdateOrSetProfilAsync(IFormFile picFormFile , long idUser);
        public Task<ResultOperation<FileSaveResult>> UpdateSetUserFileAsync(IFormFile picFormFile , long idUser , FileTypeEnum FileType);

        //public ResultOperation<FileSaveResult> SaveFile(IFormFile formFile , string path, FileTypeEnum FileType, string FileDestinationType);
        public Task<ResultOperation<FileSaveResult>> UploadImageAsync(IFormFile formFile, string path, ImageType imageDestinationType);

        public Task<ResultOperation<FileSaveResult>> UploadPdfAsync(IFormFile formFile, string path);

        public Task<ResultOperation<FileSaveResult>> UploadVideoAsync(IFormFile formFile, string path);
    }
}
