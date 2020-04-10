using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        
        public async Task<User> Login(string username, string password)
        {
            User user = await _context.Users.FirstOrDefaultAsync(user => user.Username == username);

            if (user == null)
                return null;

            if (!PasswordHashVerified(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        private bool PasswordHashVerified(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (HMACSHA512 userHMAC = new HMACSHA512(passwordSalt))
            {
                byte[] computedHash = userHMAC.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                //Compare each byte in both hashes
                for (int i = 0; i < computedHash.Length; i++)
                {
                    //if password hashes don't match, password is incorrect
                    if (computedHash[i] != userHMAC.Hash[i])
                        return false;
                }
            }

            //The hashes matched, the passwords match
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (HMACSHA512 newUserHMAC = new HMACSHA512())
            {
               passwordSalt = newUserHMAC.Key;
               passwordHash = newUserHMAC.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(user => user.Username == username))
                return true;
            
            return false;
        }
    }
}