using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp_Project3.Server.Models
{
    public class GameQuiz
    {
        public int Id { get; set; }

        [ForeignKey("Quiz")]
        public int QuizId { get; set; }
        public virtual Quiz? Quiz { get; set; }

        [ForeignKey("User")]
        public string? AttemptedBy { get; set; }
        public virtual ApplicationUser? User { get; set; }

        public int Score { get; set; }
    }
}
