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

    // Approve Tour Button
    $("a[data-original-title='Approve']").on("click", function (e) {
        e.preventDefault();
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
                    $("#successModalApprove").modal('show');
                    that.hide().prev().show();
                    that.parent().find("span.badge-danger").removeClass("badge-danger").addClass("badge-success").text("Active");
                }
            }
        });

        return false;
    });

    // Disapprove Tour Button
    $("a[data-original-title='Disapprove']").on("click", function (e) {
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
                        that.parent().find("span.badge-success").removeClass("badge-success").addClass("badge-danger").text("Not Approved");
                    }
                }
            });
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

    // Get Cities in Settings
    $("select[name='country']").on("change", function () {
        var that = $(this);
        console.log(that.val());
        that.parent().parent().next().children().children("select").empty().append("<option value='' disabled selected>City</option>");
        $.ajax({
            url: "/home/getcities/" + that.val(),
            method: "get",
            dataType: "json",
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var option = "<option value=" + data[i].Id + ">" + data[i].CityName + "</option>";
                    that.parent().parent().next().children().children("select").append(option);
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
            $("#dynamicImage").append("<img src=" + URL.createObjectURL(event.target.files[i]) + " class='dynamicImg mr-2 mt-2' width='150px' height='100px'/>");
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
        var url = "/manage/tours/deletetourimage/" + that.parent().data("img-id");

        $("#deleteWarningModal").modal('show');
        $("#confirmDelete").on("click", function () {

            var request = $.ajax({
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
        $("#cancelDelete").on("click", function () {
            that = null;
        });
    });

    // Slider Image Delete
    $("#sliderimages li i").on("click", function () {
        var that = $(this);
        var url = "/admin/origins/deleteimage/" + that.parent().data("img-id");

        $("#deleteWarningModal").modal('show');
        $("#confirmDelete").on("click", function () {
            var request = $.ajax({
                url: url,
                method: "post",
                dataType: "json",
                success: function (data) {
                    if (data == 1) {
                        that.parent().remove();
                    } else {
                        $("#dangerModal").modal('show');
                    }
                }
            });
        });
        $("#cancelDelete").on("click", function () {
            that = null;
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
                        var or = $("#open-requests");
                        orCount = parseInt(or.text()) - 1;
                        if (orCount > 0) {
                            or.text(orCount);
                        } else {
                            or.remove();
                        }
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

    // Feedbacks Perfect Scrollbar
    if ($("div.feedbacks").length > 0) {
        var ps = new PerfectScrollbar("div.feedbacks");
    }

    // Messages List Perfect Scrollbar
    if ($("div.messages-list").length > 0) {
        var ps = new PerfectScrollbar("div.messages-list");
        $("div.messages-list").scrollTop = $("div.messages-list").scrollHeight;
    }

    // Notifications Seen
    $("#noti-dropdown").on("shown.bs.dropdown", function () {
        var that = $(this),
            url = "/home/notiseen/" + that.data("guide");

        $.ajax({
            url: url,
            method: "post",
            dataType: "json",
            success: function (data) {
                if (data == 1) {
                    that.find(".notis").attr("style", "background-color: transparent !important");
                    that.find(".dropdown-header strong").text("You have 0 notifications");
                    that.find(".icon-bell").next().remove();
                }
            }
        });
    });

    // Change Password Modal
    $("#changePassBtn").on("click", function () {
        $("#passModal").modal('show');
        $("#changePass").on("submit", function () {

            var that = $(this),
                url = that.attr("action"),
                method = that.attr("method"),
                request = {};

            that.find("[name]").each(function (idnex, value) {
                var that = $(this),
                    name = that.attr("name"),
                    value = that.val();

                request[name] = value;
            });

            $.ajax({
                url: url,
                method: method,
                data: request,
                dataType: "json",
                success: function (data) {
                    if (data == 1) {
                        $("#passModal").modal('hide');
                        $("#passModal form")[0].reset();
                        $("#successModal").modal('show');
                    } else {
                        $("#dangerModal").modal('show');
                    }
                }
            });

            return false;
        });

        return false;
    });


    // Reply to Message
    $("#reply-to-message").on("submit", function () {
        var that = $(this),
            url = that.attr("action"),
            method = that.attr("method"),
            request = {};

        that.find("[name]").each(function (index, value) {
            var that = $(this),
                name = that.attr("name"),
                value = that.val();

            request[name] = value;
        });

        $.ajax({
            url: url,
            method: method,
            data: request,
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    var message = `<div class="message-body rounded p-3 mt-2 w-75 animated fadeIn ml-auto bg-gray">
                        <p class="text-justify">`+data.Message+`</p>
                        <small class="d-block text-right">
                            `+data.Date+`
                            <i class="fas fa-check text-gray ml-2" data-toggle="tooltip" data-placement="bottom" title="Delivered"></i>
                        </small>
                    </div>`;

                    that.find("[name='message']").val("");
                    $(".messages-list").prepend(message);
                }
            }
        });

        return false;
    });

});