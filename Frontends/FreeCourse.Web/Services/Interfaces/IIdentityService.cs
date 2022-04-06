using System;
using System.Threading.Tasks;
using FreeCourse.Shared.Dtos;
using IdentityModel.Client;

namespace FreeCourse.Web.Services.Interfaces
{
	public interface IIdentityService
	{

		Task<Response<bool>> SignIn();

		Task<TokenResponse> GetAccesTokenByRefreshToken();

		Task RevokeRefreshToken();
        
	}
}

