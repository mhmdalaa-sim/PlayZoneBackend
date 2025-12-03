using System.Web;

namespace PlayZone.Utilities;

public static class WhatsAppHelper
{
    public static string FormatBookingMessage(
        string roomName,
        string date,
 string startTime,
        string endTime,
   decimal duration,
      string customerName,
        string customerPhone,
    string? notes)
    {
   var durationText = TimeHelper.FormatDuration(duration);

     var message = $@"?? *PlayZone Booking Confirmation*

?? Room: {roomName}
?? Date: {date}
? Time: {startTime} – {endTime}
?? Duration: {durationText}

?? Name: {customerName}
?? Phone: {customerPhone}";

     if (!string.IsNullOrWhiteSpace(notes))
        {
    message += $"\n\n?? Notes: {notes}";
   }

 message += "\n\nPlease confirm your booking.";

    return message;
 }

    public static string GenerateWhatsAppUrl(string phoneNumber, string message)
  {
   // Remove any spaces, dashes, or special characters from phone number
   var cleanNumber = new string(phoneNumber.Where(c => char.IsDigit(c) || c == '+').ToArray());

   // Encode the message for URL
   var encodedMessage = HttpUtility.UrlEncode(message);

  // Generate WhatsApp URL (works on both web and mobile)
   return $"https://wa.me/{cleanNumber}?text={encodedMessage}";
  }
}
