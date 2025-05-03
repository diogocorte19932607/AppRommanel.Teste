using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Thomagreg.Interface.Model;

namespace Thomagreg.Interface.Controllers
{
    public class ClienteController : BaseController
    {
        private readonly ILogger<ClienteController> _logger;
        public ClienteController(ILogger<ClienteController> logger, IConfiguration configuration, IHttpClientFactory httpClient) : base(configuration, httpClient)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> Editar(Guid id)
        {
            await GetToken();
            var getCandidato = await _httpClient.GetAsync("api/Cliente/RetornaPorId/" + id.ToString());
            if (!getCandidato.IsSuccessStatusCode)
            {
                throw new HttpRequestException(getCandidato.ToString());
            }
            var resultCliente = JsonConvert.DeserializeObject<DtoCliente>(await getCandidato.Content.ReadAsStringAsync());
            return View(resultCliente);
        }
        [HttpGet]
        public async Task<IActionResult> ListarClientes()
        {
            await GetToken();
            var getCandidato = await _httpClient.GetAsync("api/Cliente/Listar");
            if (!getCandidato.IsSuccessStatusCode)
            {
                throw new HttpRequestException(getCandidato.ToString());
            }
            var resultCliente = JsonConvert.DeserializeObject<List<DtoCliente>>(await getCandidato.Content.ReadAsStringAsync());
            return Json(resultCliente);
        }
        [HttpPost]
        public async Task<IActionResult> CriarCliente([FromBody] DtoClienteCreate cliente)
        {
            try
            {
                await GetToken();

                var jsonContent = JsonConvert.SerializeObject(cliente);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_configuration["ApiURL"] + "api/Cliente/Adicionar", contentString);

                if (response.IsSuccessStatusCode)
                {
                    return Json(new { sucesso = true, ret = await response.Content.ReadAsStringAsync() });
                }
                else
                {
                    return Json(new { sucesso = false, ret = await response.Content.ReadAsStringAsync() });
                }
            }
            catch (System.Exception)
            {

                throw;
            }

        }
        [HttpPost]
        public async Task<bool> EditarCliente([FromBody] DtoClienteCreate cliente)
        {
            await GetToken();

            var jsonContent = JsonConvert.SerializeObject(cliente);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_configuration["ApiURL"] + "api/Cliente/Atualizar", contentString);

            return response.IsSuccessStatusCode;
        }
        [HttpDelete("DeletarCliente/{id}")]
        public async Task<bool> DeletarCliente(string id)
        {
            await GetToken();

            var response = await _httpClient.DeleteAsync(_configuration["ApiURL"] + "api/Cliente/Deletar/" + id);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        [HttpPost]
        public async Task<bool> AdicionarLogradouro([FromBody] DtoLogradouroCreate logradouro)
        {
            try
            {
                await GetToken();

                var jsonContent = JsonConvert.SerializeObject(logradouro);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_configuration["ApiURL"] + "api/Cliente/AdicionarLogradouro", contentString);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception)
            {

                throw;
            }

        }
        public async Task<IActionResult> ListarLogradouros(Guid id)
        {
            await GetToken();
            var getCandidato = await _httpClient.GetAsync("api/Cliente/ObterLogradouros/" + id);
            if (!getCandidato.IsSuccessStatusCode)
            {
                throw new HttpRequestException(getCandidato.ToString());
            }
            var resultCliente = JsonConvert.DeserializeObject<List<DtoLogradouro>>(await getCandidato.Content.ReadAsStringAsync());
            return Json(resultCliente);
        }
        [HttpDelete("RemoverLogradouro/{id}")]
        public async Task<bool> RemoverLogradouro(string id)
        {
            await GetToken();

            var response = await _httpClient.DeleteAsync(_configuration["ApiURL"] + "api/Cliente/RemoverLogradouro/" + id);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public async Task<IActionResult> EditarLogradouro(Guid id)
        {
            await GetToken();
            var getCandidato = await _httpClient.GetAsync("api/Cliente/RetornaLogradouroPorId/" + id.ToString());
            if (!getCandidato.IsSuccessStatusCode)
            {
                throw new HttpRequestException(getCandidato.ToString());
            }
            var resultCliente = JsonConvert.DeserializeObject<DtoLogradouro>(await getCandidato.Content.ReadAsStringAsync());
            return Json(resultCliente);
        }
        [HttpPost]
        public async Task<bool> EditarLogradouro([FromBody] DtoLogradouro logradouro)
        {
            try
            {
                await GetToken();

                var jsonContent = JsonConvert.SerializeObject(logradouro);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_configuration["ApiURL"] + "api/Cliente/EditarLogradouro", contentString);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
