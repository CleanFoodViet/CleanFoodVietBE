using CleanFoodVietAPI.Application.DTOs.AuthDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IAccountService
    {

        #region Authentication Functions
        Task<AuthDTO> Login(LoginDTO loginData);
        Task<AuthDTO> Register(RegisterDTO registerData);
        #endregion
    }
}
