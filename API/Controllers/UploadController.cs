using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml;
using System.Collections.Generic;
using Newtonsoft.Json;
using API.Helpers;
using Microsoft.Extensions.Caching.Memory;
using System.Xml.Serialization;
using API.DTOs;
using System.Text;
using System;
using Azure.Storage.Blobs;
using API.BackgrounServices;

namespace API.Controllers
{
    public class UploadController:BaseApiController
    {
        private readonly CacheManager _cacheManager;
        private string connectionString = "";

        private ImportProcessingChannel _importProcessingChannel;    

        private Dictionary<string,List<FileDto>> UserFileList = new Dictionary<string, List<FileDto>>();  
        public UploadController(CacheManager cacheManager,ImportProcessingChannel importProcessingChannel)
        {
            _cacheManager = cacheManager;
            _importProcessingChannel = importProcessingChannel;
        }

        [HttpPost("preview")]
        public  ActionResult Preview(List<IFormFile> files){
            
             var translationFiles = new List<FileDto>();

             var translationList = new List<string>();
             var json = string.Empty;
             var translationKeysCount = 0;

             if(files.Count==0)
                return BadRequest("No files selected");

            foreach(var file in files){
                if(file.Length >0){
                    using var stream = file.OpenReadStream();
                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(stream);

                    var translationKeys = xmlDoc.GetElementsByTagName("trans-unit");
                    translationKeysCount = translationKeys.Count;
                    //convert to json
                    json = JsonConvert.SerializeXmlNode(xmlDoc);
                    //convert to string
                    var xliff = ObjectSerializer.Serialize<XmlDocument>(xmlDoc);
                    _cacheManager.Set(file.FileName,xliff);

                    translationFiles.Add(new FileDto{ FileName=file.FileName,
                    TranslationsKeyCount=translationKeysCount});
        
                }

                 
             }
       

            return Ok(translationFiles);
        }



        [HttpPost("import")]
        public async Task<ActionResult> ProcessFiles(FileListDto files){

            var _importChannelDto = new ImportChannelDto();
            var importedFiles = new List<string>();

            _importChannelDto.ProjectId = $"Project-{Guid.NewGuid()}";

            if(files==null) 
               return BadRequest("No filenames specified");
            
            foreach(var fileDto in files.files){
                   
                    string xliff =  _cacheManager.Get(fileDto.FileName);
                   
                    if(string.IsNullOrEmpty(xliff))
                       return BadRequest($"File not found { fileDto.FileName }");
                
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xliff);
                
                    byte[] byteArray = Encoding.ASCII.GetBytes( xliff );
                    MemoryStream stream = new MemoryStream( byteArray );

                    //upload to azure storage                             
                    // BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                    // BlobContainerClient containerClient =  blobServiceClient.GetBlobContainerClient("staging");
                    // BlobClient blobClient = containerClient.GetBlobClient(fileDto.FileName);
                    // await blobClient.UploadAsync(stream);
                    importedFiles.Add(fileDto.FileName);
                   
                   
                    
            }
             _importChannelDto.Files = importedFiles;
            //add message to channel to be process by background service.
            await _importProcessingChannel.AddImportAsync(_importChannelDto);
           
            
           

            return Ok("Files uploaded to storage");

        }

       
        
    }
}