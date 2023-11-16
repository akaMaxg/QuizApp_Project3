using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp_Project3.Shared.ViewModels
{
    public class GameQuizViewModel
    {
        public int Id { get; set; }
        public string? QuizName { get; set; }
        public int QuizId { get; set; }
        public string? AttemptedBy { get; set; }
        public int Score { get; set; }
    }
}
