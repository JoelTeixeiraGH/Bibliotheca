using GroupLibraryManagment.API.DTOs;
using Microsoft.VisualBasic;
using System.Text.Json.Serialization;

namespace GroupLibraryManagment.API.Entities
{
    public enum EvaluationScore
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5
    }
    public class Evaluation
    {
        public int EvaluationId { get; set; }
        public string EvaluationDescription { get; set; } = string.Empty;
        public EvaluationScore EvaluationScore { get; set; }
        public DateTime EmittedDate { get; set; } = DateTime.Now.Date;
        [JsonIgnore]
        public GenericBook GenericBook { get; set; }
        public string ISBN { get; set; } = string.Empty;
        [JsonIgnore]
        public User User { get; set; }
        public int UserId { get; set; }
        public static Evaluation Create(EvaluationDTO input)
        {
            var evaluation = new Evaluation
            {
                EvaluationDescription = input.EvaluationDescription,
                EvaluationScore = input.EvaluationScore,
                UserId = input.UserId,
                ISBN = input.ISBN,
            };

            return evaluation;
        }
        public void Update(EvaluationDTO input)
        {
            EvaluationDescription = input.EvaluationDescription;
            EvaluationScore = input.EvaluationScore;
        }
    }
}
