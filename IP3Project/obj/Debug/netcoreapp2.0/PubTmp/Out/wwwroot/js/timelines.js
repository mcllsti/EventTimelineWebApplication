/*

Author: Team16
Date: Trimester 2, 2018
Version: 1.0

Javascript functions that attend to the timelines forms 

Extensions and Tutorials used:
Modal Tutorial - Modal Loading - https://www.codeproject.com/Tips/826002/Bootstrap-Modal-Dialog-Loading-Content-from-MVC-Pa - Free Use

*/


///Controls the bootstrap modal elements
$(function () {
    // attach modal-container bootstrap attributes to links with .modal-link class.
    // when a link is clicked with these attributes, bootstrap will display the href content in a modal dialog.
    $('body').on('click', '.modal-link', function (e) {
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