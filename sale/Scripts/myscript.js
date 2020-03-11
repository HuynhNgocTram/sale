
function Detail(id)
{
    window.location.href = "/Client/DetailProduct?id=" + id;
}

function ExportCustomer()
{
    window.location.href = "/Admin/ExportListCustomer";
}



function ResendOTP()
{
    $('#btnResend').prop("disabled", true);
    $.post("/Client/ResendOTP", null, function (data, status) {
        $('#btnResend').prop("disabled", false);
    });

}