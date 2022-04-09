using Entities.DataTransferObjects;

namespace UnitsOfTest.Hotel
{
    public class HotelData
    {
        public static TasksDetailsDto MockHotelDetailsDto() => new()
        {
            Name = "Ahoy",
            Address = "Dubai",
            Contact= "97555555555",
            Description = "New",
            Email= "Ahoy@hotel.com",
         
            Location =null,
            NumberOfRooms=0,
            RatingScale = null,
            
        };
    }
}
