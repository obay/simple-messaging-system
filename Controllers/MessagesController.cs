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
        public IActionResult CreateMessage([FromBody] MessageDto messageDto)
        {
            using (var uow = _dataStore.CreateUnitOfWork())
            {
                var newMessage = new Message(uow)
                {
                    To = messageDto.To,
                    From = messageDto.From,
                    Subject = messageDto.Subject,
                    Body = messageDto.Body,
                    IsRead = false
                };
                uow.CommitChanges();

                var response = new MessageResponseDto
                {
                    Id = newMessage.Oid,
                    To = newMessage.To,
                    From = newMessage.From,
                    Subject = newMessage.Subject,
                    Body = newMessage.Body,
                    IsRead = newMessage.IsRead
                };

                return CreatedAtAction(nameof(GetMessage), new { id = response.Id }, response);
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

                var response = new MessageResponseDto
                {
                    Id = message.Oid,
                    To = message.To,
                    From = message.From,
                    Subject = message.Subject,
                    Body = message.Body,
                    IsRead = message.IsRead
                };

                return Ok(response);
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

                var response = new MessageResponseDto
                {
                    Id = message.Oid,
                    To = message.To,
                    From = message.From,
                    Subject = message.Subject,
                    Body = message.Body,
                    IsRead = message.IsRead
                };

                return Ok(response);
            }
        }

        [HttpGet]
        public IActionResult GetAllMessages()
        {
            using (var uow = _dataStore.CreateUnitOfWork())
            {
                var messages = uow.Query<Message>().ToList();
                var response = messages.Select(m => new MessageResponseDto
                {
                    Id = m.Oid,
                    To = m.To,
                    From = m.From,
                    Subject = m.Subject,
                    Body = m.Body,
                    IsRead = m.IsRead
                }).ToList();

                return Ok(response);
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

                var response = messages.Select(m => new MessageResponseDto
                {
                    Id = m.Oid,
                    To = m.To,
                    From = m.From,
                    Subject = m.Subject,
                    Body = m.Body,
                    IsRead = m.IsRead
                }).ToList();

                return Ok(response);
            }
        }
    }
} 