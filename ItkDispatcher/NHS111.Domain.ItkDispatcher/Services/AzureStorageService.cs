using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NHS111.Domain.Itk.Dispatcher.Models;

namespace NHS111.Domain.Itk.Dispatcher.Services
{
    public class NullAzureStorageService
        : IAzureStorageService {
        public int AddHash(Journey journey) {
            return 0;
        }

        public Journey GetHash(string journeyId) {
            return new Journey();
        }
    }


    public interface IAzureStorageService
    {
        int AddHash(Journey journey);
        Journey GetHash(string journeyId);
    }
}
