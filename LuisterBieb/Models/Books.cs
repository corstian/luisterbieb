namespace LuisterBieb.Models
{
	public class Books
	{
		public Info info { get; set; }
		public Book[] books { get; set; }

		public class Info
		{
			public int amount { get; set; }
		}

		public class Book
		{
			public int id { get; set; }
			public int category_id { get; set; }
			public string category { get; set; }
			public int subcategory_id { get; set; }
			public string subcategory { get; set; }
			public string title { get; set; }
			public string description { get; set; }
			public string author { get; set; }
			public string publication_date { get; set; }
			public int duration { get; set; }
			public int size { get; set; }
			public int rating { get; set; }
			public string fragment { get; set; }
			public int fragment_duration { get; set; }
			public string menu_image_location { get; set; }
			public string cover_image_location { get; set; }
		}
	}
}
