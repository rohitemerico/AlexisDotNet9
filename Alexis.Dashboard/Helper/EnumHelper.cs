namespace Alexis.Dashboard.Helper;

public class EnumHelper
{
    public static bool TryParseEnum<TEnum>(string value, out TEnum result, bool ignoreCase = true) where TEnum : struct, Enum
    {
        return Enum.TryParse(value, ignoreCase, out result);
    }

    public static TEnum ParseEnum<TEnum>(string value, bool ignoreCase = true) where TEnum : struct, Enum
    {
        return Enum.Parse<TEnum>(value, ignoreCase);
    }
}

public enum TabGroup
{
    Role = 1,
    User = 2,
    Branch = 3,
}

public enum MachineTabGroup
{
    Alert = 1,
    Application = 2,
    Operating_Hour = 3,
    Card = 4,
    Machine_Management = 5,
    Document_Type = 6,
    Document = 7,
    Group = 8,
    Hopper = 9
}

public enum AdvTabGroup
{
    Advertisement = 1,
}

public enum MDMTabGroup
{
    By_Device = 1,
    By_Device_Group = 2,
}

public enum MDMiOSTabGroup
{
    By_Device = 1,
    By_Branch = 2,
}

public enum DeviceListingTabGroup
{
    Devices = 1,
    Device_Groups = 2,
    Applications = 3,
    Profiles = 4
}