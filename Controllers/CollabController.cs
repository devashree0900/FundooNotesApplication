using BusinessLayer.Interfaces;
using Experimental.System.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using ModelLayer;
using System.Collections.Generic;

namespace FundooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollabController : ControllerBase
    {
        private readonly ICollabBusiness colabBusiness;
        public CollabController(ICollabBusiness colabBusiness)
        {
            this.colabBusiness = colabBusiness;
        }

        [HttpPost("AddCollaborator")]
        public IActionResult AddCollaborator(long noteId, long userId, string colabEmail)
        {
            var result = colabBusiness.AddCollaborator(noteId, userId, colabEmail);
            if(result!=null)
            {
                return Ok(new ResponseModel<CollabEntity> { Status = true, Message = "Collaborator Added Successfully", Data=result } );
            }
            else
            {
                return BadRequest(new ResponseModel<CollabEntity> { Status = false, Message = "Collaborator Addition failed", Data = result });
            }
        }

        [HttpPost("RemoveCollaborator")]
        public IActionResult RemoveCollaborator(long userId, long colabId)
        {
            var result = colabBusiness.RemoveColab(userId, colabId);
            if (result)
            {
                return Ok(new ResponseModel<bool> { Status = true, Message = "Collaborator Deleted Successfully", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<bool> { Status = false, Message = "Collaborator Deletiontion failed", Data = result });
            }
        }

        [HttpGet("GetAllCollaborators")]
        public IActionResult GetAllCollaborators()
        {
            var result = colabBusiness.GetAllCollaborators();
            if (result!=null)
            {
                return Ok(new ResponseModel<List<CollabEntity>> { Status = true, Message = "All Collaborators Displayed Successfully", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<List<CollabEntity>> { Status = false, Message = "Display failed", Data = result });
            }
        }

        [HttpGet("GetAllCollabsByNoteId")]
        public IActionResult GetAllCollaboratorsByNoteId(long noteId,long userId)
        {
            var result = colabBusiness.GetAllColabsByNoteId(noteId,userId);
            if (result != null)
            {
                return Ok(new ResponseModel<List<CollabEntity>> { Status = true, Message = "All Collaborators Displayed Successfully", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<List<CollabEntity>> { Status = false, Message = "Display failed", Data = result });
            }
        }
    }
}
