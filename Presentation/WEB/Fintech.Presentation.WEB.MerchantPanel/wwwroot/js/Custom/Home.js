function CurrencyModalload(CurrencySymbolTxt) {

    $('#CurrencySymbolTxt').val(CurrencySymbolTxt);
}

function PostCurrency() {
    //değişiklere göre tekrar düzenlenecek
    //todo
    var PostData = {
        CurrencySymbol: $('#CurrencySymbolTxt').val(),
        Price: $('#CurrencyPrice').val()
    }

    $.ajax({
        type: "POST",
        url: "/Home/AddCurrency",
        data: PostData,
        success: function (response) {
          
            //$('#CategoryAttributesDiv').empty();
            if (response.success != false) {
               
            } 

        },
        error: function () {
            alert("Ürün Özellikleri Alınamadı");
        }
    });


}