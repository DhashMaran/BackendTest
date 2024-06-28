var builder = WebApplication.CreateBuilder(args);

// Add controller services
builder.Services.AddControllers();

// Configure the CORS for front end
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("https://localhost:7147")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();


app.UseStaticFiles();


app.UseRouting();

// Apply CORS rules
app.UseCors("AllowSpecificOrigin");


app.UseAuthorization();


app.MapControllers();


app.Run();
