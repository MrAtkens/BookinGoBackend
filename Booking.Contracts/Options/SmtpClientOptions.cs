using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Contracts.Options
{
    public class SmtpClientOptions
    {
        public string Address { get; set; }
        public int Port { get; set; }
    }

}
