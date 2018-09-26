using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNS.Messages.Records;
using DNS.Messages;
using DNS.Messages.Components;

namespace DNS
{
    public class DomainHandler : IRecordRetriever
    {
        private List<Domain> domains;
        public IReadOnlyList<Domain> Domains { get => domains; }
        public DomainHandler() { domains = new List<Domain>(); }

        public Domain AddTLD(Domain domain)
        {
            domains.Add(domain);
            return domain;
        }
                
        public Domain RetrieveSubDomain(byte[][] name)
        {
            int length = name.Length;
            Domain dom = RetrieveSubDomain(name[name.Length - 1]);
            Domain last = dom;
            for (int i = name.Length - 2; dom != null && (last = dom) != null && i > -1; i--)
                dom = dom.RetrieveSubDomain(name[i]);
            return dom;
        }

        public Domain RetrieveSubDomain(byte[] zone)
        {
            int length = zone.Length;
            IEnumerable<Domain> selected = new List<Domain>(domains.Where(n => n.RawName.Length == zone.Length));
            for (int i = 0; i < zone.Length; i++)
            {
                selected = new List<Domain>(selected.Where(n => n.RawName[i] == zone[i]));
            }
            return selected.FirstOrDefault();
        }
    }
}
