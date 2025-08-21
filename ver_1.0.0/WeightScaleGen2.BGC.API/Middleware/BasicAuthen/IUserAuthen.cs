namespace WeightScaleGen2.BGC.API.Middleware.BasicAuthen
{
    public interface IUserAuthen
    {
        bool ValidateCredentials(string username, string password);

    }
}
