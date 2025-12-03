namespace PlayZone.DTOs;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Error { get; set; }
    public object? Details { get; set; }

    public static ApiResponse<T> SuccessResponse(T data)
    {
        return new ApiResponse<T>
        {
  Success = true,
   Data = data
        };
    }

    public static ApiResponse<T> ErrorResponse(string error, object? details = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Error = error,
            Details = details
     };
    }
}

public class ApiResponse
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public object? Details { get; set; }

  public static ApiResponse SuccessResponse()
    {
        return new ApiResponse { Success = true };
    }

    public static ApiResponse ErrorResponse(string error, object? details = null)
    {
   return new ApiResponse
        {
         Success = false,
      Error = error,
      Details = details
        };
    }
}
