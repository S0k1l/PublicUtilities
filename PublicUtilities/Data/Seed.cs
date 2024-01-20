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

                    //Utilities=-------------------------------
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

                    var indicators = GanareteIndocatorsForYear(waterUtilities, gasUtilities, electricityUtilities);
                    var indicators2 = GanareteIndocatorsForYear(waterUtilities, gasUtilities, electricityUtilities); ;

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
                                    Indicators = indicators,
                                }
                            },
                            new UsersPlacesOfResidence
                            {
                                PlacesOfResidence = new PlacesOfResidence
                                {
                                    Apartment = "29",
                                    House = "1",
                                    Streets = new Streets{ Name = "Львівська"},
                                    Indicators = indicators2,
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

                    var indicators3 = GanareteIndocatorsForYear(waterUtilities,gasUtilities,electricityUtilities);

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
                                    Indicators = indicators3,
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

        private static List<Indicators> GanareteIndocatorsForYear(Utilities waterUtilities, Utilities gasUtilities, Utilities electricityUtilities)
        {
            var newIndicators = new List<Indicators>();
            Random rand = new Random();
            int idicator;

            //waterUtilities
            for (int i = 11; i >= 1; i--)
            {
                idicator = rand.Next(5, 5);
                newIndicators.Add(new Indicators
                {
                    Indicator = idicator.ToString(),
                    Date = DateTime.Now.AddMonths(-i),
                    Price = idicator * waterUtilities.Price,
                    Paid = true,
                    Utilities = waterUtilities
                });
            }

            idicator = rand.Next(5, 5);
            newIndicators.Add(new Indicators
            {
                Indicator = idicator.ToString(),
                Date = DateTime.Now,
                Price = idicator * waterUtilities.Price,
                Paid = false,
                Utilities = waterUtilities,
            });

            //gasUtilities
            for (int i = 11; i >= 1; i--)
            {
                idicator = rand.Next(21, 31);
                newIndicators.Add(new Indicators
                {
                    Indicator = idicator.ToString(),
                    Date = DateTime.Now.AddMonths(-i),
                    Price = idicator * gasUtilities.Price,
                    Paid = true,
                    Utilities = gasUtilities
                });
            }

            idicator = rand.Next(21, 31);
            newIndicators.Add(new Indicators
            {
                Indicator = idicator.ToString(),
                Date = DateTime.Now,
                Price = idicator * gasUtilities.Price,
                Paid = false,
                Utilities = gasUtilities
            });

            //electricityUtilities
            for (int i = 11; i >= 1; i--)
            {
                idicator = rand.Next(332, 382);
                newIndicators.Add(new Indicators
                {
                    Indicator = idicator.ToString(),
                    Date = DateTime.Now.AddMonths(-i),
                    Price = idicator * electricityUtilities.Price,
                    Paid = true,
                    Utilities = electricityUtilities
                });
            }

            idicator = rand.Next(332, 382);
            newIndicators.Add(new Indicators
            {
                Indicator = idicator.ToString(),
                Date = DateTime.Now,
                Price = idicator * electricityUtilities.Price,
                Paid = false,
                Utilities = electricityUtilities
            });

            return newIndicators;
        }
    }
}
