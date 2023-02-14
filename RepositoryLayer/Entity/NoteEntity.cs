﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace RepositoryLayer.Entity
{
    public class NoteEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NoteId { get; set; }
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

        [ForeignKey("User")]
        public long UserId { get; set; }
        [JsonIgnore]
        //Virtual Table-will be craeted locally-only those u r using, data will be added
        public virtual UserEntity User { get; set; }
    }
}
