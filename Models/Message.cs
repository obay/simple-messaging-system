using DevExpress.Xpo;
using System.Threading;

namespace simple_messaging_system.Models
{
    public class Message : XPObject
    {
        public Message(Session session) : base(session) 
        {
            _to = string.Empty;
            _from = string.Empty;
            _subject = string.Empty;
            _body = string.Empty;
            _childMessages = new XPCollection<Message>(session);
        }

        private string _to;
        private string _from;
        private string _subject;
        private string _body;
        private bool _isRead;
        private Message? _parentMessage;
        private XPCollection<Message> _childMessages;

        public string To
        {
            get => _to;
            set => SetPropertyValue(nameof(To), ref _to, value);
        }

        public string From
        {
            get => _from;
            set => SetPropertyValue(nameof(From), ref _from, value);
        }

        public string Subject
        {
            get => _subject;
            set => SetPropertyValue(nameof(Subject), ref _subject, value);
        }

        public string Body
        {
            get => _body;
            set => SetPropertyValue(nameof(Body), ref _body, value);
        }

        public bool IsRead
        {
            get => _isRead;
            set => SetPropertyValue(nameof(IsRead), ref _isRead, value);
        }

        [Association("Message-ChildMessages")]
        public Message? ParentMessage
        {
            get => _parentMessage;
            set => SetPropertyValue(nameof(ParentMessage), ref _parentMessage, value);
        }

        [Association("Message-ChildMessages")]
        public XPCollection<Message> ChildMessages => GetCollection<Message>(nameof(ChildMessages));
    }
}
