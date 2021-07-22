using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using API.DTOs;
using API.Helpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.BackgrounServices
{
    public class ImportProcessingService : BackgroundService
    {

        private readonly ILogger<ImportProcessingService> _logger;
        private readonly ImportProcessingChannel _importProcessingChannel;

        public ImportProcessingService(ILogger<ImportProcessingService> logger, 
        ImportProcessingChannel importProcessingChannel)
        {
            _logger = logger;
            _importProcessingChannel = importProcessingChannel;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var importData in _importProcessingChannel.ReadAllAsync(stoppingToken))
            {
                if(importData==null)
                    _logger.Log(LogLevel.Information,EventIds.WaitingMessage,"Channel empty");
                try
                {
                    foreach(var file in importData.Files){
                        //read from blob storage
                        //process the file
                        //structure the file to be read by the ImportExport Service
                        //move the file to the processed container
                        //delete the source file
                        
                        _logger.Log(LogLevel.Information,EventIds.StartedProcessing,$"Processing {file}");
                        await Task.Delay(10000);
                       _logger.Log(LogLevel.Information,EventIds.ProcessedMessage,$"Finished processing {file}");
                      

                    }
                    await UpdateImportStatus(importData);
                   
                }catch(Exception e){
                    
                    _logger.Log(LogLevel.Error,EventIds.StoppedProcessing,e.Message);
                    
                }
              
            }
        }

        private static async Task<string> UpdateImportStatus(ImportChannelDto importChannelDto)
        {
            try
            {
                   
                using (HttpClient _client = new HttpClient())
                {
                    foreach(var file in importChannelDto.Files){
                        var messageData = new MessageData{
                             FileName = file,
                             Status = 1
                        };
                        HttpContent httpContent = new StringContent(JsonSerializer.Serialize(messageData), 
                        Encoding.UTF8,"application/json");
                   
                        HttpResponseMessage _response = 
                        await _client.PostAsync(new Uri("https://localhost:5001/api/messenger/postmessage"), httpContent);

                    }
                   
                   
                    
                    string _content = "Request Complete";
                    return _content;
                }
            }catch(Exception e)
            {
                throw e;
            }
            
        }

        internal static class EventIds
        {
            public static readonly EventId StartedProcessing = new EventId(100, "StartedProcessing");
            public static readonly EventId ProcessorStopping = new EventId(101, "ProcessorStopping");
            public static readonly EventId StoppedProcessing = new EventId(102, "StoppedProcessing");
            public static readonly EventId ProcessedMessage = new EventId(110, "ProcessedMessage");

            public static readonly EventId WaitingMessage = new EventId(120, "WaitingMessage");

        }
    }
}