using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using DaingApp.API.Dtos;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthoRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public AuthController(IAuthoRepository repo, IConfiguration config, IMapper mapper)
        {
            _mapper = mapper;
            _config = config;
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegistrationDto userForRegistrationDto)
        {
            // if (!ModelState.IsValid)
            // return BadRequest(ModelState);

            userForRegistrationDto.UserName = userForRegistrationDto.UserName.ToLower();
            if (await _repo.UserExists(userForRegistrationDto.UserName))
                return BadRequest("User Already exists");

            var userToCreate = _mapper.Map<User>(userForRegistrationDto);
            var createdUser = await _repo.Register(userToCreate, userForRegistrationDto.Password);
            var userToReturn = _mapper.Map<UserForDetailedDto>(createdUser);
            return CreatedAtRoute("GetUser", new { Controller = "Users", id = createdUser.Id }, userToReturn);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _repo.Login(userForLoginDto.UserName, userForLoginDto.Password);

            if (userFromRepo == null)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
new Claim(ClaimTypes.NameIdentifier , userFromRepo.Id.ToString()),
new Claim(ClaimTypes.Name , userFromRepo.UserName.ToLower())
};
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var user = _mapper.Map<UserForListDto>(userFromRepo);
            return Ok(new
            {
                token = tokenHandler.WriteToken(token) , user
            });

        }
    }
}