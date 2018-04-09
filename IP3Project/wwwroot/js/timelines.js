$(function () {
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
        console.log("I've been calleda");
        $(this).attr('data-toggle', 'modal');
        console.log("I've been calledb");

    });
});