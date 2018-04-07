//// Write your JavaScript code.

$(function () {
    var inputs = $('.input');
    var paras = $('.description-flex-container').find('p');

    $(inputs).click(function () {
        var id = $(this).attr("id");
        var t = $(this),
            ind = t.index(),
            matchedPara = $(paras).eq(ind);

       $(t).add(matchedPara).addClass('active');
        $(inputs).not(t).add($(paras).not(matchedPara)).removeClass('active');
        $.ajax({

            url: '/Event/DisplayEvent',
            data: { Id: id },
            type: "GET",
            success: function (responce) {
                console.log(responce);
                $('.description-flex-container').html(responce);
            },
            error: function (xhr) {
                alert('error');
            }
        });
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