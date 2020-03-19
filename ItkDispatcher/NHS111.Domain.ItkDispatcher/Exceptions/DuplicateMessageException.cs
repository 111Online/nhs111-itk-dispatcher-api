using System;

namespace NHS111.Domain.Itk.Dispatcher.Exceptions
{
    public class DuplicateMessageException : Exception
    {
        public DuplicateMessageException(string message) : base(message)
        {
        }
    }
}
