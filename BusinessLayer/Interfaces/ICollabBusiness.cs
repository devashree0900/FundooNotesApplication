using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface ICollabBusiness
    {
        public CollabEntity AddCollaborator(long noteId, long userId, string colabEmail);

        public bool RemoveColab(long userId, long colabId);

        public List<CollabEntity> GetAllCollaborators();

        public List<CollabEntity> GetAllColabsByNoteId(long noteId, long userId);
    }
}
