namespace NguyenCoffeeWeb.Models.Dtos
{
    public class OrderBillDto
    {
        public List<CoffeeOrderDto> CoffeeOrderList { get; set; }
        public string? ImagePath { get; set; }
    }
}
