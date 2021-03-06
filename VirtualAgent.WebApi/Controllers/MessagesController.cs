﻿using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VirtualAgent.Core.Dialogs;
using VirtualAgent.Core.Services;
using VirtualAgent.Core.WebApiContracts.Authentication;
using VirtualAgent.WebApi.Models;

namespace VirtualAgent.WebApi.Controllers
{
	public class MessagesController : ApiController
    {
        private const string LUIS_MODEL_ID_KEY = "LUIS_MODEL_ID";
        private const string LUIS_SUBSCRIPTION_KEY_KEY = "LUIS_SUBSCRIPTION_KEY";

        [HttpPost]
		public async Task<HttpResponseMessage> Post()
		{
			try
			{
				string body = await Request.Content.ReadAsStringAsync();
				if (string.IsNullOrWhiteSpace(body))
					throw new ArgumentException(nameof(body));

				MessagesModel messageModel = JsonConvert.DeserializeObject<MessagesModel>(body);
				AuthenticationResult result = new AuthService(messageModel.AuthenticationInfo, GetBaseUri("api/Messages/Authenticated").AbsoluteUri).GetAuthenticationResult();
                await Conversation.SendAsync(messageModel.Activity, () => new RootDialog(result));

                return Request.CreateResponse(HttpStatusCode.OK);
			}
			catch (Exception e)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, e);
			}
		}

		[HttpGet]
		[ActionName("Authenticated")]
		public HttpResponseMessage Get()
		{
			try
			{
				return Request.CreateResponse(HttpStatusCode.OK, "Authenticated.");
			}
			catch (Exception e)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, e);
			}
		}

		private Uri GetBaseUri(string path)
		{
			return new UriBuilder()
			{
				Host = Request.RequestUri.Host,
				Scheme = Request.RequestUri.Scheme,
				Port = Request.RequestUri.Port,
				Path = path
			}.Uri;
		}
	}
}
