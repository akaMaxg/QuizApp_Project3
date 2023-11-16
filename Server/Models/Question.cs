using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp_Project3.Server.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Content { get; set; }

        [ForeignKey("Quiz")]
        public int QuizId { get; set; }
        public Quiz? Quiz { get; set; }
        public string? MediaLink { get; set; }
        public List<Answer>? Answers { get; set; }

    }
}