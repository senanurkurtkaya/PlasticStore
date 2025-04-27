using System.Text.Json.Serialization;

namespace BLL.Dtos.FaqDto
{
    public class UpdateFaqDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int CategoryId { get; set; }
    }
}
