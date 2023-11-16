using QuizApp_Project3.Server.Data;
using QuizApp_Project3.Server.Models;
using QuizApp_Project3.Shared.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace QuizApp_Project3.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public QuizController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [HttpPost("createquiz")]
        public async Task<ActionResult<QuizViewModel>> CreateQuiz([FromBody] QuizViewModel quiz)
        {

            var newQuiz = new Quiz
            {
                Title = quiz.Title,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            };

            _context.Quizzes.Add(newQuiz);
            await _context.SaveChangesAsync();
            return Ok(newQuiz);
        }

        [HttpPost("gamequiz")]
        public async Task<ActionResult<GameQuizViewModel>> GameQuizEntry([FromBody] GameQuizViewModel game)
        {

            var newGame = new GameQuiz
            {
                QuizId = game.QuizId,
                AttemptedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Score = game.Score
            };

            _context.GameQuizzes.Add(newGame);
            await _context.SaveChangesAsync();
            return Ok(newGame);
        }

        [HttpPost("createquestion")]
        public async Task<ActionResult<QuestionViewModel>> CreateQuestions([FromBody] QuestionViewModel questions)
        {
            var newQuestion = new Question
            {
                Content = questions.Content,
                QuizId = questions.QuizId,
                MediaLink = questions.MediaLink
            };
            _context.Questions.Add(newQuestion);
            await _context.SaveChangesAsync();
            return Ok(newQuestion);
        }

        private async Task<IActionResult> IncrementQuizScore(int quizId)
        {
            var quiz = await _context.Quizzes.FindAsync(quizId);
            if (quiz == null)
                return NotFound();
            quiz.Score += 1;
            await _context.SaveChangesAsync();
            return Ok();
        }

        private async Task<IActionResult> IncrementAttempt(int quizId)
        {
            var quiz = await _context.Quizzes.FindAsync(quizId);
            if (quiz == null)
                return NotFound();
            quiz.NumberOfAttempts += 1;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("createanswer")]
        public async Task<ActionResult<List<AnswerViewModel>>> CreateAnswers([FromBody] List<AnswerViewModel> answerList)
        {
            var answers = new List<Answer>();

            foreach (var answerVM in answerList)
            {
                var answer = new Answer
                {
                    Content = answerVM.Content,
                    IsCorrect = answerVM.IsCorrect,
                    QuestionId = answerVM.QuestionId
                };

                answers.Add(answer);
            }
            _context.Answers.AddRange(answers);
            await _context.SaveChangesAsync();
            return Ok(answers);
        }

        [HttpGet("userquiz")]
        public async Task<ActionResult<List<QuizViewModel>>> GetUserData()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);

                var quizzes = await _context.Quizzes
                    .Where(q => q.UserId == userId)
                    .Select(q => new QuizViewModel
                    {
                        Title = q.Title,
                        Created = q.Created,
                        NumberOfAttempts = q.NumberOfAttempts,
                        Score = _context.Questions.Count(gq => gq.QuizId == q.Id),

                    })
                        .ToListAsync();
                return Ok(quizzes);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("myquiz")]
        public async Task<ActionResult<List<GameQuizViewModel>>> GetMyQuizzes()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var gameEntries = await _context.GameQuizzes
                    .Where(q => q.Quiz.UserId == userId) 
                    .Select(q => new GameQuizViewModel
                    {
                        QuizName = q.Quiz.Title,
                        QuizId = q.QuizId,
                        AttemptedBy = q.Quiz.User.UserName,
                        Score = q.Score
                    })
                    .ToListAsync();

                return Ok(gameEntries);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getquiz/{title}")]
        public async Task<QuizViewModel> GetQuiz(string title)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(qz => qz.Title == title);

            if (quiz == null)
            {
                throw new Exception("Quiz could not be found.");
            }

            var questionsView = quiz.Questions.Select(question => new QuestionViewModel
            {
                Id = question.Id,
                Content = question.Content,
                QuizId = question.QuizId,
                MediaLink = question.MediaLink,
                Answers = question.Answers.Select(answer => new AnswerViewModel
                {
                    Id = answer.Id,
                    Content = answer.Content,
                    IsCorrect = answer.IsCorrect,
                    IsSelected = answer.IsSelected,
                    QuestionId = answer.QuestionId
                }).ToList()
            }).ToList();

            var quizViewModel = new QuizViewModel
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Created = quiz.Created,
                NumberOfAttempts = quiz.NumberOfAttempts,
                Questions = questionsView,
                Score = _context.Questions.Count(gq => gq.QuizId == quiz.Id)
            };
            IncrementAttempt(quiz.Id);
            return quizViewModel;
        }
    }
}
