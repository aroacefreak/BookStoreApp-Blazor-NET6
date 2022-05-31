using Microsoft.AspNetCore.Components;
using BookStoreApp.Blazor.Server.UI.Services.Authentication;
using BookStoreApp.Blazor.Server.UI.Services.Base;

namespace BookStoreApp.Blazor.Server.UI.Pages.Users
{
	public partial class Login
	{
		[Inject] private IAuthenticationService authService { get; set; }
		[Inject] private NavigationManager _navManager { get; set; }
		LoginUserDto LoginModel = new LoginUserDto();
		string message = string.Empty;
		public async Task HandleLogin()
		{
			var response = await authService.AuthenticateAsync(LoginModel);

			if (response.Success)
			{
				_navManager.NavigateTo("/");
			}
			message = response.Message;
		}
	}
}
