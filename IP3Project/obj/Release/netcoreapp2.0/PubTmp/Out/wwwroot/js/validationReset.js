/*

Author: Team16
Date: Trimester 2, 2018
Version: 1.0

Used to ensure that validation is hitting forms in cascading partial views


*/

///Resets form validation
$(function () {
    var form = $("#Form");
    $(form).removeData("validator");
    $.validator.unobtrusive.parse(form);
});