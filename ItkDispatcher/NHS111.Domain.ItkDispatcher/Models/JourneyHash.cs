using System;

using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace NHS111.Domain.Itk.Dispatcher.Models
{
    public class Journey : TableEntity
    {
        public Journey()
        {
            PartitionKey = string.Format("{0:yyyy-MM}", DateTime.UtcNow);
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "hash")]
        public string Hash { get; set; }
    }
}
