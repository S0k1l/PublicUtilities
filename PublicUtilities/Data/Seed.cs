using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using Microsoft.VisualBasic;
using PublicUtilities.Models;
using System;
using System.Diagnostics;
using System.Net;
using System.Security;
using System.Security.Cryptography.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                if (!await roleManager.RoleExistsAsync(UserRoles.Employee))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Employee));
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
                        ImageUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fjurliga.ligazakon.net%2Fru%2Fnews%2F140243_podlinnost-id-karty-mozhno-proverit-po-elementam-zashchity&psig=AOvVaw1qsbE1lOV_cZL3_nqDZfjD&ust=1707985399765000&source=images&cd=vfe&opi=89978449&ved=0CBAQjRxqFwoTCJD2rNWzqoQDFQAAAAAdAAAAABAj",
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
                                            Header = "Планове відключення водопостачання",
                                            Text = "У зв'язку з плановими ремонтними роботами, буде припинено подачу води у вашому районі з 15/02/2024 до 18/02/2024. Просимо зробити необхідні запаси.",
                                            Date = DateTime.ParseExact("10/02/2024", "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None),
                                        },
                                        new Notifications
                                        {
                                            Header = "Тимчасове обмеження руху на вулиці Шевченка",
                                            Text = "У зв'язку з аварійним ремонтом дорожнього покриття, буде тимчасово обмежено рух на вулиці Шевченка з 10/02/2024 до 12/02/2024. Просимо водіїв врахувати це при плануванні маршруту.",
                                            Date = DateTime.ParseExact("08/02/2024", "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None),
                                        },
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
                                    Notifications  = new List<Notifications>
                                    {
                                        new Notifications
                                        {
                                            Header = "Планове відключення електроенергії",
                                            Text = "У зв'язку з плановими технічними роботами, буде припинено подачу електроенергії в вашому районі з 20/02/2024 08:00 до 21/02/2024 18:00. Просимо вибачення за незручності.",
                                            Date = DateTime.ParseExact("15/02/2024", "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None),
                                        },
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
                        ImageUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fjurliga.ligazakon.net%2Fru%2Fnews%2F140243_podlinnost-id-karty-mozhno-proverit-po-elementam-zashchity&psig=AOvVaw1qsbE1lOV_cZL3_nqDZfjD&ust=1707985399765000&source=images&cd=vfe&opi=89978449&ved=0CBAQjRxqFwoTCJD2rNWzqoQDFQAAAAAdAAAAABAj",
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
                                            Header = "Нові графіки сміттєвозів",
                                            Text = "Шановні мешканці! Нагадуємо, що з 25/02/2024 змінюється графік вивезення сміття. Будь ласка, перегляньте новий графік на нашому веб-сайті або зверніться до міської адміністрації для отримання додаткової інформації.",
                                            Date = DateTime.ParseExact("20/02/2024", "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None),
                                        },

                                        new Notifications
                                        {
                                            Header = "Ремонт водопровідної мережі",
                                            Text = "У зв'язку з аварійною ситуацією, буде проводитися ремонт водопровідної мережі на вулиці Пушкіна. Роботи заплановані з 10/02/2024 до 12/02/2024. Просимо вибачення за тимчасові незручності.",
                                            Date = DateTime.ParseExact("05/02/2024", "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None),
                                        },

                                        new Notifications
                                        {
                                            Header = "Тимчасове припинення роботи трамваїв",
                                            Text = "У зв'язку з ремонтними роботами на колії, трамваї №3 тимчасово не курсуватимуть з 15/02/2024 до 20/02/2024. Просимо вибачення за тимчасові незручності та користуватися альтернативними маршрутами.",
                                            Date = DateTime.ParseExact("12/02/2024", "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None),
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

                    context.News.AddRange(
                       new News
                       {
                           Header = "Міська рада затвердила план реконструкції центрального парку",
                           Text = "Сьогодні, на засіданні міської ради, був затверджений довгоочікуваний план реконструкції центрального парку міста. За проектом, який був розроблений з урахуванням думок та пропозицій місцевих мешканців, передбачено збільшення кількості зелених насаджень, розширення зон відпочинку та створення нових дитячих майданчиків. Також у плані передбачено встановлення сучасного освітлення та ремонт існуючих інженерних мереж. Реалізація проекту розпочнеться вже у наступному місяці і планується завершити до початку наступного літа. Міська рада закликає мешканців долучитися до обговорення деталей реконструкції та висловлювати свої пропозиції щодо покращення облаштування улюбленого парку.",
                           Date = DateTime.ParseExact("09/02/2024", "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None),
                           ImageUrl = "https://res.cloudinary.com/dihwzdmiw/image/upload/v1706539311/PublicUtilities/umrqb323gzapmucah18s.jpg"
                       },
                        new News
                        {
                            Header = "Міська рада запроваджує програму 'Зелене місто'",
                            Text = "Сьогодні міська рада оголосила про запуск нової програми під назвою 'Зелене місто', спрямованої на збільшення кількості зелених насаджень у місті та покращення екологічної ситуації. У рамках програми планується висадити тисячі нових дерев, кущів та квітів на вулицях, в парках та на громадських територіях. Крім того, передбачено проведення інформаційних кампаній з популяризації догляду за рослинами та участь місцевих жителів у збереженні природного середовища. Програма 'Зелене місто' стане важливим кроком у розвитку екологічно чистого та комфортного життя у місті.",
                            Date = DateTime.ParseExact("15/02/2024", "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None),
                            ImageUrl = "https://res.cloudinary.com/dihwzdmiw/image/upload/v1706539311/PublicUtilities/umrqb323gzapmucah18s.jpg"
                        },
                        new News
                        {
                            Header = "Міська рада затвердила бюджет на розвиток інфраструктури",
                            Text = "На засіданні міської ради був ухвалений бюджет на наступний рік з урахуванням розвитку інфраструктури міста. Згідно з планом, значні кошти будуть спрямовані на ремонт доріг, відновлення комунальних служб, модернізацію системи водопостачання та каналізації, а також на розвиток громадського транспорту. Міська рада наголосила, що розподіл коштів проводився з урахуванням потреб містян та перспектив розвитку міста на майбутнє.",
                            Date = DateTime.ParseExact("21/02/2024", "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None),
                            ImageUrl = "https://res.cloudinary.com/dihwzdmiw/image/upload/v1706539311/PublicUtilities/umrqb323gzapmucah18s.jpg"
                        },
                        new News
                        {
                            Header = "Міська рада впроваджує нову програму підтримки малих бізнесів",
                            Text = "Сьогодні міська рада анонсувала запуск нової програми підтримки малих та середніх підприємств. Програма передбачає надання фінансової допомоги, консультаційних послуг та навчання підприємцям з метою стимулювання розвитку місцевої економіки та підтримки новаторських ідей. За словами представників міської влади, ця ініціатива спрямована на створення сприятливих умов для розвитку бізнесу та залучення інвестицій у місто.",
                            Date = DateTime.ParseExact("03/03/2024", "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None),
                            ImageUrl = "https://res.cloudinary.com/dihwzdmiw/image/upload/v1706539311/PublicUtilities/umrqb323gzapmucah18s.jpg"
                        },
                        new News
                        {
                            Header = "Міська рада оголосила конкурс на кращий проект благоустрою дворових територій",
                            Text = "Сьогодні міська рада оголосила про проведення конкурсу на кращий проект благоустрою дворових територій. Згідно з оголошенням, мешканці мають можливість подати свої ідеї щодо розбудови і озеленення дворів, встановлення дитячих та спортивних майданчиків, а також реалізацію екологічних ініціатив. Переможці отримають фінансову підтримку від міської влади для втілення своїх проектів. Цей конкурс спрямований на залучення громадян до активної участі в формуванні простору свого міста та покращенні якості життя.",
                            Date = DateTime.ParseExact("10/03/2024", "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None),
                            ImageUrl = "https://res.cloudinary.com/dihwzdmiw/image/upload/v1706539311/PublicUtilities/umrqb323gzapmucah18s.jpg"
                        },
                        new News
                        {
                            Header = "Міська рада запускає програму 'Молодь для майбутнього'",
                            Text = "Сьогодні міська рада оголосила про старт нової соціальної програми під назвою 'Молодь для майбутнього'. Програма спрямована на підтримку молоді в процесі навчання, професійного розвитку та здобуття перспективної роботи. В рамках ініціативи планується організація курсів, тренінгів та майстер-класів з ключових сфер знань та професій, а також створення програм підтримки для молодих підприємців. Міська рада підкреслює, що ця програма є важливим кроком у створенні сприятливого середовища для розвитку молоді та забезпечення їхнього успіху у майбутньому.",
                            Date = DateTime.ParseExact("17/03/2024", "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None),
                            ImageUrl = "https://res.cloudinary.com/dihwzdmiw/image/upload/v1706539311/PublicUtilities/umrqb323gzapmucah18s.jpg"
                        },
                        new News
                        {
                            Header = "Міська рада запускає проект 'Доступне житло'",
                            Text = "Сьогодні міська рада анонсувала старт нового соціального проекту під назвою 'Доступне житло'. Проект спрямований на забезпечення доступного житла для молодих сімей, одиноких матерів, ветеранів та інших соціально вразливих категорій населення. За словами представників міської влади, ця ініціатива передбачає підтримку в покупці або оренді житла за спеціальними програмами та встановлення соціально справедливих вимог до цінової політики ринку нерухомості. 'Доступне житло' має стати важливим кроком у забезпеченні всіх мешканців міста якісним та доступним житлом.",
                            Date = DateTime.ParseExact("25/03/2024", "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None),
                            ImageUrl = "https://res.cloudinary.com/dihwzdmiw/image/upload/v1706539311/PublicUtilities/umrqb323gzapmucah18s.jpg"
                        },
                        new News
                        {
                            Header = "Міська рада затвердила програму 'Культурна столиця'",
                            Text = "Сьогодні міська рада ухвалила програму 'Культурна столиця', спрямовану на розвиток культурного життя та підтримку мистецтва в місті. За допомогою цієї програми планується організація мистецьких заходів, фестивалів, виставок та концертів, підтримка місцевих творчих ініціатив та створення нових культурних просторів. Міська рада закликає мешканців активно долучатися до культурного життя міста та брати участь у заходах, які сприяють розквіту мистецтва та культури.",
                            Date = DateTime.ParseExact("02/04/2024", "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None),
                            ImageUrl = "https://res.cloudinary.com/dihwzdmiw/image/upload/v1706539311/PublicUtilities/umrqb323gzapmucah18s.jpg"
                        },
                        new News
                        {
                            Header = "Міська рада розпочала проект 'Енергоефективне місто'",
                            Text = "Сьогодні міська рада оголосила про старт нового проекту під назвою 'Енергоефективне місто', спрямованого на зменшення споживання енергії та підвищення ступеня енергоефективності у місті. У рамках цього проекту планується встановлення енергозберігаючих технологій у громадських та житлових будівлях, організація інформаційних кампаній з популяризації енергоефективного споживання та стимулювання використання альтернативних джерел енергії. Міська рада вважає, що реалізація цього проекту допоможе зменшити відповідальність за навантаження на енергетичні системи та сприятиме збереженню природних ресурсів.",
                            Date = DateTime.ParseExact("08/04/2024", "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None),
                            ImageUrl = "https://res.cloudinary.com/dihwzdmiw/image/upload/v1706539311/PublicUtilities/umrqb323gzapmucah18s.jpg"
                        },
                        new News
                        {
                            Header = "Міська рада відкрила новий центр допомоги безпритульним та потребуючим",
                            Text = "Сьогодні міська рада відкрила новий центр допомоги безпритульним та потребуючим. Цей центр надає безкоштовні послуги проживання, харчування та медичної допомоги для тих, хто опинився в скрутній життєвій ситуації. Крім того, у центрі працюють соціальні працівники та психологи, які надають підтримку та консультації у вирішенні проблем. Міська рада закликає всіх мешканців, які потребують допомоги, звертатися до нового центру, де їх чекають доброзичливість та підтримка.",
                            Date = DateTime.ParseExact("15/04/2024", "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None),
                            ImageUrl = "https://res.cloudinary.com/dihwzdmiw/image/upload/v1706539311/PublicUtilities/umrqb323gzapmucah18s.jpg"
                        });

                    context.Add(new GarbageRemoval
                    {
                        Text = "1. Розклад вивозу:\r\n" +
                                    "\t- Понеділок: Центральні райони міста.\r\n" +
                                    "\t- Вівторок: Північні райони.\r\n" +
                                    "\t- Середа: Східні райони.\r\n" +
                                    "\t- Четвер: Південні райони.\r\n" +
                                    "\t- П'ятниця: Західні райони.\r\n" +
                                "2. Частота вивозу:\r\n" +
                                    "\t- Резиденціальні райони: 2 рази на тиждень.\r\n" +
                                    "\t- Комерційні та промислові райони: Щоденно.\r\n" +
                                    "\t- Центральні райони: 3 рази на тиждень.\r\n" +
                                "3. Спеціалізовані вивози:\r\n" +
                                    "\t- Великогабаритне сміття: Один раз на тиждень (середа).\r\n" +
                                    "\t- Небезпечні відходи: Раз на місяць (остання п'ятниця місяця).\r\n" +
                                "4. Додаткові заходи:\r\n" +
                                    "\t- Вивіз зелених відходів (гілки, листя): Раз на тиждень (субота).\r\n" +
                                    "\t- Акції з утилізації електронних відходів: Кожні два місяці (неділя).\r\n" +
                                "5. Спеціальні події:\r\n" +
                                    "\t- Підготовка до свят: Збільшення частоти вивозу на тиждень до свят.\r\n" +
                                    "\t- Екологічні заходи: Додатковий вивіз під час масових прибирань."
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
            for (int i = 13; i >= 1; i--)
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
                    PaymentDate = DateTime.Now.AddMonths(-i).AddDays(5),
                    Price = consumed * waterUtilities.Price,
                    isPaid = true,
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
                isPaid = false,
                Utilities = waterUtilities,
                Consumed = consumed.ToString(),
            });

            idicator = 0;

            //gasUtilities
            for (int i = 13; i >= 1; i--)
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
                    PaymentDate = DateTime.Now.AddMonths(-i).AddDays(5),
                    Price = consumed * gasUtilities.Price,
                    isPaid = true,
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
                isPaid = false,
                Utilities = gasUtilities,
                Consumed = consumed.ToString(),
            });

            idicator = 0;
            //electricityUtilities
            for (int i = 13; i >= 1; i--)
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
                    PaymentDate = DateTime.Now.AddMonths(-i).AddDays(5),
                    Price = consumed * electricityUtilities.Price,
                    isPaid = true,
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
                isPaid = false,
                Utilities = electricityUtilities,
                Consumed = consumed.ToString(),
            });

            return newIndicators;
        }
    }
}
