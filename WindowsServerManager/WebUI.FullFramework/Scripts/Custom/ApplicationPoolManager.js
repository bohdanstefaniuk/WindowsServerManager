function ApplicationPoolManager() {
    var post = function(url) {
        $.ajax({
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: url,
            type: "POST",
            success: function (response) {
                if (response != null && response.success) {
                    alert(response.responseText);
                } else {
                    alert(response.responseText);
                }
            },
            error: function (response) {
                alert(response.responseText);
            },
            complete: function () {
                location.reload();
            }
        });
    };

    return {
        post: post
    }
}