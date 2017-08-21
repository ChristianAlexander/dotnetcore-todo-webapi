using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoWebApi.Data
{
	public interface ITodoRepository
	{
		Task<IEnumerable<Todo>> GetAll();

		Task<Todo> Get(int id);

		Task<Todo> Add(Todo todo);

		Task<Todo> Update(int id, Todo todo);

		Task Delete(int id);

		Task Clear();
	}
}
