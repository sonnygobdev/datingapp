using System.Collections.Generic;
using System.Threading.Channels;
using API.BackgrounServices;
using Microsoft.Extensions.Logging;

namespace API.DTOs
{
    public class ImportChannelDto
    {
        public string ProjectId { get; set; }
        public List<string> Files { get; set; }

        public override string ToString(){
             return $"Project: {ProjectId} \n File Count:{Files.Count}  "; 
        }
        
    }
}