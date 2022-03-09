
//using KioskStream.Core.Constants.Domain;
//using KioskStream.Core.Constants.Permissions.Dashboard;
//using KioskStream.Core.Constants.Permissions.Organization;
//using KioskStream.Core.Enums;
//using KioskStream.Data;
//using KioskStream.Data.Models;
//using KioskStream.Data.Models.Feadback;
//using KioskStream.Models;

//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Reflection;
//using System.Threading.Tasks;

//namespace KioskStream.Web.Server
//{
//    public class AppDatabaseInitializer
//    {
//        private readonly UserManager<User> _userManager;
//        private readonly RoleManager<Role> _roleManager;
//        private readonly IRepository<Permission> _permissionRepository;
//        private readonly IRepository<RolePermission> _rolePermissionRepository;
//        private readonly IRepository<AbsenceType> _absenceTypeRepository;
//        private readonly IRepository<DomainState> _domainStateRepository;
//        private readonly IRepository<AnswerOptionType> _answerOptionTypeRepository;

//        public AppDatabaseInitializer(UserManager<User> userManager,
//                                      RoleManager<Role> roleManager,
//                                      IRepository<Permission> permissionRepository,
//                                      IRepository<RolePermission> rolePermissionRepository,
//                                      IRepository<AbsenceType> absenceTypeRepository,
//                                      IRepository<DomainState> domainStateRepository,
//                                      IRepository<AnswerOptionType> answerOptionTypeRepository)
//        {
//            _userManager = userManager;
//            _roleManager = roleManager;
//            _permissionRepository = permissionRepository;
//            _rolePermissionRepository = rolePermissionRepository;
//            _absenceTypeRepository = absenceTypeRepository;
//            _domainStateRepository = domainStateRepository;
//            _answerOptionTypeRepository = answerOptionTypeRepository;
//        }

//        public async void Initialize()
//        {
//            await AssertRoleExistence(nameof(EAuthorizationRoles.Administrator));
//            await AssertRoleExistence(nameof(EAuthorizationRoles.User));
//            await AssertPermissionsExistence();
//            await AssertAdminUserExistence();
//            await AssertSimpleUserExistence();
//            await AssertAbsenceTypesExistence();
//            await AssertDomainStatesAsync();
//            await AssertAnswerOptionTypeExistence();
//        }

//        private async Task AssertAnswerOptionTypeExistence()
//        {
//            await AssertSingleAnswerOptionType(AnswerOptionTypes.DropDownMultiple);
//            await AssertSingleAnswerOptionType(AnswerOptionTypes.DropDownSingle);
//            await AssertSingleAnswerOptionType(AnswerOptionTypes.CheckBoxMultiple);
//            await AssertSingleAnswerOptionType(AnswerOptionTypes.CheckBoxSingle);
//            await AssertSingleAnswerOptionType(AnswerOptionTypes.OpenText);

//            await _answerOptionTypeRepository.SaveChangesAsync();
//        }

//        private async Task AssertSingleAnswerOptionType(string type)
//        {
//            var answerOptionType = await _answerOptionTypeRepository.Get(x => x.Description == type).FirstOrDefaultAsync();
//            if (answerOptionType != null) return;
//            answerOptionType = new AnswerOptionType
//            {
//                Description = type
//            };
//            _answerOptionTypeRepository.Insert(answerOptionType);
//        }

//        private async Task AssertDomainStatesAsync()
//        {
//            foreach (EDomainState state in Enum.GetValues(typeof(EDomainState)))
//            {
//                await CheckDomainStateComplianceAsync(state);
//            }

//            await _domainStateRepository.SaveChangesAsync();
//        }

//        private async Task CheckDomainStateComplianceAsync(EDomainState state)
//        {
//            int stateId = (int)state;
//            string stateName = Enum.GetName(typeof(EDomainState), state);

//            DomainState domainState = await _domainStateRepository.Get(x => x.Id == stateId).FirstOrDefaultAsync();
//            if (domainState != null)
//            {
//                if (domainState.Description.Equals(stateName)) return;
//                _domainStateRepository.Delete(domainState);
//                _domainStateRepository.Insert(new DomainState
//                {
//                    Id = stateId,
//                    Description = stateName
//                });
//            }
//            else
//            {
//                _domainStateRepository.Insert(new DomainState
//                {
//                    Id = stateId,
//                    Description = stateName
//                });
//            }
//        }

//        private async Task AssertPermissionsExistence()
//        {
//            AssertPermission(typeof(Dashboard.Role), nameof(EPermissionAction.Create));
//            AssertPermission(typeof(Dashboard.Role), nameof(EPermissionAction.Read));
//            AssertPermission(typeof(Dashboard.Role), nameof(EPermissionAction.Update));
//            AssertPermission(typeof(Dashboard.Role), nameof(EPermissionAction.Delete));

//            AssertPermission(typeof(Dashboard.User), nameof(EPermissionAction.Create));
//            AssertPermission(typeof(Dashboard.User), nameof(EPermissionAction.Read));
//            AssertPermission(typeof(Dashboard.User), nameof(EPermissionAction.Update));
//            AssertPermission(typeof(Dashboard.User), nameof(EPermissionAction.Delete));

//            AssertPermission(typeof(Dashboard.Department), nameof(EPermissionAction.Create));
//            AssertPermission(typeof(Dashboard.Department), nameof(EPermissionAction.Read));
//            AssertPermission(typeof(Dashboard.Department), nameof(EPermissionAction.Update));
//            AssertPermission(typeof(Dashboard.Department), nameof(EPermissionAction.Delete));

//            AssertPermission(typeof(Dashboard.Employee), nameof(EPermissionAction.Create));
//            AssertPermission(typeof(Dashboard.Employee), nameof(EPermissionAction.Read));
//            AssertPermission(typeof(Dashboard.Employee), nameof(EPermissionAction.Update));
//            AssertPermission(typeof(Dashboard.Employee), nameof(EPermissionAction.Delete));

//            AssertPermission(typeof(Organization.Absence), nameof(EPermissionAction.Create));
//            AssertPermission(typeof(Organization.Absence), nameof(EPermissionAction.Read));
//            AssertPermission(typeof(Organization.Absence), nameof(EPermissionAction.Update));
//            AssertPermission(typeof(Organization.Absence), nameof(EPermissionAction.Delete));
//            AssertPermission(typeof(Organization.Absence), nameof(EPermissionAction.Approve));
//            AssertPermission(typeof(Organization.Absence), nameof(EPermissionAction.Reject));

//            AssertPermission(typeof(Organization.Salary), nameof(EPermissionAction.Create));
//            AssertPermission(typeof(Organization.Salary), nameof(EPermissionAction.Read));
//            AssertPermission(typeof(Organization.Salary), nameof(EPermissionAction.Update));
//            AssertPermission(typeof(Organization.Salary), nameof(EPermissionAction.Delete));

//            AssertPermission(typeof(Core.Constants.Permissions.Feedback.Feedback.Template), nameof(EPermissionAction.Create));
//            AssertPermission(typeof(Core.Constants.Permissions.Feedback.Feedback.Template), nameof(EPermissionAction.Read));
//            AssertPermission(typeof(Core.Constants.Permissions.Feedback.Feedback.Template), nameof(EPermissionAction.Update));
//            AssertPermission(typeof(Core.Constants.Permissions.Feedback.Feedback.Template), nameof(EPermissionAction.Delete));

//            AssertPermission(typeof(Core.Constants.Permissions.Feedback.Feedback.Review), nameof(EPermissionAction.Create));
//            AssertPermission(typeof(Core.Constants.Permissions.Feedback.Feedback.Review), nameof(EPermissionAction.Read));
//            AssertPermission(typeof(Core.Constants.Permissions.Feedback.Feedback.Review), nameof(EPermissionAction.Update));
//            AssertPermission(typeof(Core.Constants.Permissions.Feedback.Feedback.Review), nameof(EPermissionAction.Delete));

//            await _permissionRepository.SaveChangesAsync();
//        }

//        private void AssertPermission(Type permissionType, string action)
//        {
//            if (!DoesPermissionExist(permissionType, action))
//            {
//                InsertActionIntoPermission(permissionType, action);
//            }
//        }

//        private bool DoesPermissionExist(Type permissionType, string action)
//        {
//            return _permissionRepository
//                .Get()
//                .Any(p => p.Code == permissionType.GetField(action).GetValue(permissionType).ToString());
//        }

//        private void InsertActionIntoPermission(Type permissionType, string action)
//        {
//            _permissionRepository.Insert(new Permission
//            {
//                Name = GetPermissionFieldDescription(permissionType, action),
//                Code = permissionType.GetField(action)?.GetValue(permissionType)?.ToString()
//            });
//        }

//        private static string GetPermissionFieldDescription(Type permissionType, string permissionField)
//        {
//            return permissionType.GetField(permissionField)?.GetCustomAttribute<DescriptionAttribute>()?.Description;
//        }

//        private async Task AssertSimpleUserExistence()
//        {
//            var user = await _userManager.FindByNameAsync("user");
//            if (user == null)
//            {
//                var result = await _userManager.CreateAsync(new User
//                {
//                    UserName = "user",
//                    FirstName = "User",
//                    LastName = "Useryan",
//                    Email = "user@mail.com",
//                    EmailConfirmed = true,
//                    Employee = new Employee
//                    {
//                        Position = "Software Engineer",
//                        ContractStart = DateTime.UtcNow,
//                        Salaries = new List<Salary>
//                        {
//                            new Salary
//                            {
//                                GrossAmount = 1000000,
//                                NetAmount  = 900000,
//                                AssignmentDate = DateTime.UtcNow
//                            }
//                        }
//                    }
//                }, "useruser");

//                if (result.Succeeded)
//                {
//                    user = await _userManager.FindByNameAsync("user");
//                    await _userManager.AddToRoleAsync(user, nameof(EAuthorizationRoles.User));
//                }
//            }
//        }

//        private async Task AssertAdminUserExistence()
//        {
//            var user = await _userManager.FindByNameAsync("admin");
//            if (user == null)
//            {
//                var result = await _userManager.CreateAsync(new User
//                {
//                    UserName = "admin",
//                    FirstName = "Admin",
//                    LastName = "Adminyan",
//                    Email = "admin@mail.com",
//                    EmailConfirmed = true,
//                    Employee = new Employee
//                    {
//                        Position = "Executive",
//                        ContractStart = DateTime.UtcNow,
//                        Salaries = new List<Salary>
//                        {
//                            new Salary
//                            {
//                                GrossAmount = 1500000,
//                                NetAmount  = 1300000,
//                                AssignmentDate = DateTime.UtcNow
//                            }
//                        }
//                    }
//                }, "adminadmin");

//                if (result.Succeeded)
//                {
//                    user = await _userManager.FindByNameAsync("admin");
//                    await _userManager.AddToRoleAsync(user, nameof(EAuthorizationRoles.Administrator));
//                }
//            }
//            await AssertAdminRolePermissions();
//        }

//        private async Task AssertAdminRolePermissions()
//        {
//            var role = await _roleManager.FindByNameAsync(nameof(EAuthorizationRoles.Administrator));

//            AssertPermissionForRole(typeof(Dashboard.Role), nameof(EPermissionAction.Create), role);
//            AssertPermissionForRole(typeof(Dashboard.Role), nameof(EPermissionAction.Read), role);
//            AssertPermissionForRole(typeof(Dashboard.Role), nameof(EPermissionAction.Update), role);
//            AssertPermissionForRole(typeof(Dashboard.Role), nameof(EPermissionAction.Delete), role);

//            AssertPermissionForRole(typeof(Dashboard.User), nameof(EPermissionAction.Create), role);
//            AssertPermissionForRole(typeof(Dashboard.User), nameof(EPermissionAction.Read), role);
//            AssertPermissionForRole(typeof(Dashboard.User), nameof(EPermissionAction.Update), role);
//            AssertPermissionForRole(typeof(Dashboard.User), nameof(EPermissionAction.Delete), role);

//            AssertPermissionForRole(typeof(Dashboard.Department), nameof(EPermissionAction.Create), role);
//            AssertPermissionForRole(typeof(Dashboard.Department), nameof(EPermissionAction.Read), role);
//            AssertPermissionForRole(typeof(Dashboard.Department), nameof(EPermissionAction.Update), role);
//            AssertPermissionForRole(typeof(Dashboard.Department), nameof(EPermissionAction.Delete), role);

//            AssertPermissionForRole(typeof(Dashboard.Employee), nameof(EPermissionAction.Create), role);
//            AssertPermissionForRole(typeof(Dashboard.Employee), nameof(EPermissionAction.Read), role);
//            AssertPermissionForRole(typeof(Dashboard.Employee), nameof(EPermissionAction.Update), role);
//            AssertPermissionForRole(typeof(Dashboard.Employee), nameof(EPermissionAction.Delete), role);

//            AssertPermissionForRole(typeof(Core.Constants.Permissions.Feedback.Feedback.Template), nameof(EPermissionAction.Create), role);
//            AssertPermissionForRole(typeof(Core.Constants.Permissions.Feedback.Feedback.Template), nameof(EPermissionAction.Read), role);
//            AssertPermissionForRole(typeof(Core.Constants.Permissions.Feedback.Feedback.Template), nameof(EPermissionAction.Update), role);
//            AssertPermissionForRole(typeof(Core.Constants.Permissions.Feedback.Feedback.Template), nameof(EPermissionAction.Delete), role);

//            AssertPermissionForRole(typeof(Core.Constants.Permissions.Feedback.Feedback.Review), nameof(EPermissionAction.Create), role);
//            AssertPermissionForRole(typeof(Core.Constants.Permissions.Feedback.Feedback.Review), nameof(EPermissionAction.Read), role);
//            AssertPermissionForRole(typeof(Core.Constants.Permissions.Feedback.Feedback.Review), nameof(EPermissionAction.Update), role);
//            AssertPermissionForRole(typeof(Core.Constants.Permissions.Feedback.Feedback.Review), nameof(EPermissionAction.Delete), role);

//            AssertPermissionForRole(typeof(Organization.Absence), nameof(EPermissionAction.Create), role);
//            AssertPermissionForRole(typeof(Organization.Absence), nameof(EPermissionAction.Read), role);
//            AssertPermissionForRole(typeof(Organization.Absence), nameof(EPermissionAction.Update), role);
//            AssertPermissionForRole(typeof(Organization.Absence), nameof(EPermissionAction.Delete), role);
//            AssertPermissionForRole(typeof(Organization.Absence), nameof(EPermissionAction.Approve), role);
//            AssertPermissionForRole(typeof(Organization.Absence), nameof(EPermissionAction.Reject), role);

//            AssertPermissionForRole(typeof(Organization.Salary), nameof(EPermissionAction.Create), role);
//            AssertPermissionForRole(typeof(Organization.Salary), nameof(EPermissionAction.Read), role);
//            AssertPermissionForRole(typeof(Organization.Salary), nameof(EPermissionAction.Update), role);
//            AssertPermissionForRole(typeof(Organization.Salary), nameof(EPermissionAction.Delete), role);

//            await _rolePermissionRepository.SaveChangesAsync();
//        }

//        private async Task AssertRoleExistence(string roleName)
//        {
//            var roleExists = await _roleManager.RoleExistsAsync(roleName);
//            if (roleExists) return;

//            var role = new Role
//            {
//                Name = roleName
//            };
//            var result = await _roleManager.CreateAsync(role);
//            if (!result.Succeeded) IdentityResult.Failed();
//        }

//        private void AssertPermissionForRole(Type permissionType, string action, Role role)
//        {
//            if (!DoesPermissionTiedToRole(permissionType, action, role))
//            {
//                InsertPermissionIntoRole(permissionType, action, role);
//            }
//        }

//        private bool DoesPermissionTiedToRole(Type permissionType, string action, Role role)
//        {
//            return _rolePermissionRepository
//                .Get()
//                .Any(rp =>
//                    rp.Permission.Code == permissionType
//                        .GetField(action)
//                        .GetValue(permissionType)
//                        .ToString()
//                    && rp.RoleId == role.Id);
//        }

//        private void InsertPermissionIntoRole(Type permissionType, string action, Role role)
//        {
//            _rolePermissionRepository.Insert(new RolePermission
//            {
//                PermissionId = _permissionRepository
//                    .Get(p => p.Code.Equals(permissionType.GetField(action).GetValue(permissionType).ToString()))
//                    .FirstOrDefault()
//                    .Id,
//                RoleId = role.Id
//            });
//        }

//        private static IEnumerable<string> GetTypeDescriptions(Type type)
//        {
//            var descriptions = new List<string>();
//            var names = Enum.GetNames(type);
//            foreach (var name in names)
//            {
//                var field = type.GetField(name);
//                var fields = field?.GetCustomAttributes(typeof(DescriptionAttribute), true);
//                if (fields == null) continue;
//                descriptions.AddRange(from DescriptionAttribute fd in fields select fd.Description);
//            }
//            return descriptions;
//        }

//        private async Task AssertAbsenceTypesExistence()
//        {
//            IEnumerable<string> employeeAbsenceTypeDescriptions = GetTypeDescriptions(typeof(EEmployeeAbsenceType));
//            foreach (var description in employeeAbsenceTypeDescriptions)
//            {
//                await AssertAbsenceTypeForDescription(description);
//            }
//        }

//        private async Task AssertAbsenceTypeForDescription(string description)
//        {
//            var absenceType = _absenceTypeRepository.Get(x => x.Description == description).FirstOrDefault();
//            if (absenceType != null) return;

//            absenceType = new AbsenceType
//            {
//                Description = description
//            };
//            _absenceTypeRepository.Insert(absenceType);
//            await _absenceTypeRepository.SaveChangesAsync();
//        }
//    }
//}
