namespace WebApplication1.Models.dto
{
	public class Searchdto
	{
		//搜尋相關
		public string? keyword { get; set; }
		public int? catagoryId { get; set; } = 0;

		//排序相關
		public string? sortBy { get; set; }
		public string? sortType { get; set; } = "asc";

		//分頁相關
		public int? page { get; set; } = 1;
		public int? pageSize { get; set; } = 9;

	}
}
