using System;
using ScrapySharp.Network;
using Microsoft.Extensions.Caching.Memory;

namespace ScrapySharp.Cache
{
    public sealed class WebResourceStorage
    {
        private const string basePath = "_WebResourcesCache";
        private MemoryCache cache;

        public WebResourceStorage()
        {
            Initialize();
        }

        private void Initialize()
        {
            cache = new MemoryCache(new MemoryCacheOptions());
        }

        public void Save(WebResource webResource)
        {
            var cacheItem = new CacheItem<WebResource>(webResource.AbsoluteUrl.ToString(), webResource);
            var absoluteExpiration = new DateTimeOffset(DateTime.UtcNow.AddHours(2));

            cache.Set(cacheItem.Key, cacheItem, absoluteExpiration);
        }

        public bool Exists(string key)
        {
            return cache.TryGetValue(key, out var result);
        }

        private static WebResourceStorage current;

        public static WebResourceStorage Current
        {
            get
            {
                if (current == null)
                    current = new WebResourceStorage();
                return current;
            }
        }
    }

    internal class CacheItem<TValue>
    {
      public CacheItem(string key, TValue value)
      {
        Key = key;
        Value = value;
      }

      public string Key { get; set; }
      public TValue Value { get; set; }
    }
}