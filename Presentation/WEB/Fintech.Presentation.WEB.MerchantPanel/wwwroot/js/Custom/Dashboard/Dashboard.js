$(document).ready(function () {
    GetProducts();
});



function GetProducts() {
    $(".table > tbody").empty();
    $.ajax({
        type: "POST",
        url: "/Product/GetMerchantProductsByQuery",// serializes the form's elements.
        success: function (response) {
            if (response.success != false) {
                $.each(response.data, function (key, value) {
                    var row = '<tr>';
                    row += '<td>';
                    row += value.productName;
                    row += '</td>';
                    row += '<td>';
                    row += value.brandName;
                    row += '</td>';
                    row += '<td>';
                    row += value.stock;
                    row += '</td>';
                    row += '<td>';
                    row += value.listPrice;
                    row += ' ₺ </td>';
                    row += '<td>';
                    row += value.salePrice;
                    row += ' ₺ </td>';
                    row += '</tr>';
                    $(".table > tbody").append(row);
                });

            }
        }
    });
}