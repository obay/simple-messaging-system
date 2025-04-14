using DevExpress.Xpo;

namespace simple_messaging_system.Models
{
    public class Message : XPObject
    {
        public Message(Session session) : base(session) { }

        private string _to;
        private string _from;
        private string _subject;
        private string _body;
        private bool _isRead;

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
    }
} 