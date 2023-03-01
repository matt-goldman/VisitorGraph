using GraphVisitor.Common.DTOs;

namespace GraphVisitor.WebApi.Services;

public interface IVisitService
{
    Task SignIn(SignInDto signIn);

    Task SignOut(SignOutDto signOut);
}

public class VisitService : IVisitService
{
    public Task SignIn(SignInDto signIn)
    {
        throw new NotImplementedException();
    }

    public Task SignOut(SignOutDto signOut)
    {
        throw new NotImplementedException();
    }
}
