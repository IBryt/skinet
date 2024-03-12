namespace API.Errors;

public class ApiValidationErrorResponse : ApiResponse
{
    private const int BAD_REQUEST_ERROR = 400;
    
    public ApiValidationErrorResponse() : base(BAD_REQUEST_ERROR)
    {
    }
    public IEnumerable<string> Errors { get; set; }
}
