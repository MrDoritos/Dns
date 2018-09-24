using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpCodes = DNS.Messages.DnsMessage.OpCodes;
using ResponseCodes = DNS.Messages.DnsMessage.ResponseCodes;
using DNS.Messages;

namespace DNS.Messages.Components
{
    public class ReplyCode
    {
        public ReplyCode() { }
        public ReplyCode(ReplyCodeBuilder replyCode) { _questions = replyCode.questions; _answers = replyCode.answers; _authorities = replyCode.authorities; _additional = replyCode.additional; _authoritativeAnswer = replyCode.authoritativeAnswer; _isQuery = replyCode.isQuery; _truncation = replyCode.truncation; _recursionDesired = replyCode.recursionDesired; _recursionAvailable = replyCode.recursionAvailable; _opCode = replyCode.opCode; _responseCode = replyCode.responseCode; }

        private ushort _questions;
        private ushort _answers;
        private ushort _authorities;
        private ushort _additional;
        private bool _authoritativeAnswer;
        private bool _isQuery;
        private bool _truncation;
        private bool _recursionDesired;
        private bool _recursionAvailable;
        private OpCodes _opCode;
        private ResponseCodes _responseCode;

        public ushort QuestionCount => _questions;
        public ushort AnswerCount => _answers;
        public ushort AuthorityCount => _authorities;
        public ushort AdditionalCount => _additional;
        public bool AuthoritativeAnswer => _authoritativeAnswer;
        public bool IsQuery => _isQuery;
        public bool Truncation => _truncation;
        public bool RecursionDesired => _recursionDesired;
        public bool RecursionAvailable => _recursionAvailable;
        public OpCodes OpCode => _opCode;
        public ResponseCodes ResponseCode => _responseCode;
        
    }
}
