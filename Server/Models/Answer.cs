using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp_Project3.Server.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsSelected { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}