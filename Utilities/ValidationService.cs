using Microsoft.Extensions.Options;
using PlayZone.Configuration;

namespace PlayZone.Utilities;

public class ValidationService : IValidationService
{
 private readonly BusinessSettings _businessSettings;

    public ValidationService(IOptions<BusinessSettings> businessSettings)
    {
        _businessSettings = businessSettings.Value;
    }

    public void ValidateBookingDate(DateOnly date)
    {
   var today = DateOnly.FromDateTime(DateTime.Today);
if (date < today)
        {
      throw new ArgumentException("Booking date must be today or a future date");
   }
    }

    public void ValidateTime(string startTime, string endTime)
    {
// Validate time format
 if (!TimeHelper.IsValidTimeFormat(startTime) || !TimeHelper.IsValidTimeFormat(endTime))
     {
       throw new ArgumentException("Invalid time format. Use HH:MM (24-hour format)");
   }

  // Validate 30-minute intervals
     if (!TimeHelper.IsThirtyMinuteInterval(startTime) || !TimeHelper.IsThirtyMinuteInterval(endTime))
     {
      throw new ArgumentException("Time must be in 30-minute intervals");
  }

   // Parse times
   var start = TimeOnly.Parse(startTime);
var end = TimeOnly.Parse(endTime);
        var openingTime = TimeOnly.Parse(_businessSettings.OpeningTime);
   var closingTime = TimeOnly.Parse(_businessSettings.ClosingTime);

  // Validate operating hours
        if (start < openingTime || end > closingTime)
 {
   throw new ArgumentException(
    $"Booking must be within operating hours ({_businessSettings.OpeningTime} - {_businessSettings.ClosingTime})");
    }

  // Validate end time is after start time
 if (end <= start)
     {
       throw new ArgumentException("End time must be after start time");
   }
 }
}
