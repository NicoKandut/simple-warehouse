using System;
using System.Collections.Generic;
using System.Text;

namespace WerhausManager
{
    public class WebserviceError : Exception
    {
        public string messageDetails { get; set; }

        public WebserviceError(string message, string messageDetails) : base(message)
        {
            this.messageDetails = messageDetails;
        }
    }
}
