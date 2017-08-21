using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoWebApi.Data.InMemory
{
	public class TodoRepository : ITodoRepository
	{
		public TodoRepository()
		{
			m_todos = new Dictionary<int, Todo>();
			m_nextId = 0;
			m_idLock = new Object();
			m_dictionaryLock = new Object();
		}

		public Task<IEnumerable<Todo>> GetAll()
		{
			return Task.FromResult(m_todos
									.ToList()
									.Select(pair => pair.Value)
									.Select(todo => todo.Clone()));
		}

		public Task<Todo> Get(int id)
		{
			return Task.FromResult(m_todos.GetValueOrDefault(id, null)?.Clone());
		}

		public Task<Todo> Add(Todo todo)
		{
			if (todo.Id != null)
				throw new InvalidOperationException("New Todo ID must be null. ID will be assigned by the repository.");

			int id;
			lock(m_idLock)
			{
				id = m_nextId++;
			}

			var repositoryTodo = todo.Clone();

			repositoryTodo.Id = id;

			if (!repositoryTodo.Completed.HasValue)
				repositoryTodo.Completed = false;

			lock(m_dictionaryLock)
			{
				m_todos[id] = repositoryTodo;
			}

			return Task.FromResult(repositoryTodo);
		}

		public Task<Todo> Update(int id, Todo todo)
		{
			var resultTodo = m_todos.GetValueOrDefault(id, null)?.UpdateFieldsWhereNotNull(todo);

			if (resultTodo == null)
				return null;

			lock(m_dictionaryLock)
			{
				m_todos[id] = resultTodo;
			}

			return Task.FromResult(resultTodo);
		}

		public Task Delete(int id)
		{
			lock (m_dictionaryLock)
			{
				m_todos.Remove(id);
			}

			return Task.FromResult<object>(null);
		}

		public Task Clear()
		{
			lock(m_dictionaryLock)
			{
				m_todos.Clear();
			}

			return Task.FromResult<object>(null);
		}

		private Object m_idLock;
		private int m_nextId;
		private Object m_dictionaryLock;
		private Dictionary<int, Todo> m_todos;
	}
}
