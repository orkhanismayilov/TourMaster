$(document).ready(function () {
    // Images Loaded
    $("body").imagesLoaded(function () {
        var preloader = $("#preloader");
        preloader.fadeOut();

        // SignUp Sweet Alerts
        if ($("#signup-alert-success").length > 0) {
            swal({
                type: 'success',
                title: 'Success',
                text: 'Now you can login!'
            });
        } else if ($("#signup-alert-error").length > 0) {
            swal({
                type: 'error',
                title: 'Oops...',
                text: $("#signup-alert-error").data("msg")
            });
        }

        // LogIn Sweet Alert
        if ($("#login-alert-success").length > 0) {
            swal({
                type: 'success',
                title: 'Success',
                text: 'You are logged in!'
            });
        } else if ($("#login-alert-error").length > 0) {
            swal({
                type: 'error',
                title: 'Oops...',
                text: 'Invalid Email or Password!'
            });
        }
    });

    // Animate CSS Function Extension
    $.fn.extend({
        animateCss: function (animationName, callback) {
            var animationEnd = (function (el) {
                var animations = {
                    animation: 'animationend',
                    OAnimation: 'oAnimationEnd',
                    MozAnimation: 'mozAnimationEnd',
                    WebkitAnimation: 'webkitAnimationEnd'
                };

                for (var t in animations) {
                    if (el.style[t] !== undefined) {
                        return animations[t];
                    }
                }
            })(document.createElement('div'));

            this.addClass('animated ' + animationName).one(animationEnd, function () {
                $(this).removeClass('animated ' + animationName);

                if (typeof callback === 'function') callback();
            });

            return this;
        }
    });

    // Fixed Navbar & Scroll Top Button Animation
    $(window).scroll(function () {
        // Navbar
        if ($(document).scrollTop() >= 10) {
            $(".navbar").addClass("fixed");
        } else {
            $(".navbar").removeClass("fixed");
        }

        // Scroll Top
        if ($(document).scrollTop() > 500) {
            $("#scroll-top").css({
                "bottom": "5%",
                "opacity": "1"
            });
        } else {
            $("#scroll-top").css({
                "bottom": "-10%",
                "opacity": "0"
            });
        }
    });

    // Scroll Top Animation
    if ($("#scroll-top")) {
        $("#scroll-top").click(function (e) {
            e.preventDefault();
            $("html").animate({ scrollTop: 0 }, 'slow');
        });
    }

    // Scroll To Section
    if ($("#navigation")) {
        $("#navbar-collapsable a").on("click", function (e) {
            if (this.hash !== "") {
                e.preventDefault();
                var hash = this.hash;
                var position = $(hash).offset().top - 85;
                $("html").animate({
                    scrollTop: position
                }, 800);
                $('.navbar-collapse').collapse('hide');
            }
        });
    }

    // Notifications Modal
    $("#notifications-trigger").on("click", function (e) {
        e.preventDefault();
        $("#notifications-modal").addClass("animated fadeIn").modal('show');
        $("#notifications-modal").animateCss("fadeIn", function () {
            $("#notifications-modal").removeClass("animated fadeIn");
        });
        var nt = new PerfectScrollbar(".noti-table");
    });

    // Notifications Modal Hide
    $("#notifications-modal").on("hide.bs.modal", function (e) {
        if (!$("#notifications-modal").hasClass("animated")) {
            e.preventDefault();
            $("#notifications-modal").addClass("animated fadeOut");
            setTimeout("$('#notifications-modal').modal('hide')", 550);
        }
    });

    // Notifications Modal Hidden
    $("#notifications-modal").on("hidden.bs.modal", function () {
        $("#notifications-modal").removeClass("animated fadeOut");
    });

    // Notifications Seen
    $("#noti-dropdown").on("shown.bs.dropdown", function () {
        var that = $(this),
            url = "/home/notiseen/" + that.attr("data-user-id");

        $.ajax({
            url: url,
            method: "post",
            dataType: "json",
            success: function (data) {
                if (data == 1) {
                    that.find("a.notification").attr("style", "background-color: transparent !important");
                    that.find("#notifications-dropdown").find("i").removeClass("text-danger");
                    that.find("#notifications-dropdown").find("span").removeClass("badge-danger").addClass("badge-secondary").text(0);
                }
            }
        });
    });

    // Main Slider
    if ($(".main-slider")) {
        $(".main-slider").owlCarousel({
            items: 1,
            loop: true,
            mouseDrag: false,
            touchDrag: false,
            nav: false,
            dots: false,
            autoplay: true,
            autoplayTimeout: 10000,
            autoplayHoverPause: true,
            smartSpeed: 2500
        });
    }

    // Date Pickers
    if ($(".datepicker")) {
        // Activation Date Start Picker
        var dateStart = $("#date-start").pickadate({
            container: "#main-car",
            firstDay: 1,
            min: new Date(),
            format: "dd.mm.yyyy",
            onClose: function () {
                $(document.activeElement).blur();
            }
        });
        var startPicker = dateStart.pickadate('picker');

        // Activation Date End Picker
        var dateEnd = $("#date-end").pickadate({
            container: "#main-car",
            firstDay: 1,
            min: new Date(),
            format: "dd.mm.yyyy",
            onClose: function () {
                $(document.activeElement).blur();
            }
        });
        var endPicker = dateEnd.pickadate('picker');

        // If Start Date Selected Set End Date min to Start Date Value
        startPicker.on('set', function (e) {
            if (e.select) {
                endPicker.set('min', startPicker.get('value'));
            } else if ('clear' in e) {
                endPicker.set('min', new Date());
            }
        });

        // If End Date Selected Set Start Date max to End Date Value
        endPicker.on('set', function (e) {
            if (e.select) {
                startPicker.set('max', endPicker.get('value'));
            } else if ('clear' in e) {
                startPicker.set('max', false);
            }
        });

        // Activation User B-Day Picker
        $("#b-day-date").pickadate({
            container: "#signup-form-modal",
            firstDay: 1,
            format: "dd.mm.yyyy",
            selectMonths: true,
            selectYears: 70,
            max: true,
            onClose: function () {
                $(document.activeElement).blur();
            }
        });

    }

    // Signup Form Modal
    if ($(".signup-form-modal-wrapper")) {
        $("#signup-form-modal-trigger").click(function (e) {
            e.preventDefault();
            $("#signup-form-modal").addClass("animated fadeIn").modal('show');
            $("#signup-form-modal").animateCss("fadeIn", function () {
                $("#signup-form-modal").removeClass("animated fadeIn");
            });
        });

        // Signup Form Modal Hide
        $("#signup-form-modal").on("hide.bs.modal", function (e) {
            if (!$("#signup-form-modal").hasClass("animated")) {
                e.preventDefault();
                $("#signup-form-modal").addClass("animated fadeOut");
                setTimeout("$('#signup-form-modal').modal('hide')", 550);
            }
        });

        // Signup Form Modal Hidden
        $("#signup-form-modal").on("hidden.bs.modal", function () {
            $("#signup-form-modal").removeClass("animated fadeOut");
            $('#signup-form').get(0).reset();
        });

        // Go to Login Modal
        $("#go-to-login").on("click", function () {
            $("#signup-form-modal").modal('hide');
            $("#login-form-modal").addClass("animated fadeIn").modal('show');
            $("#login-form-modal").animateCss("fadeIn", function () {
                $("#login-form-modal").removeClass("animated fadeIn");
            });
        });

        // Guide Join Signup Modal
        $("#guide-join-signup").on("click", function () {
            $("#account-type-traveler").removeAttr("checked");
            $("#account-type-guide").attr("checked", true);
            $("#signup-form-modal").addClass("animated fadeIn").modal('show');
            $("#signup-form-modal").animateCss("fadeIn", function () {
                $("#signup-form-modal").removeClass("animated fadeIn");
            });
        });

        // Get Cities by Country Selected
        $("#signup-form #country").on("change", function () {
            $("#signup-form #city").empty();
            $("#signup-form #city").append("<option value='' disabled selected></option>");
            var GetCitiesUrl = "/Home/GetCities/" + $("#signup-form #country").val();
            $.ajax({
                url: GetCitiesUrl,
                method: "get",
                dataType: "json",
                success: function (data) {
                    for (var i = 0; i < data.length; i++) {
                        var option = $("<option></option>");
                        option.text(data[i].CityName);
                        option.attr("value", data[i].Id);
                        $("#signup-form #city").append(option);
                    }
                }
            });
        });
    }

    // Signup Form Validation
    if ($(".signup-form-modal-wrapper")) {
        $.validate({
            modules: 'security, date',
            form: '#signup-form'
        });
    }

    // Login Form Modal
    if ($(".login-form-modal-wrapper")) {
        $("#login-form-modal-trigger").click(function (e) {
            e.preventDefault();
            $("#login-form-modal").addClass("animated fadeIn").modal('show');
            $("#login-form-modal").animateCss("fadeIn", function () {
                $("#login-form-modal").removeClass("animated fadeIn");
            });
        });

        // Login Form Modal Hide
        $("#login-form-modal").on("hide.bs.modal", function (e) {
            if (!$("#login-form-modal").hasClass("animated")) {
                e.preventDefault();
                $("#login-form-modal").addClass("animated fadeOut");
                setTimeout("$('#login-form-modal').modal('hide')", 550);
            }
        });

        // Login Form Modal Hidden
        $("#login-form-modal").on("hidden.bs.modal", function () {
            $("#login-form-modal").removeClass("animated fadeOut");
            $('#traveler-login-form, #guide-login-form').get(0).reset();
        });

        // Go to Signup Modal
        $(".go-to-signup").on("click", function () {
            $("#login-form-modal").modal('hide');
            $("#signup-form-modal").addClass("animated fadeIn").modal('show');
            $("#signup-form-modal").animateCss("fadeIn", function () {
                $("#signup-form-modal").removeClass("animated fadeIn");
            });
        });

        // Password Reovery Form Modal
        $(".pass-recovery").on("click", function (e) {
            e.preventDefault();
            $("#login-form-modal").modal('hide');
            $("#pass-recovery-form-modal").addClass("animated fadeIn").modal('show');
            $("#pass-recovery-form-modal").animateCss("fadeIn", function () {
                $("#pass-recovery-form-modal").removeClass("animated fadeIn");
            });
        });
    }

    // Login Form Validation
    if ($(".login-form-modal-wrapper")) {
        $.validate({
            modules: 'security',
            form: '#traveler-login-form, #guide-login-form'
        });
    }

    // Password Recovery Form Validation
    if ($(".pass-recovery-form-modal-wrapper")) {
        $.validate({
            form: '#pass-recovery-form'
        });
    }

    // Tour Search Validation
    if ($("#tour-search-form")) {
        $("#tour-search-form").on("submit", function (e) {
            if (!$("#date-start").val() || !$("#date-end").val() || !$("#destination").val()) {
                e.preventDefault();
            }
        });
    }

    // Tours Modal
    if ($(".tours-modal-wrapper")) {
        $("#tours-modal-trigger").click(function (e) {
            e.preventDefault();
            $("#tours-modal").addClass("animated fadeInLeft").modal('show');

            // Tour Modal Content Isotope
            var toursGrid = $('.tours-grid').isotope({
                itemSelector: '.grid-item',
                layoutMode: 'fitRows',
                percentPosition: true,
                fitRows: {
                    gutter: '.gutter-sizer'
                }
            });

            // Tour Modal Content Isotope Filtering
            $('.filter-button-group').on('click', 'button', function () {
                var filter = $(this).data("filter");
                toursGrid.isotope({ filter: filter });
                $('.filter-button-group').find(".active").removeClass("active");
                $(this).addClass("active");
            });

            $("#tours-modal").animateCss("fadeInLeft", function () {
                $("#tours-modal").removeClass("animated fadeInLeft");
            });
        });

        // Tours Modal Close Button
        $("#tours-close").click(function () {
            $("#tours-modal").addClass("animated fadeOutLeft").animateCss("fadeOutLeft", function () {
                $("#tours-modal").modal('hide');
            });
        });

        // Tours Modal Hide
        $("#tours-modal").on("hide.bs.modal", function (e) {
            if (!$("#tours-modal").hasClass("animated")) {
                e.preventDefault();
                $("#tours-modal").addClass("animated fadeOutLeft");
                setTimeout("$('#tours-modal').modal('hide')", 550);
            }
        });

        // Tours Modal Hidden
        $("#tours-modal").on("hidden.bs.modal", function () {
            $("#tours-modal").removeClass("animated fadeOutLeft");
        });

        // GetTourInfo Ajax Function
        function GetTourInfo(TourId) {
            var url = "/home/gettourinfo/" + TourId;
            tvm = $("#tour-view-modal");
            var preloader = $("#preloader");
            preloader.fadeIn();

            $.ajax({
                url: url,
                type: "get",
                dataType: "json",
                success: function (data) {
                    tvm.find("form#submit-feedback").show();
                    // Tour Title
                    tvm.find("h1.section-title").text(data.TourTitle);

                    // TourId
                    tvm.find("input[name='TourId']").val(data.Id);

                    // Slider
                    tvm.find("#tour-view-car").empty();
                    for (var a = 0; a < data.TourImagesUrl.length; a++) {
                        var carItem = '<div><img src = "' + data.TourImagesUrl[a] + '"><div class="overlay"></div></div>';
                        $("#tour-view-car").append(carItem);
                    }
                    TourViewSlider();

                    // Guide Photo & Fullname
                    tvm.find(".main-info img").attr("src", data.Guide.ProfileImage);
                    tvm.find(".main-info img").attr("data-guide-id", data.Guide.Id);
                    tvm.find(".main-info h2.guide-profile").text(data.Guide.Fullname).attr("data-guide-id", data.Guide.Id);

                    // Guide Rating
                    tvm.find(".main-info .guide-rating").barrating({
                        theme: "css-stars",
                        readonly: true,
                        showSelectedRating: true
                    });
                    if (data.Guide.Rating <= 1) {
                        tvm.find(".main-info .guide-rating").barrating('set', 1);
                    } else {
                        tvm.find(".main-info .guide-rating").barrating('set', data.Guide.Rating);
                    }

                    // Guide Phone & Email
                    tvm.find(".main-info p.guide-phone").empty().append('<i class="fal fa-mobile-android"></i>+' + data.Guide.Phone);
                    tvm.find(".main-info p.guide-email").empty().append('<i class="fal fa-at"></i>' + data.Guide.Email);

                    // Guide Private Message Modal
                    $("form#pm-to-guide input#guide-fullname").val(data.Guide.Fullname);
                    $("form#pm-to-guide input[name='GuideId']").val(data.Guide.Id);

                    // Tour Title & Description
                    tvm.find("h3.tour-title").text(data.TourTitle);
                    tvm.find("p.tour-desc").text(data.Desc);

                    // Tour Categories
                    tvm.find("p.tour-type").text(data.Categories.split(',').join(" - "));

                    // Tour Duration
                    var duration = [data.Duration, data.DurationType];
                    tvm.find("p.tour-duration").empty().append('<i class="fal fa-clock"></i>' + duration.join(" "));

                    // Tour Price
                    var price = [data.Price, data.Currency];
                    tvm.find("p.tour-price").empty().append('<i class="fal fa-money-bill"></i>' + price.join(" "));

                    // Tour Transport
                    if (data.Vehicle != null) {
                        tvm.find("p.tour-transport").empty().append('<i class="fal fa-bus"></i>' + data.Vehicle);
                    } else {
                        tvm.find("p.tour-transport").empty().append('<i class="fal fa-bus"></i>Non Specified');
                    }

                    // Tour Accomodation
                    var acc = data.Accomodation;
                    var accLvl = data.AccomodationLvl;
                    tvm.find("p.tour-accomodation").empty();
                    if (acc === "Hotel") {
                        tvm.find("p.tour-accomodation").append('<i class="fal fa-home"></i>' + accLvl + " " + acc);
                    } else if (acc === null) {
                        tvm.find("p.tour-accomodation").append('<i class="fal fa-home"></i>Non Specified');
                    } else {
                        tvm.find("p.tour-accomodation").append('<i class="fal fa-home"></i>' + acc);
                    }

                    // Feedbacks
                    tvm.find("div.feedbacks-list-wrapper").empty();
                    if (data.FeedbacksList.length < 5) {
                        $("button#load-more-feedbacks").hide();
                    }
                    for (var d = 0; d < data.FeedbacksList.length; d++) {
                        var feedback = `<!-- Feedback Media Start -->
                                                <div class="media comment">
                                                    <img class="align-self-center mr-3 mb-3" src="`+ data.FeedbacksList[d].UserProfileImage + `" data-guide-id="` + data.FeedbacksList[d].UserId + `">
                                                    <div class="media-body">
                                                        <h5 class="feedback-author text-capitalize d-inline-block">`+ data.FeedbacksList[d].UserFullname + `</h5>
                                                        <select class="feedback-rated" data-feedback-id="`+ data.FeedbacksList[d].Id + `">
                                                            <option value="1">1</option>
                                                            <option value="2">2</option>
                                                            <option value="3">3</option>
                                                            <option value="4">4</option>
                                                            <option value="5">5</option>
                                                        </select>
                                                        <p class="feedback-text text-justify">`+ data.FeedbacksList[d].Text + `</p>
                                                        <span class="feedback-date float-right mt-1">`+ data.FeedbacksList[d].Date + `</span>
                                                    </div>
                                                </div>
                                                <!-- Feedback Media End -->`;
                        tvm.find("div.feedbacks-list-wrapper").append(feedback);
                        $(".feedback-rated").barrating({
                            theme: "css-stars",
                            readonly: true
                        });
                        tvm.find("select.feedback-rated[data-feedback-id=" + data.FeedbacksList[d].Id + "]").barrating('set', data.FeedbacksList[d].Rating);
                    }

                    // Hide Feedback Form if Tours User and Logged User is the Same
                    if (data.Guide.Id == tvm.find("input[name='UserId']").val()) {
                        tvm.find("form#submit-feedback").hide();
                    }

                    preloader.fadeOut();
                }
            });
        }

        // Tour Details in Tours Modal
        $("#tours-modal button.tour-details").on("click", function () {
            GetTourInfo($(this).data("tour-id"));
            $("#tours-modal").addClass("animated fadeOutLeft parent");
            setTimeout("$('#tours-modal').modal('hide')", 550);
            $("#profile-view-modal").removeClass("parent");
            $("#tour-view-modal").addClass("animated fadeInRight").modal('show');
            $("#tour-view-modal").animateCss("fadeInRight", function () {
                $("#tour-view-modal").removeClass("animated fadeInRight");
            });
        });

        // Tour View Modal Load More Feedbacks
        $("button#load-more-feedbacks").on("click", function () {
            var that = $(this);
            url = "/home/loadmorefeedbacks/";
            data = {};

            data["FeedbacksCount"] = $(".feedbacks-list-wrapper .media.comment").length;
            data["FeedbackId"] = $(".feedbacks-list-wrapper .media.comment").last().find(".feedback-rated").data("feedback-id");

            $.ajax({
                url: url,
                method: "get",
                data: data,
                dataType: "json",
                success: function (data) {
                    console.log(data);
                    if (data.length != 0) {
                        for (var i = 0; i < data.length; i++) {
                            var feedback = `<!-- Feedback Media Start -->
                                                <div class="media comment animated fadeIn">
                                                    <img class="align-self-center mr-3 mb-3" src="`+ data[i].UserProfileImage + `">
                                                    <div class="media-body">
                                                        <h5 class="feedback-author text-capitalize d-inline-block">`+ data[i].UserFullname + `</h5>
                                                        <select class="feedback-rated" data-feedback-id="`+ data[i].Id + `">
                                                            <option value="1">1</option>
                                                            <option value="2">2</option>
                                                            <option value="3">3</option>
                                                            <option value="4">4</option>
                                                            <option value="5">5</option>
                                                        </select>
                                                        <p class="feedback-text text-justify">`+ data[i].Text + `</p>
                                                        <span class="feedback-date float-right mt-1">`+ data[i].Date + `</span>
                                                    </div>
                                                </div>
                                                <!-- Feedback Media End -->`;
                            $("#tour-view-modal div.feedbacks-list-wrapper").append(feedback);
                            $(".feedback-rated").barrating({
                                theme: "css-stars",
                                readonly: true
                            });
                            $("#tour-view-modal select.feedback-rated[data-feedback-id=" + data[i].Id + "]").barrating('set', data[i].Rating);
                        }
                    }

                    if (data.length < 5) {
                        that.hide();
                    }
                }
            });
        });

        // Tour View Modal Feedback Post
        $("form#submit-feedback").on("submit", function () {
            var that = $(this);
            url = that.attr("action");
            method = that.attr("method");
            request = {};

            request["TourId"] = that.find("input[name='TourId']").val();
            request["UserId"] = that.find("input[name='UserId']").val();
            request["Text"] = that.find("textarea").val();
            that.find("textarea").val("");
            that.find("textarea").removeClass("error");
            request["Rating"] = that.find(".br-current-rating").text();
            that.find("#tour-rating").barrating('set', 1);

            $.ajax({
                url: url,
                method: method,
                data: request,
                dataType: "json",
                success: function (data) {
                    var feedback = `<!-- Feedback Media Start -->
                                                <div class="media comment animated fadeIn">
                                                    <img class="align-self-center mr-3 mb-3" src="`+ data.UserProfileImage + `">
                                                    <div class="media-body">
                                                        <h5 class="feedback-author text-capitalize d-inline-block">`+ data.UserFullname + `</h5>
                                                        <select class="feedback-rated" data-feedback-id="`+ data.Id + `">
                                                            <option value="1">1</option>
                                                            <option value="2">2</option>
                                                            <option value="3">3</option>
                                                            <option value="4">4</option>
                                                            <option value="5">5</option>
                                                        </select>
                                                        <p class="feedback-text text-justify">`+ data.Text + `</p>
                                                        <span class="feedback-date float-right mt-1">`+ data.Date + `</span>
                                                    </div>
                                                </div>
                                                <!-- Feedback Media End -->`;
                    $("#tour-view-modal div.feedbacks-list-wrapper").prepend(feedback);
                    $(".feedback-rated").barrating({
                        theme: "css-stars",
                        readonly: true
                    });
                    $("#tour-view-modal select.feedback-rated[data-feedback-id=" + data.Id + "]").barrating('set', data.Rating);
                }
            });
            return false;
        });

        // Tour View Modal Send Private Message
        $("form#pm-to-guide").on("submit", function () {
            var that = $(this);
            url = that.attr("action");
            method = that.attr("method");
            request = {};

            that.find("[name]").each(function (index, value) {
                var that = $(this);
                name = that.attr("name")
                value = that.val();

                request[name] = value;
            });
            request["SenderId"] = $("button#pm-guide").data("user-id");
            console.log(request);

            $.ajax({
                url: url,
                method: method,
                data: request,
                dataType: "json",
                success: function (data) {
                    if (data == true) {
                        swal({
                            type: 'success',
                            title: 'Success',
                            text: 'Your message has been sent!'
                        });
                        $("#pm-form-modal").modal("hide");
                        that.find("[name='subject']").text("");
                        that.find("[name='msg']").text("");
                    } else {
                        swal({
                            type: 'error',
                            title: 'Oops...',
                            text: 'Something went wrong! Please, try later!'
                        });
                    }
                }
            });
            return false;
        });

        // Tour Details in Most Popular
        $(".goto-tour-details").on("click", function () {
            GetTourInfo($(this).data("tour-id"));
            $("#tour-view-modal").addClass("animated fadeInRight").modal('show');
            $("#tour-view-modal").animateCss("fadeInRight", function () {
                $("#tour-view-modal").removeClass("animated fadeInRight");
            });
        });

        // Tour View Modal Hide
        $("#tour-view-modal").on("hide.bs.modal", function (e) {
            if (!$("#tour-view-modal").hasClass("animated")) {
                e.preventDefault();
                $("#tour-view-modal").addClass("animated fadeOutRight");
                setTimeout("$('#tour-view-modal').modal('hide')", 550);
            }
            $("button#load-more-feedbacks").show();
        });

        // Tour View Modal Hidden
        $("#tour-view-modal").on("hidden.bs.modal", function () {
            $("#tour-view-modal").removeClass("animated fadeOutRight");
            $("#tour-view-car").owlCarousel('destroy');
            $(".parent").removeClass("parent");
        });

        // Tour View Back
        $("#tour-view-back").on("click", function () {
            $("#tour-view-modal").modal('hide');
            if ($("#tours-modal").hasClass("parent")) {
                $("#tours-modal").addClass("animated fadeInLeft").modal('show');
                $("#tours-modal").animateCss("fadeInLeft", function () {
                    $("#tours-modal").removeClass("parent");
                });
            } else if ($("#profile-view-modal").hasClass("parent")) {
                $("#profile-view-modal").addClass("animated fadeInUp").modal('show');
                $("#profile-view-modal").animateCss("fadeInUp", function () {
                    $("#profile-view-modal").removeClass("parent");
                });
            }
        });

        // Tour View Slider
        function TourViewSlider() {
            var tvc = $("#tour-view-car");
            tvc.owlCarousel({
                responsive: {
                    0: {
                        items: 1,
                        nav: false
                    },
                    992: {
                        items: 5,
                        nav: true
                    }
                },
                loop: true,
                center: true,
                nav: true,
                dots: false,
                autoplay: true,
                autoplayTimeout: 10000,
                autoplayHoverPause: true
            });
        }

        // Tour View Calendar
        var clndr = $(".tour-available-dates");
        clndr.jalendar({
            type: "range",
            color: "#f15a23",
            todayColor: "#63cac5",
            dayColor: "white",
            selectingAfterToday: true,
            dateType: "yyyy-mm-dd",
            done: btnShow
        });

        // Send Request Button Tooltip
        $('#send-request-button-wrapper').tooltip({
            trigger: 'hover',
            title: "Please, choose dates first!",
            placement: 'bottom'
        });
        function btnShow() {
            $('#send-request-button-wrapper').tooltip("disable");
            $("#send-tour-request").removeAttr("disabled style");
        }

        // Tour View Feedback Rating
        $("#tour-rating").barrating({
            theme: "css-stars"
        });

        // Tour View Feedback, Tours Modal Search Form, Private Message to Guide Form, Tour Request Form Submit Validation
        $.validate({
            form: '#submit-feedback, #tours-modal-search-form, #pm-to-guide, #tour-request-guide'
        });

        // Tour View Feedback Comment Rated
        $(".feedback-rated").barrating({
            theme: "css-stars",
            readonly: true
        });

        // Tours Modal Search Values
        $("#tours-modal-search-form .form-group input").on("input", function () {
            $(this).prev().children("span").text($(this).val());
        });

        $("#tours-modal-search-form .form-group input#min-price").on("input", function () {
            $("#tours-modal-search-form .form-group input#max-price").attr("min", $(this).val());
        });

        // Tour View Guide Private Message Modal
        $("#pm-guide").on("click", function () {
            $("#pm-form-modal").addClass("animated fadeIn").modal('show');
            $("#pm-form-modal").animateCss("fadeIn", function () {
                $("#pm-form-modal").removeClass("animated fadeIn");
            });
        });

        // Tour View Guide Private Message Modal Hide
        $("#pm-form-modal").on("hide.bs.modal", function (e) {
            if (!$("#pm-form-modal").hasClass("animated")) {
                e.preventDefault();
                $("#pm-form-modal").addClass("animated fadeOut");
                setTimeout("$('#pm-form-modal').modal('hide')", 550);
            }
        });

        // Tour View Guide Private Message Modal Hidden
        $("#pm-form-modal").on("hidden.bs.modal", function () {
            $("#pm-form-modal").removeClass("animated fadeOut");
            $("#pm-to-guide")[0].reset();
        });

        // Tour Request Modal
        $("#send-tour-request").on("click", function () {
            $("#tour-request-modal input#start-date").val($(".tour-available-dates input.first-range-data").val());
            $("#tour-request-modal input#end-date").val($(".tour-available-dates input.last-range-data").val());
            $("#tour-request-modal").addClass("animated fadeIn").modal('show');
            $("#tour-request-modal").animateCss("fadeIn", function () {
                $("#tour-request-modal").removeClass("animated fadeIn");
            });
        });

        // Tour Request Form Submit
        $("form#tour-request-guide").on("submit", function () {
            var that = $(this);
            url = that.attr("action");
            method = that.attr("method");
            request = {};

            that.find("[name]").each(function (index, value) {
                var that = $(this);
                name = that.attr("name");
                value = that.val();

                request[name] = value;
            });

            request["TourId"] = $("input[name='TourId']").val();

            $.ajax({
                url: url,
                method: method,
                data: request,
                dataType: "json",
                success: function (data) {
                    if (data === 0) {
                        swal({
                            type: 'error',
                            title: 'Oops...',
                            text: 'Something went wrong! Please, try later!'
                        });
                    } else {
                        swal({
                            type: 'success',
                            title: 'Success',
                            text: 'Thank you for request! Guide will get back to you soon.'
                        });
                        that.find("input[type='date'], textarea").val("").text("");
                    }
                }
            });

            return false;
        });

        // Tour Request Modal Hide
        $("#tour-request-modal").on("hide.bs.modal", function (e) {
            if (!$("#tour-request-modal").hasClass("animated")) {
                e.preventDefault();
                $("#tour-request-modal").addClass("animated fadeOut");
                setTimeout("$('#tour-request-modal').modal('hide')", 550);
            }
        });

        // Tour Request Modal Hidden
        $("#tour-request-modal").on("hidden.bs.modal", function () {
            $("#tour-request-modal").removeClass("animated fadeOut");
            $("#tour-request-guide")[0].reset();
        });
    }

    // Profile View Modal
    if ($(".best-guide-profile")) {
        $(".best-guide-profile").on("click", function (e) {
            e.preventDefault();
            GetUserInfo($(this).data("guide-id"));
            $("#profile-view-modal").addClass("animated fadeInUp").modal('show');
            $("#profile-view-modal").animateCss("fadeInUp", function () {
                $("#profile-view-modal").removeClass("animated fadeInUp");
            });
        });

        // Profile View Modal Hide
        $("#profile-view-modal").on("hide.bs.modal", function (e) {
            if (!$("#profile-view-modal").hasClass("animated")) {
                e.preventDefault();
                $("#profile-view-modal").addClass("animated fadeOutDown");
                setTimeout("$('#profile-view-modal').modal('hide')", 550);
            }
        });

        // Profile View Modal Hidden
        $("#profile-view-modal").on("hidden.bs.modal", function () {
            $("#profile-view-modal").removeClass("animated fadeOutDown");
        });

        function GetUserInfo(UserId) {
            url = "/home/getuserinfo/" + UserId;
            var preloader = $("#preloader");
            preloader.fadeIn();

            $.ajax({
                url: url,
                method: "get",
                dataType: "json",
                success: function (data) {
                    var pvm = $("#profile-view-modal");
                    pvm.find("guide-offered-tours-wrapper row").empty();
                    pvm.find("h1.section-title").text(data.Fullname);
                    pvm.find("img.profile-photo").attr("src", data.ProfileImage);
                    pvm.find("h3.profile-country").empty().append('<span class="flag-icon flag-icon-' + data.CountryCode + '"></span> ' + data.Country + '');
                    pvm.find("h5.profile-total-tours span").text(data.ToursList.length);
                    pvm.find("h5.profile-completed-tours span").text(data.CompletedTours);
                    pvm.find("h5.profile-tour-feedbacks span").text(data.FeedbacksCount);
                    pvm.find("h5.profile-number").empty().append('<i class="fal fa-mobile-android"></i> +' + data.Phone + '');
                    pvm.find("h5.profile-email").empty().append('<i class="fal fa-at"></i> ' + data.Email + '');

                    if (data.Facebook != null) {
                        pvm.find("i.fa-facebook-f").parent().attr("href", data.Facebook).removeClass("text-muted");
                    }

                    if (data.Instagram != null) {
                        pvm.find("i.fa-instagram").parent().attr("href", data.Instagram).removeClass("text-muted");
                    }

                    if (data.Twitter != null) {
                        pvm.find("i.fa-twitter").parent().attr("href", data.Twitter).removeClass("text-muted");
                    }

                    if (data.GooglePlus != null) {
                        pvm.find("i.fa-google-plus-g").parent().attr("href", data.GooglePlus).removeClass("text-muted");
                    }

                    for (var i = 0; i < data.ToursList.length; i++) {
                        var tourItem = `<!-- Tour Item Start -->
                                            <div class="col-md-4">
                                                <div class="tour-item" style="background-image: url('`+ data.ToursList[i].TourImage + `')">
                                                    <div class="info-overlay">
                                                        <div class="tour-heading">
                                                            <h3 class="tour-title">`+ data.ToursList[i].TourTitle + ` Tour</h3>
                                                            <p class="duration" title="Duration">
                                                                <i class="fal fa-clock text-lowercase"></i>`+ data.ToursList[i].Duration + ` ` + data.ToursList[i].DurationType + `
                                                            </p>
                                                            <p class="price" title="Price">
                                                                <i class="fal fa-money-bill"></i>`+ data.ToursList[i].Price + ` ` + data.ToursList[i].Currency + `
                                                            </p>
                                                            <button class="btn btn-primary text-uppercase my-3 tour-details" data-tour-id="`+ data.ToursList[i].Id + `">Details</button>
                                                        </div>
                                                        <div class="tour-guide">
                                                            <img src="`+ data.ProfileImage + `">
                                                            <ul class="list-unstyled">
                                                                <li>`+ data.Fullname + `</li>
                                                                <li>
                                                                    <select class="guide-rating">
                                                                        <option value="1">1</option>
                                                                        <option value="2">2</option>
                                                                        <option value="3">3</option>
                                                                        <option value="4">4</option>
                                                                        <option value="5">5</option>
                                                                    </select>
                                                                </li>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                    <div class="info-overlay-bottom">
                                                        <p>
                                                            `+ data.ToursList[i].TourTitle + ` Tour
                                                            <span>`+ data.ToursList[i].Price + ` ` + data.ToursList[i].Currency + `</span>
                                                        </p>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- Tour Item End -->`;
                        pvm.find(".guide-offered-tours-wrapper .row").append(tourItem);
                    }

                    pvm.find("select.guide-rating").barrating({
                        theme: "css-stars",
                        readonly: true
                    }).barrating('set', data.Rating);

                    pvm.find("button.tour-details").on("click", function () {
                        GetTourInfo($(this).data("tour-id"));
                        $("#profile-view-modal").addClass("parent").modal('hide');
                        $("#tours-modal").removeClass("parent");
                        $("#tour-view-modal").addClass("animated fadeInRight").modal('show');
                        $("#tour-view-modal").animateCss("fadeInRight", function () {
                            $("#tour-view-modal").removeClass("animated fadeInRight");
                        });
                    });

                    preloader.fadeOut();
                }
            });
        }

        // Profile View Modal in Tour View Modal
        $("#tour-view-modal .guide-profile, .media img").on("click", function () {
            GetUserInfo($(this).data("guide-id"));
            $("#profile-view-modal").addClass("animated fadeInUp").modal('show');
            $("#profile-view-modal").animateCss("fadeInUp", function () {
                $("#profile-view-modal").removeClass("animated fadeInUp");
            });
        });
    }

    // Best Guide Star Rating
    if ($(".best-guide-rating, .tour-guide-rating")) {
        $(".best-guide-rating, .tour-guide-rating").barrating({
            theme: "css-stars",
            readonly: true,
            showSelectedRating: true
        });
    }

    // About Us - Guides Slider
    if ($(".about-guides-slider")) {
        $(".about-guides-slider").owlCarousel({
            responsive: {
                0: {
                    items: 1
                },
                720: {
                    items: 3
                }
            },
            loop: true,
            nav: false,
            dots: false,
            autoplay: true,
            autoplayTimeout: 10000
        });
    }

    // Contact Form Validation
    if ($("#contact-form")) {
        $.validate({
            form: "#contact-form"
        });
    }

    // Contact Form Submit
    $("form#contact-form").on("submit", function () {
        var that = $(this);
        url = that.attr("action");
        method = that.attr("method");
        request = {};

        that.find("[name]").each(function (index, value) {
            var that = $(this);
            name = that.attr("name");
            value = that.val();

            request[name] = value;
        });

        console.log(request);

        $.ajax({
            url: url,
            method: method,
            data: request,
            dataType: "json",
            success: function (data) {
                if (data === "error") {
                    swal({
                        type: 'error',
                        title: 'Oops...',
                        text: 'Something went wrong! Please, try later!'
                    });
                } else {
                    swal({
                        type: 'success',
                        title: 'Success',
                        text: 'Thank you for request! We will get back to you soon.'
                    });
                }
            }
        });
        that[0].reset();

        return false;
    });

    // Instagram Feed Slider
    if ($("#insta-feed-slider")) {
        $("#insta-feed-slider").owlCarousel({
            responsive: {
                0: {
                    items: 1
                },
                576: {
                    items: 3
                },
                992: {
                    items: 7
                }
            },
            loop: true,
            nav: false,
            dots: false,
            autoplay: true,
            margin: 10,
            autoplayTimeout: 10000,
            autoplayHoverPause: true,
            smartSpeed: 2500
        });
    }
});