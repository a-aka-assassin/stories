using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Stories.Models;
using Stories.Models.ViewModels;

namespace Stories.Controllers;

public class StoriesController : Controller
{
    private readonly ILogger<StoriesController> _logger;
    private readonly IConfiguration _configuration;
    public StoriesController(ILogger<StoriesController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        var storyList = GetAllStories();
        return View(storyList);
    }

    public IActionResult NewStory()
    {
        return View();
    }
    public IActionResult StoryDetail(int id)
    {
        var story = GetStoryById(id);
        var storyViewModel = new StoriesViewModel();
        storyViewModel.Story = story;
        return View(storyViewModel);
    }

    public IActionResult EditStory(int id)
    {
        var story = GetStoryById(id);
        var storyViewModel = new StoriesViewModel();
        storyViewModel.Story = story;
        return View(storyViewModel);
    }
    //INSERT NEW STORY
    public ActionResult Insert(StoriesModel story)
    {
        using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("StoryDbContext")))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"INSERT INTO stories (title, genre) VALUES ('{story.Title}', '{story.Genre}')";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }
        }
        return RedirectToAction("Index");
    }

    //GET ALL Stories
    internal StoriesViewModel GetAllStories()
    {
        List<StoriesModel> stories = new();

        using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("StoryDbContext")))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"SELECT * FROM stories";

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            stories.Add(
                                new StoriesModel
                                {
                                    Id = reader.GetInt32(0),
                                    Title = reader.GetString(1),
                                    Genre = reader.GetString(2),
                                }
                            );
                        }
                    }
                    else
                    {
                        return new StoriesViewModel { Stories = stories };
                    }
                }
            }
        }
        return new StoriesViewModel { Stories = stories };
    }

    //Getting Story By ID
    internal StoriesModel GetStoryById(int id)
    {
        StoriesModel story = new();
        using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("StoryDbContext")))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"SELECT * FROM stories WHERE Id = '{id}'";

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            story.Id = reader.GetInt32(0);
                            story.Title = reader.GetString(1);
                            story.Genre = reader.GetString(2);

                        }
                    }
                    else
                    {
                        return story;
                    }
                }
            }
        }
        return story;
    }

    //Upadte Story
    public ActionResult Update(StoriesModel story)
    {

        using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("StoryDbContext")))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"UPDATE stories SET title = '{story.Title}', genre = '{story.Genre}' WHERE Id = '{story.Id}'";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return RedirectToAction("Index");
        }

    }

    //Delete A story
    [HttpPost]
    public JsonResult Delete(int id)
    {
        using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("StoryDbContext")))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"DELETE FROM stories WHERE Id = '{id}'";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }
        return Json(new object());
    }
}
