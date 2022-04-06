(($) => {
    $(() => {
        Dropzone.options.js_dropZoneForm = {
            init: function() {
                this.on("complete", function(data) {
                    let response = JSON.parse(data.xhr.responseText);
                });
            }
        };
    });
})(jQuery);