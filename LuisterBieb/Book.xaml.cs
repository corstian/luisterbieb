using System;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using LuisterBieb.Models;
using Microsoft.Phone.Controls;
using StorageHelper.Apollo;

namespace LuisterBieb
{
    public partial class Book : PhoneApplicationPage
    {
        public Book()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string bookId;
            Books.Book book;


            if (NavigationContext.QueryString.TryGetValue("bookId", out bookId))
            {
                Books books = await Storage.LoadAsync<Books>("books");

                book = books.books.Single(q => q.id == Convert.ToInt32(bookId));

                BookTitle.Text = book.title;
                BookImage.Source = new BitmapImage(new Uri(book.cover_image_location.Replace("{SIZE}", "528x528"), UriKind.Absolute));
                BookDescription.Text = book.description;
            } 
        }
    }
}