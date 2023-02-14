using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace FundooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] //claims would work only if we use this, no need to specify for each and every method
    public class NoteController : ControllerBase
    {
        private readonly INoteBusiness noteBusiness;
        private readonly ILogger<NoteController> logger;
        IDistributedCache distributedCache;
        public NoteController(INoteBusiness noteBusiness, ILogger<NoteController> logger, IDistributedCache distributedCache)
        {
            this.noteBusiness = noteBusiness;
            this.logger = logger;
            this.distributedCache = distributedCache;
        }
        //if u wont use authorise, claim wont work
        [HttpPost("AddNote")]
        //[Authorize]
        public IActionResult AddNote(NoteModel note)
        {
            long userId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "UserId").Value); //we r extracting userId from token
            var NoteCreate = noteBusiness.AddNote(note, userId);
            
            if(NoteCreate != null)
            {
                logger.LogInformation("Note Added Successfully");
                return Ok(new ResponseModel<NoteEntity> { Status = true,Message = "Note Added Successfully", Data = NoteCreate});
            }
            else
            {
                logger.LogError("Note Adding Failed");
                return BadRequest(new ResponseModel<NoteEntity> { Status = false, Message="Note Added failed", Data = null});
            }
        }

        [HttpGet("GetALlNotes")]
        //[Authorize]
        public IActionResult GetAllNotes()
        {
            long userId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "UserId").Value); //we r extracting userId from token
            var noteList = noteBusiness.GetAllNotes(userId);
            if (noteList != null)
            {
                return Ok(new ResponseModel<List<NoteEntity>> { Status = true, Message = "Successful", Data = noteList  });
            }
            else
            {
                return BadRequest(new ResponseModel<List<NoteEntity>> { Status = false, Message = "Unsuccessful operation", Data = null });
            }
        }

        [HttpGet("GetALlNotesForAllUsers")]
        [AllowAnonymous]
        public IActionResult GetAllNotesForAllUsers()
        { 
            var noteList = noteBusiness.GetAllNotesForAllUsers();
            if (noteList != null)
            {
                return Ok(new ResponseModel<List<NoteEntity>> { Status = true, Message = "Successful", Data = noteList });
            }
            else
            {
                return BadRequest(new ResponseModel<List<NoteEntity>> { Status = false, Message = "Unsuccessful operation", Data = null });
            }
        }


        [HttpGet("GetALlNotesUsingRedisCache")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllNotesUsingRedis()
        {
            try
            {
                var CacheKey = "NotesList";
                //string SerializedNoteList;
                List<NoteEntity> NoteList;
                byte[] RedisNoteList = await distributedCache.GetAsync(CacheKey); //for getting the data from the cache
                if (RedisNoteList != null) //if data exists in distributed cache
                {
                    logger.LogDebug("Getting the list from Redis Cache");
                    var SerializedNoteList = Encoding.UTF8.GetString(RedisNoteList);
                    NoteList = JsonConvert.DeserializeObject<List<NoteEntity>>(SerializedNoteList);
                }
                else
                {
                    logger.LogDebug("Setting the list to cache which is requested for the first time");
                    NoteList = noteBusiness.GetAllNotesForAllUsers();
                    var SerializedNoteList = JsonConvert.SerializeObject(NoteList);
                    var redisNoteList = Encoding.UTF8.GetBytes(SerializedNoteList);
                    var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(10)).SetSlidingExpiration(TimeSpan.FromMinutes(5));//till this time data will be vailable in cahce and then expire
                    await distributedCache.SetAsync(CacheKey, redisNoteList, options);
                }
                logger.LogInformation("Got the notes list successfully from Redis");
                return Ok(NoteList);
            }
            catch(Exception ex)
            {
                logger.LogCritical(ex, "Exception thrown...");
                return BadRequest(new { SUccess = false, Message = ex.Message });
            }
            

        }

        [HttpPost("UpdateNote")]
        //[Authorize]
        public IActionResult UpdateNote(NoteModel note ,int noteId)
        {
            long userId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "UserId").Value); //we r extracting userId from token
            var updatedNote = noteBusiness.UpdateNote(note, noteId,userId);
            if (updatedNote != null)
            {
                return Ok(new ResponseModel<NoteEntity> { Status = true, Message = "Update Successful", Data = updatedNote});
            }
            else
            {
                return BadRequest(new ResponseModel<NoteEntity> { Status = false, Message = "Unsuccessful operation", Data = null });
            }
        }

        [HttpPost("DeleteNote")]
        //[Authorize]
        public IActionResult DeleteNote(int noteId)
        {
            long userId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "UserId").Value); //we r extracting userId from token
            var deletedNote = noteBusiness.DeleteNote(noteId, userId);
            if (deletedNote != null)
            {
                return Ok(new ResponseModel<NoteEntity> { Status = true, Message = "Delete Successful", Data= deletedNote });
            }
            else
            {
                return BadRequest(new ResponseModel<NoteEntity> { Status = false, Message = "Unsuccessful delete operation", Data = null });
            }
        }

        [HttpPut("PinOrUnpinNote")]
        //[Authorize]
        public IActionResult PinOrUnpinNote(long noteId)
        {
            long userId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "UserId").Value);
            var NotePin = noteBusiness.PinOrUnpinNote(noteId, userId);
            if (NotePin)
            {
                return Ok(new ResponseModel<bool> { Status = true, Message = "Note Pinned Successfully", Data = NotePin });
            }
            else
            {
                return Ok(new ResponseModel<bool> { Status = true, Message = "Note Unpinned successfully", Data = NotePin });
            }


        }

        [HttpPut("TrashOrUntrashNote")]
        //[Authorize]
        public IActionResult TrashOrUntrash(long noteId)
        {
            long userId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "UserId").Value);
            var trash = noteBusiness.PinOrUnpinNote(noteId, userId);
            if (trash)
            {
                return Ok(new ResponseModel<bool> { Status = true, Message = "Note Put In Trash Successfully", Data = trash });
            }
            else
            {
                return Ok(new ResponseModel<bool> { Status = true, Message = "Note Removed from Trash Successfully", Data = trash });
            }


        }

        [HttpPut("ArchieveOrUnarchieveNote")]
        //[Authorize]
        public IActionResult ArchieveOrUnarchieve(long noteId)
        {
            long userId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "UserId").Value);
            var archieve = noteBusiness.PinOrUnpinNote(noteId, userId);
            if (archieve)
            {
                return Ok(new ResponseModel<bool> { Status = true, Message = "Note Archieved Successfully", Data = archieve });
            }
            else
            {
                return Ok(new ResponseModel<bool> { Status = true, Message = "Note Unarchieved successfully", Data = archieve });
            }


        }

        [HttpGet("GetNoteById")]
        [AllowAnonymous]

        public IActionResult GetNoteById(long id)
        {
            var note = noteBusiness.GetNoteById(id);
            if(note!=null)
            {
                return Ok(new ResponseModel<NoteEntity> { Status = true, Message = "Note Retreival Successful", Data = note });
            }
            else
            {
                return BadRequest(new ResponseModel<NoteEntity> { Status = true, Message = "Note Retreival unsuccessful", Data = note });
            }
        }

       
        

        [HttpPost("Color")]
        [AllowAnonymous]

        public IActionResult Color(long noteId,string color)
        {
            var note = noteBusiness.Color(noteId,color);
            if (note != null)
            {
                return Ok(new ResponseModel<NoteEntity> { Status = true, Message = "Color addition Successful", Data = note });
            }
            else
            {
                return BadRequest(new ResponseModel<NoteEntity> { Status = true, Message = "Color Addition  unsuccessful", Data = note });
            }
        }

        [HttpPost("Reminder")]
        [AllowAnonymous]

        public IActionResult Reminder(long noteId, DateTime reminder)
        {
            var note = noteBusiness.Reminder(noteId, reminder);
            if (note != null)
            {
                return Ok(new ResponseModel<NoteEntity> { Status = true, Message = "Reminder addition Successful", Data = note });
            }
            else
            {
                return BadRequest(new ResponseModel<NoteEntity> { Status = true, Message = "Reminder Addition  unsuccessful", Data = note });
            }
        }

        [HttpPost("DeleteForever")]
        [AllowAnonymous]

        public IActionResult DeleteForever(long noteId)
        {
            var note = noteBusiness.DeleteForever(noteId);
            if (note)
            {
                return Ok(new ResponseModel<bool> { Status = true, Message = "Note Deletion Successful", Data = note });
            }
            else
            {
                return BadRequest(new ResponseModel<bool> { Status = true, Message = "Note deletion unsuccessful", Data = note });
            }
        }

        [HttpPost("UploadImage")]

        public IActionResult UploadImage(long noteId, IFormFile img)
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "UserId").Value);
            var note = noteBusiness.UploadImage(noteId,userId,img);
            if (note!=null)
            {
                return Ok(new ResponseModel<string> { Status = true, Message = "Image Updation Successful", Data = note });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { Status = true, Message = "Image Updation unsuccessful", Data = note });
            }
        }
    }
}
