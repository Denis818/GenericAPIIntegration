using AutoMapper;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BSIntegrarApiSefaz.ClienteWeb
{
    public abstract class ClientBase
    {
        private readonly IHttpClientFactory _clientFactory;
        protected string _UrlBase;
        protected readonly IMapper _mapper;

        //IServiceProvider serve para acessarmos o container ID de forma manual.
        public ClientBase(IServiceProvider serviceProvider)
        {
            _clientFactory = (IHttpClientFactory)serviceProvider.GetService(typeof(IHttpClientFactory));
            _mapper = (IMapper)serviceProvider.GetService(typeof(IMapper));
        }

        protected async Task<T> GetAsync<T>(string uri)
        {
            using var client = _clientFactory.CreateClient();

            //criando string de url.
            var url = $"{_UrlBase}/{uri}";

            //criando o tipo de requisição passando minha url.-
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            //disparando minha requiasição.
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                //convertendo dados para json.    Conteúdo lido como fluxo
                using var stream = await response.Content.ReadAsStreamAsync();

                //convertento para objeto de novo.
                var responseContent = await JsonSerializer.DeserializeAsync<T>(stream);

                //retornando conteudo.
                return responseContent;
            }
            else
            {
                return default(T);
            }
        }



    }
}
