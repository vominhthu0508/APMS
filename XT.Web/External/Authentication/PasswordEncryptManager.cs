using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using XT.BusinessService;
using XT.Model;
using XT.Utilities;
using XT.Web;

namespace XT.Web.External
{
    public interface IPasswordEncrypter
    {
        /// <summary>
        /// Hashing a password.
        /// </summary>
        /// <param name="password">Plaintext password</param>
        /// <returns>Hashed password.</returns>
        string HashPassword(string password);
        /// <summary>
        /// Compare plaintext password with hashed password.
        /// </summary>
        /// <param name="password">Plaintext password.</param>
        /// <param name="hash">Hashed password.</param>
        /// <returns>True or False.</returns>
        bool ValidatePassword(string password, string hash);
    }

    public class Pbkdf2Encrypter : IPasswordEncrypter
    {
        public string HashPassword(string password)
        {
#if DEBUG
            var stopWatch = Stopwatch.StartNew();

#endif

            var encrypted = Pbkdf2HashPassword.CreateHash(password);

#if DEBUG
            stopWatch.Stop();
            Debug.WriteLine("BCrypt Hash Password Time: {0} ms", stopWatch.ElapsedMilliseconds);
#endif
            return encrypted;
        }

        public bool ValidatePassword(string password, string hash)
        {
            return Pbkdf2HashPassword.ValidatePassword(password, hash);
        }
    }

    public static class PasswordEncryptManager
    {
        public static Account Login(string identity, string password)
        {
            if (string.IsNullOrEmpty(identity))
                throw new ArgumentNullException("identity");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            try
            {
                identity = identity.Trim();

                //@PIPE: ENCRYPTE PASSWORD
                //Không quan tâm active chưa, chỉ cần đúng pass
                //Phải dùng kèm với CheckAccount ở AccountController
                var acc = IoCConfig.Service<IAccountService>().GetAccountByIdentity(identity);

                if (acc != null)
                {
                    IPasswordEncrypter _passwordEncrypter = new Pbkdf2Encrypter();
                    if (_passwordEncrypter.ValidatePassword(password, acc.Account_Password))
                        return acc;
                }
                //login failed.
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static string EncryptPassword(string password)
        {
            IPasswordEncrypter _passwordEncrypter = new Pbkdf2Encrypter();
            return _passwordEncrypter.HashPassword(password);
        }
    }
}