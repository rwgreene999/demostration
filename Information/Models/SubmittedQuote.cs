using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Information.Models
{
    public class SubmittedQuote
    {
        public string Author { get; set; }
        public string Quote { get; set; }
        public string Reference { get; set; }
        public string Email { get; set; }
        public int VoteUp { get; set; }
        public int VoteDown { get; set; }
        public bool ModeratorApproved { get; set; }
        public Guid QuoteSubmitter{ get; set; }
    }
}