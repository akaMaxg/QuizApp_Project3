
using QuizApp_Project3.Shared.ViewModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp_Project3.Server.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public int NumberOfAttempts { get; set; } = 0;

        public int Score { get; set; } = 0;

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public List<Question>? Questions { get; set; }

    }
}
