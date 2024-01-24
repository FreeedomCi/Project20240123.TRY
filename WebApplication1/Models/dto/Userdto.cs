namespace WebApplication1.Models.dto
{
	public class Userdto
	{
		public string? Name { get; set; }
		public string? Email { get; set; }	
		public int? Age { get; set; }
		public IFormFile? Avatar { get; set; }
	}
}
