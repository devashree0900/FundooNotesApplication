using Microsoft.Extensions.Configuration;
using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.Services.Account;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using static System.Net.Mime.MediaTypeNames;

namespace RepositoryLayer.Services
{
    public class NoteRepository: INoteRepository
    {
        private readonly FundooDBContext context;
        private readonly IConfiguration configuration;

        public NoteRepository(FundooDBContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public NoteEntity AddNote(NoteModel note, long userId)
        {
            NoteEntity noteEntity = new NoteEntity();
            noteEntity.Title = note.Title;
            noteEntity.Description = note.Description;
            noteEntity.Color= note.Color;
            noteEntity.ModifiedAt = DateTime.Now;
            noteEntity.CreatedAt = DateTime.Now;
            noteEntity.Reminder = note.Reminder;
            noteEntity.Image= note.Image;
            noteEntity.IsArchieve = note.IsArchieve;
            noteEntity.IsPinned= note.IsPinned;
            noteEntity.IsTrashed = note.IsTrashed;
            noteEntity.UserId = userId;
            context.NoteTable.Add(noteEntity); //if we wont write notetable, that will work too but more time taking
            context.SaveChanges();
            return noteEntity;
        }

        //if u wont pass user id, it will give u all  the notes
        public List<NoteEntity> GetAllNotes(long userId)
        {
            var noteList = this.context.NoteTable.Where(a=> a.UserId == userId).ToList();
            return noteList;
        }

        public List<NoteEntity> GetAllNotesForAllUsers()
        {
            var noteList = this.context.NoteTable.ToList();
            return noteList;
        }

        public NoteEntity UpdateNote(NoteModel note,int noteId,long userId)
        {
            var noteEntity = this.context.NoteTable.Where(a => a.NoteId == noteId).FirstOrDefault();
            if(noteEntity != null)
            {
                noteEntity.Title = note.Title;
                noteEntity.Description = note.Description;
                noteEntity.Color = note.Color;
                noteEntity.ModifiedAt = DateTime.Now;
                noteEntity.CreatedAt = DateTime.Now;
                noteEntity.Reminder = note.Reminder;
                noteEntity.Image = note.Image;
                noteEntity.IsArchieve = note.IsArchieve;
                noteEntity.IsPinned = note.IsPinned;
                noteEntity.IsTrashed = note.IsTrashed;
                noteEntity.UserId = userId;
                context.SaveChanges();
                return noteEntity;
            }
            else
            {
                return null;
            }
        }

        public NoteEntity DeleteNote(int noteId, long userId)
        {
            var noteEntity = this.context.NoteTable.Where(a => a.NoteId == noteId).FirstOrDefault();
            if (noteEntity != null)
            {
                context.NoteTable.Remove(noteEntity);
                context.SaveChanges();
                return noteEntity;
            }
            else
            {
                return null;
            }
        }

        public bool PinOrUnpinNote(long noteId, long userId)
        {
            NoteEntity note = this.context.NoteTable.Where(e => e.NoteId == noteId).FirstOrDefault();
            if (note.IsPinned == false)
            {
                note.IsPinned = true;
                context.SaveChanges();
                return true;
            }
            else
            {
                note.IsPinned = false;
                context.SaveChanges();
                return false;
            }
        }

        public bool TrashOrUntrash(long noteId, long userId)
        {
            NoteEntity note = this.context.NoteTable.Where(e => e.NoteId == noteId).FirstOrDefault();
            if (note.IsTrashed == false)
            {
                note.IsTrashed = true;
                context.SaveChanges();
                return true;
            }
            else
            {
                note.IsTrashed = false;
                context.SaveChanges();
                return false;
            }
        }

        public bool ArchieveOrUnarchieve(long noteId, long userId)
        {
            NoteEntity note = this.context.NoteTable.Where(e => e.NoteId == noteId).FirstOrDefault();
            if (note.IsArchieve == false)
            {
                note.IsArchieve = true;
                context.SaveChanges();
                return true;
            }
            else
            {
                note.IsArchieve = false;
                context.SaveChanges();
                return false;
            }
        }

        public NoteEntity GetNoteById(long noteId)
        {
            NoteEntity note = this.context.NoteTable.Where(e=> e.NoteId == noteId).FirstOrDefault();
            return note;
        }

        public NoteEntity Color(long noteId, string color)
        {
            try
            {
                NoteEntity note = this.context.NoteTable.FirstOrDefault(x=> x.NoteId == noteId);
                if(note.Color !=null)
                {
                    note.Color = color;
                    context.SaveChanges();
                    return note;
                }
                return null;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        public NoteEntity Reminder(long noteId, DateTime reminder)
        {
            try
            {
                NoteEntity note = this.context.NoteTable.FirstOrDefault(x => x.NoteId == noteId);
                if(note!=null)
                {
                    note.Reminder = reminder;
                    context.SaveChanges();
                    return note;
                }
                return null;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteForever(long noteId)
        {
            try
            {
                NoteEntity result = this.context.NoteTable.FirstOrDefault(e=> e.NoteId == noteId);
                if(result!= null && result.IsTrashed==true)
                {
                    context.Remove(result);
                    context.SaveChanges();
                    return true;
                }
                if (result != null)
                {
                    result.IsTrashed = true;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public string UploadImage(long noteId, long userId, IFormFile img) //for selecting image from local system
        {
            try
            {
                var result = this.context.NoteTable.FirstOrDefault(x => x.NoteId == noteId && x.UserId == userId);
                if (result != null)
                {
                    CloudinaryDotNet.Account account = new CloudinaryDotNet.Account(
                        this.configuration["CloudinarySettings:CloudName"],
                        this.configuration["CloudinarySettings:ApiKey"],
                        this.configuration["CloudinarySettings:ApiSecret"]
                        );
                    Cloudinary cloudinary = new Cloudinary(account);
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(img.FileName, img.OpenReadStream()),
                    };
                    var uploadrResult = cloudinary.Upload(uploadParams);
                    string imagePath = uploadrResult.Url.ToString();
                    result.Image = imagePath;
                    context.SaveChanges();
                    return "Image Updated Successfully";
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
    }
}
