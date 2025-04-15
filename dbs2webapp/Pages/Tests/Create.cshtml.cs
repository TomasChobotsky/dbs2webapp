using dbs2webapp.Data;
using dbs2webapp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace dbs2webapp.Pages.Tests
{
    [Authorize(Roles = "Teacher,Admin")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TestInputModel Input { get; set; } = new();

        public Chapter Chapter { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Chapter = await _context.Chapters.FindAsync(id);
            if (Chapter == null)
            {
                return NotFound();
            }

            Input.ChapterId = (int)id;

            // Start with one empty question and 4 options
            Input.Questions.Add(new QuestionInputModel
            {
                Options = new List<OptionInputModel> { new(), new(), new(), new() }
            });

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // If ModelState is invalid, re-fetch any properties
            // the view expects (like Chapter) before returning Page().
            if (!ModelState.IsValid)
            {
                // IMPORTANT: repopulate properties needed by the view.
                Chapter = await _context.Chapters.FindAsync(Input.ChapterId);

                // Optionally, log ModelState errors for debugging.
                // foreach(var error in ModelState.Values.SelectMany(v => v.Errors))
                // {
                //     // Log error.ErrorMessage (e.g., to the console or a log file)
                // }

                return Page();
            }

            var test = new Test
            {
                Title = Input.Title,
                ChapterId = Input.ChapterId,
                Questions = new List<Question>()
            };

            foreach (var q in Input.Questions)
            {
                var question = new Question
                {
                    Content = q.Content,
                    Options = new List<Option>()
                };

                // Ensure the correct option index is within bounds
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

            _context.Tests.Add(test);
            await _context.SaveChangesAsync();

            return RedirectToPage("/TeacherPanel");
        }

        public class TestInputModel
        {
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
