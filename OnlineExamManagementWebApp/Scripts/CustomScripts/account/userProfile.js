(($) => {
    $(() => {
        const updateProfileModal = $("#js-modal-updateProfile");

        $(document).on("click", ".js-updateProfile-popup", (event) => {
            updateProfileModal.modal("toggle");
        });
    });
})(jQuery);