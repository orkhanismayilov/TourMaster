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
                        that.parent().parent().find("i[data-original-title='Active']").removeClass("icon-check text-success").addClass("icon-close text-warning").attr("data-original-title", "Inactive");
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
                if (data == 0) {
                    $("#dangerModal").modal('show');
                } else {
                    $("#successModalActivate").modal('show');
                    that.hide().prev().show();
                    that.parent().parent().find("i[data-original-title='Inactive']").removeClass("icon-close text-warning").addClass("icon-check text-success").attr("data-original-title", "Active");
                }
            }
        });
    });

    // Login Error
    if ($("#login-error").length > 0) {
        $("#dangerModal").modal('show');
    };

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

    // Select 2 Plugin
    if ($(".categories").length > 0) {
        $(".categories").select2({
            theme: "classic"
        });
    };

    // Accomodation Level Control
    $("select[name='accomodation']").on("change", function () {
        var that = $(this);
        if (that.val() != 3) {
            $("select[name='accomodationLvl']").attr("disabled", true);
        } else {
            $("select[name='accomodationLvl']").attr("disabled", false);
        }
    });

    // LoadImages
    $("input[type='file']").on("change", function (event) {
        var images = event.target.files;
        $("#dynamicImage").empty();
        console.log(images);
        for (var i = 0; i < images.length; i++) {
            $("#dynamicImage").append("<img src=" + URL.createObjectURL(event.target.files[i]) + " class='dynamicImg mr-2 mt-2' width='100px' />");
        }
    });

    // Main Image Select
    $("#tourimages li").css({"cursor": "pointer"});
    $("#tourimages li .img").on("click", function () {
        var that = $(this);
        var prevImage = $("#tourimages").find("li.border");
        $("#warningModal").modal("show");
        $("#confirm").on("click", function () {
            var url = "/manage/tours/SetMainImage";
            request = {};

            request["TourId"] = that.parent().data("tour-id");
            request["ImageId"] = that.parent().data("img-id");

            $.ajax({
                url: url,
                method: "post",
                data: request,
                dataType: "json",
                success: function (data) {
                    if (data == 1) {
                        prevImage.removeClass("border border-success").removeAttr("data-toggle").removeAttr("data-placement").removeAttr("data-original-title");
                        that.parent().addClass("border border-success").attr("data-toggle", "tooltip").attr("data-placement", "bottom").attr("data-original-title", "Main-Image");
                        $('[data-toggle="tooltip"]').tooltip();
                    }
                }
            });
        });
    });

    // Tour Image Delete
    $("#tourimages li i").on("click", function () {
        var that = $(this);
        $("#deleteWarningModal").modal('show');
        $("#confirmDelete").on("click", function () {
            url = "/manage/tours/deletetourimage/" + that.parent().data("img-id");

            $.ajax({
                url: url,
                method: "post",
                dataType: "json",
                success: function (data) {
                    if (data == 1) {
                        that.parent().remove();
                    }
                }
            });
        });
    });
});