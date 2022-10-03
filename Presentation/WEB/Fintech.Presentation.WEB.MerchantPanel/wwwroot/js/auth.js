// login
$('#login-btn').click(e => {
    e.preventDefault();

    let email = $('#login-email').val();
    let password = $('#login-password').val();
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: 'login',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        headers: {
            'RequestVerificationToken': token
        },
        async: false,
        data: JSON.stringify({
            "email": email,
            "password": password
        }),
        success: function (result) {
            if (result.success) {
                toastr.success(result.message, 'Başarılı');

                setCookie("ModalogToken", result.data.token, result.data.expiration);
            }
            else {
                if (result.data) {
                    result.data.forEach(message => {
                        toastr.error(message, 'Hata!');
                    });
                }
                else
                    toastr.error(result.message, 'Hata!');
            }
        },
        error: function (xhr) {
            toastr.error(xhr.responseText, 'Hata!');
        }
    });
});

// forgot password
$('#forgot-password-btn').click(e => {
    e.preventDefault();

    let email = $('#forgot-password-email').val();
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: 'forgot-password',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        headers: {
            'RequestVerificationToken': token
        },
        async: false,
        data: JSON.stringify({
            "email": email
        }),
        success: function (result) {
            if (result.success) {
                toastr.success(result.message, 'Başarılı');
            }
            else {
                if (result.data) {
                    result.data.forEach(message => {
                        toastr.error(message, 'Hata!');
                    });
                }
                else
                    toastr.error(result.message, 'Hata!');
            }
        },
        error: function (xhr) {
            toastr.error(xhr.responseText, 'Hata!');
        }
    });
});