using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Stories.Models;
using Stories.Models.ViewModels;

namespace Stories.Controllers;

public class ChaptersController : Controller
{
    private readonly ILogger<ChaptersController> _logger;
    private readonly IConfiguration _configuration;
    public ChaptersController(ILogger<ChaptersController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public IActionResult Index(int id)
    {
        var storyList = GetAllChapters(id);
        ViewBag.StoryId = id;
        return View(storyList);
    }
    public IActionResult NewChapter(int id)
    {
        var storyId = id;
        ViewBag.StoryId = storyId;
        return View();
    }

    public IActionResult ChapterDetail(int id)
    {
        var chapter = GetChapterById(id);
        var chapterDetail = new ChapterViewModel();
        chapterDetail.Chapter = chapter;
        return View(chapterDetail);
    }

    public IActionResult EditChapter(int id)
    {
        var chapter = GetChapterById(id);
        var chapterDetail = new ChapterViewModel();
        chapterDetail.Chapter = chapter;
        return View(chapterDetail);
    }
    //Get All Chapters Based On story Id
    internal ChapterViewModel GetAllChapters(int id)
    {
        List<ChapterModel> ChapterList = new();

        using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("StoryDbContext")))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"SELECT * FROM chapters WHERE StoryId = '{id}'";

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ChapterList.Add(
                                new ChapterModel
                                {
                                    Id = reader.GetInt32(0),
                                    Title = reader.GetString(1),
                                    Body = reader.GetString(2),
                                }
                            );
                        }
                    }
                    else
                    {
                        return new ChapterViewModel { ChaptersList = ChapterList };
                    }
                }
            }
        }
        return new ChapterViewModel { ChaptersList = ChapterList };
    }

    //INSERT NEW Chapter
    public ActionResult Insert(ChapterModel chapter)
    {
        using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("StoryDbContext")))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"INSERT INTO chapters (title, body, storyId) VALUES ('{chapter.Title}', '{chapter.Body}', '{chapter.StoryId}')";
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
        return RedirectToAction("Index", new { id = chapter.StoryId });
    }

    //Get Chapter By Id
    internal ChapterModel GetChapterById(int id)
    {
        ChapterModel chapter = new();

        using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("StoryDbContext")))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"SELECT * FROM chapters WHERE id = '{id}'";

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            chapter.Id = reader.GetInt32(0);
                            chapter.Title = reader.GetString(1);
                            chapter.Body = reader.GetString(2);
                        }
                    }
                    else
                    {
                        return chapter;
                    }
                }
            }
        }
        return chapter;
    }
//Update Chapter
public ActionResult Update(ChapterModel chapter)
{
       using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("StoryDbContext")))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"UPDATE chapters SET title = '{chapter.Title}', Body = '{chapter.Body}' WHERE Id = '{chapter.Id}'";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return RedirectToAction("ChapterDetail", new { id = chapter.Id });

    }


}

[HttpPost]
public JsonResult Delete(int id)
{
    using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("StoryDbContext")))
    {
        using (var command = connection.CreateCommand())
        {
            connection.Open();
            command.CommandText = $"DELETE FROM chapters WHERE Id = '{id}'";

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