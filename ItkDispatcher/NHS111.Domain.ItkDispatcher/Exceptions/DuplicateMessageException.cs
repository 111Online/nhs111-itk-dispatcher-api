using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Domain.Itk.Dispatcher.Exceptions
{
    public class DuplicateMessageException : Exception
    {
        public DuplicateMessageException(string message) : base(message)
        {
        }
    }
}
