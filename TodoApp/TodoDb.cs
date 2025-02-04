using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

class TodoContext(DbContextOptions options) : DbContext(options)
{
	public DbSet<Todo> Todos { get; set; } = null!;
}

class Todo {
		[Key]
		public Guid Id { get; set; }
		public string Task { get; set; } = null!;
		public bool IsCompleted { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

