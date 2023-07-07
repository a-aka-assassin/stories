// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function deleteStory(i)
{
    $.ajax({
        url: "/Stories/Delete",
        type: "post",
        data: {
            id : i
        }
    }).then(function(data)
    {
        window.location = '/Stories';
    });
}

function deleteChapter(i)
{
    $.ajax({
        url: "/Chapters/Delete",
        type: "post",
        data: {
            id : i
        }
    }).then(function(data)
    {
        window.location = '/Stories';
    });
}