// I used IIFE around the jQuery code to avoid conflict with other js libraries

(($) => {
    $(() => {
        $("#dataTable-orgList").DataTable();

        const deleteOrgDialog = $("#js-deleteOrg-dialog");
        deleteOrgDialog.hide();

        const deleteOrgById = (orgId) => {
            $.post("/Organization/Delete/", { orgId: orgId })
                .done((data) => {
                    if (data) {
                        alert("Deleted Successfully");
                        location.reload(true);
                    } else {
                        alert("Deletion failed");
                    }
                })
                .fail((jqXhr, textStatus) => {
                    alert(`Error >>${textStatus}`);
                });
        }

        $(document).on("click", ".js-deleteOrgPopup", (event) => {
            const orgId = $(event.target).closest("tr").find('input[type="hidden"]').val();

            deleteOrgDialog.dialog({
                resizable: false,
                height: "auto",
                width: 400,
                modal: true,
                show: {
                    effect: "puff",
                    duration: 250
                },
                hide: {
                    effect: "explode",
                    duration: 250
                },
                buttons: [
                    {
                        text: "Delete?",
                        open: function () {
                            $(this).addClass("cancelClass");
                        },
                        click: function () {
                            deleteOrgById(orgId);
                            $(this).dialog("close");
                        }
                    }, {
                        text: "Cancel",
                        open: function () {
                            $(this).addClass("confirmClass");
                        },
                        click: function () {
                            $(this).dialog("close");
                        }
                    }
                ]
            });
        });
    });
})(jQuery);