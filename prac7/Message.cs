﻿using System;

namespace prac7
{
    public class Message
    {
        public DateTime TimeStamp { get; set; }
        public string MessageText { get; set; }

        public Message(DateTime timeStamp, string messageText)
        {
            TimeStamp = timeStamp;
            MessageText = messageText;
        }
    }
}
