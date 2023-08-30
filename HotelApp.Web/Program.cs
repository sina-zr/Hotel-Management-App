using DataAccessLibrary.Data;
using DataAccessLibrary.DataBase;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddTransient<ISQLiteDataAccess, SQLiteDataAccess>();

string dbChoice = builder.Configuration.GetValue<string>("DatabaseChoice").ToLower();
if (dbChoice == "sql")
{
    builder.Services.AddSingleton<IDataBaseData, SqlData>();
}
else if (dbChoice == "sqlite")
{
    builder.Services.AddSingleton<IDataBaseData, SQLiteData>();
}
else
{
    // Fallback / Default value
    builder.Services.AddSingleton<IDataBaseData, SqlData>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
