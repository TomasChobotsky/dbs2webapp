using dbs2webapp.Data;
using dbs2webapp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dbs2webapp.Pages.Tests
{
    [Authorize(Roles = "Teacher,Admin")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TestInputModel Input { get; set; } = new();

        public Chapter Chapter { get; set; }

        // OnGetAsync loads the existing Test (with questions and options) by its Id.
        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Load the test including its nested Questions and Options.
            var test = await _context.Tests
                .Include(t => t.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (test == null)
            {
                return NotFound();
            }

            // Load the Chapter to display
            Chapter = await _context.Chapters.FindAsync(test.ChapterId);

            // Map the data from the Test to our input model.
            Input.Id = test.Id;
            Input.Title = test.Title;
            Input.ChapterId = test.ChapterId;
            Input.Questions = new List<QuestionInputModel>();
            foreach (var q in test.Questions)
            {
                var qInput = new QuestionInputModel
                {
                    Content = q.Content,
                    CorrectOptionIndex = 0, // Default, then update below.
                    Options = new List<OptionInputModel>()
                };

                // Set options and determine which option is correct.
                for (int i = 0; i < q.Options.Count; i++)
                {
                    var option = q.Options[i];
                    qInput.Options.Add(new OptionInputModel
                    {
                        Text = option.Text
                    });
                    if (option.IsCorrect)
                    {
                        qInput.CorrectOptionIndex = i;
                    }
                }
                Input.Questions.Add(qInput);
            }

            return Page();
        }

        // OnPostAsync updates the test with the new data.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Repopulate Chapter for display
                Chapter = await _context.Chapters.FindAsync(Input.ChapterId);
                return Page();
            }

            // Retrieve the existing test
            var test = await _context.Tests
                .Include(t => t.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(t => t.Id == Input.Id);

            if (test == null)
            {
                return NotFound();
            }

            // Update simple properties
            test.Title = Input.Title;

            // Remove existing questions (and their options) so we can re-add them.
            // (Alternatively, you could update individual questions/options as needed.)
            _context.Questions.RemoveRange(test.Questions);

            // Build a new list of questions from the input.
            test.Questions = new List<Question>();
            foreach (var q in Input.Questions)
            {
                var question = new Question
                {
                    Content = q.Content,
                    Options = new List<Option>()
                };

                for (int i = 0; i < q.Options.Count; i++)
                {
                    var option = new Option
                    {
                        Text = q.Options[i].Text,
                        IsCorrect = (i == q.CorrectOptionIndex)
                    };
                    question.Options.Add(option);
                }

                test.Questions.Add(question);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("/TeacherPanel");
        }

        public class TestInputModel
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int ChapterId { get; set; }
            public List<QuestionInputModel> Questions { get; set; } = new();
        }

        public class QuestionInputModel
        {
            public string Content { get; set; }
            public int CorrectOptionIndex { get; set; }
            public List<OptionInputModel> Options { get; set; } = new();
        }

        public class OptionInputModel
        {
            public string Text { get; set; }
        }
    }
}
