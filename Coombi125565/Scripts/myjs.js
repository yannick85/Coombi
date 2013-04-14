$("document").ready(function () {
    $("body").on("click", ".delete", function () {
        if (confirm("Are you sure ?")) {
            var id = $(this).data("id");
            $.post($(this).attr("href"), "id=" + id, function (data) {
            });
        }
        if ($(this).parent().hasClass("one-item")) {
            parent = $(this).parent();
        } else {
            parent = $(this).parent().parent();
        }
        $(parent).hide("clip", function () {
            $(parent).remove();
        });

        return false;
    });

    $("body").on("click", ".newlike", function () {
        var id = $(this).data("id");
        $.post("/post/like", "id=" + id, function (data) {
        });
        var currentLikes = parseInt($(this).parents(".post-like").find(".nb-like").text());
        currentLikes++;
        $(this).parents(".post-like").find(".nb-like").html("" + (currentLikes));
        $(this).parent(".update-like").html("<a class=\"unlike\" href=\"#\" data-id=\"" + id + "\">Unlike</a>");
        return false;
    });

    $("body").on("click", ".unlike", function () {
        var id = $(this).data("id");
        $.post("/post/unlike", "id=" + id, function (data) {
        });
        var currentLikes = parseInt($(this).parents(".post-like").find(".nb-like").text());
        currentLikes = currentLikes - 1;
        $(this).parents(".post-like").find(".nb-like").html("" + (currentLikes));
        $(this).parent(".update-like").html("<a class=\"newlike\" href=\"#\" data-id=\"" + id + "\">Like</a>");
        return false;
    });

    $("body").on("click", ".newcomment", function () {
        var that = this;
        var comment = $(this).parents(".newcomment-zone").find(".newcomment-content").attr("value");
        var id = $(this).data("id");
        $.post("/post/comment", "id=" + id + "&Comment=" + comment, function (data) {
            $(that).parents(".post-comment").find(".comments").append(data).find("> div").last().hide().show("clip");
            $(that).parents(".newcomment-zone").find(".newcomment-content").attr("value", "");
        });
        return false;
    });
});