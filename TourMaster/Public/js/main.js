$(document).ready(function () {
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
                type: "get",
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
    if (".pass-recovery-form-modal-wrapper") {
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

        // Tour Details in Tours Modal
        $("#tours-modal .tour-details").on("click", function () {
            var GetTourInfoUrl = "/Home/GetTourInfo/" + $(this).data("tour-id");
            var tvm = "#tour-view-modal";
            $.ajax({
                url: GetTourInfoUrl,
                type: "get",
                dataType: "json",
                success: function (data) {
                    $(tvm + " h1.section-title").text(data[0].TourTitle);
                    $(tvm + " #tour-view-car").empty();
                    for (var a = 0; a < data[0].TourImages.length; a++) {
                        var carItem = '<div><img src = "' + data[0].TourImages[a] + '"><div class="overlay"></div></div>';
                        $("#tour-view-car").append(carItem);
                    }
                    TourViewSlider();
                    $(tvm + " .main-info img").attr("src", data[0].Guide.ProfileImage);
                    $(tvm + " .main-info h2.guide-profile").text(data[0].Guide.FullName).data("guide-id", data[0].Guide.Id);
                    var totalRating = 0;
                    for (var b = 0; b < data[0].FeedbacksList.length; b++) {
                        totalRating += parseInt(data[0].FeedbacksList[b].split(", ")[2].split("=")[1]);
                    }
                    var overallRating = Math.ceil(totalRating / data[0].FeedbacksList.length);
                    $(tvm + " .main-info select.guide-rating").empty();
                    for (var c = 1; c < 6; c++) {
                        var ratingOpt = '<option value="' + c + '" ' + (overallRating === c ? "selected" : "") + '>' + c + '</option>';
                        $(tvm + " .main-info select.guide-rating").append(ratingOpt);
                    }
                    $(tvm + " .main-info .guide-rating").barrating({
                        theme: "css-stars",
                        readonly: true,
                        showSelectedRating: true
                    });
                }
            });
            $("#tours-modal").addClass("animated fadeOutLeft parent");
            setTimeout("$('#tours-modal').modal('hide')", 550);
            $("#tour-view-modal").addClass("animated fadeInRight").modal('show');
            $("#tour-view-modal").animateCss("fadeInRight", function () {
                $("#tour-view-modal").removeClass("animated fadeInRight");
            });
        });

        // Tour Details in Most Popular
        $(".goto-tour-details").on("click", function () {
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
        });

        // Tour View Modal Hidden
        $("#tour-view-modal").on("hidden.bs.modal", function () {
            $("#tour-view-modal").removeClass("animated fadeOutRight");
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

        $('#send-request-button-wrapper').tooltip({
            trigger: 'hover',
            title: "Please, choose dates first!",
            placement: 'bottom'
        });

        function btnShow() {
            $('#send-request-button-wrapper').tooltip("disable");
            $("#send-tour-request").removeAttr("disabled style");
            console.log($(".tour-available-dates input.first-range-data").val(), $(".tour-available-dates input.last-range-data").val());
        }

        // Tour View Feedback Rating
        $("#tour-rating").barrating({
            theme: "css-stars"
        });

        // Tour View Feedback, Tours Modal Search Form, Private Message to Guide Form Submit Validation
        $.validate({
            form: '#submit-feedback, #tours-modal-search-form, #pm-to-guide'
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

        // Tour Details in Profile View Modal
        $("#profile-view-modal .tour-details").on("click", function () {
            $("#profile-view-modal").addClass("parent").modal('hide');
            $("#tour-view-modal").addClass("animated fadeInRight").modal('show');
            $("#tour-view-modal").animateCss("fadeInRight", function () {
                $("#tour-view-modal").removeClass("animated fadeInRight");
            });
        });

        // Profile View Modal in Tour View Modal
        $("#tour-view-modal .guide-profile, .media img").on("click", function () {
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