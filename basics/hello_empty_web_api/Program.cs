var builder = WebApplication.CreateBuilder(args);

// Service (Container)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// for running with https and Production Mode
//* dotnet run --project hello_empty_web_api/ --launch-profile https

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//* Controllers Should be Mapped
app.MapControllers();

app.Run();
