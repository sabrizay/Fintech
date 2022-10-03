$(document).ready(function () {
    $(document).on('click', '#merchant-create-password', function () {
        if ($('#merchant-password').val() == $('#merchant-password-check').val()) {
            let model = {
                Email: $('#merchant-info-contact-email').val(),
                Password: $('#merchant-password').val(),
                CreatePasswordToken: $('#CreatePasswordToken').val()
            };
            var modelData = JSON.stringify(model);
            $.ajax({
                type: "POST",
                url: "/Merchant/createPassword",
                contentType: "application/json; charset=utf-8",
                data: modelData,
                //dataType: "json",
                success: function (data) {
                    $('.fkContent').html(data)
                },
                error: function () {
                    console.log('Veri kaydedilirken bir hata oluþtu.')
                }
            })
        }
    })
})