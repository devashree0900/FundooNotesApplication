using BusinessLayer.Interfaces;
using Experimental.System.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using ModelLayer;
using RepositoryLayer.Entity;
using System.Collections.Generic;

namespace FundooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelBusiness labelBusiness;
        public LabelController(ILabelBusiness labelBusiness)
        {
            this.labelBusiness = labelBusiness;
        }

        [HttpPost("Add")]
        public IActionResult AddLabel(long noteId,long userId,string name)
        {
            var label = labelBusiness.AddLabel(noteId, userId, name);
            if(label != null)
            {
                return Ok(new ResponseModel<LabelEntity> { Status = true, Message = "Label Added Successfully", Data = label });
            }
            else
            {
                return BadRequest(new ResponseModel<LabelEntity> { Status = false, Message = "Label Adding Failed", Data = label });
            }
        }

        [HttpGet("GetLabel")]
        public IActionResult GetLabelByNoteId(long noteId,long userId)
        {
            
            var labelList = labelBusiness.GetLabelsByNoteId(noteId, userId);
            if (labelList != null)
            {
                return Ok(new ResponseModel<List<LabelEntity>> { Status = true, Message = "Labels Displayed Successful", Data = labelList });
            }
            else
            {
                return BadRequest(new ResponseModel<List<LabelEntity>> { Status = false, Message = "Display Unsuccessful", Data = labelList });
            }
        }

        [HttpPost("RemoveLabel")]
        public IActionResult RemoveLabel(long noteId, long labelId)
        {

            var remove = labelBusiness.RemoveLabel(noteId, labelId);
            if (remove)
            {
                return Ok(new ResponseModel<bool> { Status = true, Message = "Label removed successfully", Data = remove });
            }
            else
            {
                return BadRequest(new ResponseModel<bool> { Status = false, Message = "Label removal Unsuccessful", Data = remove });
            }
        }

        [HttpPost("RenameLabel")]
        public IActionResult RenameLabel(long userId, string oldName, string newName)
        {

            var rename = labelBusiness.RenameLabel(userId, oldName,newName);
            if (rename)
            {
                return Ok(new ResponseModel<bool> { Status = true, Message = "Label updated successfully", Data = rename });
            }
            else
            {
                return BadRequest(new ResponseModel<bool> { Status = false, Message = "Label updation Unsuccessful", Data = rename });
            }
        }
    }
}
