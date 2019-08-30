// I used IIFE around the jQuery code to avoid conflict with other js libraries

(() => {
    $(() => {
        $("#org-list").select2({
            placeholder: "--Select Organization--"
        });

        $("#tag-list").select2({
            tags: true,
            tokenSeparators: [',', ' '],
            placeholder: "--Select or Create Tags--",
            ajax: {
                delay: 250,
                url: "/Course/GetAllTags",
                data: function(params) {
                    return {
                        search: params.term,
                        type: 'public'
                    };
                },
                processResults: function (response) {
                    console.log(response);
                    return {
                        results: response
                    };
                }
            }


        });

        $(".close").alert();
    });
})(jQuery);