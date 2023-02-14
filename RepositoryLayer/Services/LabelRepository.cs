using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace RepositoryLayer.Services
{
    public class LabelRepository: ILabelRepository
    {
        private readonly FundooDBContext context;
        private readonly IConfiguration configuration;

        public LabelRepository(FundooDBContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public LabelEntity AddLabel(long noteId,long userId,string labelName)
        {
            LabelEntity label = new LabelEntity();
            label.LabelName = labelName;
            label.NoteId = noteId;
            label.UserId = userId;
            this.context.LabelTable.Add(label);
            int result = this.context.SaveChanges();
            if(result>0)
            {
                return label;
            }
            else
            {
                return null;    
            }
        }

        public List<LabelEntity> GetLabelsByNoteId(long noteId,long userId)
        {
            var result = context.LabelTable.Where(e=>e.NoteId == noteId && e.UserId==userId).ToList();
            return result;
        }

        public bool RemoveLabel(long userId,long labelId)
        {
            var result = this.context.LabelTable.FirstOrDefault(e => e.LabelId == labelId && e.UserId == userId);
            if(result!=null)
            {
                context.Remove(result);
                context.SaveChanges();
                return true;
            }
            return false;

        }

        public bool RenameLabel(long userId, string oldName, string newName)
        {
            var result = this.context.LabelTable.Where(e => e.UserId == userId && e.LabelName == oldName).FirstOrDefault();
            if(result!=null)
            {
                result.LabelName = newName;
                context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
