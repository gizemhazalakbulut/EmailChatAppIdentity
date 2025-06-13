namespace EmailChatAppIdentity.Entities
{
    public class Message
    {
        public int MessageId { get; set; }
        public string SenderEmail { get; set; } /* gönderici maili */
        public string ReceiverEmail { get; set; } /* alıcı maili */
        public string Subject { get; set; }
        public string MessageDetail { get; set; }
        public bool IsRead { get; set; }
        public DateTime SendDate { get; set; }
    }
}
