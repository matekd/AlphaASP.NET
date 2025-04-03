using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("alpha")));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMemberService, MemberService>();

builder.Services.AddIdentity<MemberEntity, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8;
})
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/auth/login";
    options.LogoutPath = "/auth/logout";
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Administrator", "User" };

    foreach (var roleName in roleNames)
    {
        var roleExists = await roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
            await roleManager.CreateAsync(new IdentityRole(roleName));
    }

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<MemberEntity>>();
    var user = new MemberEntity { UserName = "admin@domain.com", Email = "admin@domain.com" };

    var memberExist = await userManager.Users.AnyAsync(x => x.Email == user.Email);
    if (!memberExist)
    {
        var result = await userManager.CreateAsync(user, "BytMig123!");
        if (result.Succeeded)
            await userManager.AddToRoleAsync(user, "Administrator");
    }
}

app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Projects}/{action=Index}")
    .WithStaticAssets();
app.Run();
