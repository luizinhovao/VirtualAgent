﻿using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace BotApplication.Controllers
{
	public class MessageController : ApiController
	{
		public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
		{
			if (activity.Type == ActivityTypes.Message)
			{
				using (HttpClient client = new HttpClient())
				{
					string myContent = JsonConvert.SerializeObject(new
					{
						AuthenticationInfo = new
						{
							MicrosoftAppId = "af3119fb-f1fb-478e-9dd3-ebc25651381b",
							MicrosoftPassword = "ii$-D_IGtK-k^X#B",
							Channel = "skype",
							AdTenant = "5cda9700-603b-4822-8f63-3b9471b0d8b1",
							AdClient = "947fa91c-0996-467d-837e-07b17a3146c4",
							UserId = activity.From.Id
						},
						Activity = activity
					});
					byte[] buffer = Encoding.UTF8.GetBytes(myContent);
					ByteArrayContent byteContent = new ByteArrayContent(buffer);
					await client.PostAsync("http://localhost:7071/api/Messages", byteContent);
				}
			}

			return Request.CreateResponse(HttpStatusCode.OK);
		}
	}
}
