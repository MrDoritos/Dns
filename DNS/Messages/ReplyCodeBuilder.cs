using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNS.Messages.Components;
using OpCodes = DNS.Messages.Components.Header.OpCodes;
using ResponseCodes = DNS.Messages.Components.Header.ResponseCodes;

namespace DNS.Messages
{
    public class ReplyCodeBuilder
    {
        public ReplyCodeBuilder() { }
        public ReplyCodeBuilder(ReplyCodeBuilder replyCode) { questions = replyCode.questions; answers = replyCode.answers; authorities = replyCode.authorities; additional = replyCode.additional; authoritativeAnswer = replyCode.authoritativeAnswer; isQuery = replyCode.isQuery;
            truncation = replyCode.truncation; recursionDesired = replyCode.recursionDesired; recursionAvailable = replyCode.recursionAvailable; opCode = replyCode.opCode; responseCode = replyCode.responseCode;
        }
        public ReplyCodeBuilder(ReplyCode replyCode) {
            questions = replyCode.QuestionCount; answers = replyCode.AnswerCount; authorities = replyCode.AuthorityCount; additional = replyCode.AdditionalCount; authoritativeAnswer = replyCode.AuthoritativeAnswer; isQuery = replyCode.IsQuery;
            truncation = replyCode.Truncation; recursionDesired = replyCode.RecursionDesired; recursionAvailable = replyCode.RecursionAvailable; opCode = replyCode.OpCode; responseCode = replyCode.ResponseCode;
        }
        public ushort questions;
        public ushort answers;
        public ushort authorities;
        public ushort additional;
        public bool authoritativeAnswer;
        public bool isQuery;
        public bool truncation;
        public bool recursionDesired;
        public bool recursionAvailable;
        public OpCodes opCode;
        public ResponseCodes responseCode;
    }
}
