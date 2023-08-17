using System;
using System.Collections.Generic;

namespace prjCoreWebWantWant.Models;

public partial class ChatMessage
{
    public int ChatMessageId { get; set; }

    public int SenderId { get; set; }

    public int ReceiverId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime Created { get; set; }

    public virtual MemberAccount Receiver { get; set; } = null!;

    public virtual MemberAccount Sender { get; set; } = null!;
}
