using FinalProject.Core.Entities;

namespace FinalProject.ViewModels
{
	public class HomeViewModel
	{
		public IEnumerable<House>? houses { get; set; }
		public IEnumerable<Price>? Prices { get; set; }
		public IEnumerable<Advertise>? Advertises { get; set; }
		public IEnumerable<SalePrice>? SalePrices { get; set; }
		public IEnumerable<Typee>? Typees { get; set; }
		public IEnumerable<City>? Cities { get;set; }
		public IEnumerable<Metro>? Metros { get; set; }
		public IEnumerable<Country>? Countries { get; set; }
		public IEnumerable<Distinct>? Distincts { get; set; }
		public int? CityId { get; set; }
		public int? TypeeId { get;set; }
		public int? PriceId { get; set; }
		public int? SalePriceId { get;set; }
		public int? RoomCount { get; set; }
		public int? CountryId { get;set; }
		public int? MetroId { get;set; }
		public int? DistinceId { get;set; }
	}
}
