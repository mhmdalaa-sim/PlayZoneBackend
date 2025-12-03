namespace PlayZone.Utilities;

public static class TimeHelper
{
  public static decimal CalculateDuration(string startTime, string endTime)
    {
        var start = TimeOnly.Parse(startTime);
   var end = TimeOnly.Parse(endTime);
        var duration = end - start;
  return (decimal)duration.TotalHours;
    }

    public static bool IsValidTimeFormat(string time)
    {
     return TimeOnly.TryParse(time, out _);
    }

    public static bool IsThirtyMinuteInterval(string time)
    {
  if (!TimeOnly.TryParse(time, out var timeOnly))
   return false;

return timeOnly.Minute == 0 || timeOnly.Minute == 30;
    }

    public static string FormatDuration(decimal hours)
    {
     var totalMinutes = (int)(hours * 60);
     var h = totalMinutes / 60;
   var m = totalMinutes % 60;

if (m == 0)
      return $"{h} hour{(h != 1 ? "s" : "")}";

    return $"{h}h {m}m";
}

    public static List<string> GenerateTimeSlots(string startTime, string endTime, int intervalMinutes = 30)
    {
   var slots = new List<string>();
   var current = TimeOnly.Parse(startTime);
   var end = TimeOnly.Parse(endTime);

    while (current <= end)
   {
    slots.Add(current.ToString("HH:mm"));
      current = current.AddMinutes(intervalMinutes);
   }

    return slots;
    }
}
