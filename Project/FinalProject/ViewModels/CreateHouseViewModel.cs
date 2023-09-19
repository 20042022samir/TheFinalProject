using FinalProject.Core.Entities;

namespace FinalProject.ViewModels
{
	public class CreateHouseViewModel
	{
		public string FullName { get; set; }
		public string Description { get; set; }
		public int FloorCount { get; set; }
		public string PhoneNumber { get; set; }
		public int Area { get; set; }
		public string Adrees { get; set; }
		public List<FormFile> files { get; set; }
		public decimal Price { get; set; }
		public int RoomCount { get; set; }

		public bool ForSale { get; set; }
		public bool ForRent { get; set; }

		


		public int CityId { get; set; }
		public int CounryId { get; set; }
		public int DistinctId { get;set; }
		public int? MetroId { get; set; }
		public int TypeeId { get; set; }

	}
}
