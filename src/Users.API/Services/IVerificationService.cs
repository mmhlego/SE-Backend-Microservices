using System;
namespace Users.API.Services
{
    public interface IVerificationService
    {
        bool SaveVerificationCode(string phone, string code);
        bool CheckVerificationCode(string phone, string code);
    }
}

