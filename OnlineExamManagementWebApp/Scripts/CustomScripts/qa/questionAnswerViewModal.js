//IIFE
(() => {
    $(() => {
        /*
         * Initialization
         */
        const qoViewModal = $("#js-modal-viewQo");

        // End

        // Popup Question Option Modal

        $(document).on("click",".js-qoModalPopup", function(event) {
            setTimeout(function(){
                console.log("Question ID >>"+$(event.target).data("data-question-id"));
                qoViewModal.modal("toggle");
            }, 300);
        });

        // End
    });
})(jQuery);