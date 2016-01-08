using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using LuisterBieb.Models;
using Newtonsoft.Json;

namespace LuisterBieb
{
	public class Group<T> : List<T>
	{
		public Group(string name, IEnumerable<T> items) : base(items)
		{
			this.Title = name;
		}

		public string Title { get; set; }
	}

	public enum Method
	{
		books,
		categories
	}
}
