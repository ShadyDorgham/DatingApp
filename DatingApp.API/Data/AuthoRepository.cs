using System;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthoRepository : IAuthoRepository
    {
        private readonly DataContext _context;
        public AuthoRepository(DataContext _context)
        {
            this._context = _context;

        }
        public async Task<bool> UserExists( string userName)
        {
            if (await _context.Users.AnyAsync(x=>x.UserName == userName))
            return true;

            return false;
        }

        public async Task<User> Login(string userName, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);

            if (user == null)
                return null;


            if (!VerifyPasswordHash(password, user.PasswordHash, user.PaswordSalt))
                return null;

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] paswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(paswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PaswordSalt = passwordSalt;
            _context.Add(user);
            _context.SaveChanges();
            return user;

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }

        public Task<User> Login(User userName, string password)
        {
            throw new NotImplementedException();
        }

       
    }
}