using System;

namespace GetMeX.Models
{
    public class AccountDetail
    {
        public int AccId { get; set; }

        public string Email { get; set; }

        public DateTimeOffset LastSync { get; set; }
    }
}
