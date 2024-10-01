using GroupLibraryManagment.API.DTOs;
using System.Text.Json.Serialization;

namespace GroupLibraryManagment.API.Entities
{
    public enum PunishmentLevel
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5
    }
    public class Punishment
    {
        public int PunishmentId { get; set; }
        public string PunishmentReason { get; set; } = string.Empty;
        public PunishmentLevel PunishmentLevel { get; set; } = PunishmentLevel.One;
        public DateTime EmittedDate { get; set; } = DateTime.Now;
        [JsonIgnore]
        public Request Request { get; set; }
        public int RequestId { get; set; }
        public static Punishment Create(PunishmentDTO input)
        {
            var punishment = new Punishment
            {
                PunishmentReason = input.PunishmentReason,
                RequestId = input.RequestId,
            };

            return punishment;
        }
        public void Update(PunishmentDTO input)
        {
            PunishmentReason = input.PunishmentReason;
            RequestId = input.RequestId;
        }
        public void UpdateLevel(PunishmentLevel level)
        {
            PunishmentLevel = level;
        }
    }
}
