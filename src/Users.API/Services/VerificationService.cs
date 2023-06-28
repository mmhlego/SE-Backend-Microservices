using System;
using Users.API.Data;
using Users.API.Models;

namespace Users.API.Services
{
    public class VerificationService : IVerificationService
    {
        private readonly UsersContext _context;
        public VerificationService(UsersContext context)
        {
            _context = context;
        }
        public bool SaveVerificationCode(string phone, string code)
        {
            List<Verify> v = _context.verifies.Where(c => c.PhoneNumber == phone).ToList();
            _context.verifies.RemoveRange(v);
            _context.SaveChanges();
            Verify verify = new Verify()
            {
                Id = Guid.NewGuid(),
                Code = code,
                PhoneNumber = phone
            };
            _context.verifies.Add(verify);
            _context.SaveChanges();
            return true;

        }
        public bool CheckVerificationCode(string phone, string code)
        {
            var check = _context.verifies.FirstOrDefault(c => c.PhoneNumber == phone && c.Code == code);
            if (check == null)
                return false;
            _context.verifies.Remove(check);
            _context.SaveChanges();
            return true;

        }

    }
}

