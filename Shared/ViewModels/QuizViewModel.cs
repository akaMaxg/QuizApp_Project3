using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp_Project3.Shared.ViewModels
{
    public class QuizViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public int NumberOfAttempts { get; set; }

        public List<QuestionViewModel>? Questions { get; set; }

        public int? Score { get; set; }
    }
}
