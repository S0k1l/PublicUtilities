using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PublicUtilities.Models;
using System.Diagnostics;
using System.Net;
using System.Security;
using System.Security.Cryptography.Xml;

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

                    //Departments=-------------------------------
                    var infrastructureAndService = new Departments { Name = "Відділ інфраструктури та обслуговування" };
                    var administrationAndFinance = new Departments { Name = "Відділ адміністрації та фінансів" };
                    var ecologyAndEnvironmentalProtection = new Departments { Name = "Відділ екології та охорони довкілля" };
                    var publicRelationsAndCommunications = new Departments { Name = "Відділ зав’язків з громадськістю та комунікації" };

                    await context.AddRangeAsync(infrastructureAndService, 
                        administrationAndFinance, 
                        ecologyAndEnvironmentalProtection,
                        publicRelationsAndCommunications);

                    //StatementsTypes=-------------------------------
                    //administrationAndFinance
                    var receivingTaxBenefitsOrTaxRefunds = new StatementsTypes
                    {
                        Type = "Заява про отримання податкових пільг або повернення податків",
                        SignatureCount = 1,
                        Department = administrationAndFinance,
                        isPhotoNeeded = false,
                        isStreetNeeded = false,
                    };

                    var financialSupportOrSocialServices = new StatementsTypes
                    {
                        Type = "Звернення щодо фінансової підтримки або соціальних послуг",
                        SignatureCount = 1,
                        Department = administrationAndFinance,
                        isPhotoNeeded = false,
                        isStreetNeeded = false,
                    };

                    var keepingACertificateOfIncome = new StatementsTypes
                    {
                        Type = "Заява про отримання довідки про доходи для отримання соціальних пільг",
                        SignatureCount = 1,
                        Department = administrationAndFinance,
                        isPhotoNeeded = false,
                        isStreetNeeded = false,
                    };

                    var financialSupportProgramsForEntrepreneurs = new StatementsTypes
                    {
                        Type = "Звернення щодо участі у програмах фінансової підтримки підприємців",
                        SignatureCount = 1,
                        Department = administrationAndFinance,
                        isPhotoNeeded = false,
                        isStreetNeeded = false,
                    };

                    var registrationOfANewBusiness = new StatementsTypes
                    {
                        Type = "Заява про реєстрацію нового бізнесу чи зміни в існуючому",
                        SignatureCount = 1,
                        Department = administrationAndFinance,
                        isPhotoNeeded = false,
                        isStreetNeeded = false,
                    };

                    await context.AddRangeAsync(receivingTaxBenefitsOrTaxRefunds,
                        financialSupportOrSocialServices,
                        keepingACertificateOfIncome,
                        financialSupportProgramsForEntrepreneurs,
                        registrationOfANewBusiness);

                    //infrastructureAndService
                    var repairOrMaintenanceOfRoadSurface = new StatementsTypes
                    {
                        Type = "Заява про ремонт або утримання дорожнього покриття",
                        SignatureCount = 20,
                        Department = infrastructureAndService,
                        isPhotoNeeded = true,
                        isStreetNeeded = true,
                    };

                    var problemsWithWaterSupplyOrDrainage = new StatementsTypes
                    {
                        Type = "Звернення щодо проблем з водопостачанням або водовідведенням",
                        SignatureCount = 20,
                        Department = infrastructureAndService,
                        isPhotoNeeded = false,
                        isStreetNeeded = true,
                    };

                    var installationOfNewCommunicationLines = new StatementsTypes
                    {
                        Type = "Заява про встановлення нових комунікаційних ліній або підключення до існуючих",
                        SignatureCount = 20,
                        Department = infrastructureAndService,
                        isPhotoNeeded = false,
                        isStreetNeeded = true,
                    };

                    var installationOrRegistrationOfANewPedestrian = new StatementsTypes
                    {
                        Type = "Заява на встановлення або реєстрацію нового пішохідного чи велосипедного шляху",
                        SignatureCount = 20,
                        Department = infrastructureAndService,
                        isPhotoNeeded = false,
                        isStreetNeeded = true,
                    };

                    var establishmentOfNewPublicTransportStops = new StatementsTypes
                    {
                        Type = "Звернення щодо встановлення нових зупинок громадського транспорту",
                        SignatureCount = 20,
                        Department = infrastructureAndService,
                        isPhotoNeeded = false,
                        isStreetNeeded = true,
                    };

                    var arrangementofTheSiteFoPublicRecreation = new StatementsTypes
                    {
                        Type = "Заява про облаштування майданчика для громадської рекреації",
                        SignatureCount = 20,
                        Department = infrastructureAndService,
                        isPhotoNeeded = false,
                        isStreetNeeded = true,
                    };

                    await context.AddRangeAsync(repairOrMaintenanceOfRoadSurface,
                        problemsWithWaterSupplyOrDrainage,
                        installationOfNewCommunicationLines,
                        installationOrRegistrationOfANewPedestrian,
                        establishmentOfNewPublicTransportStops,
                        arrangementofTheSiteFoPublicRecreation);

                    //ecologyAndEnvironmentalProtection
                    var removalOrPruningOfTreesInACertainArea = new StatementsTypes
                    {
                        Type = "Звернення щодо видалення або обрізки дерев на певній території",
                        SignatureCount = 20,
                        Department = ecologyAndEnvironmentalProtection,
                        isPhotoNeeded = false,
                        isStreetNeeded = true,
                    };

                    var factsOfEnvironmentalPollution = new StatementsTypes
                    {
                        Type = "Повідомлення про факти забруднення навколишнього середовища",
                        SignatureCount = 20,
                        Department = ecologyAndEnvironmentalProtection,
                        isPhotoNeeded = true,
                        isStreetNeeded = true,
                    };

                    var obtainingAPermitForGarbageRemoval = new StatementsTypes
                    {
                        Type = "Заява про отримання дозволу на винос сміття або відходів",
                        SignatureCount = 20,
                        Department = ecologyAndEnvironmentalProtection,
                        isPhotoNeeded = false,
                        isStreetNeeded = true,
                    };

                    var implementationOfEnergyEfficiencyPrograms = new StatementsTypes
                    {
                        Type = "Звернення щодо впровадження програм з енергоефективності та використання відновлювальних джерел енергії",
                        SignatureCount = 20,
                        Department = ecologyAndEnvironmentalProtection,
                        isPhotoNeeded = false,
                        isStreetNeeded = true,
                    };

                    var removalOrPlantsInACertainArea = new StatementsTypes
                    {
                        Type = "Заява про видалення або пересадження рослин на певній території",
                        SignatureCount = 20,
                        Department = ecologyAndEnvironmentalProtection,
                        isPhotoNeeded = false,
                        isStreetNeeded = true,
                    };

                    var participationInEnvironmentalLightingProjects = new StatementsTypes
                    {
                        Type = "Звернення щодо участі у проектах з екологічного освітлення та енергозбереження",
                        SignatureCount = 20,
                        Department = ecologyAndEnvironmentalProtection,
                        isPhotoNeeded = false,
                        isStreetNeeded = true,
                    };

                    await context.AddRangeAsync(removalOrPruningOfTreesInACertainArea,
                        factsOfEnvironmentalPollution,
                        obtainingAPermitForGarbageRemoval,
                        implementationOfEnergyEfficiencyPrograms,
                        removalOrPlantsInACertainArea,
                        participationInEnvironmentalLightingProjects);

                    //publicRelationsAndCommunications
                    var providingInformationAboutTheWorkOfLocalAuthorities = new StatementsTypes
                    {
                        Type = "Звернення щодо надання інформації про роботу місцевої влади",
                        SignatureCount = 1,
                        Department = publicRelationsAndCommunications,
                        isPhotoNeeded = false,
                        isStreetNeeded = false,
                    };

                    var participationInPublicDiscussionsOrEvents = new StatementsTypes
                    {
                        Type = "Заява про участь у громадських обговореннях або заходах",
                        SignatureCount = 1,
                        Department = publicRelationsAndCommunications,
                        isPhotoNeeded = false,
                        isStreetNeeded = false,
                    };

                    var ubmissionOfSuggestionsOrComplaints = new StatementsTypes
                    {
                        Type = "Подання пропозицій чи скарг щодо якості наданих послуг",
                        SignatureCount = 1,
                        Department = publicRelationsAndCommunications,
                        isPhotoNeeded = false,
                        isStreetNeeded = false,
                    };

                    var obtainingPermissionToHoldPublicEvents = new StatementsTypes
                    {
                        Type = "Заява на отримання дозволу на проведення громадських заходів чи фестивалів",
                        SignatureCount = 1,
                        Department = publicRelationsAndCommunications,
                        isPhotoNeeded = false,
                        isStreetNeeded = false,
                    };

                    var placementOfPublicInformationOnLocalSites = new StatementsTypes
                    {
                        Type = "Звернення щодо розміщення громадської інформації на місцевих площадках",
                        SignatureCount = 1,
                        Department = publicRelationsAndCommunications,
                        isPhotoNeeded = false,
                        isStreetNeeded = false,
                    };

                    var receivingAGrantForTheImplementationOfAPublicProject = new StatementsTypes
                    {
                        Type = "Заява про отримання гранту для реалізації громадського проекту",
                        SignatureCount = 1,
                        Department = publicRelationsAndCommunications,
                        isPhotoNeeded = false,
                        isStreetNeeded = false,
                    };

                    await context.AddRangeAsync(providingInformationAboutTheWorkOfLocalAuthorities,
                        participationInPublicDiscussionsOrEvents,
                        ubmissionOfSuggestionsOrComplaints,
                        obtainingPermissionToHoldPublicEvents,
                        placementOfPublicInformationOnLocalSites,
                        receivingAGrantForTheImplementationOfAPublicProject);

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
                                    StatementsType = problemsWithWaterSupplyOrDrainage,
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
                                    StatementsType = receivingTaxBenefitsOrTaxRefunds,
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
            int idicator = 0;
            int consumed;

            //waterUtilities
            for (int i = 11; i >= 1; i--)
            {
                consumed = rand.Next(5, 5);

                if (idicator == 0)
                    idicator = consumed;
                else
                    idicator += consumed;

                newIndicators.Add(new Indicators
                {
                    Indicator = idicator.ToString(),
                    Date = DateTime.Now.AddMonths(-i),
                    Price = consumed * waterUtilities.Price,
                    Paid = true,
                    Utilities = waterUtilities,
                    Consumed = consumed.ToString(),
                });
            }

            consumed = rand.Next(5, 5);
            idicator += consumed;

            newIndicators.Add(new Indicators
            {
                Indicator = idicator.ToString(),
                Date = DateTime.Now,
                Price = consumed * waterUtilities.Price,
                Paid = false,
                Utilities = waterUtilities,
                Consumed = consumed.ToString(),
            });

            idicator = 0;

            //gasUtilities
            for (int i = 11; i >= 1; i--)
            {
                consumed = rand.Next(21, 31);

                if (idicator == 0)
                    idicator = consumed;
                else
                    idicator += consumed;

                newIndicators.Add(new Indicators
                {
                    Indicator = idicator.ToString(),
                    Date = DateTime.Now.AddMonths(-i),
                    Price = consumed * gasUtilities.Price,
                    Paid = true,
                    Utilities = gasUtilities,
                    Consumed = consumed.ToString(),
                });
            }

            consumed = rand.Next(21, 31);
            idicator += consumed;

            newIndicators.Add(new Indicators
            {
                Indicator = idicator.ToString(),
                Date = DateTime.Now,
                Price = consumed * gasUtilities.Price,
                Paid = false,
                Utilities = gasUtilities,
                Consumed = consumed.ToString(),
            });

            idicator = 0;
            //electricityUtilities
            for (int i = 11; i >= 1; i--)
            {
                consumed = rand.Next(332, 382);

                if (idicator == 0)
                    idicator = consumed;
                else
                    idicator += consumed;

                newIndicators.Add(new Indicators
                {
                    Indicator = idicator.ToString(),
                    Date = DateTime.Now.AddMonths(-i),
                    Price = consumed * electricityUtilities.Price,
                    Paid = true,
                    Utilities = electricityUtilities,
                    Consumed = consumed.ToString(),
                });
            }

            consumed = rand.Next(332, 382);
            idicator += consumed;

            newIndicators.Add(new Indicators
            {
                Indicator = idicator.ToString(),
                Date = DateTime.Now,
                Price = consumed * electricityUtilities.Price,
                Paid = false,
                Utilities = electricityUtilities,
                Consumed = consumed.ToString(),
            });

            return newIndicators;
        }
    }
}
