using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace RepositoryLayer.Entity
{
    public class LabelEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LabelId { get; set; }
        public string LabelName { get; set; }

        [ForeignKey("User")]

        public long UserId { get; set; }

        [ForeignKey("Notes")]

        public long NoteId { get; set; }

       
        public virtual UserEntity User { get; set; }

       
        public virtual NoteEntity Notes { get; set;}



    }
}
