using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ZippingEasy.Common.Utils
{
    public class ConfigNotExistsException : Exception
    {
        public ConfigNotExistsException()
        {
        }

        public ConfigNotExistsException(string message) : base(message)
        {
        }

        public ConfigNotExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConfigNotExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
