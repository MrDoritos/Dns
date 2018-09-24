using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Messages.Components
{
    /*
     * 
        public Domain GetAnswer(Question question)
        {
            Domain domain = this;
            Domain lastDomain = domain;
            for (int i = 0; domain != null && (lastDomain = domain) != null && i < question.RawBytes.Length; i++ )
                domain = domain.RetrieveSubDomain(question.RawBytes[i]);
            return lastDomain;
        }
     */
    public class Domain : Record
    {
        public byte[] RawName;
        public string Name { get => Encoding.ASCII.GetString(RawName); }
        public string FQDN { get => GetFQDN(); }
        public Domain Parent { get; private set; }
        private List<Domain> _subDomains;
        public IReadOnlyList<Domain> Subdomains { get => _subDomains.ToList(); }

        public string GetFQDN()
        {
            if (Parent == null) return Name + '.'; else return Name + '.' + Parent.FQDN;
        }

        public Domain(byte[] name, Domain parent) { Parent = parent; RawName = name; _subDomains = new List<Domain>(); }
        public Domain(string name, Domain parent) { Parent = parent; RawName = Encoding.ASCII.GetBytes(name); _subDomains = new List<Domain>(); }
        /// <summary>
        /// Top level domain constructor
        /// </summary>
        public Domain(byte[] name) { RawName = name; _subDomains = new List<Domain>(); }
        public Domain(string name) { RawName = Encoding.ASCII.GetBytes(name); _subDomains = new List<Domain>(); }

        public Domain AddSubdomain(Domain domain)
        {
            domain.Parent = this;
            _subDomains.Add(domain);
            return domain;
        }        
        
        public Domain RetrieveSubDomain(byte[] name)
        {
            int length = name.Length;
            IEnumerable<Domain> selected = new List<Domain>(_subDomains.Where(n => n.RawName.Length == name.Length));
            for (int i = 0; i < length && selected == null; i++)
                selected = selected.Where(n => n.RawName[i] == name[i]);
            return selected.FirstOrDefault();
        }

        public Domain RetrieveSubDomain(byte[][] name)
        {
            int length = name.Length;
            Domain dom = this;
            Domain last = dom;
            for (int i = name.Length - 2; dom != null && (last = dom) != null && i > -1; i--)
                dom = dom.RetrieveSubDomain(name[i]);
            return dom;
        }

        public IEnumerable<Domain> GetDomains(IEnumerable<Domain> selecteddomains, byte domain, int index)
        {
            return selecteddomains.Where(n => n.RawName.Length < index && n.RawName[index] == domain);
        }
    }
}
