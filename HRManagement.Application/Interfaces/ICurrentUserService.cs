namespace HRManagement.Application.Interfaces
{
    public interface ICurrentUserService
    {
        long? UserId { get; }
        string? UserName { get; }
    }
}
