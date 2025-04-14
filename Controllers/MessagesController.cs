using DevExpress.Xpo;
using Microsoft.AspNetCore.Mvc;
using simple_messaging_system.Data;
using simple_messaging_system.Models;
using System.Collections.Generic;
using System.Linq;

namespace simple_messaging_system.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly XpoDataStore _dataStore;

        public MessagesController()
        {
            _dataStore = XpoDataStore.Instance;
        }

        [HttpPost]
        public IActionResult CreateMessage([FromBody] Message message)
        {
            using (var uow = _dataStore.CreateUnitOfWork())
            {
                var newMessage = new Message(uow)
                {
                    To = message.To,
                    From = message.From,
                    Subject = message.Subject,
                    Body = message.Body,
                    IsRead = false
                };
                uow.CommitChanges();
                return CreatedAtAction(nameof(GetMessage), new { id = newMessage.Oid }, newMessage);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMessage(int id)
        {
            using (var uow = _dataStore.CreateUnitOfWork())
            {
                var message = uow.GetObjectByKey<Message>(id);
                if (message == null)
                    return NotFound();

                uow.Delete(message);
                uow.CommitChanges();
                return NoContent();
            }
        }

        [HttpPut("{id}/read")]
        public IActionResult MarkAsRead(int id)
        {
            using (var uow = _dataStore.CreateUnitOfWork())
            {
                var message = uow.GetObjectByKey<Message>(id);
                if (message == null)
                    return NotFound();

                message.IsRead = true;
                uow.CommitChanges();
                return Ok(message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetMessage(int id)
        {
            using (var uow = _dataStore.CreateUnitOfWork())
            {
                var message = uow.GetObjectByKey<Message>(id);
                if (message == null)
                    return NotFound();

                return Ok(message);
            }
        }

        [HttpGet]
        public IActionResult GetAllMessages()
        {
            using (var uow = _dataStore.CreateUnitOfWork())
            {
                var messages = uow.Query<Message>().ToList();
                return Ok(messages);
            }
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetUserMessages(string userId)
        {
            using (var uow = _dataStore.CreateUnitOfWork())
            {
                var messages = uow.Query<Message>()
                    .Where(m => m.To == userId || m.From == userId)
                    .ToList();
                return Ok(messages);
            }
        }
    }
} 