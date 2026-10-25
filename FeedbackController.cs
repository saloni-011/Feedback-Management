using Microsoft.AspNetCore.Mvc;
using FeedbackManagementApp.Models;

namespace FeedbackManagementApp.Controllers
{
    public class FeedbackController : Controller
    {
        // In-memory static list to retain feedback entries during application runtime
        private static readonly List<Feedback> _feedbackList = new List<Feedback>();

        [HttpGet]
        public IActionResult Index()
        {
            // Analytics computations passed to the View
            ViewBag.TotalCount = _feedbackList.Count;
            ViewBag.AverageRating = _feedbackList.Any() ? _feedbackList.Average(f => f.Rating) : 0.0;
            ViewBag.PositivePercentage = _feedbackList.Any()
                ? (_feedbackList.Count(f => f.Rating >= 4) * 100 / _feedbackList.Count)
                : 0;

            return View(_feedbackList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                feedback.Id = _feedbackList.Count + 1;
                feedback.SubmittedOn = DateTime.Now;
                _feedbackList.Add(feedback);

                TempData["SuccessMessage"] = "Thank you! Your feedback has been submitted successfully.";
                return RedirectToAction(nameof(Index));
            }

            // Return to view with validation errors if invalid
            return View(feedback);
        }
    }
}
