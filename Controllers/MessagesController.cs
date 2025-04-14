using DevExpress.Xpo;
using Microsoft.AspNetCore.Mvc;
using simple_messaging_system.Data;
using simple_messaging_system.Models;

namespace simple_messaging_system.Controllers
{
    /// <summary>
    /// Controller for managing messages in the messaging system
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly XpoDataStore _dataStore;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(ILogger<MessagesController> logger)
        {
            _dataStore = XpoDataStore.Instance;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new message
        /// </summary>
        /// <param name="messageDto">The message data to create</param>
        /// <returns>The created message</returns>
        /// <response code="201">Returns the newly created message</response>
        /// <response code="400">If the message data is invalid</response>
        /// <response code="404">If the parent message is specified but not found</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult CreateMessage([FromBody] MessageDto messageDto)
        {
            using (var uow = _dataStore.CreateUnitOfWork())
            {
                var newMessage = new Message(uow)
                {
                    To = messageDto.To,
                    From = messageDto.From,
                    Subject = messageDto.Subject ?? string.Empty,
                    Body = messageDto.Body ?? string.Empty,
                    IsRead = false
                };

                if (messageDto.ParentMessageId.HasValue)
                {
                    var parentMessage = uow.GetObjectByKey<Message>(messageDto.ParentMessageId.Value);
                    if (parentMessage == null)
                        return NotFound($"Parent message with ID {messageDto.ParentMessageId} not found");
                    
                    newMessage.ParentMessage = parentMessage;
                }

                uow.CommitChanges();

                var response = MapToResponseDto(newMessage);
                return CreatedAtAction(nameof(GetMessage), new { id = response.Id }, response);
            }
        }

        /// <summary>
        /// Deletes a message and all its child messages
        /// </summary>
        /// <param name="id">The ID of the message to delete</param>
        /// <returns>No content if successful</returns>
        /// <response code="204">If the message was successfully deleted</response>
        /// <response code="404">If the message is not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteMessage(int id)
        {
            using (var uow = _dataStore.CreateUnitOfWork())
            {
                var message = uow.GetObjectByKey<Message>(id);
                if (message == null)
                    return NotFound();

                // Delete all child messages first
                foreach (var childMessage in message.ChildMessages.ToList())
                {
                    uow.Delete(childMessage);
                }

                uow.Delete(message);
                uow.CommitChanges();
                return NoContent();
            }
        }

        /// <summary>
        /// Marks a message as read
        /// </summary>
        /// <param name="id">The ID of the message to mark as read</param>
        /// <returns>The updated message</returns>
        /// <response code="200">If the message was successfully marked as read</response>
        /// <response code="404">If the message is not found</response>
        [HttpPut("{id}/read")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkAsRead(int id)
        {
            using (var uow = _dataStore.CreateUnitOfWork())
            {
                var message = uow.GetObjectByKey<Message>(id);
                if (message == null)
                    return NotFound();

                message.IsRead = true;
                uow.CommitChanges();

                var response = MapToResponseDto(message);
                return Ok(response);
            }
        }

        /// <summary>
        /// Gets a specific message by its ID
        /// </summary>
        /// <param name="id">The ID of the message to retrieve</param>
        /// <returns>The requested message</returns>
        /// <response code="200">Returns the requested message</response>
        /// <response code="404">If the message is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMessage(int id)
        {
            using (var uow = _dataStore.CreateUnitOfWork())
            {
                var message = uow.GetObjectByKey<Message>(id);
                if (message == null)
                    return NotFound();

                var response = MapToResponseDto(message);
                return Ok(response);
            }
        }

        /// <summary>
        /// Gets all root-level messages (messages without a parent)
        /// </summary>
        /// <returns>A list of all root messages</returns>
        /// <response code="200">Returns the list of root messages</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllMessages()
        {
            using (var uow = _dataStore.CreateUnitOfWork())
            {
                var messages = uow.Query<Message>().Where(m => m.ParentMessage == null).ToList();
                var response = messages.Select(m => MapToResponseDto(m)).ToList();
                return Ok(response);
            }
        }

        /// <summary>
        /// Gets all messages for a specific user
        /// </summary>
        /// <param name="userId">The ID of the user whose messages to retrieve</param>
        /// <returns>A list of messages for the specified user</returns>
        /// <response code="200">Returns the list of user messages</response>
        /// <response code="404">If no messages are found for the user</response>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserMessages(string userId)
        {
            using (var uow = _dataStore.CreateUnitOfWork())
            {
                _logger.LogInformation("Searching for messages with To: {UserId}", userId);
               
                var messages = uow.Query<Message>().Where(m => m.To == userId && m.ParentMessage == null).ToList();
                if (messages == null || !messages.Any())
                {
                    _logger.LogWarning("No messages found for user {UserId}", userId);
                    return NotFound();
                }

                _logger.LogInformation("Found {MessageCount} messages for user {UserId}", messages.Count, userId);
                _logger.LogInformation("First message details - To: {To}, From: {From}", messages[0].To, messages[0].From);

                var response = messages.Select(m => MapToResponseDto(m)).ToList();
                return Ok(response);
            }
        }

        /// <summary>
        /// Gets all child messages of a specific message
        /// </summary>
        /// <param name="id">The ID of the parent message</param>
        /// <returns>A list of child messages</returns>
        /// <response code="200">Returns the list of child messages</response>
        /// <response code="404">If the parent message is not found</response>
        [HttpGet("{id}/children")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetChildMessages(int id)
        {
            using (var uow = _dataStore.CreateUnitOfWork())
            {
                var message = uow.GetObjectByKey<Message>(id);
                if (message == null)
                    return NotFound();

                var childMessages = message.ChildMessages.ToList();
                var response = childMessages.Select(m => MapToResponseDto(m)).ToList();
                return Ok(response);
            }
        }

        /// <summary>
        /// Marks a message as unread
        /// </summary>
        /// <param name="id">The ID of the message to mark as unread</param>
        /// <returns>The updated message</returns>
        /// <response code="200">If the message was successfully marked as unread</response>
        /// <response code="404">If the message is not found</response>
        [HttpPut("{id}/unread")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult MarkAsUnread(int id)
        {
            using (var uow = _dataStore.CreateUnitOfWork())
            {
                var message = uow.GetObjectByKey<Message>(id);
                if (message == null)
                    return NotFound();

                message.IsRead = false;
                uow.CommitChanges();

                var response = MapToResponseDto(message);
                return Ok(response);
            }
        }

        private MessageResponseDto MapToResponseDto(Message message)
        {
            var response = new MessageResponseDto
            {
                Id = message.Oid,
                To = message.To,
                From = message.From,
                Subject = message.Subject,
                Body = message.Body,
                IsRead = message.IsRead,
                ParentMessageId = message.ParentMessage?.Oid
            };

            // Recursively map child messages
            foreach (var childMessage in message.ChildMessages)
            {
                response.ChildMessages.Add(MapToResponseDto(childMessage));
            }

            return response;
        }
    }
}
