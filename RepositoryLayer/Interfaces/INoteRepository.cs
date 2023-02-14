using Microsoft.AspNetCore.Http;
using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface INoteRepository
    {
        public NoteEntity AddNote(NoteModel note, long userId);
        public List<NoteEntity> GetAllNotes(long userId);

        public NoteEntity UpdateNote(NoteModel note, int noteId, long userId);

        public NoteEntity DeleteNote(int noteId, long userId);
        public bool PinOrUnpinNote(long noteId, long userId);

        public bool TrashOrUntrash(long noteId, long userId);
        public bool ArchieveOrUnarchieve(long noteId, long userId);

        public List<NoteEntity> GetAllNotesForAllUsers();

        public NoteEntity GetNoteById(long noteId);

        public NoteEntity Color(long noteId, string color);
        public bool DeleteForever(long noteId);

        public NoteEntity Reminder(long noteId, DateTime reminder);

        public string UploadImage(long noteId, long userId, IFormFile img);
    }
}
