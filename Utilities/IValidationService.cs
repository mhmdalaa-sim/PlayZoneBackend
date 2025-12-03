namespace PlayZone.Utilities;

public interface IValidationService
{
    void ValidateBookingDate(DateOnly date);
void ValidateTime(string startTime, string endTime);
}
