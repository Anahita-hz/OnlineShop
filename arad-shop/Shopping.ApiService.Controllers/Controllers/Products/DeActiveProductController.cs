﻿using System.Threading.Tasks;
using System.Web.Http;
using Shopping.Commands.Commands.Products.Commands;
using Shopping.Commands.Commands.Products.Responses;
using Shopping.Infrastructure.Core.CommandBus;

namespace Shopping.ApiService.Controllers.Controllers.Products
{
	public class DeActiveProductController:ApiControllerBase
	{
		public DeActiveProductController(ICommandBus bus):base(bus)
		{
	    }
		/// <summary>
		/// غیر فعال کردن یک محصول
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
        [Authorize(Roles = "Support,Admin")]
		public async Task<IHttpActionResult> Put(DeActiveProductCommand command)
		{
			var response = await Bus.Send<DeActiveProductCommand, DeActiveProductCommandResponse>(command);
			return Ok(response);
		}
	}
}