using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using LuisterBieb.Models;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using StorageHelper.Apollo;

namespace LuisterBieb
{
	public partial class MainPage : PhoneApplicationPage
	{
		public MainPage()
		{
			InitializeComponent();

			//Categories categories = apiLogic.GetCategories();
			//Books books = apiLogic.GetBooks();

			// ToDo: Fill the categories with this data
			// ToDo: Retrieve the current state of the application

			// ToDo: Add a panorama item with offline books.

			// MainPage (Panorama)	-> Display the categories
			// Second page			-> Show the books
			// Third page			-> Show information about the book
            
			GetCategories();
			GetBooks();
		}

		private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
		{
			// Le me is happy
		}

		//
		//	Don't go beyond this point!
		//

		private const string BaseUri = "http://api.luisterboeken.infostradainteractive.com/api/";

		
		// ToDo: Reuse the common code...

		// ToDo: Ignore the books with author "Promotie pagina"
		// ToDo: Ignore books with a filesize of 0Mb
		// ToDo: Determine the size of the sound files. How?
		// ToDo: Add termination to these methods to prevent an infinite loop when no connection is available.

		public void GetBooks()
		{
			WebRequest request =
				(HttpWebRequest)HttpWebRequest.CreateHttp(new Uri(string.Format("{0}{1}/{2}", BaseUri, Method.books, "all")));

			request.BeginGetResponse(async delegate(IAsyncResult ar)
			{
				HttpWebRequest wrequest = (HttpWebRequest)ar.AsyncState;
				HttpWebResponse response = (HttpWebResponse)wrequest.EndGetResponse(ar);

				string str = new StreamReader(response.GetResponseStream()).ReadToEnd();
				Books books = JsonConvert.DeserializeObject<Books>(str);
				
				Deployment.Current.Dispatcher.BeginInvoke(() => Storage.SaveAsync("books", books));

			}, request);
		}

		public void GetCategories()
		{
			WebRequest request =
				(HttpWebRequest) HttpWebRequest.CreateHttp(new Uri(string.Format("{0}{1}/{2}", BaseUri, Method.categories, "all")));

			request.BeginGetResponse(async delegate(IAsyncResult ar)
			{
				// Prepopulate the list with known data.
				FillCategoriesLongList(Storage.LoadAsync<Categories>("categories").Result);

				// Request the new data from the web
				HttpWebRequest wrequest = (HttpWebRequest) ar.AsyncState;
				HttpWebResponse response = (HttpWebResponse) wrequest.EndGetResponse(ar);

				string str = new StreamReader(response.GetResponseStream()).ReadToEnd();
				Categories categories = JsonConvert.DeserializeObject<Categories>(str);

				// Store the new data in the isostore
				Deployment.Current.Dispatcher.BeginInvoke(() => Storage.SaveAsync("categories", categories));

				// Repopulate the list again with the new data
				FillCategoriesLongList(categories);

			}, request);
		}

		public Book GetBookDetail(int bookId)
		{
			var client = new WebClient();

			Book book = new Book();

			client.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs args)
			{
				book = JsonConvert.DeserializeObject<Book>(args.Result);
			};

			client.DownloadStringAsync(new Uri(string.Format("{0}{1}/{2}", BaseUri, Method.books, bookId)));

			while (book != null)
			{
				return book;
			}

			// Fuck it, code never comes this far.
			return null;
		}

		public void FillCategoriesLongList(Categories categories)
		{
			if (categories == null)
			{
				return;
			}

			var collection = new List<Group<Categories.Category.Subcategory>>();

			foreach (var category in categories.categories)
			{
				var group = new Group<Categories.Category.Subcategory>(category.name, category.subcategories);
				collection.Add(group);
			}

			Deployment.Current.Dispatcher.BeginInvoke(() => CategoryItems.ItemsSource = collection);
		}

		private void CategoryItems_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			var selectedItem = (Categories.Category.Subcategory) e.AddedItems[0];
		    if (selectedItem != null)
		    {
		        CategoryItems.SelectedItem = null;
		        NavigationService.Navigate(new Uri(string.Format("/Category.xaml?categoryId={0}", selectedItem.id),
		            UriKind.Relative));
		    }
		}
	}
}