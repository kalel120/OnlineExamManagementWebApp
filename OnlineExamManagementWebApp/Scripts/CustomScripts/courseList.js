﻿// I used IIFE around the jQuery code to avoid conflict with other js libraries

(($) => {
    $(() => {
        $("#dataTable-courseList").DataTable();
    });
})(jQuery);