namespace WebApplication1.Models.dto
{
	public class SpotsPagingdto
	{
		public int TotalPages { get; set; }
		public List<SpotImagesSpot>? SpotsResult { get; set; }

		public List<Category>? Categories { get; set; } 
	}
}
