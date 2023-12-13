using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using CarHub_Web.Service.IService;
using CarHub_Web.Models.Dto;
using CarHub_Web.Models;
using CarHub_Utility;
using System.IdentityModel.Tokens.Jwt;

namespace CarHub_Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO obj = new();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO obj)
        {
            APIResponse response = await _authService.LoginAsync<APIResponse>(obj);
            if (response != null && response.IsSuccess)
            {
                LoginResponseDTO model = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));

                var handler = new JwtSecurityTokenHandler();
                //token male se
                var jwt = handler.ReadJwtToken(model.Token);
                //token mathin value leva mate ni mujab
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "unique_name").Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, jwt.Claims.FirstOrDefault(u => u.Type == "nameid").Value));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


                HttpContext.Session.SetString(SD.SessionToken, model.Token);
				if (principal.IsInRole("Customer"))
				{
					return RedirectToAction("Index", "Home", new { area = "Customer" });
				}
				else
				{
					return RedirectToAction("Index", "Home", new { area = "Admin" });
				}
				
            }
            else
            {
                ModelState.AddModelError("CustomError", response.ErrorMessages.FirstOrDefault());
                return View(obj);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterationRequestDTO obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Password != obj.ConfirmPassword)
                {
                    TempData["error"] = "Password not match.";
                    return View(obj);
                }
                APIResponse result = await _authService.RegisterAsync<APIResponse>(obj);
                if (result != null && result.IsSuccess)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    if (result.ErrorMessages.Count > 0)
                    {
                        // ModelState.AddModelError("CustomError", response.ErrorMessages.FirstOrDefault());
                        TempData["error"] = result.ErrorMessages.FirstOrDefault();
                        return View(obj);
                    }

                }

            }
            return View(obj);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.SessionToken, "");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}



