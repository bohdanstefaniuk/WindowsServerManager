function AjaxPost() {
    var _post = function(url, data) {
        $.ajax({
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            url: url,
            data: data,
            type: "POST",
            success: function (response) {
                if (response != null && response.success) {
                    alert(response.responseText);
                } else {
                    alert(response.responseText);
                }
                if (response.redirectUrl) {
                    document.location.href = response.redirectUrl;
                }
            },
            error: function (response) {
                alert(response.responseText);
            },
            complete: function (response) {
                location.reload();
            }
        });
    };

    const post = function (url, data) {
        if (data) {
            _post(url, JSON.stringify(data));
        } else {
            _post(url, null); 
        }
    };
    return {
        post: post
    }
}