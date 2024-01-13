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

                    var waterUtilities = new Utilities
                    {
                        Name = "Вода",
                        Price = 25.88m,
                        Date = DateTime.Now,
                    };

                    var gasUtilities = new Utilities
                    {
                        Name = "Газ",
                        Price = 7.96m,
                        Date = DateTime.Now,
                    };

                    var electricityUtilities = new Utilities
                    {
                        Name = "Електроенергія",
                        Price = 2.64m,
                        Date = DateTime.Now,
                    };

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
                                    Apartment = "5",
                                    House = "10",
                                    Streets = new Streets{ Name = "За Рудкою"},
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
                                             Indicator = "456",
                                             Date = DateTime.Now,
                                             Price = 1203.84m,
                                             Paid = false,
                                             Utilities = electricityUtilities
                                         },
                                         new Indicators
                                         {
                                             Indicator = "82",
                                             Date = DateTime.Now,
                                             Price = 2122.16m,
                                             Paid = false,
                                             Utilities = waterUtilities
                                         },
                                         new Indicators
                                         {
                                             Indicator = "108",
                                             Date = DateTime.Now,
                                             Price = 859.68m,
                                             Paid = false,
                                             Utilities = gasUtilities
                                         }
                                    }
                                }
                            },
                            new UsersPlacesOfResidence
                            {
                                PlacesOfResidence = new PlacesOfResidence
                                {
                                    Apartment = "29",
                                    House = "1",
                                    Streets = new Streets{ Name = "Львівська"},
                                    Indicators = new List<Indicators>
                                    {
                                         new Indicators
                                         {
                                             Indicator = "450",
                                             Date = DateTime.Now,
                                             Price = 1188m,
                                             Paid = false,
                                             Utilities = electricityUtilities
                                         },
                                         new Indicators
                                         {
                                             Indicator = "80",
                                             Date = DateTime.Now,
                                             Price = 2070.4m,
                                             Paid = false,
                                             Utilities = waterUtilities
                                         },
                                         new Indicators
                                         {
                                             Indicator = "100",
                                             Date = DateTime.Now,
                                             Price = 796m,
                                             Paid = false,
                                             Utilities = gasUtilities
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

                    var newUser2 = new AppUser
                    {
                        UserName = "fakeMail2@mail.com",
                        Email = "fakeMail2@mail.com",
                        PhoneNumber = "9638520741",
                        Surname = "User2Surname",
                        Name = "User2Name",
                        Patronymic = "User2Patronymic",
                        UsersPlacesOfResidenceId = new List<UsersPlacesOfResidence>() {
                            new UsersPlacesOfResidence
                            {
                                PlacesOfResidence = new PlacesOfResidence
                                {
                                    Apartment = "10",
                                    House = "7",
                                    Streets = new Streets { Name = "Володимира Лучаковського" },
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
                                            Indicator = "400",
                                            Date = DateTime.Now,
                                            Price = 1056m,
                                            Paid = false,
                                            Utilities = electricityUtilities
                                        },
                                        new Indicators
                                        {
                                            Indicator = "90",
                                            Date = DateTime.Now,
                                            Price = 2329.2m,
                                            Paid = false,
                                            Utilities = waterUtilities
                                        },
                                        new Indicators
                                        {
                                            Indicator = "110",
                                            Date = DateTime.Now,
                                            Price = 875.6m,
                                            Paid = false,
                                            Utilities = gasUtilities
                                        }
                                    }
                                }
                            },
                        },
                        UsersStatementsId = new List<UsersStatements>() {
                            new UsersStatements
                            {
                                Statements = new Statements
                                {
                                    Text = "User2Text",
                                    StatementUrl = "https://res.cloudinary.com/dihwzdmiw/image/upload/v1703158790/viveswvd2bkjzwx6ofej.png",
                                    Status = Enum.StatementsStatus.Зареєстровано,
                                    Date = DateTime.Now,
                                    Department = new Departments { Name = "UserDepartment" },
                                    StatementsType = new StatementsTypes { SignatureCount = 50, Type = "UserStatementsTypes" }
                                }
                            }
                        }
                    };

                    await userManager.CreateAsync(newUser2, "fakeMail2@mail.com");
                    await userManager.AddToRoleAsync(newUser2, UserRoles.User);

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
