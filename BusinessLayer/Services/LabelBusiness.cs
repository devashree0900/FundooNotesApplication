using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using BusinessLayer.Interfaces;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class LabelBusiness: ILabelBusiness
    {
        private readonly ILabelRepository label;
        public LabelBusiness(ILabelRepository Label)
        {
            this.label = Label;
        }
        public LabelEntity AddLabel(long noteId, long userId, string labelName)
        {
            return label.AddLabel(noteId, userId, labelName);
        }

        public List<LabelEntity> GetLabelsByNoteId(long noteId, long userId)
        {
            return label.GetLabelsByNoteId(noteId, userId);
        }

        public bool RemoveLabel(long userId, long labelId)
        {
            return label.RemoveLabel(userId, labelId);
        }

        public bool RenameLabel(long userId, string oldName, string newName)
        {
            return label.RenameLabel(userId, oldName, newName);
        }
    }
}
