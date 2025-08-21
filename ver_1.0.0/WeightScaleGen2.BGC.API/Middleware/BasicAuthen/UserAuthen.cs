namespace WeightScaleGen2.BGC.API.Middleware.BasicAuthen
{
    public class UserAuthen : IUserAuthen
    {
        public bool ValidateCredentials(string username, string password)
        {
            return username.Equals("admin") && password.Equals("Pa$$WoRd");
        }
    }
}
