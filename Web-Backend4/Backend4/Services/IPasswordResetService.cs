using System;

namespace Backend4.Services
{
    public interface IPasswordResetService
    {
        void SendResetCode(String email);

        Boolean VerifyResetCode(String email, String resetCode);

        Boolean ApplyResetCode(String email, String resetCode, String newPassword);
    }
}
