using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Messages.Components
{
    public interface IRecordRetriever
    {
        Domain RetrieveSubDomain(byte[] vs);
    }
}
