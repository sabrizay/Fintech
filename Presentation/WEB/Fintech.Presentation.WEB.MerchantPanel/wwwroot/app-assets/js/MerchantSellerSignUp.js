$(document).ready(function () {
    $('#btnEdit').hide()
    $('#merchantSpinner').hide()
    $("#MerchantSellerInfo").click(function (e) {
        e.preventDefault();
        const dataToSend = $(".SingupForMerchantSeller").serializeArray();
        $(".merchant-info-full-name").html(dataToSend[0].value + " " + dataToSend[1].value);
        $(".merchant-info-phone").html(dataToSend[2].value);
        $(".merchant-info-email").html(dataToSend[3].value);
        $(".merchant-info-company-type").val($("#CompanyType option:selected").html());
        $(".merchant-info-tax-number").val(dataToSend[5].value);
        $(".merchant-info-company-name").val(dataToSend[6].value);
        
    })
    $("#SingUpMerchantNext").click(function (e) {
        $('#merchantSpinner').show()
        $('#btnEdit').hide()
        e.preventDefault();
        $(".merchantDataSuccess").remove();
        $(".merchantDataError").remove();
        const dataToSend = $(".SingupForMerchantSeller").serialize();
        $.ajax({
            url: "/satici-Kayit",
            type: "POST",
            //dataType: 'json',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            data: dataToSend,
            success: function (data) {
                $('#merchantSpinner').hide()
                $('#billing').prepend(data)
                if (data.indexOf("Hata") >= 0)
                    $('#btnEdit').show()
                else
                    $('#btnEdit').hide()
            },
            error: function () {
                console.log("Veri kaydedilirken hata oluþtu.");
            }

        });  
    })
    $('#CityId').change(function () {
        const cityId = $(this).val();
        let firstOption = $('.firstOption').html()
        $.ajax({
            url: "/Merchant/DistrictsByCityId?cityId=" + cityId,
            type: "GET",
            success: function (data) {
                $('#DistrictId').html('<option value="0" class="firstOption">' + firstOption + '</option>')
                $.each(data, function (i, item) {
                    $('#DistrictId').append('<option value="'+data[i].id+'">'+data[i].name+'</option>')
                })
            }
        });  
    })
})