namespace LuisterBieb.Models
{
	public class Categories
	{
		public Category[] categories { get; set; }

		public class Category
		{
			public int id { get; set; }
			public string name { get; set; }
			public Subcategory[] subcategories { get; set; }

			public class Subcategory
			{
				public int id { get; set; }
				public string name { get; set; }
			}
		}
	}
}
