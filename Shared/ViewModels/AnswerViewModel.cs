using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp_Project3.Shared.ViewModels
{
    public class AnswerViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsSelected { get; set; }
        public int QuestionId { get; set; }
    }
}
