using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Messages.Components
{
    public class BaseOfBaseRecord : BaseRecord
    {
        public BaseOfBaseRecord(Classes classs, Types type) : base(classs, type) { }
        public BaseOfBaseRecord(Types type) : base(type) { }
        public BaseOfBaseRecord(Types type, uint TTL) : base(type) { this.TTL = TTL; }
        public uint TTL { get; }
    }
}
