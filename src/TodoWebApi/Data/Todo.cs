using System;

namespace TodoWebApi.Data
{
	public sealed class Todo
	{
		public int? Id { get; set; }
		public int? Order { get; set; }
		public string Title { get; set; }
		public string Url { get; set; }
		public bool? Completed { get; set; }

		public Todo Clone()
		{
			return MemberwiseClone() as Todo;
		}

		public Todo UpdateFieldsWhereNotNull(Todo todo) {
			var result = this.Clone();

			if (todo == null)
				return result;

			if (todo.Order.HasValue)
				result.Order = todo.Order;

			if (todo.Title != null)
				result.Title = todo.Title;

			if (todo.Url != null)
				result.Url = todo.Url;

			if (todo.Completed.HasValue)
				result.Completed = todo.Completed;

			return result;
		}

		public override bool Equals(object obj)
		{
			var todo = obj as Todo;
			return (todo != null) && (Id == todo.Id);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}
