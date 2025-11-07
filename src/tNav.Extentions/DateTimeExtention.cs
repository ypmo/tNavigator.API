namespace tNav;

public static class DateTimeExtention
{
    static string time_format = @"'year='yyyy',month='%M', day='%d";
    public static string tNavFormat(this DateTime dateTime)
    {
        return dateTime.ToString(time_format);
    }
}
