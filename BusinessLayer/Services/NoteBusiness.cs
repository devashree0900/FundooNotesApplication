using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using ModelLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class NoteBusiness: INoteBusiness
    {
        private readonly INoteRepository notes;
        public NoteBusiness(INoteRepository note)
        {
            this.notes = note;
        }

        public NoteEntity AddNote(NoteModel note,long userId)
        {
            return notes.AddNote(note, userId);
        }

        public List<NoteEntity> GetAllNotes(long userId)
        {
            return notes.GetAllNotes(userId);
        }

        public NoteEntity UpdateNote(NoteModel note, int noteId, long userId)
        {
            return notes.UpdateNote(note, noteId, userId);
        }

        public NoteEntity DeleteNote(int noteId, long userId)
        {
            return notes.DeleteNote(noteId, userId);
        }

        public bool PinOrUnpinNote(long noteId, long userId)
        {
            return notes.PinOrUnpinNote(noteId, userId);


        }

        public bool TrashOrUntrash(long noteId, long userId)
        {
            return notes.TrashOrUntrash(noteId, userId);
        }
        public bool ArchieveOrUnarchieve(long noteId, long userId)
        {
            return notes.ArchieveOrUnarchieve(noteId, userId);
        }

        public List<NoteEntity> GetAllNotesForAllUsers()
        {
            return notes.GetAllNotesForAllUsers();
        }

        public NoteEntity GetNoteById(long noteId)
        {
            return notes.GetNoteById(noteId);
        }

        public NoteEntity Color(long noteId, string color)
        {
            return notes.Color(noteId, color);
        }
        public bool DeleteForever(long noteId)
        {
            return notes.DeleteForever(noteId);
        }

        public NoteEntity Reminder(long noteId, DateTime reminder)
        {
            return notes.Reminder(noteId, reminder);
        }

        public string UploadImage(long noteId, long userId, IFormFile img)
        {
            return notes.UploadImage(noteId, userId, img);
        }
    }
}
