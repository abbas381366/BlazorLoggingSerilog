using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Serilog;
using System.Security.Claims;
namespace BlazorLoggingSeriLog.Components.Pages;

public partial class LoginPage
{
    [SupplyParameterFromForm] public string Username { get; set; }
    [SupplyParameterFromForm] public string Password { get; set; }
    [CascadingParameter] public HttpContext HttpContext { get; set; }
    string message;

    protected override void OnInitialized()
    {
        Log.Logger.Information("hello From {page}","login Page");
    }
    async Task Login()
    {
        if (Username != "user1" && Password != "pass" ||
            Username != "user2" && Password != "pass")
        {
            message = "نام کاربری یا کلمه عبور اشتباه است";
            Log.Logger.Information("Failed Loggin ... User : {User} Pass : {Pass}",Username,Password);
            return;
        }

        if (Username == "user1")
        {

            await HttpContext.SignInAsync(new ClaimsPrincipal(new[]
            {
            new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "abbas hasanzadeh"),
                new Claim("Channel","CodeEasyMS")
            },"Cookie")
            }
            ));
        }
        else
        {
            await HttpContext.SignInAsync(new ClaimsPrincipal(new[]
            {
            new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "jafar asghari"),
                new Claim("Channel","CodeEasyMS")
            },"Cookie")
            }
            ));
        }
    }
}