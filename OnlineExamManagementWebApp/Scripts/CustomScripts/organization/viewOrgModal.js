// IIFE
(($) => {
    $(() => {
        const viewOrgModal = $("#js-modal-viewOrg");

        $(document).on("click", ".js-viewOrgModalPopup", (event) => {
            viewOrgModal.modal("toggle");
        });
    });
})(jQuery);