using Company.Test.BLL;
using Company.Test.BLL.Interfaces;
using Company.Test.BLL.Repositories;
using Company.Test.DAL.Data.Contexts;
using Company.Test.DAL.Models;
using Company.Test.PL.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});



//builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>();
//builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()    //Allow DI to Identity user , role
    .AddEntityFrameworkStores<ApplicationDbContext>()
	 .AddDefaultTokenProviders();           //Allow DI to Store

builder.Services.ConfigureApplicationCookie(configure =>
{
    configure.LoginPath = "/Account/SignIn";
});

builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();


builder.Services.AddAutoMapper(typeof(EmployeeProfile));
builder.Services.AddAutoMapper(typeof(Departmentprofile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthorization();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
