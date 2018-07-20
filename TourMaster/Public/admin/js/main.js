$(document).ready(function () {

    // Tooltips
    $('[data-toggle="tooltip"]').tooltip();

    // Disable Tour Button
    $("a[data-original-title='Disable']").on("click", function (e) {
        e.preventDefault();
        var that = $(this);

        $("#warningModal").modal('show');
        $("#confirm").on("click", function () {
            url = that.attr("href");

            $.ajax({
                url: url,
                method: "get",
                dataType: "json",
                success: function (data) {
                    if (data == 0) {
                        $("#dangerModal").modal('show');
                    } else {
                        $("#successModal").modal('show');
                        that.hide().next().show();
                        $("i[data-original-title='Active']").removeClass("icon-check text-success").addClass("icon-close text-warning").attr("data-original-title", "Inactive");
                    }
                }
            });
        });
    });

    // Activate Tour Button
    $("a[data-original-title='Activate']").on("click", function (e) {
        e.preventDefault();
        var that = $(this);
        url = that.attr("href");

        $.ajax({
            url: url,
            method: "get",
            dataType: "json",
            success: function (data) {
                if (data == 1) {
                    that.hide().prev().show();
                    $("i[data-original-title='Inactive']").removeClass("icon-close text-warning").addClass("icon-check text-success").attr("data-original-title", "Active");
                } else {
                    $("#dangerModal").modal('show');
                }
            }
        });
    })

    // Login Error
    if ($("#login-error").length > 0) {
        $("#dangerModal").modal('show');
    }

    // GetCities
    $("select[name='fromCountry'], select[name='destCountry']").on("change", function () {
        var that = $(this);
        that.parent().next().children("select").empty().append("<option value='' disabled selected>City</option>");
        $.ajax({
            url: "/home/getcities/" + that.val(),
            method: "get",
            dataType: "json",
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var option = "<option value=" + data[i].Id + ">" + data[i].CityName + "</option>"
                    that.parent().next().children("select").append(option);
                }
            }
        });
    });

});