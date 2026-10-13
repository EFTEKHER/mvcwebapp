using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvcwebapp.Data;
using mvcwebapp.Dtos;
using mvcwebapp.Models;

namespace mvcwebapp.Controllers
{
    public class AuthController(AppDbContext _context) : Controller // base -controllerbase api
    {
        // private readonly AppDbContext _context;
        // public AuthController(AppDbContext context)
        // {
        //    _context=context;  //dependency injection //previous
        // }

        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            ViewBag.SuccessMessage = TempData["Success"]; // controller -> view
            return View();
        }

      
        public async Task<IActionResult> CreateUser(UserDto dto)
        {

            if(dto==null || string.IsNullOrEmpty(dto.Username) || string.IsNullOrEmpty(dto.Password) || string.IsNullOrEmpty(dto.Email))
            {
                ViewBag.ErrorMessage="All fields are required.";
                return View("Register", dto);
            }
            var existingUser=await  _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (existingUser != null)
            {
                // ViewBag only lives for this request, so show the Register view directly (no redirect)
                ViewBag.ErrorMessage="User with this email already exists.";
                // return View("Register", dto);
                // return RedirectToAction("Register");
                return View("Register", dto);
            }

            // Create new user
            var  user = new User
            {
                Email = dto.Email,
                Password=dto.Password,
                Username=dto.Username
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // TempData survives one redirect, so the Login page can show it
            TempData["Success"] = "Registration successful. Please log in.";
            return RedirectToAction("Login");
        }
    
        public async Task <IActionResult> LoginUser(UserDto dto)
        {

            if(dto==null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
            {
                ViewBag.ErrorMessage="All fields are required.";
                return View("Login", dto);
            }
            // var isUserExists= await  _context.Users.AnyAsync(x => x.Email == dto.Email && x.Password == dto.Password);

            var isUserExists= await _context.Users.FirstOrDefaultAsync(x => x.Email==dto.Email);

            if (isUserExists == null)
            {
                ViewBag.ErrorMessage="user with this Email does not exist ";
                return View("Login", dto);
            }
            else
            {
                if (isUserExists.Password == dto.Password)
                {
                    // TempData["Success"] = "Login successful.";

                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ViewBag.ErrorMessage="Invalid password."; return View("Login", dto);
                }

            }
        }

    
    }}