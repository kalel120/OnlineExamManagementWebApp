// IIFE

(($) => {
    $(() => {
        const editOrgModal = $("#js-modal-editOrg");

        $(document).on("click", ".js-editOrgModalPopup", (event) => {
            setTimeout(() => {
                editOrgModal.modal("toggle");
            },300);
        });
    });
})(jQuery);