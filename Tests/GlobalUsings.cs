global using Microsoft.VisualStudio.TestTools.UnitTesting;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.Extensions.DependencyInjection;
global using WebApplication1.Shared;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure services and dependencies
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseInMemoryDatabase(databaseName: "TestDatabase"));
builder.Services.AddScoped<DAO>();

