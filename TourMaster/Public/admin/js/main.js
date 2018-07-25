$(document).ready(function () {

    // Tooltips
    $('[data-toggle="tooltip"]').tooltip();

    // Disable Tour Button
    $("a[data-original-title='Disable']").on("click", function (e) {
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
                        that.parent().find("span.badge-success").removeClass("badge-success").addClass("badge-warning").text("Inactive");
                    }
                }
            });
        });

        return false;
    });

    // Activate Tour Button
    $("a[data-original-title='Activate']").on("click", function (e) {
        var that = $(this),
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
                    that.parent().find("span.badge-warning").removeClass("badge-warning").addClass("badge-success").text("Active");
                }
            }
        });

        return false;
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
        for (var i = 0; i < images.length; i++) {
            $("#dynamicImage").append("<img src=" + URL.createObjectURL(event.target.files[i]) + " class='dynamicImg mr-2 mt-2' width='100px' />");
        }
    });

    // Main Image Select
    $("#tourimages li").css({ "cursor": "pointer" });
    $("#tourimages li .img").on("click", function () {
        var that = $(this),
            prevImage = $("#tourimages").find("li.border");
        $("#warningModal").modal("show");
        $("#confirm").on("click", function () {
            var url = "/manage/tours/SetMainImage",
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
            var url = "/manage/tours/deletetourimage/" + that.parent().data("img-id");

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

    // Booking Request Confirm
    $("a[data-original-title='Confirm']").on("click", function () {
        var that = $(this);
        $("#warningModal").modal('show');
        $("#confirmRequest").on("click", function () {
            var url = that.attr("href");

            $.ajax({
                url: url,
                method: "post",
                dataType: "json",
                success: function (data) {
                    if (data == 1) {
                        $("#successModal").modal('show');
                        var td = that.parent();
                        td.empty();
                        td.append('<span class="badge badge-success">Confirmed</span>');
                        var or = $("#open-requests");
                        orCount = parseInt(or.text()) - 1;
                        if (orCount > 0) {
                            or.text(orCount);
                        } else {
                            or.remove();
                        }
                    } else if (data == 2) {
                        $("#dangerModal .modal-body p").html("Oops... Error occured while confirming. <br />Please, try later!");
                        $("#dangerModal").modal('show');
                    } else {
                        $("#dangerModal .modal-body p").text("You already have a booking on these dates. Please, check your bookings first.");
                        $("#dangerModal").modal('show');
                    }
                }
            });
        });

        return false;
    });

    // Booking Request Reject
    $("a[data-original-title='Reject']").on("click", function () {
        var that = $(this);
        $("#rejectWarningModal").modal('show');
        $("#confirmRejectRequest").on("click", function () {
            url = that.attr("href");

            $.ajax({
                url: url,
                method: "post",
                dataType: "json",
                success: function (data) {
                    if (data == 1) {
                        var td = that.parent();
                        td.empty();
                        td.append('<span class="badge badge-secondary">Rejected</span>');
                    } else {
                        $("#dangerModal .modal-body p").html("Oops... Error occured while rejecting. <br />Please, try later!");
                        $("#dangerModal").modal('show');
                    }
                }
            });
        });

        return false;
    });

    // Booking Cancel
    $("a[data-original-title='Cancel']").on("click", function () {
        var that = $(this);
        $("#warningModal").modal('show');
        $("#cancelOK").on("click", function () {
            var url = that.attr("href");

            $.ajax({
                url: url,
                method: "post",
                dataType: "json",
                success: function (data) {
                    if (data == 1) {
                        that.prev().removeClass("badge-success").addClass("badge-danger").text("Cancelled");
                        that.remove();
                        $("#successModal").modal('show');
                    } else {
                        $("dangerModal").modal('show');
                    }
                }
            });
        });

        return false;
    });
});