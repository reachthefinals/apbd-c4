using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace LegacyApp
{
    public class UserService
    {
        private int? GetCreditLimit(UserCreditService userCreditService, User user)
        {
            try
            {
                int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                return creditLimit;
            }
            catch (ArgumentException e)
            {
                return null;
            }
        }
        public bool AddUser(
            [NotNull] string firstName,
            [NotNull] string lastName,
            [NotNull] string email,
            DateTime dateOfBirth,
            int clientId
        )
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return false;
            }

            if (!UserUtil.IsValidMail(email))
            {
                return false;
            }

            var age = UserUtil.RelativeAge(dateOfBirth);

            if (age < 21)
            {
                return false;
            }

            var clientRepository = new ClientRepository();
            Client client;
            try
            {
                client = clientRepository.GetById(clientId);
            }
            catch (ArgumentException a)
            {
                return false;
            }

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            switch (client.Type)
            {
                case "VeryImportantClient":
                {
                    user.HasCreditLimit = false;
                    break;
                }
                case "ImportantClient":
                {
                    using (var userCreditService = new UserCreditService())
                    {
                        int? creditLimit = GetCreditLimit(userCreditService, user);
                        if (!creditLimit.HasValue) return false;
                        user.CreditLimit = creditLimit.Value * 2;
                    }

                    break;
                }
                default:
                {
                    user.HasCreditLimit = true;
                    using (var userCreditService = new UserCreditService())
                    {
                        int? creditLimit = GetCreditLimit(userCreditService, user);
                        if (!creditLimit.HasValue) return false;
                        user.CreditLimit = creditLimit.Value;
                    }
                    break;
                }
            }

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }
    }
}