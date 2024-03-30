using System.Security.Claims;

namespace PublicUtilities.Models
{
    public static class ClaimStore
    {
        public static List<Claim> claimsList = new List<Claim>()
        {
            new Claim("isDIS", "Інфраструктури та обслуговування"),
            new Claim("isDAF", "Адміністрації та фінансів"),
            new Claim("isDEEP", "Укології та охорони довкілля"),
            new Claim("isDPRC", "Зав’язків з громадськістю та комунікації"),
        };
    }
}
