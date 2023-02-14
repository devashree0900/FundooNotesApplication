using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class CollabEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long CollabId { get; set; }
        public string CollabEmail { get; set; }

  

        [ForeignKey("User")]

        public long UserId { get; set; }

        [ForeignKey("Notes")]

        public long NoteId { get; set; }


        public virtual UserEntity User { get; set; }


        public virtual NoteEntity Notes { get; set; }
    }
}
