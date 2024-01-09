using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PublicUtilities.Models;
using System.Diagnostics;
using System.Net;

namespace PublicUtilities.Data
{
    public class Seed
    {
        public static async Task SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                //Users
                if (!context.Users.Any())
                {
                    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                    var newUser = new AppUser
                    {
                        UserName = "fakeMail@mail.com",
                        Email = "fakeMail@mail.com",
                        PhoneNumber = "1234567890",
                        Surname = "UserSurname",
                        Name = "UserName",
                        Patronymic = "UserPatronymic",
                        UsersPlacesOfResidenceId = new List<UsersPlacesOfResidence>() {
                            new UsersPlacesOfResidence
                            {
                                PlacesOfResidence = new PlacesOfResidence
                                {
                                    Apartment = "UserApartment",
                                    House = "UserHouse",
                                    Streets = new Streets{ Name = "UserStreets"},
                                    Notifications = new List<Notifications>
                                    {
                                        new Notifications
                                        {
                                            Header = "Header",
                                            Text = "Text",
                                        }
                                    },
                                    Indicators = new List<Indicators>
                                    {
                                         new Indicators
                                         {
                                             Indicator = "Indicators",
                                             Date = DateTime.Now,
                                             Price = 1204.25m,
                                             Paid = false,
                                             Utilities = new Utilities
                                             {
                                                 Name = "Name",
                                                 Price = 12.75m,
                                                 Date = DateTime.Now,
                                             }
                                         }
                                    }

                                }
                            }
                        },
                        UsersStatementsId = new List<UsersStatements>() {
                            new UsersStatements
                            {
                                Statements = new Statements
                                {
                                    Text = "UserText",
                                    StatementUrl = "https://res.cloudinary.com/dihwzdmiw/image/upload/v1703158790/viveswvd2bkjzwx6ofej.png",
                                    Status = Enum.StatementsStatus.Зареєстровано,
                                    Date = DateTime.Now,
                                    Department = new Departments{ Name = "UserDepartment"},
                                    StatementsType = new StatementsTypes { SignatureCount = 50, Type = "UserStatementsTypes" }
                                }
                            }
                        }
                    };
                    await userManager.CreateAsync(newUser, "1fakeMail@mail.com");
                    await userManager.AddToRoleAsync(newUser, UserRoles.User);

                    var newAdminUser = new AppUser
                    {
                        UserName = "fakeAdminMail@mail.com",
                        Email = "fakeAdminMail@mail.com",
                        PhoneNumber = "0987654321",
                        Surname = "AdminSurname",
                        Name = "AdminName",
                        Patronymic = "AdminPatronymic",
                    };
                    await userManager.CreateAsync(newAdminUser, "1fakeAdminMail@mail.com");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);

                    context.News.Add(
                        new News
                        {
                            Header = "Header",
                            Text = "Text",
                            Date = DateTime.Now,
                            ImageUrl = "https://res.cloudinary.com/dihwzdmiw/image/upload/v1691596383/pbnuvibisbet6jc4qzst.png"
                        });

                    context.SaveChanges();
                }
            }
        }
    }
}
