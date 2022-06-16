/*--- Use only for Custom Javascript  ---*/
"use strict";

// Window Load
$(window).on('load', function () {
    bannerHeight();  //Banner Height
    headerHeight(); //Sticky Header
});

// Window Ready
$(function ($) {
    /*======== Banner Slider ========*/
    if ($("#banner").length) {
        $('.banner-slider').slick({
            dots: false,
            arrows: true,
        });
    }
    /*======== Offers Slider ========*/
    if ($(".offers").length) {
        $('.offers-slider').slick({
            dots: false,
            arrows: true,
            slidesToShow: 3,
            slidesToScroll: 1,
            responsive: [
                {
                    breakpoint: 768,
                    settings: {
                        slidesToShow: 2,
                    }
                },
                {
                    breakpoint: 480,
                    settings: {
                        slidesToShow: 1,
                    }
                }
            ]
        });
    }
    /*======== Brands Slider ========*/
    if ($(".brands-slider").length) {
        $('.brands-slider').slick({
            dots: false,
            arrows: true,
            slidesToShow: 5,
            slidesToScroll: 1,
            responsive: [
                {
                    breakpoint: 1200,
                    settings: {
                        slidesToShow: 4,
                        infinite: true,
                        dots: true
                    }
                },
                {
                    breakpoint: 992,
                    settings: {
                        slidesToShow: 3,
                        infinite: true,
                        dots: true
                    }
                },
                {
                    breakpoint: 600,
                    settings: {
                        slidesToShow: 2,
                    }
                },
                {
                    breakpoint: 480,
                    settings: {
                        slidesToShow: 1,
                    }
                }
            ]
        });
    }
    /*======== Client Slider ========*/
    if ($(".our-customers").length) {
        $('.client-slider').slick({
            dots: true,
            arrows: false,
        });
    }
    /*======== Similar Product Slider ========*/
    if ($(".similar-product-slider").length) {
        $('.similar-product-slider').slick({
            dots: false,
            arrows: true,
            slidesToShow: 3,
            slidesToScroll: 1,
            responsive: [
                {
                    breakpoint: 1199,
                    settings: {
                        slidesToShow: 3,
                        infinite: true,
                        dots: true
                    }
                },
                {
                    breakpoint: 991,
                    settings: {
                        slidesToShow: 2,
                    }
                },
                {
                    breakpoint: 480,
                    settings: {
                        slidesToShow: 1,
                    }
                }
            ]
        });
    }

    /*======== Gift Slider ========*/
    if ($(".sidebar .gift-box").length) {
        $('.gift-slider').slick({
            dots: true,
            arrows: false,
        });
    }

    bannerHeight();  //Banner Height
    subMenuMobile(); //Mobile Menu
    headerHeight(); //Sticky Header
    rangeSlider(); // Price Range Selector
    chocoCategories(); // Show Chocolates sub Categories
    toolTip(); // Show Tooltip

    // CheckBox/Radio Js
    $('.label_check, .label_radio').on("click", function () {
        setupLabel();
    });
    setupLabel();

    // Counter JS
    if ($(".counter").length) {
        $('.counter').counterUp({
            delay: 10,
            time: 1000
        });
    }
    // Search-Box
    $(".searchBox .search-boxSmall").on("click", function () {
        $(".searchBox .search-box").slideToggle();
    });
    // View Cart
    $(".cart-items .cart-icon").on("click", function () {
        $(".cart-items .cart-table").slideToggle();
    });

    //Copyright Year
    var currentYear = (new Date).getFullYear();
    $("#footer .copyright .year").text(currentYear);

});

// Window Resize
$(window).on('resize', function () {
    bannerHeight(); //Banner Height
    headerHeight(); //Sticky Header
});

//Fixed nav bar on top
$(window).on('scroll', function () {
    var header = $("#header");
    var HeadTopHeight = $(".top-bar").outerHeight();
    if ($(window).width() > 767) {
        if ($(".navbar").offset().top > 40) {
            header.addClass("sticky-header").css('top', '-' + HeadTopHeight + 'px');
        } else {
            header.removeClass("sticky-header").css('top', 0);;
        }
    }
});

function toolTip() {
    $('[data-toggle="tooltip"]').tooltip();
}

// Show Chocolates sub Categories
function chocoCategories() {
    if ($(".choclate-categories").length) {
        $(".choclate-categories li.has-child > a").before('<span class="expand">+</span>');
        $(".choclate-categories li a").on("click", function () {
            if ($(this).next("ul.cat-list").is(":visible")) {
                $(this).prev(".expand").text("+");
                $(this).next("ul.cat-list").slideUp();
            }
            else {
                $(".choclate-categories li .expand").text("+");
                $("ul.cat-list").slideUp();
                $(this).prev(".expand").text("-");
                $(this).next("ul.cat-list").slideDown();
            }
        });
    }
}
// Price Range Selector
function rangeSlider() {
    if ($("#keypress").length) {
        var keypressSlider = document.getElementById('keypress');
        var input0 = document.getElementById('input-with-keypress-0');
        var input1 = document.getElementById('input-with-keypress-1');
        var inputs = [input0, input1];
        noUiSlider.create(keypressSlider, {
            start: [20, 80],
            connect: true,
            tooltips: [
                wNumb({
                    decimals: 1,
                    postfix: ' ($)'
                }),
                wNumb({
                    decimals: 1,
                    postfix: ' ($)'
                })
            ],
            range: {
                'min': 0,
                'max': 100
            },
            format: wNumb({
                decimals: 1,

            }),

        });

        keypressSlider.noUiSlider.on('update', function (values, handle) {
            inputs[handle].value = values[handle];
        });

        function setSliderHandle(i, value) {
            var r = [null, null];
            r[i] = value;
            keypressSlider.noUiSlider.set(r);
        }

        // Listen to keydown events on the input field.
        inputs.forEach(function (input, handle) {

            input.addEventListener('change', function () {
                setSliderHandle(handle, this.value);
            });

            input.addEventListener('keydown', function (e) {

                var values = keypressSlider.noUiSlider.get();
                var value = Number(values[handle]);

                // [[handle0_down, handle0_up], [handle1_down, handle1_up]]
                var steps = keypressSlider.noUiSlider.steps();

                // [down, up]
                var step = steps[handle];

                var position;

                // 13 is enter,
                // 38 is key up,
                // 40 is key down.
                switch (e.which) {

                    case 13:
                        setSliderHandle(handle, this.value);
                        break;

                    case 38:

                        // Get step to go increase slider value (up)
                        position = step[1];

                        // false = no step is set
                        if (position === false) {
                            position = 1;
                        }

                        // null = edge of slider
                        if (position !== null) {
                            setSliderHandle(handle, value + position);
                        }

                        break;

                    case 40:

                        position = step[0];

                        if (position === false) {
                            position = 1;
                        }

                        if (position !== null) {
                            setSliderHandle(handle, value - position);
                        }

                        break;
                }
            });
        });

    }
}
// Set Banner Height
function bannerHeight() {
    var winHeight = $(window).height();
    $(".banner-slider").css('height', winHeight + 'px');
}

// For Mobile Menu
function subMenuMobile() {
    $(".navbar .nav li ul.dropdown-menu").before('<span class="mobile-arrow">+</span>');
    $(".navbar .nav li .mobile-arrow").on("click", function () {
        if ($(this).next("ul.dropdown-menu").is(":visible")) {
            $(this).text("+");
            $(this).next("ul.dropdown-menu").slideUp();
        }
        else {
            $(".navbar .nav li .mobile-arrow").text("+");
            $("ul.dropdown-menu").slideUp();
            $(this).text("-");
            $(this).next("ul.dropdown-menu").slideDown();
        }
    });
}

// Set Sticky Header
function headerHeight() {
    var wrapper = $("#wrapper");
    var header_height = $("#header").outerHeight();

    wrapper.css("padding-top", "0px");

    if ($(".inner-page").length) {
        if ($(window).width() > 767) {
            wrapper.css("padding-top", header_height + "px");
        }
        else {
            wrapper.css("padding-top", "0px");
        }
    }
}

// Checkbox/Radio Style
function setupLabel() {
    if ($('.label_check input').length) {
        $('.label_check').each(function () {
            $(this).removeClass('c_on');
        });
        $('.label_check input:checked').each(function () {
            $(this).parent('label').addClass('c_on');
        });
    };
    if ($('.label_radio input').length) {
        $('.label_radio').each(function () {
            $(this).removeClass('r_on');
        });
        $('.label_radio input:checked').each(function () {
            $(this).parent('label').addClass('r_on');
        });
    };
};


var workFilter = $('.creations-item li');
workFilter.on("click", function () {
    workFilter.removeClass('active');
    $(this).addClass('active');
});
if ($(".creations-grid").length == 1) {
    $('.creations-grid').filterizr();
}
if ($(".testimonials-slider").length) {
    $('.testimonials-slider').slick({
        dots: false,
        arrows: true,
        slidesToShow: 3,
        slidesToScroll: 1,
        responsive: [
            {
                breakpoint: 1199,
                settings: {
                    slidesToShow: 3,
                }
            },
            {
                breakpoint: 991,
                settings: {
                    slidesToShow: 2,
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1,
                }
            }
        ]
    });
}
$(".quck-menuIcon").on('click', function () {
    if ($(this).hasClass("open")) {
        $(".quck-menuIcon").removeClass("open");
        $(this).children(".fa").removeClass("fa-angle-right").addClass("fa-bars");
        $("body").animate({ left: "0px" }, 500);
        $("#header").animate({ left: "0px" }, 500);
        $(".quck-linkRight").animate({ right: "-500px" }, 500);
    }
    else {
        $("body").animate({ left: "-500px" }, 500);
        $("#header").animate({ left: "-500px" }, 500);
        $(".quck-linkRight").animate({ right: "0px" }, 500);
        //$(".quck-menuOverlay").fadeIn(500);
        $(".quck-menuIcon").addClass("open");

        $(this).children(".fa").removeClass("fa-bars").addClass("fa-angle-right");
    }
});
$(".search-icon .fa").on("click", function (e) {
    e.stopPropagation();
    if ($(this).hasClass("open")) {
        $(this).removeClass("open");
        $(".search-input").animate({ left: "56px" }, 1000, function () { $(".search-input").hide(); });
    }
    else {
        $(this).addClass("open");
        $(".search-input").show()
        $(".search-input").animate({ left: "-244px" }, 1000);
    }
})

$("body").click(function (e) {
    $(".search-icon .fa").removeClass("open");
    $(".search-input").animate({ left: "56px" }, 1000, function () { $(".search-input").hide(); });
});

$(".search-icon").on("click", function (e) {
    e.stopPropagation();
});
