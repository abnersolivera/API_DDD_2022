using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.InterfaceServices;
using Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIs.Models;

namespace WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMapper _Imapper;

        private readonly IMessage _IMessage;
        private readonly IServiceMessage _ServiceMessage;

        public MessageController(IMapper Imapper, IMessage Imessage, IServiceMessage ServiceMessage)
        {
            _Imapper = Imapper;
            _IMessage = Imessage;
            _ServiceMessage = ServiceMessage;
        }

        private async Task<string> RetornaIdUsuarioLogado()
        {
            if(User != null)
            {
                var idUsuario = User.FindFirst("idUsuario");
                return idUsuario.Value;
            }

            return string.Empty;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/Add")]
        public async Task<List<Notifies>> Add(MessageViewModel message)
        {
            message.UserId = await RetornaIdUsuarioLogado();

            var messageMap = _Imapper.Map<Message>(message);
            //await _IMessage.Add(messageMap);// Add Generic
            await _ServiceMessage.Adicionar(messageMap);
            return messageMap.Notitycoes;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/Update")]
        public async Task<List<Notifies>> Update(MessageViewModel message)
        {
            var messageMap = _Imapper.Map<Message>(message);
            //await _IMessage.Update(messageMap); // Add Generic
            await _ServiceMessage.Atualizar(messageMap);
            return messageMap.Notitycoes;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/Delete")]
        public async Task<List<Notifies>> Delete(MessageViewModel message)
        {
            var messageMap = _Imapper.Map<Message>(message);
            await _IMessage.Delete(messageMap);
            return messageMap.Notitycoes;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/GetEntityById")]
        public async Task<MessageViewModel> GetEntityById(Message message)
        {
            message = await _IMessage.GetEntityById(message.Id);
            var messageMap = _Imapper.Map<MessageViewModel>(message);            
            return messageMap;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/List")]
        public async Task<List<MessageViewModel>> List()
        {
            var messagens = await _IMessage.List();
            var messageMap = _Imapper.Map<List<MessageViewModel>>(messagens);
            return messageMap;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/ListarMessageAtivas")]
        public async Task<List<MessageViewModel>> ListarMessageAtivas()
        {
            var messagens = await _ServiceMessage.ListarMessageAtivas();
            var messageMap = _Imapper.Map<List<MessageViewModel>>(messagens);
            return messageMap;
        }
    }
}
