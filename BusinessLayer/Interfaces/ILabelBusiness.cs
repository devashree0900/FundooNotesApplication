using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface ILabelBusiness
    {
        public LabelEntity AddLabel(long noteId, long userId, string labelName);

        public List<LabelEntity> GetLabelsByNoteId(long noteId, long userId);

        public bool RemoveLabel(long userId, long labelId);

        public bool RenameLabel(long userId, string oldName, string newName);
    }
}
