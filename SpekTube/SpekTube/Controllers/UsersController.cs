using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SpekTube.Models;

namespace SpekTube.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SpekTubeDbContext _dbContext;

        public UsersController(SpekTubeDbContext context)
        {
            _dbContext = context;
        }



        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _dbContext.Users.ToListAsync();
            if (users == null || users.Count == 0)
                return NotFound();
            return Ok(users);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            if (_dbContext.Users == null)
                return NotFound();

            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }



        [HttpPost("addSpekUser")]
        public IActionResult PostUser(SpekUser spekUserDetails)
        {
            var obj = _dbContext.Users.FirstOrDefault(u => u.Email == spekUserDetails.Email);
            if (obj != null)
                return Ok("Already Exists");

            byte[] passwordBytes = Encoding.UTF8.GetBytes(spekUserDetails.Password);
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                spekUserDetails.Password = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }

            // Call the stored procedure to insert user data
            _dbContext.Database.ExecuteSqlRaw("EXEC InsertSpekUser @first_name={0}, @last_name={1}, @email={2}, @mobile={3}, @password={4}",
                spekUserDetails.First_Name, spekUserDetails.Last_Name, spekUserDetails.Email, spekUserDetails.Mobile, spekUserDetails.Password);

            return CreatedAtAction("GetUser", new { id = spekUserDetails.Id }, spekUserDetails);
        }

     
        [HttpPost("signInSpekUser")]
        public IActionResult Login(loginSpekUsers request)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user != null)
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(request.Password);
                using (var sha256 = SHA256.Create())
                {
                    byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                    string hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                    SqlParameter loginSuccessParam = new SqlParameter("@loginSuccess", SqlDbType.Bit);
                    loginSuccessParam.Direction = ParameterDirection.Output;

                    _dbContext.Database.ExecuteSqlRaw("EXEC loginSpekUser @email = {0}, @password = {1}, @loginSuccess = @loginSuccess OUT",
                        request.Email, hashedPassword, loginSuccessParam);

                    bool loginSuccess = (bool)loginSuccessParam.Value;

                    if (loginSuccess)
                    {
                        return Ok(user);
                    }
                }
            }

            return Unauthorized(new { message = "Invalid usernamee or password" });
        }


        [HttpPost("signInGoogleUser")]
        public IActionResult signInGoogleUsers(User googleUserData)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == googleUserData.Email);
            if (user == null)
            {
                _dbContext.Users.Add(googleUserData);
                _dbContext.SaveChanges();
                return CreatedAtAction(nameof(GetUser), new { Id = googleUserData.Id }, googleUserData);
            }

            user.Oauth_Provider = googleUserData.Oauth_Provider;
            user.Oauth_Id = googleUserData.Oauth_Id;
            user.First_Name = googleUserData.First_Name;
            user.Last_Name = googleUserData.Last_Name;
            user.Email = googleUserData.Email;
            user.Picture = googleUserData.Picture;
            _dbContext.SaveChanges();

            return Ok(user);

        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_dbContext.Users == null)
            {
                return NotFound();
            }
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

      


    }
    public class SpekUser
    {
        public int Id { get; set; }
        public string? First_Name { get; set; }
        public string? Last_Name { get; set; }
        public string? Email { get; set; }
        public long Mobile { get; set; }
        public string Password { get; set; }
    }
    public class loginSpekUsers
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }


}
