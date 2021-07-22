using System.Collections.Generic;

namespace API.Helpers
{
    public static class MessageManager
    {
        public static List<MessageData> GetMessages(){
            return new List<MessageData>(){
                new MessageData{ FileName = "wtw_en_de.xlf", Status=1},
                new MessageData{ FileName = "wtw_en_fr.xlf", Status=1},
                new MessageData{ FileName = "wtw_en_ja.xlf", Status=2},
                new MessageData{ FileName = "wtw_en_ch.xlf", Status=3},
 
            };
        }
    }

    public class MessageData{

        public string FileName { get; set; }
        public int Status { get; set; }
    }

    
}