namespace ApiContracts;

public class UpdatePostDto
{
    public string Title { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }
}