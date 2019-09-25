// IIFE

(($) => {
    $(() => {
        $("#tbl-orgSearchResult").DataTable();
    });

    /*
     * Delete Organization
     */
    const deleteOrgById = (id) => {
        $.ajax({
            url: "/Organization/Delete/",
            type: "POST",
            dataType: "json",
            data: { orgId: id }
        })
            .done((data) => {
                if (data) {
                    setTimeout(() => {
                        alert("Organization has deleted");
                        location.reload(true);
                    }, 300);
                }
            })
            .fail((jqXhr, textStatus) => {
                alert(`Error >>${textStatus}`);
            });
    }

    $(document).on("click", ".js-deleteOrgPopup", (event) => {
        const orgId = $(event.target).closest("tr").find(`input[type="hidden"]`).val();

        bootbox.confirm({
            message: "Are you sure you want to delete this Organization?",
            buttons: {
                confirm: {
                    label: `<i class="fa fa-check"></i> Yes`,
                    className: "btn-danger"
                }
                , cancel: {
                    label: `<i class="fa fa-times"></i> No`,
                    className: "btn-success"
                }
            },
            callback: (result) => {
                if (result) {
                    deleteOrgById(orgId);
                }
            }
        });
    });
    /*
     * END
     */
})(jQuery);