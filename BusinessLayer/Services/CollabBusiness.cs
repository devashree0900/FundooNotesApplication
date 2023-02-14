using System;
using System.Collections.Generic;
using System.Text;
using BusinessLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class CollabBusiness: ICollabBusiness
    {
        private readonly ICollabRepository colab;
        public CollabBusiness(ICollabRepository colab)
        {
            this.colab = colab;
        }

        public CollabEntity AddCollaborator(long noteId, long userId, string colabEmail)
        {
            return colab.AddCollaborator(noteId, userId, colabEmail);
        }

        public bool RemoveColab(long userId, long colabId)
        {
            return colab.RemoveColab(userId, colabId);
        }

        public List<CollabEntity> GetAllCollaborators()
        {
            return colab.GetAllCollaborators();
        }

        public List<CollabEntity> GetAllColabsByNoteId(long noteId, long userId)
        {
            return colab.GetAllColabsByNoteId(noteId, userId);
        }
    }
}
