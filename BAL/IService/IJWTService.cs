namespace BAL.IService
{
    public interface IJWTService
    {
        string GenerateToken(string email, int? role);
    }
}
