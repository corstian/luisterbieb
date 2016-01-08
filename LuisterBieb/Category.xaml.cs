using System;
using System.Linq;
using System.Windows.Navigation;
using LuisterBieb.Models;
using Microsoft.Phone.Controls;
using StorageHelper.Apollo;

namespace LuisterBieb
{
	public partial class Category : PhoneApplicationPage
	{
		public Category()
		{
			InitializeComponent();
		}

		protected async override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			string categoryId;
			if (NavigationContext.QueryString.TryGetValue("categoryId", out categoryId))
			{
				Books books = await Storage.LoadAsync<Books>("books");
				Categories categories = await Storage.LoadAsync<Categories>("categories");

				//foreach (var cat in categories.categories)
				//{
				//	foreach (var subcat in cat.subcategories)
				//	{
				//		if (subcat.id == Convert.ToInt32(categoryId))
				//		{
				//			CategoryName.Text = subcat.name;
				//		}
				//	}
				//}

				foreach (var subcat in from cat in categories.categories from subcat in cat.subcategories where subcat.id == Convert.ToInt32(categoryId) select subcat)
				{
					CategoryName.Text = subcat.name;
					var booklist = books.books.Where(q => q.subcategory_id == subcat.id).ToList();

					foreach (var book in booklist)
					{
						book.cover_image_location = book.cover_image_location.Replace("{SIZE}", "528x528");
					}

					BookList.ItemsSource = booklist;
				}
			} 
		}

		private void BookList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			var selectedItem = (Books.Book) e.AddedItems[0];
		    if (selectedItem != null)
		    {
		        BookList.SelectedItem = null;
		        NavigationService.Navigate(new Uri(string.Format("/Book.xaml?bookId={0}", selectedItem.id), UriKind.Relative));
		    }
		}
	}
}