using System.Security.Claims;

namespace PublicUtilities.Models
{
    public static class ClaimStore
    {
        public static List<Claim> claimsList = new List<Claim>()
        {
            new Claim("isDIS", "Відділ інфраструктури та обслуговування"),
            new Claim("isDAF", "Відділ адміністрації та фінансів"),
            new Claim("isDEEP", "Відділ екології та охорони довкілля"),
            new Claim("isDPRC", "Відділ зав’язків з громадськістю та комунікації"),
        };
    }
}
