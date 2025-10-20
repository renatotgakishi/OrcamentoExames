
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrcamentoMedico.Application.Interfaces;
using OrcamentoMedico.Domain.DTO;
using OrcamentoMedico.Domain.Events;

namespace OrcamentoMedico.API.Controllers
{


    [ApiController]    
    [Route("api/upload")]
    public class UploadController : ControllerBase
    {
        private readonly IS3Service _s3;
        private readonly ISqsService _sqs;

        public UploadController(IS3Service s3, ISqsService sqs)
        {
            _s3 = s3;
            _sqs = sqs;
        }

        [HttpPost]
        [Authorize]
        [Route("upload")]
        public async Task<IActionResult> UploadExame([FromForm] UploadExameRequest request)
        {
            using var stream = request!.Imagem!.OpenReadStream();
            var s3Key = await _s3.UploadAsync(stream, request.Imagem.FileName);

            var evento = new  UploadCriadoEvent
            {
                IdUsuario = request.IdUsuario,
                ImagemS3 = s3Key,
                DataCriacao = DateTime.UtcNow
            };

            await _sqs.PublishAsync(evento);
            return Created("", evento);
        }
    }
}