using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNet.Identity;
using DecouplingAspNetIdentity.Business;
using DecouplingAspNetIdentity.Infrastructure;
using DecouplingAspNetIdentity.Models;

namespace DecouplingAspNetIdentity.Web.Models.Identity
{
    public class UserStore : IUserLoginStore<ApplicationUser, int>,
        IUserClaimStore<ApplicationUser, int>,
        IUserRoleStore<ApplicationUser, int>,
        IUserPasswordStore<ApplicationUser, int>,
        IUserSecurityStampStore<ApplicationUser, int>,
        IUserLockoutStore<ApplicationUser, int>,
        IUserStore<ApplicationUser, int>,
        IQueryableUserStore<ApplicationUser, int>,
        IUserTwoFactorStore<ApplicationUser, int>,
        IUserEmailStore<ApplicationUser, int>,
        IUserPhoneNumberStore<ApplicationUser, int>,
        IDisposable
    {
        private readonly IUserService _userService;

        private readonly IExternalLoginService _externalLoginService;

        private readonly IRoleService _roleService;

        private readonly IMapper _mapper;


        public UserStore(IMapper mapper,
            IUserService userService,
            IExternalLoginService externalLoginService,
            IRoleService roleService)
        {
            Debug.Assert(mapper != null);
            Debug.Assert(userService != null);
            Debug.Assert(externalLoginService != null);
            Debug.Assert(roleService != null);

            _mapper = mapper;
            _userService = userService;
            _externalLoginService = externalLoginService;
            _roleService = roleService;
        }

        #region IUserStore<IdentityUser, int> Members

        public Task CreateAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var u = GetUser(user);
            u.ObjectState = EntityObjectState.Added;
            _userService.Save(u);
            user.Id = u.Id;

            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var u = GetUser(user);
            _userService.Delete(u);
            return Task.FromResult<object>(null);
        }

        public Task<ApplicationUser> FindByIdAsync(int userId)
        {
            var user = GetUserById(userId);
            return Task.FromResult(GetIdentityUser(user));
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var user = GetUserByUserName(userName);
            return Task.FromResult(GetIdentityUser(user));
        }

        public Task UpdateAsync(ApplicationUser applicationUser)
        {
            if (applicationUser == null)
                throw new ArgumentException(nameof(applicationUser));

            var user = GetUserById(applicationUser.Id);
            if (user == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.",
                    nameof(applicationUser));

            user = GetUser(applicationUser);

            _userService.Save(user);
            return Task.FromResult<object>(null);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // Dispose does nothing since we want Unity to manage the lifecycle of our Unit of Work
        }

        #endregion

        #region IUserClaimStore<IdentityUser, int> Members

        public Task AddClaimAsync(ApplicationUser user, System.Security.Claims.Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (claim == null)
                throw new ArgumentNullException(nameof(claim));

            var u = GetUserById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            var c = new Claim
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                User = u
            };
            u.Claims.Add(c);
            _userService.Save(u);
            return Task.FromResult<object>(null);
        }

        public Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var u = GetUserById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            return Task.FromResult<IList<System.Security.Claims.Claim>>(u.Claims
                .Select(x => new System.Security.Claims.Claim(x.ClaimType, x.ClaimValue)).ToList());
        }

        public Task RemoveClaimAsync(ApplicationUser user, System.Security.Claims.Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (claim == null)
                throw new ArgumentNullException(nameof(claim));

            var u = GetUserById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            var c = u.Claims.FirstOrDefault(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
            u.Claims.Remove(c);

            _userService.Save(u);
            return Task.FromResult<object>(null);
        }

        #endregion

        #region IUserLoginStore<IdentityUser, int> Members

        public Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (login == null)
                throw new ArgumentNullException(nameof(login));

            var u = GetUserById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            var l = new ExternalLogin
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                User = u
            };
            u.Logins.Add(l);

            _userService.Save(u);
            return Task.FromResult<object>(null);
        }

        public Task<ApplicationUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
                throw new ArgumentNullException(nameof(login));

            var identityUser = default(ApplicationUser);

            var l = _externalLoginService.GetByProviderAndKey(login.LoginProvider, login.ProviderKey);
            if (l != null)
                identityUser = GetIdentityUser(l.User);

            return Task.FromResult(identityUser);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var u = GetUserById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            return Task.FromResult<IList<UserLoginInfo>>(u.Logins
                .Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey)).ToList());
        }

        public Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (login == null)
                throw new ArgumentNullException(nameof(login));

            var u = GetUserById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            var l = u.Logins.FirstOrDefault(x => x.LoginProvider == login.LoginProvider &&
                                                 x.ProviderKey == login.ProviderKey);
            u.Logins.Remove(l);

            _userService.Save(u);
            return Task.FromResult<object>(null);
        }

        #endregion

        #region IUserRoleStore<IdentityUser, int> Members

        public Task AddToRoleAsync(ApplicationUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: roleName.");

            var u = GetUserById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));
            var r = _roleService.FindByName(roleName);
            if (r == null)
                throw new ArgumentException("roleName does not correspond to a Role entity.", nameof(roleName));

            u.Roles.Add(r);
            _userService.Save(u);

            return Task.FromResult<object>(null);
        }

        public Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var u = GetUserById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            return Task.FromResult<IList<string>>(u.Roles.Select(x => x.Name).ToList());
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role.");

            var u = GetUserById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            return Task.FromResult(u.Roles.Any(x => x.Name == roleName));
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role.");

            var u = GetUserById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            var r = u.Roles.FirstOrDefault(x => x.Name == roleName);
            u.Roles.Remove(r);

            _userService.Save(u);

            return Task.FromResult<object>(null);
        }

        #endregion

        #region IUserPasswordStore<IdentityUser, int> Members

        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        #endregion

        #region IUserSecurityStampStore<IdentityUser, int> Members

        public Task<string> GetSecurityStampAsync(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(ApplicationUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        #endregion

        #region Private Methods

        private User GetUser(ApplicationUser identityUser)
        {
            return identityUser == null ? null : _mapper.Map<User>(identityUser);
        }

        private ApplicationUser GetIdentityUser(User user)
        {
            return user == null ? null : _mapper.Map<ApplicationUser>(user);
        }

        #endregion

        public Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(GetUserById(user.Id).AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(GetUserById(user.Id).LockoutEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var dbUser = GetUserById(user.Id);
            return Task.FromResult(new DateTimeOffset(
                DateTime.SpecifyKind(dbUser != null && dbUser.LockoutEndDateUtc.HasValue
                    ? dbUser.LockoutEndDateUtc.Value
                    : DateTime.UtcNow.AddDays(-1), DateTimeKind.Utc)));
        }

        public Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            return UpdateUserData<int>(user, dbUser =>
            {
                dbUser.AccessFailedCount++;
                return dbUser;
            }, dbUser => dbUser.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            return UpdateUserData<object>(user, dbUser =>
            {
                dbUser.AccessFailedCount = 0;
                return dbUser;
            });
        }

        public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
        {
            return UpdateUserData<object>(user, dbUser =>
            {
                dbUser.LockoutEnabled = enabled;
                return dbUser;
            });
        }

        public Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset lockoutEnd)
        {
            return UpdateUserData<object>(user, dbUser =>
            {
                dbUser.LockoutEndDateUtc = lockoutEnd.UtcDateTime;
                return dbUser;
            });
        }

        private User GetUserById(int userId)
        {
            var user = _userService.GetUserWithRelatedDataById(userId);
            if (user != null && string.IsNullOrEmpty(user.SecurityStamp))
            {
                user.SecurityStamp = Guid.NewGuid().ToString();
            }

            return user;
        }

        private User GetUserByUserName(string userName)
        {
            var user = _userService.FindByUserName(userName);
            if (user != null && string.IsNullOrEmpty(user.SecurityStamp))
            {
                user.SecurityStamp = Guid.NewGuid().ToString();
            }

            return user;
        }

        private User GetUserByEmail(string email)
        {
            var user = _userService.GetAll()
                .FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (user != null && string.IsNullOrEmpty(user.SecurityStamp))
            {
                user.SecurityStamp = Guid.NewGuid().ToString();
            }

            return user;
        }

        private Task<T> UpdateUserData<T>(ApplicationUser user, Func<User, User> userUpdateOperation,
            Func<User, T> userUpdateResult = null)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            User dbUser;
            if (user.Id > 0)
            {
                dbUser = GetUserById(user.Id);

                dbUser.ObjectState = EntityObjectState.Modified;
                dbUser = userUpdateOperation?.Invoke(dbUser);
                _userService.Save(dbUser);
            }
            else
            {
                dbUser = GetUser(user);
                dbUser = userUpdateOperation?.Invoke(dbUser);
            }

            return userUpdateResult == null
                ? Task.FromResult(default(T))
                : Task.FromResult(userUpdateResult.Invoke(dbUser));
        }

        public Task SetEmailAsync(ApplicationUser user, string email)
        {
            return UpdateUserData<object>(user, dbUser =>
            {
                dbUser.Email = email;
                return dbUser;
            });
        }

        public Task<string> GetEmailAsync(ApplicationUser user)
        {
            return Task.FromResult(string.IsNullOrEmpty(user.Email) ? GetUserById(user.Id)?.Email : user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            return Task.FromResult(GetUserById(user.Id).EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            return UpdateUserData<object>(user, dbUser =>
            {
                dbUser.EmailConfirmed = confirmed;
                return dbUser;
            });
        }

        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return Task.FromResult(GetIdentityUser(GetUserByEmail(email)));
        }

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            return UpdateUserData<object>(user, dbUser =>
            {
                dbUser.TwoFactorEnabled = enabled;
                return dbUser;
            });
        }

        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            return Task.FromResult<bool>(GetUserById(user.Id).TwoFactorEnabled);
        }

        public Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber)
        {
            return UpdateUserData<object>(user, dbUser =>
            {
                dbUser.PhoneNumber = phoneNumber;
                return dbUser;
            });
        }

        public Task<string> GetPhoneNumberAsync(ApplicationUser user)
        {
            return Task.FromResult<string>(GetUserById(user.Id).PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user)
        {
            return Task.FromResult<bool>(GetUserById(user.Id).PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            return UpdateUserData<object>(user, dbUser =>
            {
                dbUser.PhoneNumberConfirmed = confirmed;
                return dbUser;
            });
        }

        public IQueryable<ApplicationUser> Users => _userService.GetAll().ProjectTo<ApplicationUser>();
    }
}