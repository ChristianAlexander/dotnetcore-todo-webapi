using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Extensions;
using TodoWebApi.Data;

namespace TodoWebApi.Controllers
{
	[Route("v1/[controller]")]
	public class TodosController : Controller
	{
		public TodosController(ITodoRepository todoRepository)
		{
			m_todoRepository = todoRepository;
		}

		[HttpGet]
		public async Task<IEnumerable<Todo>> Get()
		{
			var todos = await m_todoRepository.GetAll();

			return todos.Select(WithUrl);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			var todo = await m_todoRepository.Get(id);

			if (todo == null)
				return NotFound();

			return new ObjectResult(WithSameUrl(todo));
		}

		[HttpPost]
		public async Task<Todo> Post([FromBody]Todo todo)
		{
			var committedTodo = await m_todoRepository.Add(todo);

			return WithUrl(committedTodo);
		}

		[HttpDelete("{id}")]
		public void Delete(int id)
		{
			m_todoRepository.Delete(id);
		}

		[HttpDelete]
		public void Clear()
		{
			m_todoRepository.Clear();
		}

		[HttpPatch("{id}")]
		public async Task<IActionResult>Patch([FromBody]Todo todo, int id)
		{
			var updatedTodo = await m_todoRepository.Update(id, todo);

			if (updatedTodo == null)
				return NotFound();

			return new ObjectResult(WithSameUrl(updatedTodo));
		}

		private Todo WithUrl(Todo todo) {
			if (forceHttps)
				Request.IsHttps = true;

			todo.Url = $"{Request.GetEncodedUrl()}/{todo.Id.ToString()}";
			return todo;
		}

		private Todo WithSameUrl(Todo todo) {
			if (forceHttps)
				Request.IsHttps = true;

			todo.Url = Request.GetEncodedUrl();
			return todo;
		}

		private static readonly bool forceHttps = Environment.GetEnvironmentVariable("FORCE_HTTPS") == "1";
		private readonly ITodoRepository m_todoRepository;
	}
}
