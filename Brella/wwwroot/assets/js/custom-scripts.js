$(document).ready(function () {
  'use strict';

  //===== Dropdown Anmiation =====// 
  var drop = $('nav > div ul ul li');
  $('nav > div ul ul').each(function () {
    var delay = 50;
    $(this).find(drop).each(function () {
      $(this).css({transitionDelay: delay + 'ms'});
      delay += 50;
    });
  });

  //===== Menu Active =====//
  var pgurl = window.location.href.substr(window.location.href.lastIndexOf("/") + 1);
  $("nav ul li a").each(function () {
    if ($(this).attr("href") == pgurl || $(this).attr("href") == '')
    $(this).parent('li').addClass("active").parent().parent().addClass("active").parent().parent().addClass("active");
  });

  //===== Menu Active =====//
  var pgurl = window.location.href.substr(window.location.href.lastIndexOf("/") + 1);
  $(".menu-wrap ul li a").each(function () {
    if ($(this).attr("href") == pgurl || $(this).attr("href") == '')
    $(this).parent('li').addClass("active").parent().parent().addClass("active-parent").parent().parent().addClass("active-parent");
  });

  //===== Width =====//
  var width = window.innerWidth;

  //===== Wow Animation Setting =====//
  if ($(".wow").length > 0) {
    var wow = new WOW({
      boxClass:     'wow',      // default
      animateClass: 'animated', // default
      offset:       0,          // default
      mobile:       true,       // default
      live:         true        // default
    }); 

    wow.init();
  }

  //===== Side Menu =====//
  $('.rspn-mnu-btn').on('click', function () {
    $('.rsnp-mnu').addClass('slidein');
    return false;
  });
  $('.rspn-mnu-cls').on('click', function () {
    $('.rsnp-mnu').removeClass('slidein');
  });
  $('.rsnp-mnu li.menu-item-has-children > a').on('click', function () {
    $(this).parent().siblings('li').children('ul').slideUp();
    $(this).parent().siblings('li').removeClass('active');
    $(this).parent().children('ul').slideToggle();
    $(this).parent().toggleClass('active');
    return false;
  });

  //===== Select =====//
  if ($('.slc-wrp > select').length > 0) {
    $('.slc-wrp > select').selectpicker();
  }

  //===== Touchspin =====//
  if ($('.qty').length > 0) {
    $('.qty').TouchSpin();
  } 

  //===== Sticky Sidebar =====//
  if(width > 851) {
    if ($('.post-detail-wrap > div.row > div').length > 0) {
      $('.post-detail-wrap > div.row > div').theiaStickySidebar({
        additionalMarginTop: 40,
        additionalMarginBottom: 40
      });
    }
  }

  //===== Accordions =====//
  if ($(".toggle").length > 0) {
    $(function() {
      $('#toggle .toggle-content').hide();
      $('#toggle h4:first').next().slideDown(500).parent().addClass("active");
      $('#toggle h4').on("click",function() {
        if($(this).next().is(':hidden')) {
          $('#toggle h4').next().slideUp(500).parent().removeClass("active");
          $(this).next().slideDown(500).parent().toggleClass("active");
        }
      });
    });
  }

  //===== Counter Up =====//
  if ($.isFunction($.fn.counterUp)) {
    $('.counter').counterUp({
      delay: 10,
      time: 2000
    });
  }

  //===== LightBox =====//
  if ($.isFunction($.fn.fancybox)) {
    $('[data-fancybox],[data-fancybox="gallery"]').fancybox({});
  }

  //===== Contact Form Validation =====//
  if($('#email-form').length){
    $('#submit').on('click',function(){

      var o = new Object();
      var form = '#email-form';

      var fname = $('#email-form .fname').val();
      var email = $('#email-form .email').val();

      if(fname == '' || email == '') {
        $('#email-form .response').html('<div class="failed alert alert-warning">Please fill the required fields.</div>');
        return false;
      }

      $.ajax({
        url:"sendemail.php",
        method:"POST",
        data: $(form).serialize(),
        beforeSend:function(){
          $('#email-form .response').html('<div class="text-info"><img src="assets/images/preloader.gif"> Loading...</div>');
        },
        success:function(data){
          $('form').trigger("reset");
          $('#email-form .response').fadeIn().html(data);
          setTimeout(function(){
            $('#email-form .response').fadeOut("slow");
          }, 5000);
        },
        error:function(){
          $('#email-form .response').fadeIn().html(data);
        }
      });
    });
  }
  
  if(width < 851) {
    //===== Scrollbar =====//
    if ($('.rsnp-mnu').length > 0) {
      var ps = new PerfectScrollbar('.rsnp-mnu');
    }
    
    //===== Responsive Carousel =====//
    if ($.isFunction($.fn.slick)) {
      $('.res-caro').slick({
        slidesToShow: 2,
        slidesToScroll: 1,
        dots: true,
        arrows: false,
        centerPadding: '0',
        focusOnSelect: true,
        responsive: [
        {
          breakpoint: 851,
          settings: {
            slidesToShow: 2,
            slidesToScroll: 1,
            infinite: true,
            arrows: false,
          }
        },
        {
          breakpoint: 771,
          settings: {
            slidesToShow: 2,
            slidesToScroll: 1,
            infinite: true,
            arrows: false,
          }
        },
        {
          breakpoint: 576,
          settings: {
            slidesToShow: 1,
            slidesToScroll: 1,
            infinite: true,
            centerMode: true,
            arrows: false,
            dots: false,
          }
        }
        ]
      });

      $('.res-caro2').slick({
        slidesToShow: 3,
        slidesToScroll: 1,
        dots: true,
        arrows: false,
        centerPadding: '0',
        focusOnSelect: true,
        responsive: [
        {
          breakpoint: 851,
          settings: {
            slidesToShow: 3,
            slidesToScroll: 1,
            infinite: true,
            arrows: false,
          }
        },
        {
          breakpoint: 770,
          settings: {
            slidesToShow: 2,
            slidesToScroll: 1,
            infinite: true,
            arrows: false,
          }
        },
        {
          breakpoint: 576,
          settings: {
            slidesToShow: 1,
            slidesToScroll: 1,
            infinite: true,
            centerMode: true,
            arrows: false,
            dots: false,
          }
        }
        ]
      });
    }
  }

  //===== Slick Carousel =====//
  if ($.isFunction($.fn.slick)) {

    //=== Featured Area Carousel ===//
    $('.feat-caro').slick({
      arrows: true,
      initialSlide: 0,
      infinite: true,
      slidesToShow: 1,
      slidesToScroll: 1,
      fade: true,
      autoplay: false,
      autoplaySpeed: 6000,
      cssEase: 'cubic-bezier(0.7, 0, 0.3, 1)',
      speed: 1500,
      draggable: true,
      dots: false,
      pauseOnDotsHover: true,
      pauseOnFocus: false,
      pauseOnHover: false,
      prevArrow:"<button type='button' class='slick-prev'><i class='flaticon-back'></i></button>",
      nextArrow:"<button type='button' class='slick-next'><i class='flaticon-next'></i></button>",
      responsive: [
      {
        breakpoint: 981,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
          arrows: true,
          dots: false,
        }
      },
      {
        breakpoint: 851,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
          arrows: true,
          dots: false,
        }
      },
      {
        breakpoint: 770,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
          arrows: false,
          dots: true,
        }
      },
      {
        breakpoint: 576,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
          arrows: false,
          dots: true,
        }
      }
      ]
    });

    //=== Testimonials Carousel ===//
    $('.testi-caro').slick({
      arrows: false,
      initialSlide: 0,
      infinite: true,
      slidesToShow: 2,
      slidesToScroll: 1,
      fade: false,
      autoplay: false,
      autoplaySpeed: 5000,
      speed: 1000,
      draggable: true,
      dots: true,
      pauseOnDotsHover: true,
      pauseOnFocus: false,
      pauseOnHover: false,
      responsive: [
      {
        breakpoint: 981,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 1,
        }
      },
      {
        breakpoint: 851,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1
        }
      },
      {
        breakpoint: 576,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1
        }
      }
      ]
    });

    //====== Projects Carousel =====//
    $('.proj-caro').slick({
      slidesToShow: 6,
      slidesToScroll: 1,
      dots: false,
      arrows: true,
      centerPadding: '0',
      focusOnSelect: true,
      vertical: false,
      prevArrow:"<button type='button' class='slick-prev'><i class='flaticon-arrow-pointing-to-left'></i></button>",
      nextArrow:"<button type='button' class='slick-next'><i class='flaticon-arrow-pointing-to-right'></i></button>",
      responsive: [
      {
        breakpoint: 1605,
        settings: {
          slidesToShow: 5,
          slidesToScroll: 1,
          arrows: true,
        }
      },
      {
        breakpoint: 1370,
        settings: {
          slidesToShow: 4,
          slidesToScroll: 1,
          arrows: true,
        }
      },
      {
        breakpoint: 981,
        settings: {
          slidesToShow: 3,
          slidesToScroll: 1,
          arrows: true,
        }
      },
      {
        breakpoint: 851,
        settings: {
          slidesToShow: 3,
          slidesToScroll: 1,
          arrows: true,
          dots: false,
        }
      },
      {
        breakpoint: 770,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 1,
          arrows: true,
          dots: false,
        }
      },
      {
        breakpoint: 500,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
          arrows: false,
          dots: false,
        }
      }
      ]
    });

    //====== Projects Carousel 2 =====//
    $('.proj-caro2').slick({
      slidesToShow: 4,
      slidesToScroll: 1,
      dots: false,
      arrows: true,
      centerPadding: '0',
      focusOnSelect: true,
      vertical: false,
      prevArrow:"<button type='button' class='slick-prev'><i class='flaticon-arrow-pointing-to-left'></i></button>",
      nextArrow:"<button type='button' class='slick-next'><i class='flaticon-arrow-pointing-to-right'></i></button>",
      responsive: [
      {
        breakpoint: 981,
        settings: {
          slidesToShow: 3,
          slidesToScroll: 1,
          arrows: true,
        }
      },
      {
        breakpoint: 851,
        settings: {
          slidesToShow: 3,
          slidesToScroll: 1,
          arrows: true,
          dots: false,
        }
      },
      {
        breakpoint: 770,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 1,
          arrows: true,
          dots: false,
        }
      },
      {
        breakpoint: 500,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
          arrows: false,
          dots: false,
        }
      }
      ]
    });

    //====== Team Carousel =====//
    $('.team-caro').slick({
      slidesToShow: 6,
      slidesToScroll: 1,
      dots: false,
      arrows: true,
      centerPadding: '0',
      focusOnSelect: true,
      vertical: false,
      prevArrow:"<button type='button' class='slick-prev'><i class='flaticon-arrow-pointing-to-left'></i></button>",
      nextArrow:"<button type='button' class='slick-next'><i class='flaticon-arrow-pointing-to-right'></i></button>",
      responsive: [
      {
        breakpoint: 1605,
        settings: {
          slidesToShow: 6,
          slidesToScroll: 1,
          arrows: true,
        }
      },
      {
        breakpoint: 1370,
        settings: {
          slidesToShow: 6,
          slidesToScroll: 1,
          arrows: true,
        }
      },
      {
        breakpoint: 1030,
        settings: {
          slidesToShow: 5,
          slidesToScroll: 1,
          arrows: true,
        }
      },
      {
        breakpoint: 981,
        settings: {
          slidesToShow: 5,
          slidesToScroll: 1,
          arrows: true,
        }
      },
      {
        breakpoint: 851,
        settings: {
          slidesToShow: 4,
          slidesToScroll: 1,
          arrows: true,
        }
      },
      {
        breakpoint: 500,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 1,
          arrows: false,
          dots: false,
        }
      }
      ]
    });

    //====== Team Carousel 2 =====//
    $('.team-caro2').slick({
      slidesToShow: 6,
      slidesToScroll: 1,
      dots: false,
      arrows: true,
      centerPadding: '0',
      focusOnSelect: true,
      vertical: false,
      prevArrow:"<button type='button' class='slick-prev'><i class='flaticon-arrow-pointing-to-left'></i></button>",
      nextArrow:"<button type='button' class='slick-next'><i class='flaticon-arrow-pointing-to-right'></i></button>",
      responsive: [
      {
        breakpoint: 1605,
        settings: {
          slidesToShow: 6,
          slidesToScroll: 1,
          arrows: true,
        }
      },
      {
        breakpoint: 1370,
        settings: {
          slidesToShow: 6,
          slidesToScroll: 1,
          arrows: true,
        }
      },
      {
        breakpoint: 1030,
        settings: {
          slidesToShow: 5,
          slidesToScroll: 1,
          arrows: true,
        }
      },
      {
        breakpoint: 981,
        settings: {
          slidesToShow: 5,
          slidesToScroll: 1,
          arrows: true,
        }
      },
      {
        breakpoint: 851,
        settings: {
          slidesToShow: 4,
          slidesToScroll: 1,
          arrows: true,
        }
      },
      {
        breakpoint: 500,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 1,
          arrows: false,
          dots: false,
        }
      }
      ]
    });

    //===== Shop Detail Carousel =====//
    $('.shop-detail-caro').slick({
      slidesToShow: 1,
      slidesToScroll: 1,
      arrows: false,
      // slide: 'li',
      fade: false,
      asNavFor: '.shop-detail-nav-caro'
    });

    $('.shop-detail-nav-caro').slick({
      slidesToShow: 4,
      slidesToScroll: 1,
      asNavFor: '.shop-detail-caro',
      dots: false,
      arrows: true,
      // slide: 'li',
      centerMode: false,
      vertical: true,
      centerPadding: '0px',
      focusOnSelect: true,
      prevArrow:"<button type='button' class='slick-prev'><i class='fas fa-chevron-up'></i></button>",
      nextArrow:"<button type='button' class='slick-next'><i class='fas fa-chevron-down'></i></button>",
      responsive: [{
      breakpoint: 1200,
        settings: {
          slidesToShow: 4,
          slidesToScroll: 1,
          infinite: true,
        }
      },
      {
      breakpoint: 850,
        settings: {
          slidesToShow: 3,
          slidesToScroll: 1,
          arrows: false,
          vertical: false,
        }
      },
      {
      breakpoint: 480,
        settings: {
          slidesToShow: 3,
          slidesToScroll: 1,
          arrows: false
        }
      }]
    });

    //====== Post Carousel =====//
    $('.post-caro').slick({
      slidesToShow: 3,
      slidesToScroll: 1,
      dots: false,
      arrows: true,
      centerPadding: '0',
      focusOnSelect: true,
      vertical: false,
      prevArrow:"<button type='button' class='slick-prev'><i class='flaticon-arrow-pointing-to-left'></i></button>",
      nextArrow:"<button type='button' class='slick-next'><i class='flaticon-arrow-pointing-to-right'></i></button>",
      responsive: [
      {
        breakpoint: 1605,
        settings: {
          slidesToShow: 3,
          slidesToScroll: 1,
          arrows: true
        }
      },
      {
        breakpoint: 981,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 1,
          arrows: true
        }
      },
      {
        breakpoint: 851,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 1,
          arrows: true
        }
      },
      {
        breakpoint: 500,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
          arrows: false,
          dots: false
        }
      }
      ]
    });

  }

}); //===== Document Ready Ends =====//

//===== Sticky Header =====//
$(window).on('scroll',function () {
  'use strict';

  var menu_height = $('header').innerHeight();
  var scroll = $(window).scrollTop();
  if (scroll >= menu_height) {
    $('body').addClass('sticky');
  } else {
    $('body').removeClass('sticky');
  }

});//===== Window onScroll Ends =====//