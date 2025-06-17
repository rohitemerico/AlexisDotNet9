public class UserManageFactory
{
    public static UserManageBase Create(string Provider)
    {
        switch (Provider)
        {
            default:
                return new UserManageDefault();
        }
    }
}
