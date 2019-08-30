// I used IIFE around the jQuery code to avoid conflict with other js libraries

(() => {
    $(() => {
        $("#org-list").select2({
            placeholder: "--Select Organization--"
        });

        $("#tag-list").select2({
            tags: true,
            tokenSeparators: [',', ' '],
            placeholder: "--Select or Create Tags--"


        });

        $(".close").alert();
    });
})(jQuery);