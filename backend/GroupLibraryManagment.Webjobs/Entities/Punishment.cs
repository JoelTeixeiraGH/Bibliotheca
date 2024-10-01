using System.Text.Json.Serialization;

namespace GroupLibraryManagment.Webjobs.Entities
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
        public static Punishment CreatePunishmentForRequest(int requestId)
        {
            var punishment = new Punishment
            {
                PunishmentReason = "Failed to return the book on time.",
                RequestId = requestId,
            };

            return punishment;
        }

        public static Punishment IncreasePunishmentLevel(Punishment punishment)
        {
            punishment.PunishmentLevel += 1;
            return punishment;
        }
    }
}
