using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp_Project3.Shared.ViewModels
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int QuizId { get; set; }
        public string? MediaLink { get; set; }
        public List<AnswerViewModel>? Answers { get; set; }
    }
}
