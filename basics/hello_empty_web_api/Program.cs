var builder = WebApplication.CreateBuilder(args);

// Service (Container)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//* Controllers Should be Mapped
app.MapControllers();

app.Run();
