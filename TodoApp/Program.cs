using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("./config.json");
builder.Services.AddDbContextPool<TodoContext>(opt => 
	opt.UseNpgsql(builder.Configuration.GetConnectionString("TodoDb")));

ConnectionMultiplexer redis = await ConnectionMultiplexer.ConnectAsync("localhost");

var app = builder.Build();

app.MapPost("/{id}", async (Guid id, HttpContext httpContext, TodoContext context) => {
	var checke = httpContext.Request.Form["checked"];
	await context.Todos.Where(todo => todo.Id == id).ExecuteUpdateAsync(setters => setters.SetProperty(todo => todo.IsCompleted, checke == "on"));
	await context.SaveChangesAsync();
	httpContext.Response.Redirect("/");
});

app.MapPost("/delete/{id}", async (Guid id, HttpContext httpContext, TodoContext context) => {
	await context.Todos.Where(todo => todo.Id == id).ExecuteDeleteAsync();
	await context.SaveChangesAsync();
	httpContext.Response.Redirect("/");
});

app.MapPost("/add", async (HttpContext httpContext, TodoContext context) => {
	var todo = httpContext.Request.Form["todo"];
	context.Todos.Add(new Todo { 
		Id = Guid.NewGuid(),
		Task = todo!,
		IsCompleted = false
	});

	await context.SaveChangesAsync();
	httpContext.Response.Redirect("/");
});

string CreateTodo(Todo todo) => @$"
	<li>
		<form action='/{todo.Id}' method='post'>
			{todo.Task}
			<input type='checkbox' name='checked' {(todo.IsCompleted ? "checked" : "")} onChange='this.form.submit()' />
		</form>
		<form action='/delete/{todo.Id}' method='post'>
				<input type='submit' value='Delete' />
			</form>
	</li>
";

app.MapGet("/", async (HttpContext httpContext, TodoContext context) => {
	IDatabase db = redis.GetDatabase();
	string? everything = await db.StringGetAsync("everything-works");

	await db.StringSetAsync("everything-works", "true");

	if (everything == null)
	{
		Console.WriteLine("Setting up db");
		await db.StringSetAsync("everything-works", "true");
		await context.Database.EnsureCreatedAsync();
	}
	var todos = await context.Todos.OrderBy(t => t.CreatedAt).ToListAsync();

	httpContext.Response.Headers.Append("Content-Type", "text/html");
	await httpContext.Response.WriteAsync(
		@$"
			<!DOCTYPE html>
			<head>
				<title>TodoApp</title>
				<style>
					form {{
						display: inline-block;
					}}
				</style>
			</head>
			<body>
				<h1>TodoApp</h1>
				<form action=""/add"" method='post'>
					<input type='text' name='todo' placeholder='Enter your todo' />
					<button type='submit'>Add</button>
				</form>
				<ul>
				{string.Join("", todos.Select(todo => CreateTodo(todo)))}
				</ul>
			</body
			"
		);
});

app.Run();
