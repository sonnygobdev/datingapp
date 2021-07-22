using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using API.DTOs;
using Microsoft.Extensions.Logging;

namespace API.BackgrounServices
{
    public class ImportProcessingChannel
    {
        private const int MaxMessagesInChannel = 100;

        private readonly Channel<ImportChannelDto> _channel;
        private readonly ILogger<ImportProcessingChannel> _logger;

        public ImportProcessingChannel(ILogger<ImportProcessingChannel> logger)
        {   
             var options = new BoundedChannelOptions(MaxMessagesInChannel){
                 FullMode = BoundedChannelFullMode.Wait
             };


            _channel = Channel.CreateBounded<ImportChannelDto>(options);
            _logger = logger;
        }

          public async Task<bool> AddImportAsync(ImportChannelDto importChannelDto, CancellationToken ct = default)
        {
            while (await _channel.Writer.WaitToWriteAsync(ct) && !ct.IsCancellationRequested)
            {
                if (_channel.Writer.TryWrite(importChannelDto))
                {
                    Log.ImportChannelMessageWritten(_logger, importChannelDto.ToString());

                    return true;
                }
            }

            return false;
        }

        public IAsyncEnumerable<ImportChannelDto> ReadAllAsync(CancellationToken ct = default) =>
            _channel.Reader.ReadAllAsync(ct);

        public bool TryCompleteWriter(Exception ex = null) => _channel.Writer.TryComplete(ex);

        internal static class EventIds
        {
            public static readonly EventId ChannelMessageWritten = new EventId(100, "ImportChannelMessageWritten");
        }

        private static class Log
        {
            private static readonly Action<ILogger, string, Exception> _importChannelMessageWritten = LoggerMessage.Define<string>(
                LogLevel.Information,
                EventIds.ChannelMessageWritten,
                "Import data {importData} was written to the channel.");

            public static void ImportChannelMessageWritten(ILogger logger, string importData)
            {
                _importChannelMessageWritten(logger, importData, null);
            }
      }
     }
    }