using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class CollabRepository:ICollabRepository
    {
        private readonly FundooDBContext context;
        private readonly IConfiguration configuration;

        public CollabRepository(FundooDBContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public CollabEntity AddCollaborator(long noteId, long userId, string colabEmail)
        {
            CollabEntity colab = new CollabEntity();
            colab.CollabEmail = colabEmail;
            colab.NoteId = noteId;
            colab.UserId = userId;
            this.context.CollabTable.Add(colab);
            int result = this.context.SaveChanges();
            if (result > 0)
            {
                return colab;
            }
            else
            {
                return null;
            }
        }

        public bool RemoveColab(long userId, long colabId)
        {
            var result = this.context.CollabTable.FirstOrDefault(e => e.CollabId == colabId && e.UserId == userId);
            if (result != null)
            {
                context.Remove(result);
                context.SaveChanges();
                return true;
            }
            return false;

        }

        public List<CollabEntity> GetAllCollaborators()
        {
            var result = context.CollabTable.ToList();
            return result;
        }

        public List<CollabEntity> GetAllColabsByNoteId(long noteId, long userId)
        {
            var result = context.CollabTable.Where(e => e.NoteId == noteId && e.UserId == userId).ToList();
            return result;
        }
    }
}
