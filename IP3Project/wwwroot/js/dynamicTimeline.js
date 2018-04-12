//=====================
// Dynamic Timeline 
//=====================

$(document).ready(function () {

    var paras = $('.description-flex-container').find('p');

    $(".event").click(function () {
        var id = $(this).attr("id");
        var t = $(this),
            ind = t.index(),
            matchedPara = $(paras).eq(ind);

        // CALCULATE OFFSET FROM WINDOW
        var position = $(this).position();

        console.log($(this).position().left);
        console.log($(window).scrollLeft());

        // CHANGE THE WIDTH OF THE LINE TO MATCH OFFSET
        $("#line").css('width', position.left + 50);


        // FIND ANY EXISTING ACTIVE AND REMOVE
        $('.timeline-wrapper').find('.active-event').removeClass('active-event');

        // ADD THE CLASS
        $(this).addClass("active-event");


        //---Ajax load event---
        $('.loader-img').show();
        $('.event-view-box').hide();
        $.ajax({

            url: '/Event/DisplayEvent',
            data: { Id: id },
            type: "GET",
            success: function (responce) {
                $('.loader-img').hide();
                $('.description-flex-container').html(responce);
            },
            error: function (xhr) {
                $('.loader-img').hide();
                alert('error');
            }
        });
        //---end of ajax---

    });
});



$(function Yes() {
    console.log("I've been called1");
    // Initialize numeric spinner input boxes
    //$(".numeric-spinner").spinedit();
    // Initialize modal dialog
    // attach modal-container bootstrap attributes to links with .modal-link class.
    // when a link is clicked with these attributes, bootstrap will display the href content in a modal dialog.
    $('body').on('click', '.modal-link', function (e) {
        console.log("I've been called2");
        e.preventDefault();
        $(this).attr('data-target', '#modal-container');
        $(this).attr('data-toggle', 'modal');
    });
    // Attach listener to .modal-close-btn's so that when the button is pressed the modal dialog disappears
    $('body').on('click', '.modal-close-btn', function () {
        $('#modal-container').modal('hide');
    });
    //clear modal cache, so that new content can be loaded
    $('#modal-container').on('hidden.bs.modal', function () {
        $(this).removeData('bs.modal');
    });
});