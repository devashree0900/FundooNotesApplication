using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace ModelLayer
{
    public class NoteModel
    {
        
        public string Title { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
        public DateTime Reminder { get; set; }
        public bool IsArchieve { get; set; }
        public bool IsPinned { get; set; }
        public bool IsTrashed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        
      
    }
}
