using Microsoft.Extensions.Caching.Memory;

namespace API.Helpers
{
    public class CacheManager
    {
        public  MemoryCache Cache {get;set;}
        public CacheManager()
        {
            Cache = new MemoryCache(new MemoryCacheOptions{
                    SizeLimit =5000
            });
        }

        public  void Set(string key,string value){
            if(!Cache.TryGetValue(key,out string cacheEntry)) {
                   cacheEntry = value;
                    var cacheEntryOptions = new MemoryCacheEntryOptions{
                          //expire after 1 hour
                          Size=1024,
                          SlidingExpiration = System.TimeSpan.FromHours(1)
                    };

                    Cache.Set(key,cacheEntry,cacheEntryOptions);
             }
        }

        public string Get(string key){
             if(!Cache.TryGetValue(key,out string cacheEntry)) 
                return string.Empty;

             var value = Cache.Get<string>(key);
             
             return value; 
        }

        public int RemoveKey(string key){
            int result=1;
           
            if(!Cache.TryGetValue(key,out string cacheEntry)) 
                result = 0;

             Cache.Remove(key);   
            
             return result;
        }

      

        
    }
}