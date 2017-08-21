using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TodoWebApi.Controllers
{
	[Route("v1")]
	public class RootController : Controller
	{
		[HttpGet]
		public string Get()
		{
			return "TodoWebApi v1";
		}
	}
}
