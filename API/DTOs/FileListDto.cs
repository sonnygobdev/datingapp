using System.Collections.Generic;

namespace API.DTOs
{
    public class FileListDto
    {
        public IEnumerable<FileDto> files {get;set;}
    }
}